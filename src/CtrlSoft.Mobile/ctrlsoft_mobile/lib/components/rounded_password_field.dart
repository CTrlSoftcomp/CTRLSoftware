import 'package:flutter/material.dart';
import 'package:ctrlsoft_mobile/components/text_field_container.dart';

class RoundedPasswordField extends StatelessWidget {
  final String hintText;
  final IconData iconFront;
  final ValueChanged<String> onChanged;
  final Color iconColor;
  final TextEditingController controller;

  const RoundedPasswordField({
    Key key,
    this.hintText,
    this.onChanged,
    this.iconFront,
    this.iconColor,
    this.controller,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return StateRoundPasswordField(
      hintText: hintText,
      onChanged: onChanged,
      iconColor: iconColor,
      iconFront: iconFront,
      controller: controller,
    );
  }
}

class StateRoundPasswordField extends StatefulWidget {
  final String hintText;
  final IconData iconFront;
  final ValueChanged<String> onChanged;
  final Color iconColor;
  final TextEditingController controller;

  StateRoundPasswordField({
    Key key,
    this.hintText,
    this.onChanged,
    this.iconFront,
    this.iconColor,
    this.controller,
  }) : super(key: key);

  @override
  MyState createState() => MyState(
        hintText: hintText,
        onChanged: onChanged,
        iconColor: iconColor,
        iconFront: iconFront,
        controller: controller,
      );
}

class MyState extends State<StateRoundPasswordField> {
  final String hintText;
  final IconData iconFront;
  final ValueChanged<String> onChanged;
  final Color iconColor;
  final TextEditingController controller;

  MyState({
    Key key,
    this.hintText,
    this.onChanged,
    this.iconFront,
    this.iconColor,
    this.controller,
  });

  bool isHidePassword = true;

  void togglePasswordVisibility() {
    setState(() {
      isHidePassword = !isHidePassword;
    });
  }

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return TextFieldContainer(
      child: TextField(
        obscureText: isHidePassword ? true : false,
        onChanged: onChanged,
        cursorColor: iconColor,
        controller: controller,
        style: TextStyle(color: iconColor),
        decoration: InputDecoration(
          hintText: hintText,
          hintStyle: TextStyle(color: iconColor),
          icon: Icon(
            iconFront,
            color: iconColor,
          ),
          suffixIcon: new GestureDetector(
              onTap: () {
                togglePasswordVisibility();
              },
              child: Icon(
                isHidePassword ? Icons.visibility : Icons.visibility_off,
                color: iconColor,
              )),
          border: InputBorder.none,
        ),
      ),
    );
  }
}
