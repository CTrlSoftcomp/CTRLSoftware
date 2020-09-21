import 'package:ctrlsoft_mobile/constants.dart';
import 'package:flutter/material.dart';
import 'package:ctrlsoft_mobile/Screens/Splash/ui/background.dart';
import 'package:ctrlsoft_mobile/Screens/Login/login_screen.dart';
import 'dart:async';

class SplashScreen extends StatefulWidget {
  @override
  SplashScreenState createState() => SplashScreenState();
}

class SplashScreenState extends State<SplashScreen> {
  @override
  void initState() {
    super.initState();
    startSplashScreen();
  }

  startSplashScreen() async {
    var duration = const Duration(seconds: 5);
    return Timer(duration, () {
      Navigator.of(context).pushReplacement(MaterialPageRoute(builder: (_) {
        return LoginScreen();
      }));
    });
  }

  @override
  Widget build(BuildContext context) {
    Size size = MediaQuery.of(context).size;
    return Background(
      child: SingleChildScrollView(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Image.asset(
              "assets/images/ctrl_soft.png",
              width: size.width * 0.5,
              height: size.width * 0.5,
              color: kPrimaryDarkColor,
            ),
            SizedBox(height: size.height * 0.01),
            Text(
              "Version 1.0",
              maxLines: 1,
              style: TextStyle(
                color: kColorWhite,
                fontSize: fontSize_Caption,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
