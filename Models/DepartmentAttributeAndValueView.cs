using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToyForSI.Models
{
    public class DepartmentAttributeAndValueView
    {
        public int aId { get; set; }
        public int? dId { get; set; }
        public int? vId { get; set; }
        public string aName { get; set; }
        public string value { get; set; }
    }
}
