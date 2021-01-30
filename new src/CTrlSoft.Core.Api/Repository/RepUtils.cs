using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace CTrlSoft.Core.Api.Repository
{
    public class RepUtils
    {
        public static double NullToDbl(object value)
        {
            double hasil = 0.0;
            if (Information.IsNumeric(value))
            {
                hasil = System.Convert.ToDouble(value);
            }
            else
            {
                hasil = 0.0;
            }

            return hasil;
        }
        public static bool NullToBool(object value)
        {
            bool hasil = false;
            try
            {
                hasil = System.Convert.ToBoolean(value);
            }
            catch (Exception ex)
            {
                hasil = false;
                ex.ToString();
            }
            return hasil;
        }
        public static string NullToStr(object value)
        {
            String hasil = "";
            if (value != null)
            {
                hasil = value.ToString();
            }
            else
            {
                hasil = "";
            }
            return hasil;
        }
        public static long NullToLong(object value)
        {
            long hasil = 0;
            if (Information.IsNumeric(value))
            {
                hasil = System.Convert.ToInt32(value);
            }
            else
            {
                hasil = 0;
            }

            return hasil;
        }
        public static int NullToInt(object value)
        {
            int hasil = 0;
            if (Information.IsNumeric(value))
            {
                hasil = System.Convert.ToInt16(value);
            }
            else
            {
                hasil = 0;
            }

            return hasil;
        }
        public static DateTime NullToDate(object value)
        {
            DateTime hasil = System.Convert.ToDateTime("1900-01-01");
            if (Information.IsDate(value))
            {
                hasil = System.Convert.ToDateTime(value);
            }
            else
            {
                hasil = System.Convert.ToDateTime("1900-01-01");
            }

            return hasil;
        }
    }

    public interface IPathProvider
    {
        string MapPath(string path);
    }
    public class PathProvider : IPathProvider
    {
        private IWebHostEnvironment _hostEnvironment;

        public PathProvider(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }

        public string MapPath(string path)
        {
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, path);
            return filePath;
        }
    }
}
