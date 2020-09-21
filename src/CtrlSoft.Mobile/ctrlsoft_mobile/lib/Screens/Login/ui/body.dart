import 'package:flutter/material.dart';
import 'package:ctrlsoft_mobile/Screens/Login/ui/background.dart';
import 'package:ctrlsoft_mobile/components/rounded_button.dart';
import 'package:ctrlsoft_mobile/components/rounded_input_field.dart';
import 'package:ctrlsoft_mobile/components/rounded_password_field.dart';
import 'package:ctrlsoft_mobile/constants.dart';

class Body extends StatelessWidget {
  const Body({
    Key key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return StateBody();
  }
}

class StateBody extends StatefulWidget {
  StateBody({
    Key key,
  }) : super(key: key);

  @override
  MyState createState() => MyState();
}

class MyState extends State<StateBody> {
  BuildContext buildContext;

  @override
  void initState() {
    super.initState();
  }

  void snackBar(String text) {
    final snackBar = SnackBar(content: Text(text));
    Scaffold.of(buildContext).showSnackBar(snackBar);
  }

  void loginPress() {
    snackBar("Sik Sabar Bosskue");
  }

  @override
  Widget build(BuildContext context) {
    buildContext = context;
    Size size = MediaQuery.of(context).size;
    return Background(
      child: SingleChildScrollView(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Image.asset(
              "assets/icons/logo.png",
              width: size.height * 0.3,
              height: size.height * 0.3,
            ),
            SizedBox(height: size.height * 0.03),
            RoundedInputField(
              hintText: "User ID",
              onChanged: (value) {},
              iconColor: kColorWhite,
              iconFront: Icons.person,
            ),
            RoundedPasswordField(
              hintText: "Password",
              onChanged: (value) {},
              iconColor: kColorWhite,
              iconFront: Icons.lock,
            ),
            RoundedButton(
              color: kPrimaryDarkColor,
              text: "LOGIN",
              press: () {
                loginPress();
              },
            ),
          ],
        ),
      ),
    );
  }
}
