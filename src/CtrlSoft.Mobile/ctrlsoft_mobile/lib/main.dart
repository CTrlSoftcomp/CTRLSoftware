import 'package:ctrlsoft_mobile/Screens/Splash/splash_screen.dart';
import 'package:ctrlsoft_mobile/constants.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'VPoint Mobile',
      theme: ThemeData(
        primaryColor: kPrimaryColor,
        scaffoldBackgroundColor: kColorWhite,
        primaryColorLight: kPrimaryLightColor,
        primaryColorDark: kPrimaryDarkColor,
        accentColor: Colors.grey[600],

        // Define the default font family.
        fontFamily: 'Roboto',

        primaryTextTheme: TextTheme(
          headline1: TextStyle(fontSize: 72.0, fontWeight: FontWeight.bold),
          headline6: TextStyle(fontSize: 36.0, fontStyle: FontStyle.italic),
          bodyText2: TextStyle(fontSize: 14.0, fontFamily: 'Hind'),
        ),
      ),
      home: SplashScreen(),
    );
  }
}
