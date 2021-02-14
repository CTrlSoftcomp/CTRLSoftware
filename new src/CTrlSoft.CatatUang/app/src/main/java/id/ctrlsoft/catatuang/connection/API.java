package id.ctrlsoft.catatuang.connection;

import id.ctrlsoft.catatuang.connection.model.Callbacks;
import retrofit2.Call;
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
    @GET("MasterJSONService.asmx/GetListKategori")
    Call<Callbacks> getListKategori(
            @Query("IDParent") long idparent
    );

    @Headers({CACHE, TYPE})
    @FormUrlEncoded
    @POST("MasterJSONService.asmx/GetListProdukGuest")
    Call<Callbacks> getListProdukGuest(
            @Field("IDJenisHarga") int idjenisharga,
            @Field("HP") String hp,
            @Field("StrFilter") String filter,
            @Field("StrSort") String sort,
            @Field("Page") int page
    );
}
