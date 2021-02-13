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
    public class KontakContext : IKontak
    {
        public string ConnectionString { get; set; }

        public KontakContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        JsonResult IBase<MKontak>.Delete(Models.Dto.MKontak obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MKontak>.GetAll()
        {
            throw new NotImplementedException();

            //JsonResult hasil = new JsonResult
            //{
            //    JSONResult = false,
            //    JSONMessage = "Data tidak ditemukan",
            //    JSONRows = 0,
            //    JSONValue = null
            //};
            //List<MKontak> obj = getListKontak(new List<DataFilters>());
            //if (obj == null)
            //{
            //    hasil = new JsonResult
            //    {
            //        JSONMessage = "Data tidak ditemukan",
            //        JSONResult = false,
            //        JSONRows = 0,
            //        JSONValue = null
            //    };
            //}
            //else
            //{
            //    hasil = new JsonResult
            //    {
            //        JSONMessage = "Data ditemukan",
            //        JSONResult = true,
            //        JSONRows = (obj != null ? obj.Count : 0),
            //        JSONValue = obj
            //    };
            //}
            //return hasil;
        }

        JsonResult IKontak.GetByKode(long iduser, string kode)
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
                FieldName = "upper(kode)",
                FieldValue = kode.ToUpper(),
                Operator = DataFilters.OperatorQuery.SamaDengan,
                Separator = DataFilters.SeparatorQuery.And
            });
            MKontak obj = getDataKontak(iduser, filters);
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
                    JSONRows = 1,
                    JSONValue = obj
                };
            }
            return hasil;
        }

        JsonResult IKontak.GetByNama(long iduser, string nama)
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
                FieldName = "upper(nama)",
                FieldValue = nama.ToUpper(),
                Operator = DataFilters.OperatorQuery.SamaDengan,
                Separator = DataFilters.SeparatorQuery.And
            });
            MKontak obj = getDataKontak(iduser, filters);
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
                    JSONRows = 1,
                    JSONValue = obj
                };
            }
            return hasil;
        }

        JsonResult IKontak.Save(long iduser, Models.Dto.MKontak obj, ref ValidationError validationError)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            List<MKontak> list = new List<MKontak>();
            using (DataTable dt = new DataTable())
            {
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

                                //cek data sudah ada atau belum
                                com.CommandText = "select top 1 id from mkontak where right(hp, LENGTH(hp)-2) = @hp";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@hp", obj.hp);
                                long jml = RepUtils.NullToLong(com.ExecuteScalar());

                                if (jml == 0)
                                {
                                    com.CommandText = "select max(id) from mkontak";
                                    obj.id = RepUtils.NullToLong(com.ExecuteScalar()) + 1;
                                    com.CommandText = "insert into mkontak (id,kode,nama,alamat1,alamat2,alamat3,hp,telpon,iswhatsapp,norekening,bank,atasnamarekening)" +
                                                      "values (@id,@kode,@nama,@alamat1,@alamat2,@alamat3,@hp,@telpon,@iswhatsapp,@norekening,@bank,@atasnamarekening)";
                                    com.Parameters.Clear();
                                    com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                                    com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.kode.Trim();
                                    com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                                    com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat1.Trim();
                                    com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat2.Trim();
                                    com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat3.Trim();
                                    com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.hp.Trim();
                                    com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.telpon.Trim();
                                    com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.bank.Trim();
                                    com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.iswhatsapp;
                                    com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.norekening.Trim();
                                    com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.atasnamarekening.Trim();

                                    com.ExecuteNonQuery();
                                } else
                                {
                                    obj.id = jml;
                                }

                                com.CommandText = "insert into mkontakd (iduser,idkontak,nama,alamat1,alamat2,alamat3,hp,telpon,iswhatsapp,norekening,bank,atasnamarekening)" +
                                                  "values (@iduser,@id,@nama,@alamat1,@alamat2,@alamat3,@hp,@telpon,@iswhatsapp,@norekening,@bank,@atasnamarekening)";
                                com.Parameters.Clear();
                                com.Parameters.Add("@iduser", NpgsqlTypes.NpgsqlDbType.Bigint).Value = iduser;
                                com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                                com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.kode.Trim();
                                com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                                com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat1.Trim();
                                com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat2.Trim();
                                com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat3.Trim();
                                com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.hp.Trim();
                                com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.telpon.Trim();
                                com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.bank.Trim();
                                com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.iswhatsapp;
                                com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.norekening.Trim();
                                com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.atasnamarekening.Trim();

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
            }
            return hasil;
        }

        JsonResult IBase<MKontak>.Save(Models.Dto.MKontak obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IKontak.Update(long iduser, Models.Dto.MKontak obj, ref ValidationError validationError)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            List<MKontak> list = new List<MKontak>();
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

                            com.CommandText = "delete from mkontakd where idkontak=@id and iduser=@iduser";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@id", obj.id);
                            com.Parameters.AddWithValue("@iduser", iduser);
                            com.ExecuteNonQuery();

                            //cek data sudah ada atau belum
                            com.CommandText = "select top 1 id from mkontak where right(hp, LENGTH(hp)-2) = @hp";
                            com.Parameters.Clear();
                            com.Parameters.AddWithValue("@hp", obj.hp);
                            long jml = RepUtils.NullToLong(com.ExecuteScalar());

                            if (jml == obj.id)
                            {
                                com.CommandText = "update mkontak set kode=@kode,nama=@nama,alamat1=@alamat1,alamat2=@alamat2,alamat3=@alamat3,hp=@hp,telpon=@telpon,iswhatsapp=@iswhatsapp,norekening=@norekening,bank=@bank,atasnamarekening=@atasnamarekening where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                                com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.kode.Trim();
                                com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                                com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat1.Trim();
                                com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat2.Trim();
                                com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat3.Trim();
                                com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.hp.Trim();
                                com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.telpon.Trim();
                                com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.bank.Trim();
                                com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.iswhatsapp;
                                com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.norekening.Trim();
                                com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.atasnamarekening.Trim();

                                com.ExecuteNonQuery();
                            } else
                            {
                                if (jml == 0)
                                {
                                    com.CommandText = "select max(id) from mkontak";
                                    obj.id = RepUtils.NullToLong(com.ExecuteScalar()) + 1;
                                    com.CommandText = "insert into mkontak (id,kode,nama,alamat1,alamat2,alamat3,hp,telpon,iswhatsapp,norekening,bank,atasnamarekening)" +
                                                      "values (@id,@kode,@nama,@alamat1,@alamat2,@alamat3,@hp,@telpon,@iswhatsapp,@norekening,@bank,@atasnamarekening)";
                                    com.Parameters.Clear();
                                    com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                                    com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.kode.Trim();
                                    com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                                    com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat1.Trim();
                                    com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat2.Trim();
                                    com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat3.Trim();
                                    com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.hp.Trim();
                                    com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.telpon.Trim();
                                    com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.bank.Trim();
                                    com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.iswhatsapp;
                                    com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.norekening.Trim();
                                    com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.atasnamarekening.Trim();

                                    com.ExecuteNonQuery();
                                } else
                                {
                                    obj.id = jml;
                                }
                            }

                            com.CommandText = "insert into mkontakd (iduser,idkontak,nama,alamat1,alamat2,alamat3,hp,telpon,iswhatsapp,norekening,bank,atasnamarekening)" +
                                              "values (@iduser,@id,@nama,@alamat1,@alamat2,@alamat3,@hp,@telpon,@iswhatsapp,@norekening,@bank,@atasnamarekening)";
                            com.Parameters.Clear();
                            com.Parameters.Add("@iduser", NpgsqlTypes.NpgsqlDbType.Bigint).Value = iduser;
                            com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = obj.id;
                            com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.kode.Trim();
                            com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                            com.Parameters.Add("@alamat1", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat1.Trim();
                            com.Parameters.Add("@alamat2", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat2.Trim();
                            com.Parameters.Add("@alamat3", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.alamat3.Trim();
                            com.Parameters.Add("@hp", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.hp.Trim();
                            com.Parameters.Add("@telpon", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.telpon.Trim();
                            com.Parameters.Add("@bank", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.bank.Trim();
                            com.Parameters.Add("@iswhatsapp", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.iswhatsapp;
                            com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.norekening.Trim();
                            com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.atasnamarekening.Trim();

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

        JsonResult IBase<MKontak>.Update(Models.Dto.MKontak obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IKontak.Get(long iduser, long id)
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
            MKontak obj = getDataKontak(iduser, filters);
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

        private MKontak getDataKontak(long iduser, List<DataFilters> filters)
        {
            MKontak obj = new MKontak();
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

                            com.CommandText = "select mkontak.*" +
                                              " from mkontak inner join mkontakd on mkontakd.idkontak=mkontak.id where mkontakd.iduser=@iduser";
                            com.Parameters.AddWithValue("@iduser", iduser);
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                   select new MKontak()
                                   {
                                       id = RepUtils.NullToLong(x["id"]),
                                       kode = RepUtils.NullToStr(x["kode"]),
                                       nama = RepUtils.NullToStr(x["nama"]),
                                       alamat1 = RepUtils.NullToStr(x["alamat1"]),
                                       alamat2 = RepUtils.NullToStr(x["alamat2"]),
                                       alamat3 = RepUtils.NullToStr(x["alamat3"]),
                                       hp = RepUtils.NullToStr(x["hp"]),
                                       telpon = RepUtils.NullToStr(x["telpon"]),
                                       iswhatsapp = RepUtils.NullToBool(x["iswhatsapp"]),
                                       norekening = RepUtils.NullToStr(x["norekening"]),
                                       atasnamarekening = RepUtils.NullToStr(x["atasnamarekening"]),
                                       bank = RepUtils.NullToStr(x["bank"])
                                   }).SingleOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        private List<MKontak> getListKontak(long iduser, List<DataFilters> filters)
        {
            List<MKontak> obj = new List<MKontak>();
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

                            com.CommandText = "select mkontak.*" +
                                              " from mkontak inner join mkontakd on mkontakd.idkontak=mkontak.id where mkontakd.iduser=@iduser";
                            com.Parameters.AddWithValue("@iduser", iduser);
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                   select new MKontak()
                                   {
                                       id = RepUtils.NullToLong(x["id"]),
                                       kode = RepUtils.NullToStr(x["kode"]),
                                       nama = RepUtils.NullToStr(x["nama"]),
                                       alamat1 = RepUtils.NullToStr(x["alamat1"]),
                                       alamat2 = RepUtils.NullToStr(x["alamat2"]),
                                       alamat3 = RepUtils.NullToStr(x["alamat3"]),
                                       hp = RepUtils.NullToStr(x["hp"]),
                                       telpon = RepUtils.NullToStr(x["telpon"]),
                                       iswhatsapp = RepUtils.NullToBool(x["iswhatsapp"]),
                                       norekening = RepUtils.NullToStr(x["norekening"]),
                                       atasnamarekening = RepUtils.NullToStr(x["atasnamarekening"]),
                                       bank = RepUtils.NullToStr(x["bank"])
                                   }).ToList();
                        }
                    }
                }
            }
            return obj;
        }

        public JsonResult GetByFilter(long iduser, List<DataFilters> filters)
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MKontak> obj = getListKontak(iduser, filters);
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

        public JsonResult GetAll(long iduser)
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MKontak> obj = getListKontak(iduser, new List<DataFilters>());
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
    }
}
