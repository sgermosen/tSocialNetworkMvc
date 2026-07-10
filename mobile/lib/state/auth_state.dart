import 'package:flutter/foundation.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../models/models.dart';
import '../services/api_service.dart';

enum AuthStatus { unknown, authenticated, unauthenticated }

class AuthState extends ChangeNotifier {
  final ApiService api;

  AuthStatus _status = AuthStatus.unknown;
  String? _displayName;
  String? _email;

  AuthState({ApiService? apiService}) : api = apiService ?? ApiService();

  AuthStatus get status => _status;
  String? get displayName => _displayName;
  String? get email => _email;

  static const _tokenKey = 'tetas_token';
  static const _nameKey = 'tetas_name';
  static const _emailKey = 'tetas_email';

  Future<void> loadSession() async {
    final prefs = await SharedPreferences.getInstance();
    final token = prefs.getString(_tokenKey);
    if (token != null && token.isNotEmpty) {
      _displayName = prefs.getString(_nameKey);
      _email = prefs.getString(_emailKey);
      api.setToken(token);
      _status = AuthStatus.authenticated;
    } else {
      _status = AuthStatus.unauthenticated;
    }
    notifyListeners();
  }

  Future<void> _persist(AuthResult result) async {
    _displayName = result.fullName;
    _email = result.email;
    api.setToken(result.token);

    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_tokenKey, result.token);
    await prefs.setString(_nameKey, result.fullName);
    await prefs.setString(_emailKey, result.email);

    _status = AuthStatus.authenticated;
    notifyListeners();
  }

  Future<void> login(String email, String password) async {
    final result = await api.login(email: email, password: password);
    await _persist(result);
  }

  Future<void> register({
    required String firstName,
    required String lastName,
    String? nickName,
    String? phone,
    required String email,
    required String password,
  }) async {
    final result = await api.register(
      firstName: firstName,
      lastName: lastName,
      nickName: nickName,
      phone: phone,
      email: email,
      password: password,
    );
    await _persist(result);
  }

  Future<void> logout() async {
    _displayName = null;
    _email = null;
    api.setToken(null);

    final prefs = await SharedPreferences.getInstance();
    await prefs.remove(_tokenKey);
    await prefs.remove(_nameKey);
    await prefs.remove(_emailKey);

    _status = AuthStatus.unauthenticated;
    notifyListeners();
  }
}
