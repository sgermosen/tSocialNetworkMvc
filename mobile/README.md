# Tetas Mobile

A Flutter client for the Tetas educational social network. It consumes the
JWT-secured REST API exposed by the `Tetas.Web` project.

## Features

- Register and log in (JWT bearer authentication, token persisted on device).
- Browse the latest posts feed with pull-to-refresh.
- Open a post to read it and its comments, and add your own comment.
- Create new posts.
- Browse the public and joined groups.
- View your profile and log out.

## Requirements

- Flutter 3.29 or newer (Dart 3.6+).
- A running instance of `Tetas.Web` (see the repository root README).

## Configure the API base URL

The app talks to the REST API under `/api`. The default base URL is
`http://10.0.2.2:5000`, which is how the Android emulator reaches the host
machine's `localhost:5000`.

Override it at build/run time:

```bash
flutter run --dart-define=TETAS_API_BASE_URL=http://192.168.1.20:5000
```

- Android emulator: `http://10.0.2.2:5000`
- iOS simulator: `http://localhost:5000`
- Physical device: `http://<your-computer-lan-ip>:5000`

## Run

```bash
cd mobile
flutter pub get
flutter run --dart-define=TETAS_API_BASE_URL=http://10.0.2.2:5000
```

## Project structure

```
lib/
  config.dart              API base URL configuration
  main.dart                App entry, theme and auth-based routing
  theme.dart               Teal/amber Material 3 theme (light + dark)
  models/models.dart       DTO models with JSON parsing
  services/api_service.dart HTTP client for the REST API
  state/auth_state.dart    Authentication state (provider ChangeNotifier)
  widgets/                 Reusable widgets (avatar, post card)
  screens/                 Login, register, feed, post detail, create post,
                           groups and profile screens
```
