import 'package:flutter/material.dart';
import 'dart:io' show Platform;

class ScreenAppBar extends StatelessWidget implements PreferredSizeWidget {
  final String? name;
  final List<Widget>? actions;

  const ScreenAppBar({
    Key? key,
    this.name,
    this.actions,
  }) : super(key: key);

  @override
  Size get preferredSize => getToolbarSize();

  bool get isAndroid => Platform.isAndroid;

  @override
  Widget build(BuildContext context) {
    if (isAndroid) {
      return AppBar(
        backgroundColor: Colors.white,
        elevation: 0,
        leading: IconButton(
          icon: const Icon(
            Icons.arrow_back,
            color: Colors.black,
          ),
          onPressed: () => Navigator.of(context).pop(),
        ),
        centerTitle: true,
        title: name != null
            ? Text(
                name!,
                style: const TextStyle(
                  color: Colors.black,
                  fontSize: 20,
                  fontFamily: 'Poppins',
                ),
              )
            : null,
        actions: actions,
      );
    } else {
      return AppBar(
        backgroundColor: Colors.white,
        elevation: 0,
        bottom: PreferredSize(
          child: Padding(
            padding: const EdgeInsets.fromLTRB(16, 0, 16, 0),
            child: name != null
                ? Align(
                    child: Text(
                      name!,
                      textAlign: TextAlign.left,
                      style: const TextStyle(
                        color: Colors.black,
                        fontSize: 32,
                        fontWeight: FontWeight.bold,
                        fontFamily: 'Poppins',
                      ),
                    ),
                    alignment: Alignment.centerLeft,
                  )
                : null,
          ),
          preferredSize: const Size(0, 0),
        ),
        leading: Padding(
          padding: const EdgeInsets.fromLTRB(11, 0, 11, 0),
          child: IconButton(
            icon: const Icon(
              Icons.arrow_back_ios,
              color: Colors.black,
              size: 16,
            ),
            onPressed: () => Navigator.of(context).pop(),
            alignment: Alignment.centerLeft,
          ),
        ),
        actions: actions,
      );
    }
  }

  Size getToolbarSize() {
    if (isAndroid) {
      return const Size.fromHeight(48);
    } else {
      double size = 100;
      if (name == null) {
        size -= 52;
      }

      return Size.fromHeight(size);
    }
  }
}