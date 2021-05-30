package id.ctrlsoft.catatuang.connection;

import id.ctrlsoft.catatuang.connection.model.Callbacks;
import id.ctrlsoft.catatuang.connection.model.CallbacksUser;
import id.ctrlsoft.catatuang.repository.model.User;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.Query;

public interface API {
    String CACHE = "Cache-Control: max-age=0";
    String TYPE = "Content-Type: application/x-www-form-urlencoded";

    @Headers({CACHE, TYPE})
    @GET("User/login")
    Call<CallbacksUser> User_Login(
            @Query("userid") String userid,
            @Query("pwd") String pwd
    );

    @Headers({CACHE, TYPE})
    @GET("User/available")
    Call<Callbacks> User_Avaiable(
            @Query("userid") String userid
    );

    @Headers({CACHE, TYPE})
    @FormUrlEncoded
    @POST("User/save")
    Call<CallbacksUser> User_Save(
            @Body User user
    );
}
