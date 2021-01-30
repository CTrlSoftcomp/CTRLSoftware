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
    public class AkunContext : IAkun
    {
        public string ConnectionString { get; set; }

        public AkunContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        JsonResult IBase<Akun>.Delete(Models.Dto.Akun obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<Akun>.GetAll()
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<Akun> obj = getListAkun(new List<DataFilters>());
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

        JsonResult IAkun.GetByKode(string kode)
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
            Akun obj = getDataAkun(filters);
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

        JsonResult IAkun.GetByNama(string nama)
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
            Akun obj = getDataAkun(filters);
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

        JsonResult IAkun.Save(Models.Dto.Akun obj, ref ValidationError validationError)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<Akun>.Save(Models.Dto.Akun obj)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            List<Akun> list = new List<Akun>();
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

                            com.CommandText = "insert into makun (id,kode,nama,idparent,iddepartemen,keterangan,idtype,isdebet,iskasbank,norekening,atasnamarekening,idtypebank,isneraca)" +
                                                "values ()";
                            com.Parameters.Clear();
                            com.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                            com.Parameters.Add("@kode", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                            com.Parameters.Add("@nama", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.nama.Trim();
                            com.Parameters.Add("@idparent", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idparent;
                            com.Parameters.Add("@iddepartemen", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.iddepartemen;
                            com.Parameters.Add("@keterangan", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.keterangan.Trim();
                            com.Parameters.Add("@idtype", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idtype;
                            com.Parameters.Add("@isdebet", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.isdebet;
                            com.Parameters.Add("@iskasbank", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.iskasbank;
                            com.Parameters.Add("@norekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.norekening.Trim();
                            com.Parameters.Add("@atasnamarekening", NpgsqlTypes.NpgsqlDbType.Varchar).Value = obj.atasnamarekening.Trim();
                            com.Parameters.Add("@idtypebank", NpgsqlTypes.NpgsqlDbType.Integer).Value = obj.idtypebank;
                            com.Parameters.Add("@isneraca", NpgsqlTypes.NpgsqlDbType.Boolean).Value = obj.isneraca;

                            com.ExecuteNonQuery();
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


        JsonResult IAkun.Update(Models.Dto.Akun obj, ref ValidationError validationError)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<Akun>.Update(Models.Dto.Akun obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IAkun.Get(long id)
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
            Akun obj = getDataAkun(filters);
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

        private Akun getDataAkun(List<DataFilters> filters)
        {
            Akun obj = new Akun();
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

                            com.CommandText = "select makun.*" +
                                              " from makun where 1=1";
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                   select new Akun()
                                   {
                                       id = RepUtils.NullToLong(x["id"]),
                                       kode = RepUtils.NullToStr(x["kode"]),
                                       nama = RepUtils.NullToStr(x["nama"]),
                                       idparent = RepUtils.NullToLong(x["idparent"]),
                                       iddepartemen = RepUtils.NullToInt(x["iddepartemen"]),
                                       keterangan = RepUtils.NullToStr(x["keterangan"]),
                                       idtype = RepUtils.NullToInt(x["idtype"]),
                                       isdebet = RepUtils.NullToBool(x["isdebet"]),
                                       iskasbank = RepUtils.NullToBool(x["iskasbank"]),
                                       norekening = RepUtils.NullToStr(x["norekening"]),
                                       atasnamarekening = RepUtils.NullToStr(x["atasnamarekening"]),
                                       idtypebank = RepUtils.NullToInt(x["idtypebank"]),
                                       isneraca = RepUtils.NullToBool(x["isneraca"])
                                   }).SingleOrDefault();
                        }
                    }
                }
            }
            return obj;
        }

        private List<Akun> getListAkun(List<DataFilters> filters)
        {
            List<Akun> obj = new List<Akun>();
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

                            com.CommandText = "select makun.*" +
                                              " from makun where 1=1";
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                   select new Akun()
                                   {
                                       id = RepUtils.NullToLong(x["id"]),
                                       kode = RepUtils.NullToStr(x["kode"]),
                                       nama = RepUtils.NullToStr(x["nama"]),
                                       idparent = RepUtils.NullToLong(x["idparent"]),
                                       iddepartemen = RepUtils.NullToInt(x["iddepartemen"]),
                                       keterangan = RepUtils.NullToStr(x["keterangan"]),
                                       idtype = RepUtils.NullToInt(x["idtype"]),
                                       isdebet = RepUtils.NullToBool(x["isdebet"]),
                                       iskasbank = RepUtils.NullToBool(x["iskasbank"]),
                                       norekening = RepUtils.NullToStr(x["norekening"]),
                                       atasnamarekening = RepUtils.NullToStr(x["atasnamarekening"]),
                                       idtypebank = RepUtils.NullToInt(x["idtypebank"]),
                                       isneraca = RepUtils.NullToBool(x["isneraca"])
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
            List<Akun> obj = getListAkun(filters);
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
