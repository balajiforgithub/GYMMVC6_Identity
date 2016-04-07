using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc.Rendering;

namespace GYMONE.Models
{
    public class AssignRoleVM
    {
        [Required(ErrorMessage = "Enter Role Name")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "Enter User name")]
        public string UserName { get; set; }
        public List<SelectListItem> Userlist { get; set; }
        public List<SelectListItem> RolesList { get; set; }
    }

    public class AllroleandUser
    {
        public string RoleName { get; set; }
        public string UserName { get; set; }

        public IEnumerable<AllroleandUser> AllDetailsUserlist { get; set; }
    }

}