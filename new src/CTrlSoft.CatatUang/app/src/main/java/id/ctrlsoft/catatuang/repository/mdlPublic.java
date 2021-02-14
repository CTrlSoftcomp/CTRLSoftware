package id.ctrlsoft.catatuang.repository;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.TextView;

import androidx.appcompat.app.AlertDialog;

import org.json.JSONArray;

import java.util.Date;
import java.util.List;

import id.ctrlsoft.catatuang.R;
import id.ctrlsoft.catatuang.repository.model.Data;

public class mdlPublic {
    public static String URI_API = "http://ctrlsoft.id/api/";

    // Activity
    public static int activity_Login        = 1;
    public static int activity_register     = 2;
    public static int activity_splash       = 3;
    public static int activity_main         = 4;
    // Activity

    public static void NavigateActivity(Activity activity, boolean finishOld, Class<?> cls, List<Data> extras) {
        Intent intent;
        try {
            intent = new Intent(activity.getApplicationContext(), cls);
            if (extras != null) {
                for (int i = 0; i < extras.size(); i++) {
                    if (extras.get(i).value instanceof Long) {
                        intent.putExtra(extras.get(i).name, (Long) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Integer) {
                        intent.putExtra(extras.get(i).name, (Integer) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Float) {
                        intent.putExtra(extras.get(i).name, (Float) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Date) {
                        intent.putExtra(extras.get(i).name, (Date) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Double) {
                        intent.putExtra(extras.get(i).name, (Double) extras.get(i).value);
                    } else if (extras.get(i).value instanceof String) {
                        intent.putExtra(extras.get(i).name, (String) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Long[]) {
                        intent.putExtra(extras.get(i).name, (Long[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Integer[]) {
                        intent.putExtra(extras.get(i).name, (Integer[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Float[]) {
                        intent.putExtra(extras.get(i).name, (Float[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Date[]) {
                        intent.putExtra(extras.get(i).name, (Date[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Double[]) {
                        intent.putExtra(extras.get(i).name, (Double[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof String[]) {
                        intent.putExtra(extras.get(i).name, (String[]) extras.get(i).value);
                    }
                }
            }
            activity.startActivity(intent);
            if (finishOld) {
                activity.finish();
            }
        } catch (Exception e) {
            LogError(activity.getApplicationContext(), e);
        }
    }

    public static void NavigateActivity(Activity activity, Class<?> cls, List<Data> extras, int activity_for_result) {
        Intent intent;
        try {
            intent = new Intent(activity.getApplicationContext(), cls);
            if (extras != null) {
                for (int i = 0; i < extras.size(); i++) {
                    if (extras.get(i).value instanceof Long) {
                        intent.putExtra(extras.get(i).name, (Long) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Integer) {
                        intent.putExtra(extras.get(i).name, (Integer) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Float) {
                        intent.putExtra(extras.get(i).name, (Float) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Date) {
                        intent.putExtra(extras.get(i).name, (Date) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Double) {
                        intent.putExtra(extras.get(i).name, (Double) extras.get(i).value);
                    } else if (extras.get(i).value instanceof String) {
                        intent.putExtra(extras.get(i).name, (String) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Long[]) {
                        intent.putExtra(extras.get(i).name, (Long[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Integer[]) {
                        intent.putExtra(extras.get(i).name, (Integer[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Float[]) {
                        intent.putExtra(extras.get(i).name, (Float[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Date[]) {
                        intent.putExtra(extras.get(i).name, (Date[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof Double[]) {
                        intent.putExtra(extras.get(i).name, (Double[]) extras.get(i).value);
                    } else if (extras.get(i).value instanceof String[]) {
                        intent.putExtra(extras.get(i).name, (String[]) extras.get(i).value);
                    }
                }
            }
            activity.startActivityForResult(intent, activity_for_result);
        } catch (Exception e) {
            LogError(activity.getApplicationContext(), e);
        }
    }

    public static void LogError(Context context, Exception ex) {
        AlertDialog.Builder dialog;
        LayoutInflater inflater;
        View dialogView;
        try {
            //Dimasukkan ke API juga boleh
            inflater = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            dialogView = inflater.inflate(R.layout.dialog_error, null);
            TextView tvMessage = dialogView.findViewById(R.id.tvMessage);
            tvMessage.setText("Err : " + ex.getMessage());

            dialog = new AlertDialog.Builder(context)
            .setCancelable(false)
            .setView(dialogView)
            .setCancelable(true)
            .setPositiveButton("OK", (dialog1, which) -> {
                dialog1.dismiss();
            });

            dialog.show();
        } catch (Exception e) {
            Log.e("ERR", e.getMessage(), e);
        }
    }
}
