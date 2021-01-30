using CTrlSoft.Core.Api.Bll;
using CTrlSoft.Core.Api.Models;
using CTrlSoft.Core.Api.Models.Dto;
using CTrlSoft.Core.Api.Repository;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CTrlSoft.Core.Api.DataAccess
{
    public class JenisTransaksiContext : IJenisTransaksi
    {
        public string ConnectionString { get; set; }

        public JenisTransaksiContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        JsonResult IJenisTransaksi.Get(long id)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            MJenisTransaksi Obj = new MJenisTransaksi();
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

                                com.CommandText = "select mjenistransaksi.*" +
                                                  " from mjenistransaksi where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", id);

                                oDA.Fill(dt);

                                Obj = (from DataRow x in dt.Rows
                                       select new MJenisTransaksi()
                                       {
                                           id = RepUtils.NullToLong(x["id"]),
                                           kode = RepUtils.NullToStr(x["kode"]),
                                           nama = RepUtils.NullToStr(x["nama"]),
                                           nourut = RepUtils.NullToInt(x["nourut"]),
                                           keterangan = RepUtils.NullToStr(x["keterangan"]),
                                           jenistransaksid = getDetil(com, oDA, RepUtils.NullToLong(x["id"]))
                                       }).SingleOrDefault();
                                hasil = new JsonResult
                                {
                                    JSONMessage = "Data ditemukan",
                                    JSONResult = true,
                                    JSONRows = (Obj == null ? 0 : 1),
                                    JSONValue = Obj
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

        JsonResult IBase<MJenisTransaksi>.Save(MJenisTransaksi obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MJenisTransaksi>.Update(MJenisTransaksi obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MJenisTransaksi>.Delete(MJenisTransaksi obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MJenisTransaksi>.GetAll()
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MJenisTransaksi> obj = getList(new List<DataFilters>());
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

        private List<MJenisTransaksiD> getDetil(NpgsqlCommand com, NpgsqlDataAdapter oDA, long header)
        {
            List<MJenisTransaksiD> detil = new List<MJenisTransaksiD>();
            com.CommandText = "select * from mjenistransaksid where idjenistransaksi=@noid";
            com.Parameters.Clear();
            com.Parameters.AddWithValue("@noid", header);
            using (DataTable dt = new DataTable())
            {
                oDA.SelectCommand = com;
                oDA.Fill(dt);

                detil = (from DataRow x in dt.Rows
                        select new MJenisTransaksiD()
                        {
                            nourut = RepUtils.NullToInt(x["nourut"]),
                            idjenistransaksi = RepUtils.NullToLong(x["idjenistransaksi"]),
                            digit = RepUtils.NullToInt(x["digit"]),
                            format = RepUtils.NullToStr(x["format"]),
                            jenis = RepUtils.NullToStr(x["jenis"]),
                            prefix = RepUtils.NullToStr(x["prefix"]),
                            sufix = RepUtils.NullToStr(x["sufix"])
                        }).ToList();
            }
            return detil;
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
            List<MJenisTransaksi> obj = getList(filters);
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

        private List<MJenisTransaksi> getList(List<DataFilters> filters)
        {
            List<MJenisTransaksi> obj = new List<MJenisTransaksi>();
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

                            com.CommandText = "select mjenistransaksi.*" +
                                              " from mjenistransaksi where 1=1";
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                    select new MJenisTransaksi()
                                    {
                                        id = RepUtils.NullToLong(x["id"]),
                                        kode = RepUtils.NullToStr(x["kode"]),
                                        nama = RepUtils.NullToStr(x["nama"]),
                                        nourut = RepUtils.NullToInt(x["nourut"]),
                                        keterangan = RepUtils.NullToStr(x["keterangan"]),
                                        jenistransaksid = getDetil(com, oDA, RepUtils.NullToLong(x["id"]))
                                    }).ToList();
                        }
                    }
                }
            }
            return obj;
        }
    }
}
