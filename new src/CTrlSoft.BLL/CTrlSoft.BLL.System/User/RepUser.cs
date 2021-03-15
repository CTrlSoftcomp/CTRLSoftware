using CTrlSoft.Models;
using CTrlSoft.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace CTrlSoft.BLL.System.User
{
    public class RepUser : IUser
    {
        private Newtonsoft.Json.JsonSerializerSettings formatting = new Newtonsoft.Json.JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        public string StrKon { get; set; }
        public RepUser(string konString)
        {
            this.StrKon = konString;
        }

        public JsonResult Delete(MUser obj, MUser user)
        {
            JsonResult jsonResult = new JsonResult();
            if (user != null && user.id >= 1)
            {
                using (NpgsqlConnection con = new NpgsqlConnection(StrKon))
                {
                    using (NpgsqlCommand com = new NpgsqlCommand("", con))
                    {
                        using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter(com))
                        {
                            try
                            {
                                con.Open();
                                com.CommandTimeout = con.ConnectionTimeout;
                                com.Transaction = com.Connection.BeginTransaction();

                                com.CommandText = "update muser set aktif=0 where id<>1 and id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data user id=" + obj.id + " dihapus";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 1;
                                jsonResult.JSONValue = obj;

                                Repository.RepLogData.insertLog(com,
                                    Newtonsoft.Json.JsonConvert.SerializeObject(user, formatting),
                                    Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult, formatting));

                                jsonResult.JSONMessage = "Data berhasil dihapus";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;

                                com.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                jsonResult.JSONMessage = "ERR : " + e.Message;
                                jsonResult.JSONResult = false;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;
                            }
                        }
                    }
                }
            }
            else
            {
                jsonResult.JSONMessage = "User untuk hapus data harus diisi";
                jsonResult.JSONResult = false;
                jsonResult.JSONRows = 0;
                jsonResult.JSONValue = null;
            }

            return jsonResult;
        }

        public JsonResult Get(long noid)
        {
            JsonResult jsonResult = new JsonResult();
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    var data = db.Query<MUser>("select * from muser where id=@Id", new { Id = noid }, null, true, db.ConnectionTimeout).SingleOrDefault();
                    if (data != null)
                    {
                        data.roleuser = GetRole(data.idrole);
                        data.roleuser.mrolesd = GetRoleD(data.idrole);

                        jsonResult.JSONMessage = "Data ditemukan.";
                        jsonResult.JSONResult = true;
                        jsonResult.JSONRows = 1;
                        jsonResult.JSONValue = data;
                    }
                }
                catch (Exception e)
                {
                    jsonResult.JSONMessage = "ERR : " + e.Message;
                    jsonResult.JSONResult = false;
                    jsonResult.JSONRows = 0;
                    jsonResult.JSONValue = null;
                }
            }
            return jsonResult;
        }

        public JsonResult List(List<DataFilters> filters)
        {
            List<MUser> list = new List<MUser>();
            NpgsqlCommand com = new NpgsqlCommand();
            JsonResult jsonResult = new JsonResult();
            using (NpgsqlConnection con = new NpgsqlConnection(StrKon))
            {
                using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter(com))
                {
                    try
                    {
                        con.Open();
                        com.Connection = con;
                        com.CommandTimeout = con.ConnectionTimeout;
                        oDA.SelectCommand = com;
                        com.Transaction = com.Connection.BeginTransaction();

                        com.CommandText = "select id, userid, '' as pwd, nama, idkontak, idrole, aktif from muser where 1=1";
                        com = Repository.RepSqlDatabase.OperatorSQL(com, filters);

                        using (NpgsqlDataReader reader = com.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (!reader.Read())
                                {
                                    MUser user = new MUser
                                    {
                                        id = reader.GetInt32(0),
                                        userid = reader.GetString(1),
                                        pwd = "###########",
                                        nama = reader.GetString(3),
                                        idkontak = reader.GetInt32(4),
                                        idrole = reader.GetInt32(5),
                                        aktif = reader.GetBoolean(6),
                                        roleuser = GetRole(reader.GetInt32(5))
                                    };
                                    user.roleuser.mrolesd = GetRoleD(reader.GetInt32(5));
                                    list.Add(user);
                                }
                            }
                            reader.Close();
                        }

                        jsonResult.JSONMessage = "Data ditemukan.";
                        jsonResult.JSONResult = true;
                        jsonResult.JSONRows = list.Count();
                        jsonResult.JSONValue = list;

                        com.Transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        jsonResult.JSONMessage = "ERR : " + e.Message;
                        jsonResult.JSONResult = false;
                        jsonResult.JSONRows = 0;
                        jsonResult.JSONValue = null;
                    }
                }
            }
            return jsonResult;
        }

        public JsonResult Save(MUser obj, MUser user)
        {
            JsonResult jsonResult = new JsonResult();
            bool validasi = true;
            if (user != null && user.id >= 1)
            {
                using (NpgsqlConnection con = new NpgsqlConnection(StrKon))
                {
                    using (NpgsqlCommand com = new NpgsqlCommand("", con))
                    {
                        using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter(com))
                        {
                            try
                            {
                                con.Open();
                                com.CommandTimeout = con.ConnectionTimeout;
                                com.Transaction = com.Connection.BeginTransaction();

                                if (validasi)
                                {
                                    if (obj.userid.Length < 5)
                                    {
                                        jsonResult.JSONMessage = "Data User ID yang anda masukkan harus terisi minimum 5 digit.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.pwd.Length < 8)
                                    {
                                        jsonResult.JSONMessage = "Data Password yang anda masukkan harus terisi minimum 8 digit.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from muser where upper(userid)=@userid";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@userid", obj.userid.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data User ID yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done

                                
                                obj.id = Repository.RepUtils.NullToLong(DateTime.UtcNow.ToString("yyMMddHHmmssfff"));
                                com.CommandText = "insert into muser (id, userid, pwd, nama, aktif, idkontak, idrole) values (@id, @userid, @pwd, @nama, @aktif, @idkontak, @idrole)";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@userid", obj.userid);
                                com.Parameters.AddWithValue("@pwd", Repository.RepUtils.CreateMD5(obj.pwd.Trim()));
                                com.Parameters.AddWithValue("@nama", obj.nama);
                                com.Parameters.AddWithValue("@aktif", obj.aktif);
                                com.Parameters.AddWithValue("@idkontak", obj.idkontak);
                                com.Parameters.AddWithValue("@idrole", obj.idrole);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data user id=" + obj.id + " ditambahkan";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 1;
                                jsonResult.JSONValue = obj;

                                Repository.RepLogData.insertLog(com,
                                    Newtonsoft.Json.JsonConvert.SerializeObject(user, formatting),
                                    Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult, formatting));

                                jsonResult.JSONMessage = "Data berhasil ditambahkan";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;

                                com.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                jsonResult.JSONMessage = "ERR : " + e.Message;
                                jsonResult.JSONResult = false;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;
                            }
                        }
                    }
                }
            }
            else
            {
                jsonResult.JSONMessage = "User untuk hapus data harus diisi";
                jsonResult.JSONResult = false;
                jsonResult.JSONRows = 0;
                jsonResult.JSONValue = null;
            }

            return jsonResult;
        }

        public JsonResult Update(MUser obj, MUser user)
        {
            JsonResult jsonResult = new JsonResult();
            bool validasi = true;
            if (user != null && user.id >= 1)
            {
                using (NpgsqlConnection con = new NpgsqlConnection(StrKon))
                {
                    using (NpgsqlCommand com = new NpgsqlCommand("", con))
                    {
                        using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter(com))
                        {
                            try
                            {
                                con.Open();
                                com.CommandTimeout = con.ConnectionTimeout;
                                com.Transaction = com.Connection.BeginTransaction();

                                if (validasi)
                                {
                                    if (obj.userid.Length < 5)
                                    {
                                        jsonResult.JSONMessage = "Data User ID yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.pwd.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data Password yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from muser where id<>@id and upper(userid)=@userid or upper(nama)=@nama";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@id", obj.id);
                                    com.Parameters.AddWithValue("@userid", obj.userid.ToUpper());
                                    com.Parameters.AddWithValue("@nama", obj.nama.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data User ID atau Nama yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done

                                com.CommandText = "update muser set userid=@userid, pwd=@pwd, nama=@nama, aktif=@aktif, idkontak=@idkontak, idrole=@idrole where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@userid", obj.userid);
                                com.Parameters.AddWithValue("@pwd", Repository.RepUtils.CreateMD5(obj.pwd.Trim()));
                                com.Parameters.AddWithValue("@nama", obj.nama);
                                com.Parameters.AddWithValue("@aktif", obj.aktif);
                                com.Parameters.AddWithValue("@idkontak", obj.idkontak);
                                com.Parameters.AddWithValue("@idrole", obj.idrole);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data user id=" + obj.id + " diupdate";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 1;
                                jsonResult.JSONValue = obj;

                                Repository.RepLogData.insertLog(com,
                                    Newtonsoft.Json.JsonConvert.SerializeObject(user, formatting),
                                    Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult, formatting));

                                jsonResult.JSONMessage = "Data berhasil diupdate";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;

                                com.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                jsonResult.JSONMessage = "ERR : " + e.Message;
                                jsonResult.JSONResult = false;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;
                            }
                        }
                    }
                }
            }
            else
            {
                jsonResult.JSONMessage = "User untuk hapus data harus diisi";
                jsonResult.JSONResult = false;
                jsonResult.JSONRows = 0;
                jsonResult.JSONValue = null;
            }

            return jsonResult;
        }

        public JsonResult GetLogin(string UserID, string pwd)
        {
            JsonResult jsonResult = new JsonResult();
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    var data = db.Query<MUser>("select * from muser where userid=@userid", new { UserID }, null, true, db.ConnectionTimeout).SingleOrDefault();
                    if (data != null)
                    {
                        string md5Checker = Repository.RepUtils.CreateMD5(pwd.Trim());
                        if (md5Checker.Equals(data.pwd))
                        {
                            data.roleuser = GetRole(data.idrole);
                            data.roleuser.mrolesd = GetRoleD(data.idrole);

                            jsonResult.JSONMessage = "Data ditemukan.";
                            jsonResult.JSONResult = true;
                            jsonResult.JSONRows = 1;
                            jsonResult.JSONValue = data;
                        } else
                        {
                            jsonResult.JSONMessage = "Password yang anda masukkan salah.";
                            jsonResult.JSONResult = false;
                            jsonResult.JSONRows = 0;
                            jsonResult.JSONValue = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    jsonResult.JSONMessage = "ERR : " + e.Message;
                    jsonResult.JSONResult = false;
                    jsonResult.JSONRows = 0;
                    jsonResult.JSONValue = null;
                }
            }
            return jsonResult;
        }

        public MRole GetRole(int idrole)
        {
            MRole mRole = null;
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    mRole = db.Query<MRole>("select * from mrole where id=@Id", new { Id = idrole }, null, true, db.ConnectionTimeout).SingleOrDefault();
                }
                catch (Exception e)
                {
                    mRole = null;
                }
            }
            return mRole;
        }

        public List<MRoleD> GetRoleD(int idrole)
        {
            List<MRoleD> mRoles = null;
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    mRoles = db.Query<MRoleD>("select * from mroled where idrole=@Id", new { Id = idrole }, null, true, db.ConnectionTimeout).ToList();
                }
                catch (Exception e)
                {
                    mRoles = null;
                }
            }
            return mRoles;
        }
    }
}