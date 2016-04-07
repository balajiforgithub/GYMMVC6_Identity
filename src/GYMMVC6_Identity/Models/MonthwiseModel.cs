using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Mvc.Rendering;

namespace GYMONE.Models
{
    public class MonthwiseModel
    {
        public string MonthID { get; set; }
        public IEnumerable<SelectListItem> MonthNameList { get; set; }
    }
}