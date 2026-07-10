import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/models.dart';
import '../state/auth_state.dart';
import '../utils/format.dart';

class NotificationsScreen extends StatefulWidget {
  const NotificationsScreen({super.key});

  @override
  State<NotificationsScreen> createState() => _NotificationsScreenState();
}

class _NotificationsScreenState extends State<NotificationsScreen> {
  late Future<List<AppNotification>> _future;

  @override
  void initState() {
    super.initState();
    _future = _load();
  }

  Future<List<AppNotification>> _load() {
    return context.read<AuthState>().api.getNotifications();
  }

  void _refresh() => setState(() => _future = _load());

  Future<void> _markAllRead() async {
    await context.read<AuthState>().api.markAllNotificationsRead();
    _refresh();
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return Scaffold(
      appBar: AppBar(
        title: const Text('Notifications'),
        actions: [
          TextButton(
            onPressed: _markAllRead,
            child: const Text('Mark all read'),
          ),
        ],
      ),
      body: RefreshIndicator(
        onRefresh: () async => _refresh(),
        child: FutureBuilder<List<AppNotification>>(
          future: _future,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const Center(child: CircularProgressIndicator());
            }
            if (snapshot.hasError) {
              return ListView(children: [
                const SizedBox(height: 120),
                Center(child: Text('${snapshot.error}')),
              ]);
            }
            final items = snapshot.data ?? [];
            if (items.isEmpty) {
              return ListView(children: [
                const SizedBox(height: 120),
                Icon(Icons.notifications_none,
                    size: 56, color: theme.colorScheme.outline),
                const SizedBox(height: 16),
                const Text('No notifications yet.', textAlign: TextAlign.center),
              ]);
            }
            return ListView.separated(
              itemCount: items.length,
              separatorBuilder: (_, __) => const Divider(height: 1),
              itemBuilder: (context, i) {
                final n = items[i];
                return ListTile(
                  leading: CircleAvatar(
                    backgroundColor: n.isRead
                        ? theme.colorScheme.surfaceContainerHighest
                        : theme.colorScheme.primaryContainer,
                    child: Icon(Icons.notifications,
                        color: theme.colorScheme.primary, size: 20),
                  ),
                  title: Text(n.message),
                  subtitle: Text(formatDate(n.createdAt)),
                  tileColor: n.isRead
                      ? null
                      : theme.colorScheme.primaryContainer.withValues(alpha: 0.25),
                );
              },
            );
          },
        ),
      ),
    );
  }
}
