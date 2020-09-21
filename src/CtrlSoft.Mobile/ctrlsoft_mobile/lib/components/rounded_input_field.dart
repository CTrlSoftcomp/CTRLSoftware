import 'package:ctrlsoft_mobile/constants.dart';
import 'package:flutter/material.dart';
import 'package:ctrlsoft_mobile/components/text_field_container.dart';

class RoundedInputField extends StatelessWidget {
  final String hintText;
  final IconData iconFront;
  final ValueChanged<String> onChanged;
  final Color iconColor;
  final TextEditingController controller;

  const RoundedInputField({
    Key key,
    this.hintText,
    this.onChanged,
    this.iconFront,
    this.iconColor,
    this.controller,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return TextFieldContainer(
      child: TextField(
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
          border: InputBorder.none,
        ),
      ),
    );
  }
}
