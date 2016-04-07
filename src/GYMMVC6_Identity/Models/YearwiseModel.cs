using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Mvc.Rendering;

namespace GYMONE.Models
{
    public class YearwiseModel
    {
        public string YearID { get; set; }
        public IEnumerable<SelectListItem> YearNameList { get; set; }
    }
}