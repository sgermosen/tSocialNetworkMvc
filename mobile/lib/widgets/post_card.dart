import 'package:flutter/material.dart';

import '../models/models.dart';
import '../utils/format.dart';
import 'avatar.dart';

class PostCard extends StatelessWidget {
  final Post post;
  final VoidCallback? onTap;

  const PostCard({super.key, required this.post, this.onTap});

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return Card(
      child: InkWell(
        borderRadius: BorderRadius.circular(16),
        onTap: onTap,
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  InitialAvatar(name: post.authorName, size: 40),
                  const SizedBox(width: 12),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          post.authorName,
                          style: const TextStyle(fontWeight: FontWeight.w700),
                        ),
                        Text(
                          formatDate(post.date),
                          style: theme.textTheme.bodySmall?.copyWith(
                            color: theme.colorScheme.onSurfaceVariant,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
              if ((post.name ?? '').isNotEmpty) ...[
                const SizedBox(height: 12),
                Text(
                  post.name!,
                  style: theme.textTheme.titleMedium?.copyWith(
                    fontWeight: FontWeight.w700,
                    color: theme.colorScheme.primary,
                  ),
                ),
              ],
              const SizedBox(height: 8),
              Text(post.body),
              const SizedBox(height: 12),
              Divider(color: theme.colorScheme.outlineVariant, height: 1),
              const SizedBox(height: 8),
              Row(
                children: [
                  Icon(Icons.mode_comment_outlined,
                      size: 18, color: theme.colorScheme.onSurfaceVariant),
                  const SizedBox(width: 6),
                  Text(
                    '${post.comments.length} comment${post.comments.length == 1 ? '' : 's'}',
                    style: theme.textTheme.bodySmall?.copyWith(
                      color: theme.colorScheme.onSurfaceVariant,
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
