using System;

namespace CTrlSoft.Models
{
    public class JsonResult
    {
        public bool JSONResult { get; set; }
        public string JSONMessage { get; set; }
        public long JSONRows { get; set; }
        public object JSONValue { get; set; }
        public JsonResult() {
            JSONResult  = false;
            JSONMessage = "Data tidak ditemukan";
            JSONRows    = 0;
            JSONValue   = null;
        }
    }
}