package id.ctrlsoft.catatuang.ui.login;

import androidx.annotation.RequiresApi;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import android.app.ProgressDialog;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ProgressBar;
import android.widget.TextView;

import com.google.android.material.button.MaterialButton;
import com.google.android.material.progressindicator.CircularProgressIndicator;
import com.google.android.material.textfield.TextInputEditText;

import id.ctrlsoft.catatuang.R;
import id.ctrlsoft.catatuang.connection.API;
import id.ctrlsoft.catatuang.connection.RestAdapter;
import id.ctrlsoft.catatuang.connection.model.CallbacksUser;
import id.ctrlsoft.catatuang.repository.mdlPublic;
import id.ctrlsoft.catatuang.ui.register.RegisterActivity;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

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
    private TextInputEditText txtHP, txtPwd, txt62;
    private MaterialButton btnLogin;
    private CircularProgressIndicator progressIndicator;

    private void initLayout() {
        tvRegister          = findViewById(R.id.tvRegister);
        tvForgotPassword    = findViewById(R.id.tvLupaPassword);
        txt62               = findViewById(R.id.txt62);
        txtHP               = findViewById(R.id.txtHandphone);
        txtPwd              = findViewById(R.id.txtPassword);
        btnLogin            = findViewById(R.id.btnLogin);
        progressIndicator   = findViewById(R.id.progress_circular);

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
                boolean valid = true;
                if (valid && txtHP.getText().length()<=0) {
                    mdlPublic.ShowMessage(LoginActivity.this, getString(R.string.login_txtHP));
                    valid = false;
                }
                if (valid && txtPwd.getText().length()<=8) {
                    mdlPublic.ShowMessage(LoginActivity.this, getString(R.string.login_txtPwd));
                    valid = false;
                }

                if (valid) {
                    LoginAsync(
                            txt62.getText().toString() + txtHP.getText().toString(),
                            txtPwd.getText().toString());
                }
            }
        });
    }

    private void LoginAsync(String Phone, String Pwd) {
        Call<CallbacksUser> userCall = RestAdapter.createAPI().User_Login(
                Phone,
                Pwd);
        userCall.enqueue(new Callback<CallbacksUser>() {
            @Override
            public void onResponse(Call<CallbacksUser> call, Response<CallbacksUser> response) {

            }

            @Override
            public void onFailure(Call<CallbacksUser> call, Throwable t) {
                mdlPublic.ShowMessage(getApplicationContext(), getString(R.string.app_disconnect));
            }
        });
    }

    private void initData() {

    }
}