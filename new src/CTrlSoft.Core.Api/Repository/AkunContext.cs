using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Models;
using CTrlSoft.Core.Api.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.Data;

namespace CTrlSoft.Core.Api.Repository
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
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            List<Akun> list = new List<Akun>();
            using (NpgsqlConnection conn = GetConnection())
            {
                using (NpgsqlCommand com = new NpgsqlCommand())
                {
                    using (NpgsqlDataAdapter oDA = new NpgsqlDataAdapter())
                    {
                        using (DataTable dt = new DataTable())
                        {
                            try
                            {
                                conn.Open();
                                com.Connection = conn;
                                com.CommandTimeout = conn.ConnectionTimeout;
                                oDA.SelectCommand = com;

                                com.CommandText = "select makun.*" +
                                                  " from makun";
                                oDA.Fill(dt);

                                list = (from DataRow x in dt.Rows
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
                                hasil = new JsonResult
                                {
                                    JSONMessage = "Data ditemukan",
                                    JSONResult = true,
                                    JSONRows = list.Count,
                                    JSONValue = list
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

        JsonResult IAkun.GetByName(string name)
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
                FieldValue = "%" + name.ToUpper() + "%",
                Operator = DataFilters.OperatorQuery.Like,
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
            throw new NotImplementedException();
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
                            try
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
                            catch (Exception ex)
                            {
                                RepSqlDatabase.LogErrorQuery(ex.Source, ex);
                                obj = null;
                            }
                        }
                    }
                }
            }
            return obj;
        }

    }
}
