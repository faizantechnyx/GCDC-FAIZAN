using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.Api.Request.CRM_Request
{
    public class FormData
    {
        public string type { get; set; } = "FormData";
        public List<FieldData> fieldValues { get; set; } = new List<FieldData>();
    }
}
