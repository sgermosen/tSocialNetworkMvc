import 'dart:io';

import 'package:integration_test/integration_test_driver_extended.dart';

Future<void> main() async {
  await integrationDriver(
    onScreenshot: (String name, List<int> bytes, [Map<String, Object?>? args]) async {
      final dir = Directory('screenshots');
      if (!dir.existsSync()) {
        dir.createSync(recursive: true);
      }
      File('screenshots/$name.png').writeAsBytesSync(bytes);
      return true;
    },
  );
}
