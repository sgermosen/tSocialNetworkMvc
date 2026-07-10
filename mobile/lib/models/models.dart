class AuthResult {
  final String token;
  final DateTime expiration;
  final String email;
  final String fullName;

  AuthResult({
    required this.token,
    required this.expiration,
    required this.email,
    required this.fullName,
  });

  factory AuthResult.fromJson(Map<String, dynamic> json) {
    return AuthResult(
      token: json['token'] as String,
      expiration: DateTime.parse(json['expiration'] as String),
      email: json['email'] as String? ?? '',
      fullName: json['fullName'] as String? ?? '',
    );
  }
}

class UserProfile {
  final String email;
  final String fullName;
  final String? nickName;
  final String? phone;
  final String? bio;

  UserProfile({
    required this.email,
    required this.fullName,
    this.nickName,
    this.phone,
    this.bio,
  });

  factory UserProfile.fromJson(Map<String, dynamic> json) {
    return UserProfile(
      email: json['email'] as String? ?? '',
      fullName: json['fullName'] as String? ?? '',
      nickName: json['nickName'] as String?,
      phone: json['phone'] as String?,
      bio: json['bio'] as String?,
    );
  }
}

class Comment {
  final int id;
  final String? name;
  final String body;
  final DateTime date;
  final String authorName;
  final String authorEmail;

  Comment({
    required this.id,
    this.name,
    required this.body,
    required this.date,
    required this.authorName,
    required this.authorEmail,
  });

  factory Comment.fromJson(Map<String, dynamic> json) {
    return Comment(
      id: json['id'] as int,
      name: json['name'] as String?,
      body: json['body'] as String? ?? '',
      date: DateTime.parse(json['date'] as String),
      authorName: json['authorName'] as String? ?? '',
      authorEmail: json['authorEmail'] as String? ?? '',
    );
  }
}

class Post {
  final int id;
  final String? name;
  final String body;
  final DateTime date;
  final DateTime? updatedDate;
  final String authorName;
  final String authorEmail;
  final bool isMine;
  final int reactionCount;
  final String? myReaction;
  final List<Comment> comments;

  Post({
    required this.id,
    this.name,
    required this.body,
    required this.date,
    this.updatedDate,
    required this.authorName,
    required this.authorEmail,
    required this.isMine,
    required this.reactionCount,
    this.myReaction,
    required this.comments,
  });

  bool get iReacted => myReaction != null;

  factory Post.fromJson(Map<String, dynamic> json) {
    final commentsJson = (json['comments'] as List<dynamic>?) ?? <dynamic>[];
    return Post(
      id: json['id'] as int,
      name: json['name'] as String?,
      body: json['body'] as String? ?? '',
      date: DateTime.parse(json['date'] as String),
      updatedDate: json['updatedDate'] != null
          ? DateTime.parse(json['updatedDate'] as String)
          : null,
      authorName: json['authorName'] as String? ?? '',
      authorEmail: json['authorEmail'] as String? ?? '',
      isMine: json['isMine'] as bool? ?? false,
      reactionCount: json['reactionCount'] as int? ?? 0,
      myReaction: json['myReaction'] as String?,
      comments: commentsJson
          .map((c) => Comment.fromJson(c as Map<String, dynamic>))
          .toList(),
    );
  }
}

class ReactionResult {
  final int total;
  final String? myReaction;

  ReactionResult({required this.total, this.myReaction});

  bool get reacted => myReaction != null;

  factory ReactionResult.fromJson(Map<String, dynamic> json) {
    return ReactionResult(
      total: json['total'] as int? ?? 0,
      myReaction: json['myReaction'] as String?,
    );
  }
}

class AppNotification {
  final int id;
  final String? actorName;
  final String message;
  final String? url;
  final bool isRead;
  final DateTime createdAt;

  AppNotification({
    required this.id,
    this.actorName,
    required this.message,
    this.url,
    required this.isRead,
    required this.createdAt,
  });

  factory AppNotification.fromJson(Map<String, dynamic> json) {
    return AppNotification(
      id: json['id'] as int,
      actorName: json['actorName'] as String?,
      message: json['message'] as String? ?? '',
      url: json['url'] as String?,
      isRead: json['isRead'] as bool? ?? false,
      createdAt: DateTime.parse(json['createdAt'] as String),
    );
  }
}

class Group {
  final int id;
  final String name;
  final String? description;
  final String? typeName;
  final String? privacyName;
  final bool isAdmin;
  final bool isMember;

  Group({
    required this.id,
    required this.name,
    this.description,
    this.typeName,
    this.privacyName,
    required this.isAdmin,
    required this.isMember,
  });

  factory Group.fromJson(Map<String, dynamic> json) {
    return Group(
      id: json['id'] as int,
      name: json['name'] as String? ?? '',
      description: json['description'] as String?,
      typeName: json['typeName'] as String?,
      privacyName: json['privacyName'] as String?,
      isAdmin: json['isAdmin'] as bool? ?? false,
      isMember: json['isMember'] as bool? ?? false,
    );
  }
}
