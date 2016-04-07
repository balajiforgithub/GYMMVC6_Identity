﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace GYMONE.Models
{
    public class PlanMasterDTO
    {
        [Key]
        public int PlanID { get; set; }
        [Remote("PlannameExists", "Plan", ErrorMessage = "Plan Name Already Exists ")]
        [Required(ErrorMessage = "Enter Plan Name")]
        public string PlanName { get; set; }
        [Required(ErrorMessage = "Enter Plan Amount")]
        public decimal? PlanAmount { get; set; }
        [Required(ErrorMessage = "Enter Servicetax Amout")]
        public decimal ServicetaxAmout { get; set; }
        [Required(ErrorMessage = "Enter ServiceTax")]
        public string ServiceTax { get; set; }
        public int CreateUserID { get; set; }
        public int ModifyUserID { get; set; }
        public string RecStatus { get; set; }

        [Display(Name = "Scheme")]
        [ValidateScheme_Plan(ErrorMessage = "Select Scheme")]
        public int? SchemeID { get; set; }

        [Display(Name = "Period")]
        [ValidatePeriod(ErrorMessage = "Select Period")]
        public int? PeriodID { get; set; }


        public decimal? TotalAmout { get; set; }
        public string ServicetaxNo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        [NotMapped]
        public IEnumerable<SchemeMasterDTO> ListScheme { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem> ListofPeriod { get; set; }



    }

    public class ValidateScheme_Plan : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (Convert.ToInt32(value) == 0)
                return false;
            else
                return true;
        }
    }

    public class ValidatePeriod : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (Convert.ToInt32(value) == 0)
                return false;
            else
                return true;
        }
    }


}