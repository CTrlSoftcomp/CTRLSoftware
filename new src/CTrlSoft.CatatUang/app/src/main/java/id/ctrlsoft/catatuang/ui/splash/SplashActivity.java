package id.ctrlsoft.catatuang.ui.splash;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.content.IntentSender;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.material.progressindicator.LinearProgressIndicator;
import com.google.android.material.snackbar.Snackbar;
import com.google.android.play.core.appupdate.AppUpdateInfo;
import com.google.android.play.core.appupdate.AppUpdateManager;
import com.google.android.play.core.appupdate.AppUpdateManagerFactory;
import com.google.android.play.core.install.InstallState;
import com.google.android.play.core.install.InstallStateUpdatedListener;
import com.google.android.play.core.install.model.AppUpdateType;
import com.google.android.play.core.install.model.InstallStatus;
import com.google.android.play.core.install.model.UpdateAvailability;
import com.google.android.play.core.tasks.OnFailureListener;
import com.google.android.play.core.tasks.OnSuccessListener;
import com.google.android.play.core.tasks.Task;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.Calendar;

import id.ctrlsoft.catatuang.R;
import id.ctrlsoft.catatuang.ui.login.LoginActivity;
import retrofit2.Call;
import retrofit2.Response;

public class SplashActivity extends AppCompatActivity {
    private AppUpdateManager mAppUpdateManager;

    private static String TAG = "UpdatePlayStore";
    private static final int RC_APP_UPDATE = 2399;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);

        setContentView(R.layout.activity_splash);

        initLayouts();
        initData();
    }

    private TextView txtAppVersion;
    private LinearProgressIndicator progressBar;
    private void initLayouts() {
        txtAppVersion   = findViewById(R.id.tvVersion);
        progressBar     = findViewById(R.id.progress_horizontal);
    }

    private void initData() {
        try {
            PackageInfo pInfo = getPackageManager().getPackageInfo(getPackageName(), 0);
            String version = pInfo.versionName;
            txtAppVersion.setText(getString(R.string.app_version) + " : " + version);
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
            txtAppVersion.setText(getString(R.string.app_version) + " : -");
        }
        CheckUpdate();
    }

    InstallStateUpdatedListener installStateUpdatedListener = new InstallStateUpdatedListener() {
        @Override
        public void onStateUpdate(InstallState state) {
            if (state.installStatus() == InstallStatus.DOWNLOADED) {
                popupSnackbarForCompleteUpdate();
            } else if (state.installStatus() == InstallStatus.INSTALLED) {
                if (mAppUpdateManager != null) {
                    mAppUpdateManager.unregisterListener(installStateUpdatedListener);
                }
            } else {
                Log.i(TAG, "InstallStateUpdatedListener: state: " + state.installStatus());
            }
        }
    };

    private void CheckUpdate() {
        mAppUpdateManager = AppUpdateManagerFactory.create(this);

        mAppUpdateManager.registerListener(installStateUpdatedListener);

        Task<AppUpdateInfo> appUpdateInfoTask = mAppUpdateManager.getAppUpdateInfo();

        // Checks that the platform will allow the specified type of update.
        appUpdateInfoTask.addOnSuccessListener(new OnSuccessListener<AppUpdateInfo>() {
            @Override
            public void onSuccess(AppUpdateInfo appUpdateInfo) {
                if (appUpdateInfo.updateAvailability() == UpdateAvailability.UPDATE_AVAILABLE
                        // For a flexible update, use AppUpdateType.FLEXIBLE
                        && appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.IMMEDIATE)) {
                    try {
                        mAppUpdateManager.startUpdateFlowForResult(
                                appUpdateInfo,
                                AppUpdateType.IMMEDIATE,
                                SplashActivity.this,
                                RC_APP_UPDATE);
                    } catch (IntentSender.SendIntentException e) {
                        e.printStackTrace();
                    }
                } else if(appUpdateInfo.updateAvailability() == UpdateAvailability.UPDATE_AVAILABLE
                        // For a flexible update, use AppUpdateType.FLEXIBLE
                        && appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.FLEXIBLE)) {
                    try {
                        mAppUpdateManager.startUpdateFlowForResult(
                                appUpdateInfo,
                                AppUpdateType.FLEXIBLE,
                                SplashActivity.this,
                                RC_APP_UPDATE);
                    } catch (IntentSender.SendIntentException e) {
                        e.printStackTrace();
                    }
                } else {
                    GetLogin();
                }
            }
        });

        appUpdateInfoTask.addOnFailureListener(new OnFailureListener() {
            @Override
            public void onFailure(Exception e) {
                GetLogin();
            }
        });
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == RC_APP_UPDATE) {
            if (resultCode != RESULT_OK) {
                Log.e(TAG, "onActivityResult: app download failed");
                System.exit(0);
            }
        }
    }

    private void popupSnackbarForCompleteUpdate() {
        Snackbar snackbar =
                Snackbar.make(
                        findViewById(R.id.coordinator),
                        "Versi Terbaru telah hadir!",
                        Snackbar.LENGTH_INDEFINITE);
        snackbar.setAction("Install", new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (mAppUpdateManager != null){
                    mAppUpdateManager.completeUpdate();
                }
            }
        });
        snackbar.setActionTextColor(getResources().getColor(R.color.colorPrimaryDark));
        snackbar.show();
    }

    private void GetLogin() {
        // Start lengthy operation in a background thread
        new Thread(new Runnable() {
            public void run() {
                doWork();
                ShowMainActivity();
            }
        }).start();
//        final String StrUserID, StrPassword;
//
//        //Save Preference
//        final SharedPreferences settingsGuest = getSharedPreferences(mdlPublic.PREFS_GUEST, 0);
//        final SharedPreferences settings = getSharedPreferences(mdlPublic.PREFS_NAME, 0);
//        mdlPublic.MemberLogin = new Customer();
//
//        if (!settings.getBoolean("SettingUp", false)) {
//            // No Login
//            new Thread(new Runnable() {
//                public void run() {
//                    doWork();
//                    ShowSettingUpActivity();
//                }
//            }).start();
//        } else {
//            if (!settings.getBoolean("AutoLogin", false)) {
//                if (settingsGuest.getBoolean("AutoLogin", false)) {
//                    //Login As Guest
//                    new Thread(new Runnable() {
//                        public void run() {
//                            try {
//                                doWork();
//                                Gson gson = new GsonBuilder().setDateFormat("yyyy-MM-dd HH:mm:ss").create();
//                                Customer customer = gson.fromJson(settingsGuest.getString("Customer", ""), Customer.class);
//                                customer.ExpiredMember = Calendar.getInstance().getTime();
//                                if (customer!=null &&
//                                        !customer.getNama().equalsIgnoreCase("") &&
//                                        !customer.HP.equalsIgnoreCase("")) {
//                                    mdlPublic.MemberLogin = customer;
//                                }
//                                ShowMainActivity();
//                            } catch (Exception e) {
//                                e.printStackTrace();
//                                Log.e("ERR", e.getMessage(), e);
//                                ShowSettingUpActivity();
//                            }
//                        }
//                    }).start();
//                } else {
//                    // No Login
//                    new Thread(new Runnable() {
//                        public void run() {
//                            doWork();
//                            ShowMainActivity();
//                        }
//                    }).start();
//                }
//            } else {
//                StrUserID = settings.getString("UserID", "");
//                StrPassword = settings.getString("Password", "");
//                // Start lengthy operation in a background thread
//                new Thread(new Runnable() {
//                    public void run() {
//                        doWork();
//                        if ((StrUserID.equalsIgnoreCase("---") && StrPassword.equalsIgnoreCase("!@#$%^&*()")) ||
//                                StrUserID.equalsIgnoreCase("") ||
//                                StrPassword.equalsIgnoreCase("")) {
//                            ShowMainActivity();
//                        } else {
//                            Gson gson = new GsonBuilder().setDateFormat("yyyy-MM-dd HH:mm:ss").create();
//                            callbacksCall = api.getLogin(StrUserID, StrPassword);
//                            callbacksCall.enqueue(new retrofit2.Callback<Callback>() {
//                                @Override
//                                public void onResponse(Call<Callback> call, Response<Callback> response) {
//                                    Callback resp = response.body();
//                                    Log.e("TAG", "onResponse: " + resp );
//                                    if (resp != null && resp.JSONResult) {
//                                        String json = gson.toJson(resp.JSONValue);
//                                        Customer customer = gson.fromJson(json, Customer.class);
//                                        if (customer!=null) {
//                                            mdlPublic.MemberLogin = customer;
//                                        }
//                                    }
//
//                                    ShowMainActivity();
//                                }
//
//                                @Override
//                                public void onFailure(Call<Callback> call, Throwable t) {
//                                    if (!call.isCanceled()) {
//                                        onFailRequest();
//
//                                        ShowMainActivity();
//                                    }
//                                }
//
//                                private void onFailRequest() {
//                                    Toast.makeText(SplashActivity.this, "Gagal Konek ke Server!", Toast.LENGTH_SHORT).show();
//                                }
//                            });
//                        }
//                    }
//                }).start();
//            }
//        }
    }

    private void doWork() {
        for (int progress=0; progress<100; progress+=10) {
            try {
                Thread.sleep(300);
                progressBar.setProgress(progress);
            } catch (Exception e) {
                e.printStackTrace();
                //Timber.e(e.getMessage());
            }
        }
    }

    private void ShowMainActivity() {
        Intent intent = new Intent(getApplicationContext(), LoginActivity.class);
        startActivity(intent);
        this.finish();
    }
}