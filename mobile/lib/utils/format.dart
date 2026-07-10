import 'package:intl/intl.dart';

String formatDate(DateTime date) {
  final local = date.toLocal();
  final now = DateTime.now();
  final diff = now.difference(local);

  if (diff.inMinutes < 1) {
    return 'just now';
  }
  if (diff.inMinutes < 60) {
    return '${diff.inMinutes}m ago';
  }
  if (diff.inHours < 24) {
    return '${diff.inHours}h ago';
  }
  if (diff.inDays < 7) {
    return '${diff.inDays}d ago';
  }
  return DateFormat('MMM d, yyyy').format(local);
}
