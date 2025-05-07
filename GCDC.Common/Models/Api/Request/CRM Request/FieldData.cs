using Lucene.Net.Documents;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCDC.Common.Models.Api.Request.CRM_Request
{
    public class FieldData
    {
        public string type { get; set; } = "FieldValue";
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }

        public FieldData(string _id, string _name, string _value) 
        {
            id = _id;
            name = _name;
            value = _value;
        }
    }
}
