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
      child: Center(
        child: Image.asset(
          "assets/images/ctrl_soft.png",
          width: size.height * 0.4,
          height: size.height * 0.4,
          color: kPrimaryDarkColor,
        ),
      ),
    );
  }
}
