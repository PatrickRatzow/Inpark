import "package:flutter/material.dart";
import "colors.dart";
import "package:flutter_custom_tabs/flutter_custom_tabs.dart";

class Browser {
  static bool _isOpening = false;

  static Future openUrl(BuildContext context, String url) async {
    if (_isOpening) return;

    try {
      _isOpening = true;

      await launch(
        url,
        customTabsOption: CustomTabsOption(
          toolbarColor: CustomColor.green.lightest,
          enableDefaultShare: true,
          enableUrlBarHiding: true,
          showPageTitle: true,
          extraCustomTabs: const <String>[
            // ref. https://play.google.com/store/apps/details?id=org.mozilla.firefox
            "org.mozilla.firefox",
            // ref. https://play.google.com/store/apps/details?id=com.microsoft.emmx
            "com.microsoft.emmx",
          ],
        ),
        safariVCOption: SafariViewControllerOption(
          preferredBarTintColor: Theme.of(context).primaryColor,
          preferredControlTintColor: Colors.white,
          barCollapsingEnabled: true,
          entersReaderIfAvailable: false,
          dismissButtonStyle: SafariViewControllerDismissButtonStyle.close,
        ),
      );
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text("Kunne ikke tilgå billetsiden på nuværende tidspunkt"),
        ),
      );
    } finally {
      _isOpening = false;
    }
  }
}
