package id.ctrlsoft.catatuang.ui.register;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.View;
import android.view.WindowManager;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.google.android.material.button.MaterialButton;
import com.google.android.material.checkbox.MaterialCheckBox;
import com.google.android.material.textfield.TextInputEditText;

import id.ctrlsoft.catatuang.R;
import id.ctrlsoft.catatuang.connection.RestAdapter;
import id.ctrlsoft.catatuang.connection.model.Callbacks;
import id.ctrlsoft.catatuang.connection.model.CallbacksUser;
import id.ctrlsoft.catatuang.repository.mdlPublic;
import id.ctrlsoft.catatuang.repository.model.User;
import id.ctrlsoft.catatuang.ui.login.LoginActivity;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegisterActivity extends AppCompatActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        initLayout();
        initData();
    }

    private TextView tvSignIn, tvTerm, tvCondition, tvAvailable;
    private TextInputEditText txtHP, txtPwd, txtName;
    private MaterialButton btnRegister;
    private MaterialCheckBox ckAgree;
    private boolean isAvailable = false;

    private void initLayout() {
        tvSignIn            = findViewById(R.id.tvSignIn);
        tvTerm              = findViewById(R.id.tvTerm);
        tvCondition         = findViewById(R.id.tvConditions);
        txtHP               = findViewById(R.id.txtHandphone);
        tvAvailable         = findViewById(R.id.tvAvailable);
        txtPwd              = findViewById(R.id.txtPassword);
        txtName             = findViewById(R.id.txtName);
        btnRegister         = findViewById(R.id.btnRegister);
        ckAgree             = findViewById(R.id.ckAgree);

        //Disable inputan
        ckAgree.setKeyListener(null);

        txtHP.setOnFocusChangeListener(new View.OnFocusChangeListener() {
            @Override
            public void onFocusChange(View v, boolean hasFocus) {
                if (!hasFocus) {
                    if (txtHP.getText().length() >= 8) {
                        AvailableAsync(txtHP.getText().toString());
                    } else {
                        tvAvailable.setVisibility(View.GONE);
                        isAvailable = false;
                        txtHP.setCompoundDrawables(null, null, null, null);
                    }
                }
            }
        });

        tvSignIn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                mdlPublic.NavigateActivity(RegisterActivity.this, true, LoginActivity.class, null);
            }
        });
        tvTerm.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showBottomSheetDialog();
            }
        });
        tvCondition.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showBottomSheetDialog();
            }
        });
        btnRegister.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (!isAvailable) {
                    mdlPublic.ShowMessage(RegisterActivity.this, getString(R.string.register_user_notavailable));
                } else if (!ckAgree.isChecked()) {
                    mdlPublic.ShowMessage(RegisterActivity.this, "Baca dahulu term and condition");
                } else if (txtPwd.getText().length() < 3) {
                    mdlPublic.ShowMessage(RegisterActivity.this, "Isi password minimal 3 karakter!");
                } else if (txtName.getText().length() <= 0) {
                    mdlPublic.ShowMessage(RegisterActivity.this, "Isi nama anda dengan benar!");
                } else {
                    User user = new User();
                    user.id = 1;
                    user.idkontak = 0;
                    user.idrole = 0;
                    user.userid = txtHP.getText().toString();
                    user.nama = txtName.getText().toString();
                    user.pwd = txtPwd.getText().toString();

                    Call<CallbacksUser> userCall = RestAdapter.createAPI().User_Save(user);
                    userCall.enqueue(new Callback<CallbacksUser>() {
                        @Override
                        public void onResponse(Call<CallbacksUser> call, Response<CallbacksUser> response) {
                            if (response.isSuccessful()) {
                                if (response.body().JSONResult) {
                                    //Success Register
                                    Intent intent = new Intent().putExtra("UserRegister", mdlPublic.toJson(response.body()));
                                    GoBackMenu(RESULT_OK, intent);
                                } else {
                                    mdlPublic.ShowMessage(RegisterActivity.this, response.body().JSONMessage);
                                }
                                tvAvailable.setVisibility(View.VISIBLE);
                            }
                        }

                        @Override
                        public void onFailure(Call<CallbacksUser> call, Throwable t) {

                        }
                    });
                }
            }
        });
    }

    private void GoBackMenu(int Result, Intent intent) {
        try {
            if (intent != null) {
                this.setResult(Result, intent);
            } else {
                this.setResult(Result);
            }
            this.finish();
        } catch (Exception e) {
            mdlPublic.LogError(RegisterActivity.this, e);
        }
    }

    private void AvailableAsync(String Phone) {
        tvAvailable.setVisibility(View.GONE);
        isAvailable = false;
        Call<Callbacks> userCall = RestAdapter.createAPI().User_Avaiable(
                Phone);
        userCall.enqueue(new Callback<Callbacks>() {
            @Override
            public void onResponse(Call<Callbacks> call, Response<Callbacks> response) {
                if (response.isSuccessful()) {
                    if (response.body().JSONResult) {
                        tvAvailable.setText(getString(R.string.register_user_available));
                        txtHP.setCompoundDrawables(null, null, getDrawable(R.drawable.ic_check_16), null);
                        isAvailable = true;
                    } else {
                        tvAvailable.setText(getString(R.string.register_user_notavailable));
                        txtHP.setCompoundDrawables(null, null, getDrawable(R.drawable.ic_error_16), null);
                        isAvailable = false;
                    }
                    tvAvailable.setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onFailure(Call<Callbacks> call, Throwable t) {
                mdlPublic.ShowMessage(getApplicationContext(), getString(R.string.app_disconnect));
            }
        });
    }

    private void initData() {

    }

    BottomSheetBehavior sheetBehavior;
    BottomSheetDialog sheetDialog;
    View bottom_sheet;
    private void showBottomSheetDialog() {
        View view = getLayoutInflater().inflate(R.layout.bottom_term_and_condition, null);

        if (sheetBehavior.getState() == BottomSheetBehavior.STATE_EXPANDED) {
            sheetBehavior.setState(BottomSheetBehavior.STATE_COLLAPSED);
        }

        (view.findViewById(R.id.ckAgree)).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ckAgree.setChecked(true);

                sheetDialog.dismiss();
            }
        });

        sheetDialog = new BottomSheetDialog(this);
        sheetDialog.setContentView(view);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
            sheetDialog.getWindow().addFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
        }

        sheetDialog.show();
        sheetDialog.setOnDismissListener(new DialogInterface.OnDismissListener() {
            @Override
            public void onDismiss(DialogInterface dialog) {
                sheetDialog = null;
            }
        });
    }
}
