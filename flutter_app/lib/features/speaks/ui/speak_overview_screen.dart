import "package:flutter/material.dart";
import "package:flutter_app/common/screen.dart";
import "package:flutter_app/common/ui/screen_app_bar.dart";
import "package:flutter_app/features/speaks/models/speak.dart";
import "package:flutter_app/features/speaks/models/speak_model.dart";
import "package:flutter_app/features/speaks/ui/speaks_list.dart";
import "package:flutter_app/hooks/hooks.dart";

class SpeaksOverviewScreen extends StatelessWidget implements Screen {
  final List<Speak> speaks;

  const SpeaksOverviewScreen({
    super.key,
    required this.speaks,
  });

  @override
  Widget build(BuildContext context) {
    useProvider<SpeakModel>().fetchSpeaksForToday();

    return Scaffold(
      appBar: const ScreenAppBar(title: "Alle Aktiviteter og Speaks"),
      body: Padding(
        padding: const EdgeInsets.all(8),
        child: Column(
          children: [
            SpeaksList(speaks: speaks),
          ],
        ),
      ),
    );
  }
}
