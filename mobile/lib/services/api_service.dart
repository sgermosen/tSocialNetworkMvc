import 'dart:convert';
import 'package:http/http.dart' as http;

import '../config.dart';
import '../models/models.dart';

class ApiException implements Exception {
  final String message;
  final int? statusCode;

  ApiException(this.message, [this.statusCode]);

  @override
  String toString() => message;
}

class ApiService {
  final String baseUrl;
  String? _token;

  ApiService({String? baseUrl}) : baseUrl = baseUrl ?? AppConfig.apiBaseUrl;

  void setToken(String? token) {
    _token = token;
  }

  Map<String, String> _headers({bool withAuth = true}) {
    final headers = <String, String>{'Content-Type': 'application/json'};
    if (withAuth && _token != null) {
      headers['Authorization'] = 'Bearer $_token';
    }
    return headers;
  }

  Uri _uri(String path) => Uri.parse('$baseUrl$path');

  Never _fail(http.Response response) {
    String message = 'Request failed (${response.statusCode}).';
    try {
      final body = jsonDecode(response.body);
      if (body is Map && body['message'] is String) {
        message = body['message'] as String;
      }
    } catch (_) {}
    throw ApiException(message, response.statusCode);
  }

  Future<AuthResult> register({
    required String firstName,
    required String lastName,
    String? nickName,
    String? phone,
    required String email,
    required String password,
  }) async {
    final response = await http.post(
      _uri('/api/auth/register'),
      headers: _headers(withAuth: false),
      body: jsonEncode({
        'firstName': firstName,
        'lastName': lastName,
        'nickName': nickName,
        'phone': phone,
        'email': email,
        'password': password,
      }),
    );
    if (response.statusCode == 200) {
      return AuthResult.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    }
    _fail(response);
  }

  Future<AuthResult> login({
    required String email,
    required String password,
  }) async {
    final response = await http.post(
      _uri('/api/auth/login'),
      headers: _headers(withAuth: false),
      body: jsonEncode({'email': email, 'password': password}),
    );
    if (response.statusCode == 200) {
      return AuthResult.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    }
    _fail(response);
  }

  Future<UserProfile> me() async {
    final response = await http.get(_uri('/api/auth/me'), headers: _headers());
    if (response.statusCode == 200) {
      return UserProfile.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    }
    _fail(response);
  }

  Future<List<Post>> getPosts() async {
    final response = await http.get(_uri('/api/posts'), headers: _headers());
    if (response.statusCode == 200) {
      final list = jsonDecode(response.body) as List<dynamic>;
      return list.map((p) => Post.fromJson(p as Map<String, dynamic>)).toList();
    }
    _fail(response);
  }

  Future<Post> getPost(int id) async {
    final response = await http.get(_uri('/api/posts/$id'), headers: _headers());
    if (response.statusCode == 200) {
      return Post.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    }
    _fail(response);
  }

  Future<Post> createPost({String? name, required String body}) async {
    final response = await http.post(
      _uri('/api/posts'),
      headers: _headers(),
      body: jsonEncode({'name': name, 'body': body}),
    );
    if (response.statusCode == 200 || response.statusCode == 201) {
      return Post.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    }
    _fail(response);
  }

  Future<Comment> addComment(int postId, {String? name, required String body}) async {
    final response = await http.post(
      _uri('/api/posts/$postId/comments'),
      headers: _headers(),
      body: jsonEncode({'name': name, 'body': body}),
    );
    if (response.statusCode == 200) {
      return Comment.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    }
    _fail(response);
  }

  Future<void> deletePost(int id) async {
    final response = await http.delete(_uri('/api/posts/$id'), headers: _headers());
    if (response.statusCode != 204 && response.statusCode != 200) {
      _fail(response);
    }
  }

  Future<List<Group>> getGroups() async {
    final response = await http.get(_uri('/api/groups'), headers: _headers());
    if (response.statusCode == 200) {
      final list = jsonDecode(response.body) as List<dynamic>;
      return list.map((g) => Group.fromJson(g as Map<String, dynamic>)).toList();
    }
    _fail(response);
  }
}
