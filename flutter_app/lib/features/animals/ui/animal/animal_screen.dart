import "package:cached_network_image/cached_network_image.dart";
import "package:flutter/material.dart";
import "package:flutter_use/flutter_use.dart";
import "../../../../common/colors.dart";
import "../../../../common/extensions/theme.dart";
import "../../../../common/screen.dart";
import "../../../../common/ui/fullscreen_image.dart";
import "../../../../common/ui/screen_app_bar.dart";
import "../../models/animal.dart";
import "../../models/animals_model.dart";
import "../conservation/conservation_status.dart";
import "../../../../generated_code/zooinator.swagger.dart";
import "../../../../hooks/hooks.dart";
import "../../../../navigation/navigation_model.dart";
import "package:flutter_hooks/flutter_hooks.dart";

import "animal_category.dart" as AnimalCategory;

class AnimalScreen extends HookWidget implements Screen {
  final Animal animal;

  const AnimalScreen({
    super.key,
    required this.animal,
  });

  @override
  Widget build(BuildContext context) {
    final navigation = useNavigator();
    final theme = useTheme();
    final model = useProvider<AnimalsModel>(watch: true);

    useEffectOnce(() {
      model.fetchAnimalAreas(animal.latinName);

      return null;
    });

    return Scaffold(
      body: CustomScrollView(
        slivers: [
          SliverAppBar(
            pinned: false,
            snap: false,
            automaticallyImplyLeading: false,
            elevation: 0,
            backgroundColor: Colors.white,
            foregroundColor: Colors.white,
            floating: true,
            flexibleSpace: ScreenAppBar(
              title: animal.displayName,
            ),
          ),
          SliverList(
            delegate: SliverChildBuilderDelegate(
              (context, index) {
                if (index != 0) return null;

                return _buildCard(context, animal, navigation, theme, model);
              },
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildCard(
    BuildContext context,
    Animal animal,
    NavigationModel navigation,
    ThemeData theme,
    AnimalsModel model,
  ) {
    return Column(
      children: [
        GestureDetector(
          onTap: () => {
            navigation.push(
              context,
              FullScreenImage(
                imageUrl: animal.fullscreenImageUrl ?? animal.previewImageUrl,
                tag: "animal-${animal.id}",
                title: animal.displayName,
              ),
            ),
          },
          child: _buildImage(animal, theme),
        ),
        /*
        ZooinatorNavigationBar(
          tabs: [
            ZooinatorNavigationTab(
              text: "Information",
              icon: Icons.menu,
              builder: (context) => Column(
                children: [
                  InkWell(
                    onTap: () => navigation.push(
                      context,
                      ConservationStatusOverviewScreen(
                        highlightedStatus: animal.status,
                      ),
                    ),
                    child: _buildConservationStatus(animal),
                  ),
                  Padding(
                    padding: const EdgeInsets.fromLTRB(16, 0, 16, 16),
                    child: _buildContent(animal.contents[0], theme),
                  ),
                ],
              ),
            ),
            ZooinatorNavigationTab(
              text: "Oversigt",
              icon: Icons.dashboard,
              builder: (context) => Padding(
                padding: const EdgeInsets.fromLTRB(16, 8, 16, 8),
                child: _buildTriviaContent(animal.contents[1], theme),
              ),
            ),
            if (animal.hasMap)
              ZooinatorNavigationTab(
                text: "Kort",
                icon: Icons.map,
                builder: (context) => AnimalMap(
                  data: model.getAnimalAreas(animal.name.latinName) ?? [],
                ),
              ),
          ],
        ),
        */
      ],
    );
  }

  Widget _buildTriviaContent(ContentDto content, ThemeData theme) {
    var items = content.children.first.children
        .where((child) => child.type == "listitem");

    var i = 0;
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: items.map(
        (item) {
          i++;
          final split = item.children.first.value.split(": ");
          final title = split[0];
          final body = split.skip(1).join(": ");

          return Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                title,
                style: theme.textTheme.bodyMedium?.copyWith(
                  height: 18 / 10,
                  color: const Color(0xff7C7C7C),
                ),
              ),
              Text(
                body,
                style: theme.textTheme.bodyLarge?.copyWith(
                  color: Colors.black,
                  height: 18 / 14,
                ),
              ),
              if (i != items.length) const SizedBox(height: 16),
            ],
          );
        },
      ).toList(),
    );

    /*
    if (content.type == "list") {
      var children =
          content.children.where((child) => child.type == "listitem");

      return BulletList(children: children.map(_buildContent).toList());
    }
    if (content.type == "listitem") {
      return Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: content.children.map(_buildContent).toList(),
      );
    }
    */
  }

  Widget _buildConservationStatus(Animal animal) {
    if (animal.conservationStatus == null) {
      return Container();
    }

    return ConservationStatus(activeStatus: animal.conservationStatus!);
  }

  Widget _buildImage(Animal animal, ThemeData theme) {
    const Color softTextColor = Color(0xffDDF8DA);

    return Column(
      children: [
        Stack(
          children: [
            CachedNetworkImage(imageUrl: animal.previewImageUrl),
            Padding(
              padding: const EdgeInsets.fromLTRB(9, 6, 9, 6),
              child: AnimalCategory.AnimalCategory(
                fontSize: 10,
                text: animal.category,
                padding: const EdgeInsets.all(6),
              ),
            ),
            Positioned.fill(
              child: Row(
                children: [
                  Expanded(
                    child: Container(
                      decoration: BoxDecoration(
                        gradient: LinearGradient(
                          begin: Alignment.bottomCenter,
                          end: Alignment.center,
                          colors: [
                            CustomColor.green.middle,
                            CustomColor.green.middle.withOpacity(0),
                          ],
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            )
          ],
        ),
        Container(
          color: CustomColor.green.middle,
          child: Padding(
            padding: const EdgeInsets.all(8.0),
            child: Column(
              children: [
                Align(
                  alignment: Alignment.centerLeft,
                  child: Text(
                    animal.displayName,
                    style: theme.textTheme.headlineMedium?.copyWith(
                      height: 18 / 20,
                      fontWeight: FontWeight.bold,
                      color: softTextColor,
                    ),
                  ),
                ),
                const SizedBox(
                  height: 5,
                ),
                Align(
                  alignment: Alignment.centerLeft,
                  child: Text(
                    animal.latinName,
                    style: theme.textTheme.bodyMedium?.copyWith(
                      height: 1.5,
                      color: softTextColor,
                    ),
                  ),
                ),
                const Padding(
                  padding: EdgeInsets.only(bottom: 4),
                ),
              ],
            ),
          ),
        )
      ],
    );
  }

  Widget _buildContents(List<ContentDto> contents, ThemeData theme) {
    return Column(
      children: contents.map((x) => _buildContent(x, theme)).toList(),
    );
  }

  Widget _buildContent(ContentDto content, ThemeData theme) {
    if (content.type == "container") {
      return Column(
        children: content.children.map((x) => _buildContent(x, theme)).toList(),
      );
    }
    if (content.type == "text") {
      final textColor = theme.adjustColor(
        light: Colors.black,
        dark: Colors.white,
      );

      return Align(
        alignment: Alignment.centerLeft,
        child: Text(
          content.value.toString(),
          textAlign: TextAlign.left,
          softWrap: true,
          style: theme.textTheme.bodyLarge?.copyWith(
            color: textColor.withOpacity(0.6),
            height: 1.35,
          ),
        ),
      );
    }
    if (content.type == "spacer") {
      return const SizedBox(height: 4);
    }
    /*if (content.type == "image") {
      return CachedNetworkImage(
        imageUrl: content.value.toString(),
      );
    }*/

    return Container();
  }
}
