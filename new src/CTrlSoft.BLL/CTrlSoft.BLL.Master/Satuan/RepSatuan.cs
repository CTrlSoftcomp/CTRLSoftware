using CTrlSoft.Models;
using CTrlSoft.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Data;
using Dapper;

namespace CTrlSoft.BLL.Master.Satuan
{
    public class RepSatuan : ISatuan
    {
        private Newtonsoft.Json.Formatting formatting = new Newtonsoft.Json.Formatting();
        public string StrKon { get; set; }
        public RepSatuan(string konString)
        {
            this.StrKon = konString;
        }

        public JsonResult Delete(MSatuan obj, MUser user)
        {
            JsonResult jsonResult = new JsonResult();
            if (user != null && user.id >=1)
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

                                com.CommandText = "update msatuan set aktif=0 where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.ExecuteNonQuery();
                                
                                jsonResult.JSONMessage = "Data satuan id="+ obj.id +" dihapus";
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
                            catch (Exception e) {
                                jsonResult.JSONMessage = "ERR : " + e.Message;
                                jsonResult.JSONResult = false;
                                jsonResult.JSONRows = 0;
                                jsonResult.JSONValue = null;
                            }
                        }
                    }
                }
            } else
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
                    jsonResult.JSONValue = db.Query<MSatuan>("select * from msatuan where id=@Id", new { noid }, null, true, db.ConnectionTimeout).SingleOrDefault();
                    if (jsonResult.JSONValue != null)
                    {
                        jsonResult.JSONMessage = "Data ditemukan.";
                        jsonResult.JSONResult = true;
                        jsonResult.JSONRows = 1;
                        jsonResult.JSONValue = jsonResult.JSONValue;
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
            List<MSatuan> list = new List<MSatuan>();
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

                        com.CommandText = "select id, kode, nama, aktif, konversi from msatuan where 1=1";
                        com = Repository.RepSqlDatabase.OperatorSQL(com, filters);

                        using (NpgsqlDataReader reader = com.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (!reader.Read())
                                {
                                    list.Add(new MSatuan
                                    {
                                        id = reader.GetInt64(0),
                                        kode = reader.GetString(1),
                                        nama = reader.GetString(2),
                                        aktif = reader.GetBoolean(3),
                                        konversi = reader.GetInt32(4)
                                    });
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

        public JsonResult Save(MSatuan obj, MUser user)
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
                                    if (obj.kode.Length<=0)
                                    {
                                        jsonResult.JSONMessage = "Data kode yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.nama.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data nama yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.konversi <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data konversi yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from msatuan where upper(kode)=@kode or upper(nama)=@nama";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@kode", obj.kode.ToUpper());
                                    com.Parameters.AddWithValue("@nama", obj.nama.ToUpper());
                                    if ((long) com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data kode atau nama yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done

                                obj.id = Repository.RepUtils.NullToLong(DateTime.UtcNow.ToString("yyMMddHHmmssfff"));
                                com.CommandText = "insert into msatuan (id, kode, nama, aktif, konversi) values (@id, @kode, @nama, @aktif, @konversi)";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@kode", obj.kode);
                                com.Parameters.AddWithValue("@nama", obj.nama);
                                com.Parameters.AddWithValue("@aktif", obj.aktif);
                                com.Parameters.AddWithValue("@konversi", obj.konversi);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data satuan id=" + obj.id + " ditambahkan";
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
                jsonResult.JSONMessage = "User untuk save data harus diisi";
                jsonResult.JSONResult = false;
                jsonResult.JSONRows = 0;
                jsonResult.JSONValue = null;
            }

            return jsonResult;
        }

        public JsonResult Update(MSatuan obj, MUser user)
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
                                    if (obj.kode.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data kode yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.nama.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data nama yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.konversi <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data konversi yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from msatuan where id<>@id and upper(kode)=@kode or upper(nama)=@nama";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@id", obj.id);
                                    com.Parameters.AddWithValue("@kode", obj.kode.ToUpper());
                                    com.Parameters.AddWithValue("@nama", obj.nama.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data kode atau nama yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done

                                com.CommandText = "update msatuan set kode=@kode, nama=@nama, aktif=@aktif, konversi=@konversi where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@kode", obj.kode);
                                com.Parameters.AddWithValue("@nama", obj.nama);
                                com.Parameters.AddWithValue("@aktif", obj.aktif);
                                com.Parameters.AddWithValue("@konversi", obj.konversi);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data satuan id=" + obj.id + " diupdate";
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
                jsonResult.JSONMessage = "User untuk update data harus diisi";
                jsonResult.JSONResult = false;
                jsonResult.JSONRows = 0;
                jsonResult.JSONValue = null;
            }

            return jsonResult;
        }
    }
}
