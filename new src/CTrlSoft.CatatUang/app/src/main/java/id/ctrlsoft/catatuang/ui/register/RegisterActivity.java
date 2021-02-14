package id.ctrlsoft.catatuang.ui.register;

import android.os.Bundle;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.button.MaterialButton;
import com.google.android.material.checkbox.MaterialCheckBox;
import com.google.android.material.textfield.TextInputEditText;

import id.ctrlsoft.catatuang.R;
import id.ctrlsoft.catatuang.repository.mdlPublic;
import id.ctrlsoft.catatuang.ui.login.LoginActivity;

public class RegisterActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        initLayout();
        initData();
    }

    private TextView tvSignIn, tvTerm, tvCondition;
    private TextInputEditText txtHP, txtPwd;
    private MaterialButton btnRegister;
    private MaterialCheckBox ckAgree;

    private void initLayout() {
        tvSignIn            = findViewById(R.id.tvSignIn);
        tvTerm              = findViewById(R.id.tvTerm);
        tvCondition         = findViewById(R.id.tvConditions);
        txtHP               = findViewById(R.id.txtHandphone);
        txtPwd              = findViewById(R.id.txtPassword);
        btnRegister         = findViewById(R.id.btnRegister);
        ckAgree             = findViewById(R.id.ckAgree);

        tvSignIn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mdlPublic.NavigateActivity(RegisterActivity.this, true, LoginActivity.class, null);
            }
        });
        tvTerm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
        tvCondition.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
        btnRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
    }

    private void initData() {

    }
}
