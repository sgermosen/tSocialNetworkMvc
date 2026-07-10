import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/models.dart';
import '../state/auth_state.dart';

class GroupsTab extends StatefulWidget {
  const GroupsTab({super.key});

  @override
  State<GroupsTab> createState() => _GroupsTabState();
}

class _GroupsTabState extends State<GroupsTab> {
  late Future<List<Group>> _future;

  @override
  void initState() {
    super.initState();
    _future = _load();
  }

  Future<List<Group>> _load() {
    return context.read<AuthState>().api.getGroups();
  }

  void _refresh() => setState(() => _future = _load());

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return RefreshIndicator(
      onRefresh: () async => _refresh(),
      child: FutureBuilder<List<Group>>(
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
          final groups = snapshot.data ?? [];
          if (groups.isEmpty) {
            return ListView(children: [
              const SizedBox(height: 120),
              Icon(Icons.groups_outlined,
                  size: 56, color: theme.colorScheme.outline),
              const SizedBox(height: 16),
              const Text('No groups to show yet.',
                  textAlign: TextAlign.center),
            ]);
          }
          return ListView.builder(
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            itemCount: groups.length,
            itemBuilder: (context, i) {
              final g = groups[i];
              return Card(
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Row(
                        children: [
                          Expanded(
                            child: Text(g.name,
                                style: theme.textTheme.titleMedium
                                    ?.copyWith(fontWeight: FontWeight.w700)),
                          ),
                          if (g.isAdmin)
                            _Tag(label: 'Admin', color: theme.colorScheme.secondary)
                          else if (g.isMember)
                            _Tag(label: 'Member', color: theme.colorScheme.primary),
                        ],
                      ),
                      if ((g.typeName ?? '').isNotEmpty)
                        Text(g.typeName!,
                            style: theme.textTheme.bodySmall?.copyWith(
                                color: theme.colorScheme.onSurfaceVariant)),
                      if ((g.description ?? '').isNotEmpty) ...[
                        const SizedBox(height: 8),
                        Text(g.description!),
                      ],
                    ],
                  ),
                ),
              );
            },
          );
        },
      ),
    );
  }
}

class _Tag extends StatelessWidget {
  final String label;
  final Color color;

  const _Tag({required this.label, required this.color});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
      decoration: BoxDecoration(
        color: color.withValues(alpha: 0.15),
        borderRadius: BorderRadius.circular(999),
      ),
      child: Text(label,
          style: TextStyle(
              color: color, fontWeight: FontWeight.w700, fontSize: 12)),
    );
  }
}
