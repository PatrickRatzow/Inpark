import "dart:io";

import "package:collection_ext/all.dart";
import "package:device_preview/device_preview.dart";
import "package:firebase_core/firebase_core.dart";
import "package:flutter/foundation.dart";
import "package:flutter/material.dart";
import "package:flutter/services.dart";
import "package:flutter_app/sdui/transformers/component.dart";
import "package:flutter_app/transformers/conservation_status.dart";
import "package:flutter_app/transformers/pre/hook_transformer.dart";
import "package:flutter_hooks/flutter_hooks.dart";
import "package:google_fonts/google_fonts.dart";
import "package:intl/date_symbol_data_local.dart";
import "package:plugin/plugin_manager.dart";
import "package:provider/provider.dart";
import "package:tenants/tenant_plugin.dart";
import "package:tenants/ui/start_screen.dart";

import "common/ioc.dart";
import "features/animals/models/animals_model.dart";
import "features/calendar/models/calendar_model.dart";
import "features/home/models/home_model.dart";
import "features/park_events/models/event_model.dart";
import "features/park_map/models/park_map_model.dart";
import "features/speaks/models/notification_service.dart";
import "features/speaks/models/speak_model.dart";
import "firebase_options.dart";
import "log_in/models/user_model.dart";
import "navigation/navigation_model.dart";
import "sdui/transformers/transformer.dart";
import "transformers/navbar.dart";
import "transformers/screen_app_bar.dart";

// ...

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );
  HttpOverrides.global = MyHttpOverrides();

  setupIoC();
  if (kReleaseMode) {
    initializeDateFormatting();
  }

  TenantPlugin()..init();

  SystemChrome.setSystemUIOverlayStyle(
    const SystemUiOverlayStyle(
      statusBarColor: Colors.transparent,
    ),
  );

  await NotificationModel().init();

  Transformer.preTransformers.add(HookPreTransformer());
  Transformer.transformers.add(ScreenAppBarTransformer());
  Transformer.transformers.add(NavbarTransformer());
  Transformer.transformers.add(ConservationStatusTransformer());
  ComponentTransformer.registerComponent(
    "AnimalScreenImage",
    """
<Column>
  <GestureDetector on-tap="fullscreenImage">
    <ActionData><![CDATA[{ "previewImageUrl": "\${{ previewImage }}", "fullscreenImageUrl": "\${{ fullscreenImage }}", "tag": "\${{ tag }}", "title": "\${{ title }}", "hide": true }]]></ActionData>
    <Column>
      <Stack>
        <Image 
          src="\${{ previewImage }}" 
        />
        <Padding left="9" top="6" right="9" bottom="6">
          <Container padding="6,6,6,6">
            <Decoration border-radius="circular(4)">
              <Color>#ffECFCE5</Color>
            </Decoration>
            <Text
                style="bodyLarge"
                size="10"
                height="0.95"
                color="#ff198155"
              >
                \${{ category }}
              </Text>
          </Container>
        </Padding>
        <Positioned fill="true">
          <Row>
            <Expanded>
              <Container>
                <Decoration>
                  <LinearGradient begin="bottomCenter" end="center">
                    <Color>#ff698665</Color>
                    <Color>#00698665</Color>
                  </LinearGradient>
                </Decoration>
              </Container>
            </Expanded>
          </Row>
        </Positioned>
      </Stack>
      <Container>
        <Decoration>
          <Color>#ff698665</Color>
        </Decoration>
        <Padding all="8">
          <Column>
            <Align alignment="centerLeft">
              <Text 
                style="headlineMedium"
                height="0.9"
                weight="bold"
                color="#ffddf8da"
              >
                \${{ displayName }} 
              </Text>
            </Align>
            <Padding bottom="5" />
            <Align alignment="centerLeft">
              <Text 
                style="bodyMedium"
                height="1.5"
                color="#ffddf8da"
              >
                \${{ latinName }} 
              </Text>
            </Align>
            <Padding bottom="4" />
          </Column>
        </Padding>
      </Container>
    </Column>
  </GestureDetector>
</Column>
    """,
  );
  ComponentTransformer.registerComponent(
    "AnimalScreen",
    """
<Scaffold>
  <AppBar title="\${{ displayName }}" />
  <Body>
    <Column>
      <AnimalScreenImage
      
      />
    </Column>
  </Body>
</Scaffold>
""",
  );
  ComponentTransformer.registerComponent(
    "AnimalCard",
    """
<AspectRatio ratio="2.4676">
  <Card border-radius="circular(6)" clip="antiAlias">
    <Stack>
      <Positioned fill="true">
        <Image
          fit="cover"
          src="\${{ src }}"
          blend-mode="darken"
        />
      </Positioned> 
      <Positioned fill="true">
        <Row>
          <Expanded>
            <Container>
              <Decoration>
                <LinearGradient begin="topCenter" end="bottomCenter">
                  <Color>#87000000</Color>
                  <Color>#00000000</Color>
                </LinearGradient>
              </Decoration>
            </Container>
          </Expanded>
        </Row>
      </Positioned>
      <Positioned fill="true">
        <Padding all="8">
          <Column main-axis-alignment="spaceBetween" main-axis-size="max">
            <Column>
              <Align alignment="centerLeft">
                <Text 
                  color="#ffffffff" 
                  style="headlineMedium" 
                  weight="bold"
                >
                  \${{ title }}
                </Text>
              </Align>
              <Align alignment="centerLeft">
                <Text 
                  color="#ffffffff" 
                  style="bodyMedium" 
                  weight="w500"
                  height="1.5"
                >
                  \${{ subTitle }}
                </Text>
              </Align>
            </Column>
            <Row>
              <Container padding="6,4,6,4">
                <Decoration border-radius="circular(4)">
                  <Color>#FFECFCE5</Color>
                </Decoration>
                <Text style="bodyLarge" color="#FF198155" size="8" height="0.9375">
                  \${{ category }}
                </Text>
              </Container>
            </Row>
          </Column>
        </Padding>
      </Positioned>
    </Stack>
  </Card>
</AspectRatio>
""",
  );

  runApp(
    DevicePreview(
      enabled: !kReleaseMode,
      builder: (context) => const MyApp(),
    ),
  );
}

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}

class MyApp extends HookWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    var pluginManager = PluginManager();
    var providers =
        pluginManager.plugins.flatMap((e) => e.registerProviders()).toList();
    return MultiProvider(
      providers: [
        ChangeNotifierProvider<AnimalsModel>(
          create: (context) => AnimalsModel(),
        ),
        ChangeNotifierProvider<HomeModel>(
          create: (context) => HomeModel(),
        ),
        ChangeNotifierProvider<ParkEventModel>(
          create: (context) => ParkEventModel(),
        ),
        ChangeNotifierProvider<SpeakModel>(
          create: (context) => SpeakModel(),
        ),
        ChangeNotifierProvider<CalendarModel>(
          create: (context) {
            final time = DateTime.now();

            return CalendarModel(time);
          },
        ),
        ChangeNotifierProvider<NavigationModel>(
          create: (context) => NavigationModel(),
        ),
        ChangeNotifierProvider<ParkMapModel>(
          create: (context) => ParkMapModel(),
        ),
        ChangeNotifierProvider<UserModel>(
          create: (context) => UserModel(),
        ),
        ...providers
      ],
      child: MaterialApp(
        useInheritedMediaQuery: true,
        locale: DevicePreview.locale(context),
        builder: DevicePreview.appBuilder,
        debugShowCheckedModeBanner: false,
        //NavigationScreen() or StartScreen(),
        home: const StartScreen(),
        theme: ThemeData(
          brightness: Brightness.light,
          primaryColor: const Color(0xffECFCE5),
          primarySwatch: Colors.green,
          textTheme: TextTheme(
            displayLarge: GoogleFonts.poppins(
              color: Colors.black,
              fontSize: 48,
              fontWeight: FontWeight.bold,
            ),
            displayMedium: GoogleFonts.poppins(
              color: Colors.black,
              fontSize: 40,
              fontWeight: FontWeight.bold,
            ),
            displaySmall: GoogleFonts.poppins(
              color: Colors.black,
              fontSize: 32,
              fontWeight: FontWeight.bold,
            ),
            headlineLarge: GoogleFonts.poppins(
              color: Colors.black,
              fontSize: 24,
              fontWeight: FontWeight.bold,
            ),
            headlineMedium: GoogleFonts.poppins(
              color: Colors.black,
              fontSize: 18,
            ),
            headlineSmall: GoogleFonts.poppins(
              color: Colors.black,
              fontSize: 16,
            ),
            bodyLarge: GoogleFonts.poppins(
              fontSize: 14,
              color: Colors.black,
            ),
            bodyMedium: GoogleFonts.poppins(
              fontSize: 12,
              color: Colors.black,
            ),
            bodySmall: GoogleFonts.poppins(
              fontSize: 10,
              color: Colors.black,
            ),
          ),
          scaffoldBackgroundColor: Colors.white,
        ),
      ),
    );
  }
}
