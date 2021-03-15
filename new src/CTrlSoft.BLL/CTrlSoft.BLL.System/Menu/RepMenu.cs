using CTrlSoft.Models;
using CTrlSoft.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using System.Data;
using Dapper;

namespace CTrlSoft.BLL.System.Menu
{
    public class RepMenu : IMenu
    {
        private Newtonsoft.Json.Formatting formatting = new Newtonsoft.Json.Formatting();
        public string StrKon { get; set; }
        public RepMenu(string konString)
        {
            this.StrKon = konString;
        }
        public JsonResult Delete(MMenu obj, MUser user)
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

                                com.CommandText = "update mmenu set aktif=0 where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data menu id=" + obj.id + " dihapus";
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
                    jsonResult.JSONValue = db.Query<MMenu>("select * from mmenu where id=@Id", new { noid }, null, true, db.ConnectionTimeout).SingleOrDefault();
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
            JsonResult jsonResult = new JsonResult();
            var dapper = Repository.RepSqlDatabase.OperatorSQL(filters);
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    var data = db.Query<MMenu>("select * from mmenu where 1=1" + dapper.SQL, dapper.Parameter, null, true, db.ConnectionTimeout).ToList();
                    if (data != null)
                    {
                        jsonResult.JSONMessage = "Data ditemukan.";
                        jsonResult.JSONResult = true;
                        jsonResult.JSONRows = data.Count;
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

        public JsonResult Save(MMenu obj, MUser user)
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
                                    if (obj.name.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data name yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.caption.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data caption yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.idparent < -1 || obj.idparent == 0)
                                    {
                                        jsonResult.JSONMessage = "Data parent yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from mmenu where upper(name)=@name or upper(caption)=@caption";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@name", obj.name.ToUpper());
                                    com.Parameters.AddWithValue("@caption", obj.caption.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data name atau caption yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done
                                com.CommandText = "select max(id) from mmenu";
                                obj.id = Repository.RepUtils.NullToInt(com.ExecuteScalar()) + 1;
                                com.CommandText = "insert into mmenu (id, idparent, nourut, name, caption, isbig, isbegingroup, aktif) values (@id, @idparent, @nourut, @name, @caption, @isbig, @isbegingroup, @aktif)";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@idparent", obj.idparent);
                                com.Parameters.AddWithValue("@nourut", obj.nourut);
                                com.Parameters.AddWithValue("@name", obj.name);
                                com.Parameters.AddWithValue("@caption", obj.caption);
                                com.Parameters.AddWithValue("@isbig", obj.isbig);
                                com.Parameters.AddWithValue("@isbegingroup", obj.isbegingroup);
                                com.Parameters.AddWithValue("@aktif", obj.aktif);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data menu id=" + obj.id + " ditambahkan";
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

        public JsonResult Update(MMenu obj, MUser user)
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
                                    if (obj.name.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data name yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.caption.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data caption yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.idparent < -1 || obj.idparent == 0)
                                    {
                                        jsonResult.JSONMessage = "Data parent yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from mmenu where id<>@id and upper(name)=@name or upper(caption)=@caption";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@name", obj.name.ToUpper());
                                    com.Parameters.AddWithValue("@caption", obj.caption.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data name atau caption yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done

                                com.CommandText = "update mmenu set idparent=@idparent, nourut=@nourut, name=@name, caption=@caption, isbig=@isbig, isbegingroup=@isbegingroup, aktif=@aktif where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@idparent", obj.idparent);
                                com.Parameters.AddWithValue("@nourut", obj.nourut);
                                com.Parameters.AddWithValue("@name", obj.name);
                                com.Parameters.AddWithValue("@caption", obj.caption);
                                com.Parameters.AddWithValue("@isbig", obj.isbig);
                                com.Parameters.AddWithValue("@isbegingroup", obj.isbegingroup);
                                com.Parameters.AddWithValue("@aktif", obj.aktif);
                                com.ExecuteNonQuery();

                                jsonResult.JSONMessage = "Data menu id=" + obj.id + " ditambahkan";
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
