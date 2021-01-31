using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Models.Dto
{
    public class MAkun : Basic
    {
        public long idparent { get; set; }
        public int iddepartemen { get; set; }
        public string keterangan { get; set; }
        public int idtype { get; set; }
        public bool isdebet { get; set; }
        public bool iskasbank { get; set; }
        public string norekening { get; set; }
        public string atasnamarekening { get; set; }
        public int idtypebank { get; set; }
        public bool isneraca { get; set; }
    }

    public class AkunValidator : AbstractValidator<MAkun>
    {
        [Obsolete]
        public AkunValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

            var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

            RuleFor(c => c.kode).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.nama).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
        }
    }
}
