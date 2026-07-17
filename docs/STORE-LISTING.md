# Tetas — Ficha técnica para las tiendas (copiar y pegar)

Hoja de referencia con **cada campo y su valor** para Google Play Console y App
Store Connect. Ajusta el correo, la URL y el nombre visible que elijas
(recomendado `TETAS: Red educativa`; ver `docs/LAUNCH.md`).

> **Moderación ya incluida.** La app permite **denunciar publicaciones** y
> **bloquear usuarios**. Decláralo con honestidad: cumple el requisito de
> contenido generado por usuarios (UGC) de ambas tiendas y permite una
> clasificación de edad más baja.

Recursos gráficos listos en `docs/store-assets/`:
- `feature-graphic.png` — 1024×500 (Google Play).
- Ícono: `mobile/assets/icon/icon.png` (1024×1024; para Play redimensiona a 512×512).

---

## A) Google Play Console

### Detalles de la app (Store listing)

| Campo | Valor |
|-------|-------|
| Nombre de la app (≤30) | `TETAS: Red educativa` |
| Idioma predeterminado | Español (España) o Español (Latinoamérica) |
| Descripción corta (≤80) | `Red social educativa para conectar a docentes y estudiantes.` |
| Descripción completa (≤4000) | Ver bloque **§C** |
| Ícono (512×512, 32-bit PNG) | desde `mobile/assets/icon/icon.png` |
| Gráfico destacado (1024×500) | `docs/store-assets/feature-graphic.png` |
| Capturas de teléfono (2–8) | Ver `docs/LAUNCH.md` §7 (guion sugerido) |
| Vídeo (opcional) | — |

### Categorización y etiquetas

| Campo | Valor |
|-------|-------|
| Tipo de app | Aplicación |
| Categoría | **Educación** (alternativa: **Social**) |
| Etiquetas (hasta 5, de la lista de Play) | Educación · Comunidades · Redes sociales · Estudiantes · Comunicación |
| Correo de contacto | `[tu-correo@dominio.com]` |
| Sitio web (opcional) | `https://[tu-dominio o repo]` |
| Teléfono (opcional) | — |

### Clasificación de contenido (cuestionario IARC)

| Pregunta | Respuesta |
|----------|-----------|
| Categoría | Red social / comunicación |
| Violencia, sexo, drogas, lenguaje soez, apuestas | **No** a todo |
| ¿Los usuarios interactúan / hay contenido generado por usuarios? | **Sí** |
| ¿La app incluye moderación (denuncia/bloqueo)? | **Sí** (denunciar posts, bloquear usuarios) |
| ¿Comparte la ubicación del usuario? | **No** |
| ¿Compras dentro de la app? | **No** |

### Público objetivo y contenido

| Campo | Valor |
|-------|-------|
| Rango de edad objetivo | 13+ (ajústalo a tu política) |
| ¿Dirigida a menores? | No exclusivamente |
| Anuncios | **No** |

### Seguridad de los datos (Data safety)

| Pregunta | Respuesta |
|----------|-----------|
| ¿Recopila o comparte datos? | Recopila; **no comparte** con terceros |
| Datos personales | Nombre; dirección de correo |
| Mensajes / contenido | Publicaciones y comentarios del usuario |
| Identificadores / actividad | Cuenta de usuario |
| Ubicación, financieros, salud, contactos, fotos | **No** |
| ¿Cifrado en tránsito? | **Sí** (HTTPS) |
| ¿El usuario puede pedir eliminación de datos? | **Sí** (correo de soporte / borrado de cuenta) |
| ¿Datos usados para publicidad/tracking? | **No** |

### Distribución y precio

| Campo | Valor |
|-------|-------|
| Precio | Gratuita |
| Países / regiones | Los que elijas |
| Política de privacidad (URL pública) | `https://[tu-dominio]/Home/Privacy` (ya la sirve la app; texto en `docs/PRIVACY.md`) |
| Anuncios | No |
| Pista de lanzamiento | Interna → Cerrada → Producción |

---

## B) App Store Connect (iOS)

### Información de la app

| Campo | Valor |
|-------|-------|
| Nombre (≤30) | `TETAS: Red educativa` |
| Subtítulo (≤30) | `Docentes y estudiantes` |
| Bundle ID | `com.sgermosen.tetasMobile` |
| Categoría principal | **Educación** |
| Categoría secundaria | **Redes sociales** |
| Derechos de autor | `© 2026 [tu nombre]` |

### Versión (1.0.0)

| Campo | Valor |
|-------|-------|
| Texto promocional (≤170) | Ver **§C** |
| Descripción (≤4000) | Ver **§C** |
| Palabras clave (≤100, separadas por comas) | `educación,estudiantes,docentes,red social,grupos,clase,universidad,comunidad,aprendizaje` |
| URL de soporte | `https://[tu-dominio o repo]` |
| URL de marketing (opcional) | `https://[tu-dominio]` |
| URL de política de privacidad | `https://[tu-dominio]/Home/Privacy` |
| Novedades (What's New) | Ver **§C** |
| Ícono (1024×1024, sin alfa) | `mobile/assets/icon/icon.png` |
| Capturas | iPhone 6.9" 1290×2796 (obligatorio); ver `docs/LAUNCH.md` §4.5 |

### Clasificación por edad (Age Rating)

| Pregunta | Respuesta |
|----------|-----------|
| Violencia, sexo, terror, drogas, apuestas, lenguaje | **Ninguno** |
| Contenido generado por usuarios | **Sí**, con controles: **denuncia** y **bloqueo** implementados |
| Clasificación resultante esperada | 12+ (con moderación activa) |

### Privacidad de la app (App Privacy)

| Tipo de dato | ¿Vinculado al usuario? | Propósito |
|--------------|------------------------|-----------|
| Información de contacto (nombre, email) | Sí | Funcionamiento de la app |
| Contenido del usuario (publicaciones, comentarios) | Sí | Funcionamiento de la app |
| Identificadores (cuenta de usuario) | Sí | Funcionamiento de la app |
| ¿Se usa para tracking? | **No** | — |
| ¿Se comparte con terceros? | **No** | — |

### Cumplimiento de exportación (cifrado)

| Campo | Valor |
|-------|-------|
| ¿Usa cifrado? | Solo HTTPS estándar → **exento** |
| `Info.plist` | `ITSAppUsesNonExemptEncryption = false` |

### Precio y disponibilidad

| Campo | Valor |
|-------|-------|
| Precio | Gratis (Tier 0) |
| Disponibilidad | Países que elijas |
| Beta | TestFlight antes de producción |

---

## C) Textos compartidos (copiar y pegar)

**Descripción completa (Play y App Store, ≤4000):**

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
• Denunciar contenido y bloquear usuarios para mantener un espacio sano.
• Gestionar tu perfil y tu cuenta de forma segura.

Pensada para universidades, colegios, organizaciones y cualquier comunidad de
aprendizaje que quiera un espacio propio para comunicarse.

TETAS es open source. Tu privacidad importa: los datos viajan cifrados y no se
comparten con terceros.
```

**Texto promocional (iOS, ≤170):**
```
Publica, comenta y reacciona. Crea grupos de clase y mantén tus círculos
académicos conectados, con notificaciones en tiempo real.
```

**Novedades / What's New (1.0.0):**
```
Primer lanzamiento de TETAS. Publicaciones, comentarios, reacciones, grupos,
notificaciones en tiempo real y herramientas de moderación (denuncia y
bloqueo) para tu comunidad educativa.
```

---

## D) Checklist de recursos

- [x] Ícono 1024×1024 sin transparencia — `mobile/assets/icon/icon.png`
- [x] Íconos de app (Android/iOS) generados — `flutter_launcher_icons`
- [x] Gráfico destacado 1024×500 — `docs/store-assets/feature-graphic.png`
- [ ] 2–8 capturas de teléfono (Android) — capturar (guion en LAUNCH §7)
- [ ] Capturas iPhone 6.9" (iOS) — capturar en simulador/dispositivo
- [ ] Política de privacidad hospedada en URL pública
- [ ] Correo de soporte definido
- [ ] Cuentas de desarrollador creadas (Play 25 USD, Apple 99 USD/año)
```
