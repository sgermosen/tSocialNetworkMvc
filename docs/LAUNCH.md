# Tetas — Guía de publicación (paso a paso)

Esta guía cubre **todo** lo necesario para poner Tetas en producción: el
servidor (backend + API), Google Play y App Store, incluyendo íconos, textos
exactos para las fichas, clasificación de contenido, formularios de privacidad
y capturas.

> Léela una vez de principio a fin antes de empezar. Los pasos están ordenados;
> hay dependencias (por ejemplo, la app móvil necesita la URL del servidor ya
> desplegado).

---

## 0. Resumen y checklist

| # | Tarea | Dónde | Estado |
|---|-------|-------|--------|
| 1 | Elegir nombre público seguro para las tiendas | Decisión | ⬜ |
| 2 | Desplegar backend con HTTPS y secretos | Servidor | ⬜ |
| 3 | Configurar SMTP (correos de confirmación/reset) | Servidor | ⬜ |
| 4 | Publicar la política de privacidad (URL pública) | Servidor/web | ⬜ |
| 5 | Apuntar la app móvil a la API de producción | mobile/ | ⬜ |
| 6 | Generar keystore de firma (Android) | Local | ⬜ |
| 7 | Compilar `.aab` y subir a Google Play | Play Console | ⬜ |
| 8 | Compilar `.ipa` y subir a App Store (requiere macOS) | App Store Connect | ⬜ |
| 9 | Completar fichas, clasificación y privacidad | Ambas tiendas | ⬜ |
| 10 | Lanzar en pruebas internas → producción | Ambas tiendas | ⬜ |

### ⚠️ Decisión importante antes de publicar: el nombre

El nombre en clave **"Tetas"** es un acrónimo válido (*Tracking Education For
Teachers and Students*), pero como **palabra suelta en español es vulgar**. En
las tiendas esto puede:

- Provocar fricción en la **revisión** y en la **clasificación de contenido**.
- Perjudicar la percepción y el posicionamiento con público hispanohablante.

**Recomendación:** usa como *nombre visible en la tienda* el nombre completo o
uno neutro, dejando el código interno intacto. Por ejemplo:

- **Nombre de tienda:** `TETAS: Red educativa` o `TETAS – Education Network`
- **Nombre interno / package:** se mantiene (`tetas_mobile`, `com.sgermosen.tetas_mobile`).

En el resto de la guía, donde diga "nombre de la app", usa el nombre de tienda
que elijas.

---

## 1. Backend (servidor)

La API y el sitio MVC viven en el proyecto `Tetas.Web` (.NET 10). La app móvil
consume la API bajo `/api`.

### 1.1 Requisitos de producción

- Un host con **HTTPS** (obligatorio: las cookies seguras y los tokens JWT lo
  requieren; iOS/Android exigen `https` para tráfico normal).
- Un **dominio** (p. ej. `https://api.tudominio.com`).
- Una **base de datos** (ver 1.4).
- Credenciales **SMTP** para enviar correos (ver 1.5).

### 1.2 Secretos y variables de entorno

**Nunca** pongas secretos en `appsettings.json`. Configúralos como variables de
entorno (el separador de nivel en .NET es `__`, doble guion bajo):

| Variable | Ejemplo | Descripción |
|----------|---------|-------------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Activa HSTS y el manejo de errores de producción. |
| `Tokens__Key` | *(64+ caracteres aleatorios)* | Clave de firma JWT. **Obligatoria en producción** (la app no arranca sin ella). |
| `Tokens__Issuer` | `https://api.tudominio.com` | Emisor del token. |
| `Tokens__Audience` | `tetas-app` | Audiencia del token. |
| `DatabaseProvider` | `SqlServer` o `Sqlite` | Proveedor de base de datos. |
| `ConnectionStrings__SqliteCnn` | `Data Source=/app/data/tetas.db` | Cadena SQLite (si usas SQLite). |
| `ConnectionStrings__sGDatabaseCnn` | `Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=true` | Cadena SQL Server (si usas SqlServer). |
| `Mail__From` | `no-reply@tudominio.com` | Remitente de los correos. |
| `Mail__Smtp` | `smtp.tudominio.com` | Servidor SMTP. |
| `Mail__Port` | `587` | Puerto SMTP (STARTTLS). |
| `Mail__Password` | *(secreto)* | Contraseña/clave de aplicación SMTP. |
| `Constants__UrlBase` | `https://api.tudominio.com/` | Base para enlaces de correo. |

Genera una clave fuerte:

```bash
openssl rand -base64 64
```

### 1.3 Base de datos en producción

- **SQLite** (por defecto) funciona para pilotos y poco tráfico; guarda el
  archivo en un **volumen persistente** y haz backups del `.db`.
- Para producción real se recomienda **SQL Server** o **PostgreSQL**.

> **Nota técnica:** las migraciones incluidas se generaron para **SQLite**. Si
> cambias a SQL Server, regenera las migraciones para ese proveedor:
>
> ```bash
> # con DatabaseProvider=SqlServer y la cadena configurada
> rm -rf Tetas.Infraestructure/Migrations
> dotnet ef migrations add InitialCreate -p Tetas.Infraestructure -s Tetas.Web -o Migrations
> ```
>
> La app aplica las migraciones **automáticamente al arrancar**
> (`Database.Migrate()`), así que no hay pasos manuales de esquema al desplegar.

### 1.4 Opción A — Desplegar con Docker (recomendada)

El repo ya trae `Dockerfile` y `docker-compose.yml`.

```bash
# En el servidor, con el repo clonado:
export TOKENS_KEY="$(openssl rand -base64 64)"

docker compose up --build -d
```

Para producción, edita `docker-compose.yml` y añade las variables de 1.2
(sobre todo `ASPNETCORE_ENVIRONMENT=Production`, `Tokens__Key`, `Mail__*`).
La app queda en el puerto `8080`; ponla detrás de un proxy con TLS (1.6).

### 1.5 Opción B — VPS (Ubuntu) con Nginx + systemd + HTTPS

1. Instala el runtime de .NET 10 y publica:
   ```bash
   dotnet publish Tetas.Web/Tetas.Web.csproj -c Release -o /var/www/tetas
   ```
2. Servicio systemd `/etc/systemd/system/tetas.service`:
   ```ini
   [Unit]
   Description=Tetas
   After=network.target

   [Service]
   WorkingDirectory=/var/www/tetas
   ExecStart=/usr/bin/dotnet /var/www/tetas/Tetas.Web.dll
   Restart=always
   Environment=ASPNETCORE_ENVIRONMENT=Production
   Environment=ASPNETCORE_URLS=http://127.0.0.1:5000
   Environment=Tokens__Key=PON_AQUI_TU_CLAVE
   Environment=Mail__From=no-reply@tudominio.com
   Environment=Mail__Smtp=smtp.tudominio.com
   Environment=Mail__Port=587
   Environment=Mail__Password=TU_CLAVE_SMTP
   Environment=Constants__UrlBase=https://api.tudominio.com/

   [Install]
   WantedBy=multi-user.target
   ```
   ```bash
   sudo systemctl enable --now tetas
   ```
3. Nginx como reverse proxy con **WebSockets** (necesario para SignalR):
   ```nginx
   server {
     server_name api.tudominio.com;
     location / {
       proxy_pass         http://127.0.0.1:5000;
       proxy_http_version 1.1;
       proxy_set_header   Upgrade $http_upgrade;
       proxy_set_header   Connection keep-alive;
       proxy_set_header   Host $host;
       proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
       proxy_set_header   X-Forwarded-Proto $scheme;
     }
   }
   ```
4. TLS gratis con Let's Encrypt:
   ```bash
   sudo certbot --nginx -d api.tudominio.com
   ```

### 1.6 Opción C — Azure App Service / Render / Railway / Fly.io

Cualquier plataforma que corra un contenedor Linux o una app .NET sirve:
- Sube el contenedor del `Dockerfile` o publica el proyecto.
- Define las variables de 1.2 en el panel de configuración.
- Activa **HTTPS** (casi siempre gestionado por la plataforma).
- Habilita **WebSockets** (Azure App Service: *Configuration → General settings →
  Web sockets = On*) para SignalR.

### 1.7 Correo (SMTP)

Los correos de confirmación y de reseteo usan `Mail__*`. Con Gmail Workspace
usa una **contraseña de aplicación**; con un proveedor transaccional
(SendGrid, Mailgun, Amazon SES, Postmark) usa su SMTP y clave. Verifica el
dominio (SPF/DKIM) para que no caigan en spam.

### 1.8 Endurecimiento post-despliegue

- **CORS:** hoy la política permite cualquier origen (cómodo para desarrollo).
  Si expones un frontend web aparte, restríngela a tus dominios en
  `Program.cs` (`WithOrigins(...)` en vez de `AllowAnyOrigin`). Las apps
  móviles nativas **no** usan CORS.
- **SignalR a escala:** si corres **más de una instancia**, añade un backplane
  (Redis) o activa *sticky sessions* en el balanceador. Con una sola instancia
  no hace falta.
- **Backups:** programa copias de la base de datos.

### 1.9 Apuntar la app móvil al servidor

La app lee la URL de la API de `--dart-define=TETAS_API_BASE_URL`. En los builds
de release usarás tu dominio real (ver secciones 3 y 4):

```bash
--dart-define=TETAS_API_BASE_URL=https://api.tudominio.com
```

---

## 2. Preparación común de la app móvil

### 2.1 Identidad

| Campo | Valor sugerido |
|-------|----------------|
| Nombre de tienda | `TETAS: Red educativa` (ver decisión del nombre) |
| Package Android | `com.sgermosen.tetas_mobile` |
| Bundle ID iOS | `com.sgermosen.tetasMobile` |
| Versión | `1.0.0` (build `1`) — en `pubspec.yaml`: `version: 1.0.0+1` |

Cambia el **nombre visible**:
- Android: `mobile/android/app/src/main/AndroidManifest.xml` → `android:label`.
- iOS: `mobile/ios/Runner/Info.plist` → `CFBundleDisplayName`.

### 2.2 Íconos (ya incluidos)

El ícono de marca (birrete sobre gradiente teal con borla ámbar) ya está en
`mobile/assets/icon/` y configurado con `flutter_launcher_icons`. Para
regenerarlos tras cualquier cambio de arte:

```bash
cd mobile
dart run flutter_launcher_icons
```

Esto crea los íconos adaptativos de Android y el *asset catalog* de iOS. Para
las tiendas necesitarás además:
- **Google Play — ícono de ficha:** `mobile/assets/icon/icon.png` (1024×1024;
  Play pide 512×512, redimensiónalo).
- **App Store — ícono:** 1024×1024 **sin canal alfa/transparencia** (usa
  `icon.png`, que ya es opaco).

### 2.3 Versionado

Cada envío a las tiendas necesita subir el número de build. En `pubspec.yaml`:
`version: 1.0.1+2` (el `+2` es el build number).

---

## 3. Google Play (Android)

### 3.1 Cuenta

- Crea una cuenta de **Google Play Console** (pago único de **25 USD**):
  <https://play.google.com/console>.
- Completa la verificación de identidad/negocio (puede tardar días).

### 3.2 Firma de la app (keystore de subida)

Genera **una vez** tu keystore de subida y **guárdalo a buen recaudo** (si lo
pierdes, no puedes actualizar la app):

```bash
keytool -genkey -v -keystore ~/tetas-upload.jks \
  -keyalg RSA -keysize 2048 -validity 10000 -alias upload
```

Crea `mobile/android/key.properties` (NO lo subas a git):

```properties
storePassword=TU_PASS
keyPassword=TU_PASS
keyAlias=upload
storeFile=/ruta/absoluta/tetas-upload.jks
```

Y enlázalo en `mobile/android/app/build.gradle` (bloque `signingConfigs` +
`buildTypes.release`). Usa **Play App Signing** (Google guarda la clave final;
tú firmas con la de subida).

### 3.3 Compilar el App Bundle

```bash
cd mobile
flutter build appbundle --release \
  --dart-define=TETAS_API_BASE_URL=https://api.tudominio.com
# Resultado: build/app/outputs/bundle/release/app-release.aab
```

Asegúrate de que `targetSdkVersion` cumpla el mínimo vigente de Play (Android
15 / API 35 en 2025-2026; Flutter reciente lo pone por defecto).

### 3.4 Crear la app en Play Console

*Todas las apps → Crear app*: idioma por defecto, nombre, tipo **App**,
gratuita, y acepta las políticas.

### 3.5 Textos de la ficha (listos para pegar)

- **Nombre de la app** (máx 30): `TETAS: Red educativa`
- **Descripción corta** (máx 80):
  `Red social educativa para conectar a docentes y estudiantes.`
- **Descripción completa** (máx 4000): ver **§5**.

### 3.6 Recursos gráficos

| Recurso | Especificación |
|---------|----------------|
| Ícono | 512×512 PNG (32 bits, con alfa). |
| Gráfico destacado (*feature graphic*) | 1024×500 PNG/JPG, **sin** transparencia. Listo en `docs/store-assets/feature-graphic.png`. |
| Capturas de teléfono | 2–8 imágenes, PNG/JPEG 24-bit, relación 9:16 o 16:9, cada lado entre 320 y 3840 px. |
| Capturas de tablet (opcional) | Solo si marcas soporte de tablet. |

Cómo generar las capturas: **§7**.

### 3.7 Clasificación de contenido (cuestionario IARC)

En *Contenido de la app → Clasificación de contenido*. Para Tetas (red social
sin contenido explícito), las respuestas típicas:

- Categoría: **Red social / comunicación**.
- ¿Violencia, sexo, drogas, lenguaje soez, apuestas? → **No** a todo.
- **¿Interacción entre usuarios / contenido generado por usuarios?** → **Sí**
  (hay publicaciones, comentarios y grupos).
- ¿Comparte ubicación del usuario? → **No**.
- ¿Compras dentro de la app? → **No**.

Resultado esperado: apta para adolescentes/todo público según el país (IARC lo
calcula). Declarar el UGC es obligatorio y correcto.

### 3.8 Seguridad de los datos (*Data safety*)

En *Contenido de la app → Seguridad de los datos*. Según lo que recopila Tetas:

- **Datos recopilados:**
  - *Información personal:* nombre, dirección de correo.
  - *Mensajes/contenido:* publicaciones y comentarios del usuario.
  - *Identificadores/actividad de la app:* cuenta de usuario.
- **Cifrado en tránsito:** **Sí** (HTTPS).
- **¿Se puede solicitar eliminación de datos?** Indica cómo (correo de soporte o
  endpoint). Recomendado ofrecer borrado de cuenta.
- **¿Se comparten datos con terceros?** **No** (salvo que añadas analítica/ads).
- **¿Datos de ubicación / financieros / salud?** **No**.

### 3.9 Política de privacidad

Play **exige una URL pública** de política de privacidad. Usa la plantilla de
**§6**, hospédala (p. ej. `https://api.tudominio.com/privacy` o una página
estática) y pega la URL en *Contenido de la app → Política de privacidad*.

### 3.10 Categorización, países y precio

- **Categoría:** *Educación* (o *Social*).
- **Etiquetas:** educación, comunidad, estudiantes, docentes.
- **Público objetivo y contenido:** declara el rango de edad; al haber UGC,
  Play pedirá cumplir sus políticas de contenido y de familias.
- **Precio:** gratuita. **Países:** los que quieras distribuir.

### 3.11 Publicar

Recorrido recomendado: **Pruebas internas** (lista de correos) →
**Prueba cerrada** → **Producción**. Sube el `.aab`, redacta las **notas de la
versión** (§5) y envía a revisión.

---

## 4. Apple App Store (iOS)

> Requiere una **Mac** con Xcode para compilar y firmar (o un servicio en la
> nube como Codemagic/Xcode Cloud). No se puede compilar iOS en Windows/Linux.

### 4.1 Cuenta

- **Apple Developer Program**: **99 USD/año**
  (<https://developer.apple.com/programs/>). Verificación puede tardar días.

### 4.2 Identificador y firma

- En *App Store Connect* y *Developer → Identifiers*, crea el **App ID** con el
  Bundle ID `com.sgermosen.tetasMobile`.
- En Xcode (`mobile/ios/Runner.xcworkspace`): selecciona tu *Team*, activa
  **Automatically manage signing** (crea certificados y perfiles solo).

### 4.3 Compilar el IPA

```bash
cd mobile
flutter build ipa --release \
  --dart-define=TETAS_API_BASE_URL=https://api.tudominio.com
```

Sube el `.ipa` (o el archivo desde Xcode → *Organizer → Distribute App*) con
**Transporter** o directamente desde Xcode a App Store Connect.

### 4.4 Ficha en App Store Connect (textos listos para pegar)

- **Nombre** (máx 30): `TETAS: Red educativa`
- **Subtítulo** (máx 30): `Docentes y estudiantes`
- **Texto promocional** (máx 170): ver **§5**.
- **Descripción** (máx 4000): ver **§5**.
- **Palabras clave** (máx 100, separadas por comas): ver **§5**.
- **URL de soporte** y **URL de marketing**: tu web/correo.
- **URL de política de privacidad:** obligatoria (§6).

### 4.5 Capturas requeridas

| Dispositivo | Tamaño (px, vertical) | Requerido |
|-------------|-----------------------|-----------|
| iPhone 6.9" | 1290×2796 o 1320×2868 | **Sí** (al menos 1 set) |
| iPhone 6.7" | 1284×2778 o 1290×2796 | Recomendado |
| iPad Pro 13" | 2064×2752 o 2048×2732 | Solo si soportas iPad |

Hasta 10 capturas por tamaño. Cómo generarlas: **§7**.

### 4.6 Privacidad de la app (*nutrition labels*)

En *App Store Connect → App Privacy*. Coherente con §3.8:

- **Datos vinculados al usuario:** *Contact Info* (nombre, email),
  *User Content* (publicaciones/comentarios), *Identifiers* (cuenta).
- **Uso:** *App Functionality* (funcionamiento del producto).
- **¿Se usan para seguimiento/tracking?** **No**.
- **¿Se comparten con terceros?** **No** (salvo que integres SDKs externos).

### 4.7 Clasificación por edad

Cuestionario de *Age Rating*: sin contenido explícito → **No** a violencia,
sexo, drogas, apuestas. Hay **contenido generado por usuarios**, pero la app
**ya incluye moderación** (denunciar publicaciones y bloquear usuarios), así
que puedes declararlo y mantener una clasificación más baja (típicamente
**12+**). Declara el UGC y sus controles con honestidad.

### 4.8 Cumplimiento de exportación (cifrado)

La app usa HTTPS estándar. En *App Store Connect* declara que **usas cifrado
estándar exento**; añade en `Info.plist`:

```xml
<key>ITSAppUsesNonExemptEncryption</key>
<false/>
```

### 4.9 TestFlight y envío

- Prueba con **TestFlight** (beta) antes de producción.
- Completa la ficha, adjunta capturas, responde privacidad/edad y **envía a
  revisión**. La revisión de Apple suele tardar de horas a 1–2 días.

### 4.10 Errores comunes de rechazo (evítalos)

- **UGC — requisitos de Apple:** para apps con contenido de usuarios exige
  (a) filtro de contenido objetable, (b) **denuncia**, (c) **bloqueo** de
  usuarios y (d) contacto del desarrollador. La app **ya implementa denuncia y
  bloqueo**; asegúrate de responder los reportes y de publicar un correo de
  contacto.
- **Login que exige cuenta sin necesidad:** ofrece ver contenido o explica el
  valor; si pides registro, que sea coherente.
- **Privacidad incompleta o URL caída.**
- **Metadatos/capturas que no reflejan la app real.**

---

## 5. Textos de las tiendas (copiar y pegar)

**Nombre:** `TETAS: Red educativa`
**Subtítulo (iOS, 30):** `Docentes y estudiantes`
**Descripción corta (Android, 80):**
`Red social educativa para conectar a docentes y estudiantes.`

**Texto promocional (iOS, 170):**
`Publica, comenta y reacciona. Crea grupos de clase y mantén tus círculos
académicos conectados, con notificaciones en tiempo real.`

**Descripción completa (ambas, ≤4000):**

```
TETAS (Tracking Education For Teachers and Students) es una red social de
código abierto pensada para el mundo educativo: conecta a docentes y
estudiantes sin mezclar lo personal con lo profesional.

Con TETAS puedes:
• Publicar entradas y comentar las de tu comunidad.
• Reaccionar a las publicaciones que te gustan.
• Crear grupos públicos o privados para clases, clubes y proyectos.
• Moderar tus grupos: aceptar, rechazar, banear o quitar el baneo a miembros.
• Recibir notificaciones en tiempo real cuando alguien comenta o reacciona.
• Gestionar tu perfil y tu cuenta de forma segura.

Pensada para universidades, colegios, organizaciones y cualquier comunidad de
aprendizaje que quiera un espacio propio para comunicarse.

TETAS es open source. Tu privacidad importa: los datos viajan cifrados y no se
comparten con terceros.
```

**Palabras clave (iOS, ≤100):**
`educación,estudiantes,docentes,red social,grupos,clase,universidad,comunidad,aprendizaje`

**Notas de la versión (1.0.0):**
```
Primer lanzamiento de TETAS. Publicaciones, comentarios, reacciones, grupos y
notificaciones en tiempo real para tu comunidad educativa.
```

---

## 6. Política de privacidad (plantilla lista para hospedar)

> Sustituye los campos entre corchetes. Debe quedar en una **URL pública**.

```
Política de Privacidad de TETAS
Última actualización: [FECHA]

Responsable: [NOMBRE/ORGANIZACIÓN], contacto: [CORREO].

1. Datos que recopilamos
- Datos de cuenta: nombre, apodo, teléfono (opcional) y correo electrónico.
- Contenido que publicas: entradas, comentarios y reacciones.
- Datos técnicos mínimos necesarios para operar el servicio.

2. Para qué los usamos
Para crear y gestionar tu cuenta, mostrar tu contenido, enviarte
notificaciones y correos de confirmación/recuperación, y mantener la
seguridad del servicio.

3. Base y almacenamiento
Los datos se almacenan en nuestros servidores y viajan cifrados (HTTPS). Las
contraseñas se guardan con algoritmos de hash; no las conservamos en texto
plano.

4. Compartición
No vendemos ni compartimos tus datos con terceros con fines publicitarios.

5. Tus derechos
Puedes solicitar acceso, corrección o eliminación de tus datos y de tu cuenta
escribiendo a [CORREO].

6. Menores
[Indica tu política respecto a menores de edad según tu público.]

7. Cambios
Podemos actualizar esta política; publicaremos la nueva versión en esta misma
página.

Contacto: [CORREO].
```

---

## 7. Capturas de pantalla (cómo generarlas y enmarcarlas)

Necesitas capturas reales de la app en los tamaños que piden las tiendas (§3.6,
§4.5). Opciones:

1. **Emulador/simulador:**
   - Android: `flutter run` en un emulador Pixel, navega y usa la captura del
     emulador (o `adb exec-out screencap -p > shot.png`).
   - iOS: simulador → `Cmd+S`, o `xcrun simctl io booted screenshot shot.png`.
2. **Dispositivo físico:** toma capturas nativas navegando por Feed, Detalle de
   publicación (con reacciones y comentarios), Grupos, Notificaciones y Perfil.
3. **Enmarcado bonito (opcional):** herramientas como *fastlane frameit*,
   *Shotbot*, *AppLaunchpad* o *Previewed* ponen el marco del dispositivo y un
   fondo con texto de marketing.

**Guion sugerido (5 capturas):**
1. Feed con publicaciones, reacciones y comentarios.
2. Detalle de una publicación con comentarios.
3. Pantalla de Grupos.
4. Campana de **notificaciones** (destaca el tiempo real).
5. Perfil.

> Referencia visual de la identidad (versión web del mismo diseño) en la ficha
> del repositorio y en la guía visual adjunta.

---

## 8. Después del lanzamiento

- **Monitoreo:** revisa logs y errores; añade *health checks*/alertas.
- **Actualizaciones:** sube el *build number* (§2.3), recompila y vuelve a
  enviar. Los cambios de solo servidor no requieren reenviar la app.
- **Moderación:** si activas denuncia/bloqueo, podrás bajar la clasificación de
  edad en iOS y cumplir mejor las políticas de UGC.
- **Roadmap sugerido:** seguir a personas y feed personalizado, adjuntar
  imágenes, y un asistente de IA educativa (quizzes/resúmenes).

---

¿Dudas con algún paso concreto (keystore, Nginx, un formulario de la tienda)?
Cada sección es autocontenida; síguelas en orden y tendrás la app publicada.
```
