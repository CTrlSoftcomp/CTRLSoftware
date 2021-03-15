using CTrlSoft.Models;
using CTrlSoft.Models.Dto;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CTrlSoft.BLL.System.Role
{
    public class RepRole : IRole
    {
        private Newtonsoft.Json.Formatting formatting = new Newtonsoft.Json.Formatting();
        public string StrKon { get; set; }
        public RepRole(string konString)
        {
            this.StrKon = konString;
        }

        public JsonResult List(List<DataFilters> filters)
        {
            JsonResult jsonResult = new JsonResult();
            var dapper = Repository.RepSqlDatabase.OperatorSQL(filters);
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    var data = db.Query<MRole>("select * from mrole where 1=1" + dapper.SQL, dapper.Parameter, null, true, db.ConnectionTimeout).ToList();
                    if (data != null)
                    {
                        foreach (MRole item in data)
                        {
                            var list = db.Query<MRoleD>("select * from mroled where idrole=@id", new { item.id }, null, true, db.ConnectionTimeout).ToList();
                            item.mrolesd = list;
                        }

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

        public JsonResult Get(int id)
        {
            JsonResult jsonResult = new JsonResult();
            using (IDbConnection db = new NpgsqlConnection(StrKon))
            {
                try
                {
                    var data = db.Query<MRole>("select * from mrole where id=@Id", new { id }, null, true, db.ConnectionTimeout).SingleOrDefault();
                    if (data != null)
                    {
                        var list = db.Query<MRoleD>("select * from mroled where idrole=@id", new { id }, null, true, db.ConnectionTimeout).ToList();
                        data.mrolesd = list;

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

        public JsonResult Save(MRole obj, MUser user)
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
                                    if (obj.role.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data role yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.mrolesd == null || obj.mrolesd.Count == 0)
                                    {
                                        jsonResult.JSONMessage = "Data role menu yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from mrole where upper(role)=@name";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@role", obj.role.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data role yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done
                                com.CommandText = "select max(id) from mrole";
                                obj.id = Repository.RepUtils.NullToInt(com.ExecuteScalar()) + 1;
                                com.CommandText = "insert into mrole (id, role, issupervisor) values (@id, @role, @issupervisor)";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@role", obj.role);
                                com.Parameters.AddWithValue("@issupervisor", obj.issupervisor);
                                com.ExecuteNonQuery();

                                com.CommandText = "delete from mroled where idrole=@idrole";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@idrole", obj.id);
                                com.ExecuteNonQuery();

                                foreach (MRoleD item in obj.mrolesd)
                                {
                                    if (item.aktif)
                                    {
                                        com.CommandText = "insert into mroled (idrole, idmenu, aktif) values (@idrole, @idmenu, @aktif)";
                                        com.Parameters.AddWithValue("@idrole", obj.id);
                                        com.Parameters.AddWithValue("@idmenu", item.idmenu);
                                        com.Parameters.AddWithValue("@aktif", item.aktif);
                                        com.ExecuteNonQuery();
                                    }
                                }

                                jsonResult.JSONMessage = "Data role id=" + obj.id + " ditambahkan";
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

        public JsonResult Delete(MRole obj, MUser user)
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

                                if (obj.id == 1 || obj.role.ToUpper().Equals("SYSADM"))
                                {
                                    jsonResult.JSONMessage = "Data role SYSADM tidak boleh dihapus.";
                                    validasi = false;
                                }

                                if (validasi)
                                {
                                    com.CommandText = "delete from mroled where idrole=@id";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@id", obj.id);
                                    com.ExecuteNonQuery();

                                    com.CommandText = "delete from mrole where id=@id";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@id", obj.id);
                                    com.ExecuteNonQuery();

                                    jsonResult.JSONMessage = "Data role id=" + obj.id + " dihapus";
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

        public JsonResult Update(MRole obj, MUser user)
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
                                    if (obj.id == 1 || obj.role.ToUpper().Equals("SYSADM"))
                                    {
                                        jsonResult.JSONMessage = "Data role SYSADM tidak boleh diubah.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.role.Length <= 0)
                                    {
                                        jsonResult.JSONMessage = "Data role yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    if (obj.mrolesd == null || obj.mrolesd.Count == 0)
                                    {
                                        jsonResult.JSONMessage = "Data role menu yang anda masukkan harus terisi.";
                                        validasi = false;
                                    }
                                }
                                if (validasi)
                                {
                                    com.CommandText = "select count(id) from mrole where id<>@id and upper(role)=@name";
                                    com.Parameters.Clear();
                                    com.Parameters.AddWithValue("@id", obj.id);
                                    com.Parameters.AddWithValue("@role", obj.role.ToUpper());
                                    if ((long)com.ExecuteScalar() >= 1)
                                    {
                                        jsonResult.JSONMessage = "Data role yang anda masukkan sudah terdaftar.";
                                        validasi = false;
                                    }
                                }
                                //Validasi done
                                com.CommandText = "update mrole set role=@role, issupervisor=@issupervisor where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", obj.id);
                                com.Parameters.AddWithValue("@role", obj.role);
                                com.Parameters.AddWithValue("@issupervisor", obj.issupervisor);
                                com.ExecuteNonQuery();

                                com.CommandText = "delete from mroled where idrole=@idrole";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@idrole", obj.id);
                                com.ExecuteNonQuery();

                                foreach (MRoleD item in obj.mrolesd)
                                {
                                    if (item.aktif)
                                    {
                                        com.CommandText = "insert into mroled (idrole, idmenu, aktif) values (@idrole, @idmenu, @aktif)";
                                        com.Parameters.AddWithValue("@idrole", obj.id);
                                        com.Parameters.AddWithValue("@idmenu", item.idmenu);
                                        com.Parameters.AddWithValue("@aktif", item.aktif);
                                        com.ExecuteNonQuery();
                                    }
                                }

                                jsonResult.JSONMessage = "Data role id=" + obj.id + " diubah";
                                jsonResult.JSONResult = true;
                                jsonResult.JSONRows = 1;
                                jsonResult.JSONValue = obj;

                                Repository.RepLogData.insertLog(com,
                                    Newtonsoft.Json.JsonConvert.SerializeObject(user, formatting),
                                    Newtonsoft.Json.JsonConvert.SerializeObject(jsonResult, formatting));

                                jsonResult.JSONMessage = "Data berhasil diubah";
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
