using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Models;
using CTrlSoft.Core.Api.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Data;
using CTrlSoft.Core.Api.Repository;

namespace CTrlSoft.Core.Api.DataAccess
{
    public class UserContext : IUser
    {
        public string ConnectionString { get; set; }

        public UserContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        JsonResult IBase<MUser>.Delete(Models.Dto.MUser obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MUser>.GetAll()
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MUser> obj = getListUser(new List<DataFilters>());
            if (obj == null)
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data tidak ditemukan",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            }
            else
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data ditemukan",
                    JSONResult = true,
                    JSONRows = (obj != null ? obj.Count : 0),
                    JSONValue = obj
                };
            }
            return hasil;
        }


        JsonResult IUser.Save(Models.Dto.MUser obj, ref ValidationError validationError)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MUser>.Save(Models.Dto.MUser obj)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            List<MUser> list = new List<MUser>();
            using (NpgsqlConnection conn = GetConnection())
            {
                using (NpgsqlCommand com = new NpgsqlCommand())
                {
                    using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter())
                    {
                        try
                        {
                            conn.Open();
                            com.Connection = conn;
                            com.CommandTimeout = conn.ConnectionTimeout;
                            com.Transaction = com.Connection.BeginTransaction();

                            com.CommandText = "insert into muser (id,userid,pwd,nama,idkontak,idrole)" +
                                              "values (@id,@userid,@pwd,@nama,@idkontak,@idrole)";
                            com.Parameters.Clear();
                            com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                            com.Parameters.Add("@userid", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.userid.Trim();
                            com.Parameters.Add("@pwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = RepUtils.CreateMD5(obj.pwd.Trim());
                            com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                            com.Parameters.Add("@idkontak", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idkontak;
                            com.Parameters.Add("@idrole", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idrole;

                            com.ExecuteNonQuery();

                            //cek data sudah ada atau belum
                            com.CommandText = "select top 1 id from mkontak where right(hp, LENGTH(hp)-2) = @hp";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@hp", obj.userid);
                            long jml = RepUtils.NullToLong(com.ExecuteScalar());

                            if (jml == 0)
                            {
                                com.CommandText = "select max(id) from mkontak";
                                jml = RepUtils.NullToLong(com.ExecuteScalar()) + 1;
                                com.CommandText = "insert into mkontak (id,kode,nama,alamat1,alamat2,alamat3,hp,telpon,iswhatsapp,norekening,bank,atasnamarekening)" +
                                                  "values (@id,@kode,@nama,@alamat1,@alamat2,@alamat3,@hp,@telpon,@iswhatsapp,@norekening,@bank,@atasnamarekening)";
                                com.Parameters.Clear();
                                com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = jml;
                                com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.userid.Trim();
                                com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                                com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.userid;
                                com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = false;
                                com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";

                                com.ExecuteNonQuery();
                            }

                            obj.idkontak = jml;
                            com.CommandText = "update muser set idkontak=@idkontak where id=@id";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@idkontak", obj.idkontak);
                            com.Parameters.AddWithValue("@id", obj.id);
                            com.ExecuteNonQuery();

                            com.Transaction.Commit();
                            hasil = new JsonResult
                            {
                                JSONMessage = "Data tersimpan",
                                JSONResult = true,
                                JSONRows = 1,
                                JSONValue = obj
                            };
                        }
                        catch (Exception ex)
                        {
                            hasil = new JsonResult
                            {
                                JSONMessage = ex.StackTrace,
                                JSONResult = false,
                                JSONRows = 0,
                                JSONValue = null
                            };
                        }
                    }
                }
            }
            return hasil;
        }

        JsonResult IUser.Update(Models.Dto.MUser obj, ref ValidationError validationError)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MUser>.Update(Models.Dto.MUser obj)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            List<MUser> list = new List<MUser>();
            using (NpgsqlConnection conn = GetConnection())
            {
                using (NpgsqlCommand com = new NpgsqlCommand())
                {
                    using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter())
                    {
                        try
                        {
                            conn.Open();
                            com.Connection = conn;
                            com.CommandTimeout = conn.ConnectionTimeout;
                            com.Transaction = com.Connection.BeginTransaction();

                            com.CommandText = "update muser set nama=@nama,idkontak=@idkontak,idrole=@idrole where id=@id";
                            com.Parameters.Clear();
                            com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                            com.Parameters.Add("@userid", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.userid.Trim();
                            com.Parameters.Add("@pwd", NpgsqlTypes.NpgsqlDbType.Varchar).Value = RepUtils.CreateMD5(obj.pwd.Trim());
                            com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                            com.Parameters.Add("@idkontak", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idkontak;
                            com.Parameters.Add("@idrole", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idrole;

                            com.ExecuteNonQuery();

                            //cek data sudah ada atau belum
                            com.CommandText = "select top 1 id from mkontak where right(hp, LENGTH(hp)-2) = @hp";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@hp", obj.userid);
                            long jml = RepUtils.NullToLong(com.ExecuteScalar());

                            if (jml == 0)
                            {
                                com.CommandText = "select max(id) from mkontak";
                                jml = RepUtils.NullToLong(com.ExecuteScalar()) + 1;
                                com.CommandText = "insert into mkontak (id,kode,nama,alamat1,alamat2,alamat3,hp,telpon,iswhatsapp,norekening,bank,atasnamarekening)" +
                                                  "values (@id,@kode,@nama,@alamat1,@alamat2,@alamat3,@hp,@telpon,@iswhatsapp,@norekening,@bank,@atasnamarekening)";
                                com.Parameters.Clear();
                                com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = jml;
                                com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.userid.Trim();
                                com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                                com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.userid;
                                com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = false;
                                com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";
                                com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "";

                                com.ExecuteNonQuery();
                            }

                            obj.idkontak = jml;
                            com.CommandText = "update muser set idkontak=@idkontak where id=@id";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@idkontak", obj.idkontak);
                            com.Parameters.AddWithValue("@id", obj.id);
                            com.ExecuteNonQuery();

                            com.Transaction.Commit();
                            hasil = new JsonResult
                            {
                                JSONMessage = "Data tersimpan",
                                JSONResult = true,
                                JSONRows = 1,
                                JSONValue = obj
                            };
                        }
                        catch (Exception ex)
                        {
                            hasil = new JsonResult
                            {
                                JSONMessage = ex.StackTrace,
                                JSONResult = false,
                                JSONRows = 0,
                                JSONValue = null
                            };
                        }
                    }
                }
            }
            return hasil;
        }

        JsonResult IUser.Get(long id)
        {
            JsonResult hasil = new JsonResult { 
                JSONResult = false, 
                JSONMessage = "Data tidak ditemukan", 
                JSONRows = 0, 
                JSONValue = null };
            List<DataFilters> filters = new List<DataFilters>();
            filters.Add(new DataFilters { 
                FieldName = "id", 
                FieldValue = id , 
                Operator = DataFilters.OperatorQuery.SamaDengan, 
                Separator = DataFilters.SeparatorQuery.And});
            MUser obj = getDataUser(filters);
            if (obj == null)
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data tidak ditemukan",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            } else
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data ditemukan",
                    JSONResult = true,
                    JSONRows = 1,
                    JSONValue = obj
                };
            }
            return hasil;
        }

        private MUser getDataUser(List<DataFilters> filters)
        {
            MUser obj = new MUser();
            using (NpgsqlConnection conn = GetConnection())
            {
                using (NpgsqlCommand com = new NpgsqlCommand())
                {
                    using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter())
                    {
                        using (DataTable dt = new DataTable())
                        {
                            conn.Open();
                            com.Connection = conn;
                            com.CommandTimeout = conn.ConnectionTimeout;
                            oDA.SelectCommand = com;

                            com.CommandText = "select muser.*" +
                                              " from muser where 1=1";
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                   select new MUser()
                                   {
                                       id = RepUtils.NullToInt(x["id"]),
                                       userid = RepUtils.NullToStr(x["userid"]),
                                       pwd = "not for publish",
                                       nama = RepUtils.NullToStr(x["nama"]),
                                       idkontak = RepUtils.NullToInt(x["idkontak"]),
                                       idrole = RepUtils.NullToInt(x["idrole"])
                                   }).SingleOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        private List<MUser> getListUser(List<DataFilters> filters)
        {
            List<MUser> obj = new List<MUser>();
            using (NpgsqlConnection conn = GetConnection())
            {
                using (NpgsqlCommand com = new NpgsqlCommand())
                {
                    using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter())
                    {
                        using (DataTable dt = new DataTable())
                        {
                            conn.Open();
                            com.Connection = conn;
                            com.CommandTimeout = conn.ConnectionTimeout;
                            oDA.SelectCommand = com;

                            com.CommandText = "select muser.*" +
                                              " from muser where 1=1";
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                   select new MUser()
                                   {
                                       id = RepUtils.NullToInt(x["id"]),
                                       userid = RepUtils.NullToStr(x["userid"]),
                                       pwd = "not for publish",
                                       nama = RepUtils.NullToStr(x["nama"]),
                                       idkontak = RepUtils.NullToInt(x["idkontak"]),
                                       idrole = RepUtils.NullToInt(x["idrole"])
                                   }).ToList();
                        }
                    }
                }
            }
            return obj;
        }

        public JsonResult GetByFilter(List<DataFilters> filters)
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MUser> obj = getListUser(filters);
            if (obj == null)
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data tidak ditemukan",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            }
            else
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data ditemukan",
                    JSONResult = true,
                    JSONRows = (obj != null ? obj.Count : 0),
                    JSONValue = obj
                };
            }
            return hasil;
        }

        public JsonResult GetLogin(string userid, string pwd)
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<DataFilters> filters = new List<DataFilters>();
            filters.Add(new DataFilters
            {
                FieldName = "userid",
                FieldValue = userid,
                Operator = DataFilters.OperatorQuery.SamaDengan,
                Separator = DataFilters.SeparatorQuery.And
            });
            MUser obj = getDataUser(filters);
            if (obj == null)
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data tidak ditemukan",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            }
            else
            {
                //Cek Pwdnya
                string PwdMD5Old = "", PwdMD5New = RepUtils.CreateMD5(pwd);
                using (Npgsql.NpgsqlConnection con = GetConnection())
                {
                    using (Npgsql.NpgsqlCommand com = new NpgsqlCommand())
                    {
                        con.Open();
                        com.Connection = con;
                        com.CommandTimeout = con.ConnectionTimeout;

                        com.CommandText = "select muser.pwd from muser where id=@id";
                        com.Parameters.Clear();
                        com.Parameters.AddWithValue("@id", obj.id);

                        PwdMD5Old = RepUtils.NullToStr(com.ExecuteScalar());
                    }
                }

                if (PwdMD5New == PwdMD5Old)
                {
                    hasil = new JsonResult
                    {
                        JSONMessage = "Data ditemukan",
                        JSONResult = true,
                        JSONRows = 1,
                        JSONValue = obj
                    };
                } else
                {
                    hasil = new JsonResult
                    {
                        JSONMessage = "Password anda salah",
                        JSONResult = false,
                        JSONRows = 0,
                        JSONValue = null
                    };
                }
            }
            return hasil;
        }

        public JsonResult GetChangePwd(MUser obj, string oldpwd, string newpwd)
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<DataFilters> filters = new List<DataFilters>();
            if (obj == null)
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Data tidak ditemukan",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            } else if (oldpwd == null || oldpwd == "")
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Password lama anda salah",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            }
            else if (newpwd == null || newpwd == "" || newpwd.Length < 8)
            {
                hasil = new JsonResult
                {
                    JSONMessage = "Password harus diisi minimum 8 digit",
                    JSONResult = false,
                    JSONRows = 0,
                    JSONValue = null
                };
            }
            else
            {
                //Cek Pwdnya
                string PwdMD5Old = RepUtils.CreateMD5(oldpwd), PwdMD5New = RepUtils.CreateMD5(newpwd);
                using (Npgsql.NpgsqlConnection con = GetConnection())
                {
                    using (Npgsql.NpgsqlCommand com = new NpgsqlCommand())
                    {
                        con.Open();
                        com.Connection = con;
                        com.CommandTimeout = con.ConnectionTimeout;

                        com.CommandText = "select muser.pwd from muser where id=@id";
                        com.Parameters.Clear();
                        com.Parameters.AddWithValue("@id", obj.id);

                        if (PwdMD5Old == RepUtils.NullToStr(com.ExecuteScalar()))
                        {
                            com.CommandText = "update muser set pwd=@pwd where id=@id";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@id", obj.id);
                            com.Parameters.AddWithValue("@pwd", PwdMD5New);
                            com.ExecuteNonQuery();

                            hasil = new JsonResult
                            {
                                JSONMessage = "Data berhasil diubah",
                                JSONResult = true,
                                JSONRows = 1,
                                JSONValue = obj
                            };
                        } else
                        {
                            hasil = new JsonResult
                            {
                                JSONMessage = "Password lama anda salah",
                                JSONResult = false,
                                JSONRows = 0,
                                JSONValue = null
                            };
                        }
                    }
                }
            }
            return hasil;
        }

        public JsonResult GetAvailableUser(string userid)
        {
            JsonResult hasil = new JsonResult { JSONResult = true, JSONMessage = "Error unknow resource", JSONRows = 0, JSONValue = null };
            long jmlData = 0;
            using (NpgsqlConnection conn = GetConnection())
            {
                using (NpgsqlCommand com = new NpgsqlCommand())
                {
                    using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter())
                    {
                        try
                        {
                            conn.Open();
                            com.Connection = conn;
                            com.CommandTimeout = conn.ConnectionTimeout;

                            com.CommandText = "select count(id) as jmldata from muser where upper(userid) = upper(@userid)";
                            com.Parameters.Clear();
                            com.Parameters.Add("@userid", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userid.ToUpper();
                            jmlData = RepUtils.NullToLong(com.ExecuteScalar());
                            if (jmlData >=1)
                            {
                                hasil = new JsonResult
                                {
                                    JSONMessage = "User is not Available",
                                    JSONResult = false,
                                    JSONRows = jmlData,
                                    JSONValue = null
                                };
                            } else
                            {
                                hasil = new JsonResult
                                {
                                    JSONMessage = "User is Available",
                                    JSONResult = true,
                                    JSONRows = 0,
                                    JSONValue = null
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            hasil = new JsonResult
                            {
                                JSONMessage = ex.StackTrace,
                                JSONResult = false,
                                JSONRows = 0,
                                JSONValue = null
                            };
                        }
                    }
                }
            }
            return hasil;
        }
    }
}
