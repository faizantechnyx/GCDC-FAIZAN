using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.Api.Request
{
    public class FormRequest
    {
        public string FormUrl { get; set; }
        public int FormId { get; set; }
        public string FormData { get; set; }

        public string RecaptchaToken { get; set; }

    }
}
