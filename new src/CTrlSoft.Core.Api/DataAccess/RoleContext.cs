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
    public class RoleContext : IRole
    {
        public string ConnectionString { get; set; }

        public RoleContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        JsonResult IRole.Get(long id)
        {
            JsonResult hasil = new JsonResult { JSONResult = false, JSONMessage = "Data tidak ditemukan", JSONRows = 0, JSONValue = null };
            MRole Obj = new MRole();
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

                                com.CommandText = "select mrole.*" +
                                                  " from mrole where id=@id";
                                com.Parameters.Clear();
                                com.Parameters.AddWithValue("@id", id);

                                oDA.Fill(dt);

                                Obj = (from DataRow x in dt.Rows
                                       select new MRole()
                                       {
                                           id = RepUtils.NullToInt(x["id"]),
                                           role = RepUtils.NullToStr(x["role"]),
                                           issupervisor = RepUtils.NullToBool(x["issupervisor"])
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

        JsonResult IBase<MRole>.Save(MRole obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MRole>.Update(MRole obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MRole>.Delete(MRole obj)
        {
            throw new NotImplementedException();
        }

        JsonResult IBase<MRole>.GetAll()
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MRole> obj = getList(new List<DataFilters>());
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

        public JsonResult GetByFilter(List<DataFilters> filters)
        {
            JsonResult hasil = new JsonResult
            {
                JSONResult = false,
                JSONMessage = "Data tidak ditemukan",
                JSONRows = 0,
                JSONValue = null
            };
            List<MRole> obj = getList(filters);
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

        private List<MRole> getList(List<DataFilters> filters)
        {
            List<MRole> obj = new List<MRole>();
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

                            com.CommandText = "select mrole.*" +
                                              " from mrole where 1=1";
                            RepSqlDatabase.OperatorSQL(com, filters);
                            oDA.Fill(dt);

                            obj = (from DataRow x in dt.Rows
                                    select new MRole()
                                    {
                                        id = RepUtils.NullToInt(x["id"]),
                                        role = RepUtils.NullToStr(x["role"]),
                                        issupervisor = RepUtils.NullToBool(x["issupervisor"])
                                    }).ToList();
                        }
                    }
                }
            }
            return obj;
        }
    }
}
