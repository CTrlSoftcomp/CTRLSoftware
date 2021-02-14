package id.ctrlsoft.catatuang.ui.login;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.TextView;

import com.google.android.material.button.MaterialButton;
import com.google.android.material.textfield.TextInputEditText;

import id.ctrlsoft.catatuang.R;
import id.ctrlsoft.catatuang.repository.mdlPublic;
import id.ctrlsoft.catatuang.ui.register.RegisterActivity;

public class LoginActivity extends AppCompatActivity {
//    @RequiresApi(api = Build.VERSION_CODES.M)
//    private void decorStatusBar() {
//        Window window = getWindow();
//        window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
//        window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
//        window.setStatusBarColor(getResources().getColor(R.color.colorPrimaryDark));
//        window.getDecorView().setSystemUiVisibility(View.SYSTEM_UI_FLAG_IMMERSIVE);
//    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
//        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
//            decorStatusBar();
//        } else {
//            Window window = getWindow();
//
//            // clear FLAG_TRANSLUCENT_STATUS flag:
//            window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
//
//            // add FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS flag to the window
//            window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
//
//            // finally change the color
//            window.setStatusBarColor(ContextCompat.getColor(this,
//                    R.color.colorPrimaryDark));
//        }

        setContentView(R.layout.activity_login);

        initLayout();
        initData();
    }

    private TextView tvRegister, tvForgotPassword;
    private TextInputEditText txtHP, txtPwd;
    private MaterialButton btnLogin;

    private void initLayout() {
        tvRegister          = findViewById(R.id.tvRegister);
        tvForgotPassword    = findViewById(R.id.tvLupaPassword);
        txtHP               = findViewById(R.id.txtHandphone);
        txtPwd              = findViewById(R.id.txtPassword);
        btnLogin            = findViewById(R.id.btnLogin);

        tvRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mdlPublic.NavigateActivity(LoginActivity.this, true, RegisterActivity.class, null);
            }
        });
        tvForgotPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
    }

    private void initData() {

    }
}