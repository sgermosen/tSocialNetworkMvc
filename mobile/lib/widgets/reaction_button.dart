import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../state/auth_state.dart';

class ReactionButton extends StatefulWidget {
  final int postId;
  final int initialCount;
  final bool initialReacted;

  const ReactionButton({
    super.key,
    required this.postId,
    required this.initialCount,
    required this.initialReacted,
  });

  @override
  State<ReactionButton> createState() => _ReactionButtonState();
}

class _ReactionButtonState extends State<ReactionButton> {
  late int _count;
  late bool _reacted;
  bool _busy = false;

  @override
  void initState() {
    super.initState();
    _count = widget.initialCount;
    _reacted = widget.initialReacted;
  }

  Future<void> _toggle() async {
    if (_busy) return;
    setState(() => _busy = true);
    try {
      final result = await context.read<AuthState>().api.react(widget.postId);
      if (!mounted) return;
      setState(() {
        _count = result.total;
        _reacted = result.reacted;
      });
    } catch (_) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Could not react right now.')),
        );
      }
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return OutlinedButton.icon(
      onPressed: _busy ? null : _toggle,
      icon: Icon(
        _reacted ? Icons.favorite : Icons.favorite_border,
        color: _reacted ? Colors.redAccent : theme.colorScheme.onSurfaceVariant,
        size: 20,
      ),
      label: Text('$_count'),
      style: OutlinedButton.styleFrom(
        shape:
            RoundedRectangleBorder(borderRadius: BorderRadius.circular(999)),
      ),
    );
  }
}
