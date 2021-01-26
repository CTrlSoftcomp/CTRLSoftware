using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Models
{
    public class DataFilters
    {
        public enum OperatorQuery
        {
            SamaDengan = 0,
            Like = 1,
            KurangDari = 2,
            LebihDari = 3,
            Between = 4,
            KurangDariSamaDengan = 5,
            LebihDariSamaDengan = 6
        }
        public enum SeparatorQuery
        {
            And = 0,
            Or = 1
        }
        public string FieldName { get; set; }
        public object FieldValue { get; set; }
        public OperatorQuery Operator { get; set; }
        public SeparatorQuery Separator { get; set; }
    }
}
