#!/usr/bin/env bash
set -euo pipefail

# Captures store screenshots by driving the real app on a connected device
# or a running emulator/simulator, and writes PNGs to mobile/screenshots/.
#
# Requirements:
#   - A device/emulator/simulator running (check with: flutter devices)
#   - The Tetas API reachable from the device
#   - A demo account with some content (posts, groups, notifications)
#
# Usage:
#   API=https://api.tudominio.com \
#   EMAIL=demo@example.com PASSWORD=Password1 \
#   ./tools/take_screenshots.sh

API="${API:-http://10.0.2.2:5000}"
EMAIL="${EMAIL:-}"
PASSWORD="${PASSWORD:-}"

cd "$(dirname "$0")/.."

flutter drive \
  --driver=test_driver/integration_test.dart \
  --target=integration_test/screenshots_test.dart \
  --dart-define=TETAS_API_BASE_URL="$API" \
  --dart-define=SHOT_EMAIL="$EMAIL" \
  --dart-define=SHOT_PASSWORD="$PASSWORD"

echo "Screenshots written to: $(pwd)/screenshots"
