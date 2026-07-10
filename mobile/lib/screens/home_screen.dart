import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../state/auth_state.dart';
import 'feed_tab.dart';
import 'groups_tab.dart';
import 'profile_tab.dart';
import 'create_post_screen.dart';
import 'notifications_screen.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _index = 0;
  final _feedKey = GlobalKey<FeedTabState>();

  late final List<Widget> _tabs = [
    FeedTab(key: _feedKey),
    const GroupsTab(),
    const ProfileTab(),
  ];

  static const _titles = ['Latest posts', 'Groups', 'Profile'];

  Future<void> _createPost() async {
    final created = await Navigator.of(context).push<bool>(
      MaterialPageRoute(builder: (_) => const CreatePostScreen()),
    );
    if (created == true) {
      _feedKey.currentState?.refresh();
      setState(() => _index = 0);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(_titles[_index]),
        actions: const [NotificationBell()],
      ),
      body: IndexedStack(index: _index, children: _tabs),
      floatingActionButton: _index == 0
          ? FloatingActionButton.extended(
              onPressed: _createPost,
              icon: const Icon(Icons.add),
              label: const Text('New post'),
            )
          : null,
      bottomNavigationBar: NavigationBar(
        selectedIndex: _index,
        onDestinationSelected: (i) => setState(() => _index = i),
        destinations: const [
          NavigationDestination(
              icon: Icon(Icons.home_outlined),
              selectedIcon: Icon(Icons.home),
              label: 'Feed'),
          NavigationDestination(
              icon: Icon(Icons.groups_outlined),
              selectedIcon: Icon(Icons.groups),
              label: 'Groups'),
          NavigationDestination(
              icon: Icon(Icons.person_outline),
              selectedIcon: Icon(Icons.person),
              label: 'Profile'),
        ],
      ),
    );
  }
}

class NotificationBell extends StatefulWidget {
  const NotificationBell({super.key});

  @override
  State<NotificationBell> createState() => _NotificationBellState();
}

class _NotificationBellState extends State<NotificationBell> {
  int _unread = 0;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    try {
      final count = await context.read<AuthState>().api.unreadCount();
      if (mounted) setState(() => _unread = count);
    } catch (_) {}
  }

  Future<void> _open() async {
    await Navigator.of(context).push(
      MaterialPageRoute(builder: (_) => const NotificationsScreen()),
    );
    _load();
  }

  @override
  Widget build(BuildContext context) {
    return IconButton(
      onPressed: _open,
      icon: _unread > 0
          ? Badge(
              label: Text('$_unread'),
              child: const Icon(Icons.notifications_outlined),
            )
          : const Icon(Icons.notifications_outlined),
    );
  }
}
