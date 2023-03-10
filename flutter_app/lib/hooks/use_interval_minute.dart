part of "hooks.dart";

void useIntervalMinute() {
  final now = DateTime.now();
  final nextMinute = DateTime(
    now.year,
    now.month,
    now.day,
    now.hour,
    now.minute,
    0,
  ).add(const Duration(minutes: 1));
  final nowState = useState(now);
  useInterval(
    () => nowState.value = DateTime.now(),
    nextMinute.difference(nowState.value),
  );
}
