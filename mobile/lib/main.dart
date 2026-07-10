import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'state/auth_state.dart';
import 'theme.dart';
import 'screens/login_screen.dart';
import 'screens/home_screen.dart';

void main() {
  runApp(const TetasApp());
}

class TetasApp extends StatelessWidget {
  const TetasApp({super.key});

  @override
  Widget build(BuildContext context) {
    return ChangeNotifierProvider(
      create: (_) => AuthState()..loadSession(),
      child: MaterialApp(
        title: 'Tetas',
        debugShowCheckedModeBanner: false,
        theme: TetasTheme.light(),
        darkTheme: TetasTheme.dark(),
        home: const _Root(),
      ),
    );
  }
}

class _Root extends StatelessWidget {
  const _Root();

  @override
  Widget build(BuildContext context) {
    final status = context.watch<AuthState>().status;
    switch (status) {
      case AuthStatus.authenticated:
        return const HomeScreen();
      case AuthStatus.unauthenticated:
        return const LoginScreen();
      case AuthStatus.unknown:
        return const Scaffold(
          body: Center(child: CircularProgressIndicator()),
        );
    }
  }
}
