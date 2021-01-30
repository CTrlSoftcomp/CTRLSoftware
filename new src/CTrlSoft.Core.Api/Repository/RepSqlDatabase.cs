using CTrlSoft.Core.Api.Models;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CTrlSoft.Core.Api.Repository
{
    public class RepSqlDatabase
    {
        public static NpgsqlCommand OperatorSQL(NpgsqlCommand com, List<DataFilters> filters)
        {
            int ifieldname = 0;

            foreach (var item in filters)
            {
                switch (item.Operator)
                {
                    case DataFilters.OperatorQuery.Like:
                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or " + item.FieldName + " ilike @" + ifieldname;
                                break;
                            default:
                                com.CommandText += " and " + item.FieldName + " ilike @" + ifieldname;
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, item.FieldValue);
                        ifieldname += 1;
                        break;
                    case DataFilters.OperatorQuery.KurangDari:
                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or " + item.FieldName + "<@" + ifieldname;
                                break;
                            default:
                                com.CommandText += " and " + item.FieldName + "<@" + ifieldname;
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, item.FieldValue);
                        ifieldname += 1;
                        break;
                    case DataFilters.OperatorQuery.LebihDari:
                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or " + item.FieldName + ">@" + ifieldname;
                                break;
                            default:
                                com.CommandText += " and " + item.FieldName + ">@" + ifieldname;
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, item.FieldValue);
                        ifieldname += 1;
                        break;
                    case DataFilters.OperatorQuery.Between:
                        List<Object> Objs = (List<Object>)item.FieldValue;

                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or (" + item.FieldName + " between @" + ifieldname + " and @" + (ifieldname + 1) + ")";
                                break;
                            default:
                                com.CommandText += " and (" + item.FieldName + " between @" + ifieldname + " and @" + (ifieldname + 1) + ")";
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, Objs.FirstOrDefault());
                        ifieldname += 1;
                        com.Parameters.AddWithValue("@" + ifieldname, Objs.LastOrDefault());
                        ifieldname += 1;
                        break;
                    case DataFilters.OperatorQuery.KurangDariSamaDengan:
                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or " + item.FieldName + "<=@" + ifieldname;
                                break;
                            default:
                                com.CommandText += " and " + item.FieldName + "<=@" + ifieldname;
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, item.FieldValue);
                        ifieldname += 1;
                        break;
                    case DataFilters.OperatorQuery.LebihDariSamaDengan:
                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or " + item.FieldName + ">=@" + ifieldname;
                                break;
                            default:
                                com.CommandText += " and " + item.FieldName + ">=@" + ifieldname;
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, item.FieldValue);
                        ifieldname += 1;
                        break;
                    default:
                        switch (item.Separator)
                        {
                            case DataFilters.SeparatorQuery.Or:
                                com.CommandText += " or " + item.FieldName + "=@" + ifieldname;
                                break;
                            default:
                                com.CommandText += " and " + item.FieldName + "=@" + ifieldname;
                                break;
                        }
                        com.Parameters.AddWithValue("@" + ifieldname, item.FieldValue);
                        ifieldname += 1;
                        break;
                }
            }

            return com;
        }

        public async static void LogErrorQuery(
            IWebHostEnvironment environment, 
            string method, 
            Exception e)
        {
            DateTime dateTime = System.DateTime.Now;
            
            string fileName = Path.GetFullPath(environment.ContentRootPath + "/Log/LogSQL_" + dateTime.ToString("yyMMdd") + ".txt");
            await using (StreamWriter streamWriter = new StreamWriter(fileName, true))
            {
                try
                {
                    streamWriter.AutoFlush = true;
                    streamWriter.WriteLine(dateTime.ToString("yy-MM-dd HH:mm:ss") + " [" + method + "]" + e.Message);
                    streamWriter.Flush();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
