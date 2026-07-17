import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';

import 'package:tetas_mobile/main.dart';

const _email = String.fromEnvironment('SHOT_EMAIL');
const _password = String.fromEnvironment('SHOT_PASSWORD');

void main() {
  final binding = IntegrationTestWidgetsFlutterBinding.ensureInitialized();

  Future<void> shot(WidgetTester tester, String name) async {
    await tester.pumpAndSettle();
    try {
      await binding.convertFlutterSurfaceToImage();
    } catch (_) {}
    await tester.pumpAndSettle();
    await binding.takeScreenshot(name);
  }

  Future<void> tapText(WidgetTester tester, String text) async {
    final finder = find.text(text);
    if (finder.evaluate().isNotEmpty) {
      await tester.tap(finder.first);
      await tester.pumpAndSettle(const Duration(seconds: 2));
    }
  }

  testWidgets('capture store screenshots', (tester) async {
    await tester.pumpWidget(const TetasApp());
    await tester.pumpAndSettle(const Duration(seconds: 2));

    await shot(tester, '1-login');

    if (_email.isEmpty || _password.isEmpty) {
      return;
    }

    final fields = find.byType(TextFormField);
    if (fields.evaluate().length >= 2) {
      await tester.enterText(fields.at(0), _email);
      await tester.enterText(fields.at(1), _password);
      await tester.testTextInput.receiveAction(TextInputAction.done);
      await tester.pumpAndSettle();
      await tapText(tester, 'Log in');
      await tester.pumpAndSettle(const Duration(seconds: 3));
    }

    await shot(tester, '2-feed');

    await tapText(tester, 'Groups');
    await shot(tester, '3-groups');

    await tapText(tester, 'Profile');
    await shot(tester, '4-profile');

    final bell = find.byIcon(Icons.notifications_outlined);
    if (bell.evaluate().isNotEmpty) {
      await tester.tap(bell.first);
      await tester.pumpAndSettle(const Duration(seconds: 2));
      await shot(tester, '5-notifications');
    }
  });
}
