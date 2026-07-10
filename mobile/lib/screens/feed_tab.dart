import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/models.dart';
import '../state/auth_state.dart';
import '../widgets/post_card.dart';
import 'post_detail_screen.dart';

class FeedTab extends StatefulWidget {
  const FeedTab({super.key});

  @override
  State<FeedTab> createState() => FeedTabState();
}

class FeedTabState extends State<FeedTab> {
  late Future<List<Post>> _future;

  @override
  void initState() {
    super.initState();
    _future = _load();
  }

  Future<List<Post>> _load() {
    return context.read<AuthState>().api.getPosts();
  }

  void refresh() {
    setState(() => _future = _load());
  }

  @override
  Widget build(BuildContext context) {
    return RefreshIndicator(
      onRefresh: () async => refresh(),
      child: FutureBuilder<List<Post>>(
        future: _future,
        builder: (context, snapshot) {
          if (snapshot.connectionState == ConnectionState.waiting) {
            return const Center(child: CircularProgressIndicator());
          }
          if (snapshot.hasError) {
            return _Message(
              icon: Icons.cloud_off,
              text: 'Could not load posts.\n${snapshot.error}',
              onRetry: refresh,
            );
          }
          final posts = snapshot.data ?? [];
          if (posts.isEmpty) {
            return const _Message(
              icon: Icons.article_outlined,
              text: 'No posts yet.\nPull down to refresh.',
            );
          }
          return ListView.builder(
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            itemCount: posts.length,
            itemBuilder: (context, i) {
              final post = posts[i];
              return PostCard(
                post: post,
                onTap: () async {
                  await Navigator.of(context).push(
                    MaterialPageRoute(
                        builder: (_) => PostDetailScreen(postId: post.id)),
                  );
                  refresh();
                },
              );
            },
          );
        },
      ),
    );
  }
}

class _Message extends StatelessWidget {
  final IconData icon;
  final String text;
  final VoidCallback? onRetry;

  const _Message({required this.icon, required this.text, this.onRetry});

  @override
  Widget build(BuildContext context) {
    return ListView(
      children: [
        const SizedBox(height: 120),
        Icon(icon, size: 56, color: Theme.of(context).colorScheme.outline),
        const SizedBox(height: 16),
        Text(text, textAlign: TextAlign.center),
        if (onRetry != null) ...[
          const SizedBox(height: 16),
          Center(
            child: OutlinedButton(onPressed: onRetry, child: const Text('Retry')),
          ),
        ],
      ],
    );
  }
}
