import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../models/models.dart';
import '../state/auth_state.dart';
import '../widgets/avatar.dart';

class ProfileTab extends StatefulWidget {
  const ProfileTab({super.key});

  @override
  State<ProfileTab> createState() => _ProfileTabState();
}

class _ProfileTabState extends State<ProfileTab> {
  late Future<UserProfile> _future;

  @override
  void initState() {
    super.initState();
    _future = context.read<AuthState>().api.me();
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    final auth = context.read<AuthState>();
    return FutureBuilder<UserProfile>(
      future: _future,
      builder: (context, snapshot) {
        final name = snapshot.data?.fullName ?? auth.displayName ?? '';
        return ListView(
          padding: const EdgeInsets.all(24),
          children: [
            Center(child: InitialAvatar(name: name, size: 96)),
            const SizedBox(height: 16),
            Center(
              child: Text(name,
                  style: theme.textTheme.headlineSmall
                      ?.copyWith(fontWeight: FontWeight.w700)),
            ),
            if (snapshot.hasData) ...[
              const SizedBox(height: 4),
              Center(
                child: Text(snapshot.data!.email,
                    style: theme.textTheme.bodyMedium?.copyWith(
                        color: theme.colorScheme.onSurfaceVariant)),
              ),
              const SizedBox(height: 24),
              if ((snapshot.data!.phone ?? '').isNotEmpty)
                _InfoRow(icon: Icons.phone, text: snapshot.data!.phone!),
              if ((snapshot.data!.bio ?? '').isNotEmpty)
                _InfoRow(icon: Icons.info_outline, text: snapshot.data!.bio!),
            ],
            const SizedBox(height: 32),
            OutlinedButton.icon(
              onPressed: () => auth.logout(),
              icon: const Icon(Icons.logout),
              label: const Text('Log out'),
              style: OutlinedButton.styleFrom(
                minimumSize: const Size.fromHeight(50),
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(12)),
              ),
            ),
          ],
        );
      },
    );
  }
}

class _InfoRow extends StatelessWidget {
  final IconData icon;
  final String text;

  const _InfoRow({required this.icon, required this.text});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8),
      child: Row(
        children: [
          Icon(icon, size: 20, color: Theme.of(context).colorScheme.primary),
          const SizedBox(width: 12),
          Expanded(child: Text(text)),
        ],
      ),
    );
  }
}
