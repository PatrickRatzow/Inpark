import "package:cached_network_image/cached_network_image.dart";
import "package:flutter/material.dart";
import "package:flutter/services.dart";
import "package:flutter_hooks/flutter_hooks.dart";

import "../screen.dart";
import "screen_app_bar.dart";

class FullScreenImage extends HookWidget implements Screen {
  final String imageUrl;
  final String tag;
  final String title;

  const FullScreenImage({
    super.key,
    required this.imageUrl,
    required this.tag,
    required this.title,
  });

  @override
  Widget build(BuildContext context) {
    final transformationController = useTransformationController();

    useEffect(
      () {
        SystemChrome.setEnabledSystemUIMode(SystemUiMode.leanBack);

        return () => SystemChrome.setEnabledSystemUIMode(
              SystemUiMode.manual,
              overlays: [SystemUiOverlay.top, SystemUiOverlay.bottom],
            );
      },
      [],
    );

    return Scaffold(
      extendBodyBehindAppBar: true,
      backgroundColor: Colors.black,
      appBar: const ScreenAppBar(
        height: 64,
        leadingColor: Colors.white,
        backgroundColor: Colors.transparent,
      ),
      body: Center(
        child: SizedBox.expand(
          child: Hero(
            tag: tag,
            child: InteractiveViewer(
              transformationController: transformationController,
              maxScale: 7,
              child: CachedNetworkImage(
                imageUrl: imageUrl,
                placeholder: (context, url) {
                  return const Center(
                    child: CircularProgressIndicator(),
                  );
                },
              ),
            ),
          ),
        ),
      ),
    );
  }
}
