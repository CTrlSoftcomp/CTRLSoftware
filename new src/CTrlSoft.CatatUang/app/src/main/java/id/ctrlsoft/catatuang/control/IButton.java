package id.ctrlsoft.catatuang.control;

import android.content.Context;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.ProgressBar;
import android.widget.TextView;

import androidx.constraintlayout.widget.ConstraintLayout;

import com.google.android.material.button.MaterialButton;
import com.google.android.material.progressindicator.CircularProgressIndicator;

import id.ctrlsoft.catatuang.R;

public class IButton {
    ConstraintLayout            layout;
    MaterialButton              button;
    TextView                    text;
    CircularProgressIndicator   progressIndicator;
    ProgressBar                 progressBar;

    Animation fade_in;
    IButton(Context context,
            View view) {
        fade_in = AnimationUtils.loadAnimation(context, R.anim.fade_in);

        layout              = view.findViewById(R.id.layout);
        button              = view.findViewById(R.id.button);
        text                = view.findViewById(R.id.text);
//        progressIndicator   = view.findViewById(R.id.progressIndicator);
//        progressBar         = view.findViewById(R.id.progressBar);
    }
}
