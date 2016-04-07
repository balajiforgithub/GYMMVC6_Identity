using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GYMONE.Models;
using GYMMVC6_Identity.Models;
using GYMONE.Repository;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace GYMMVC6_Identity.Controllers
{
    public class AccountController : Controller
    {
        IAccountData objIAccountData;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        /////private readonly RoleManager<Role> _roleManager;
        private ApplicationUser loggedInUser = null;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            /////_roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()), null, null, null, null, null);
            objIAccountData = new AccountData(configuration);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                ////bool success = WebSecurity.Login(login.username, login.password, false);
                var result = await _signInManager.PasswordSignInAsync(login.username, login.password, false, lockoutOnFailure: false);
                var UserID = GetUserID_By_UserName(login.username);
                var LoginType = GetRoleBy_UserID(Convert.ToString(UserID));
                loggedInUser = GetUserBy_UserID(Convert.ToString(UserID));
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(LoginType)))
                    {
                        ModelState.AddModelError("Error", "Rights to User are not Provide Contact to Admin");
                        return View(login);
                    }
                    else
                    {
                        ViewData["Name"] = login.username;
                        ViewData["UserID"] = UserID;
                        ViewData["LoginType"] = LoginType;
                        
                        var isInRole = await _userManager.IsInRoleAsync(loggedInUser, "Admin");
                        if (isInRole)
                        {
                            return RedirectToAction("AdminDashboard", "Dashboard");
                        }
                        else
                        {
                            return RedirectToAction("UserDashboard", "Dashboard");
                        }
                     }
                }
                else
                {
                    ModelState.AddModelError("Error", "Please enter valid Username and Password");
                    return View(login);
                }
            }
            else
            {
                ModelState.AddModelError("Error", "Please enter Username and Password");
            }
            return View(login);

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register, string actionType)
        {
            if (actionType == "Save")
            {
                if (ModelState.IsValid)
                {

                    var user = new ApplicationUser { UserName = register.username, Email = register.EmailID };
                    var userFound = await _userManager.FindByEmailAsync(register.EmailID);
                    if (userFound == null)
                    {
                        var result = await _userManager.CreateAsync(user, register.password);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Login", "Account");
                        }
                    
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Please enter all details");
                }
                return View();

            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        /////////[MyExceptionHandler]
        public ActionResult RoleCreate()
        {
            return View();
        }

        ////[HttpPost]
        /////////[MyExceptionHandler]
        ////[ValidateAntiForgeryToken]
        ////public async Task<IActionResult> RoleCreate(Role role)
        ////{
        ////    if (ModelState.IsValid)
        ////    {
        ////        if (await _roleManager.RoleExistsAsync(role.RoleName))
        ////        {
        ////            ModelState.AddModelError("Error", "Rolename already exists");
        ////            return View(role);
        ////        }
        ////        else
        ////        {
        ////            await _roleManager.CreateAsync(role);
        ////            return RedirectToAction("RoleIndex", "Account");
        ////        }
        ////    }
        ////    else
        ////    {
        ////        ModelState.AddModelError("Error", "Please enter Username and Password");
        ////    }
        ////    return View(role);
        ////}

        [HttpGet]
        /////////[MyExceptionHandler]
        public ActionResult RoleAddToUser()
        {
            AssignRoleVM objvm = new AssignRoleVM();
            objvm.RolesList = GetAll_Roles();
            objvm.Userlist = GetAll_Users();
            return View(objvm);
        }


        [HttpPost]
        ////[MyExceptionHandler]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(AssignRoleVM objvm)
        {

            if (objvm.RoleName == "0")
            {
                ModelState.AddModelError("RoleName", "Please select RoleName");
            }

            if (objvm.UserName == "0")
            {
                ModelState.AddModelError("UserName", "Please select Username");
            }

            if (ModelState.IsValid)
            {

                if (objIAccountData.Get_CheckUserRoles(objvm.UserName) == true)
                {
                    ViewBag.ResultMessage = "This user already has the role specified !";
                }
                else
                {
                    var user = objIAccountData.GetUserByUserName(objvm.UserName);
                    _userManager.AddToRoleAsync(user, objvm.RoleName);
                    ////Roles.AddUserToRole(UserName, objvm.RoleName);
                    ViewBag.ResultMessage = "Username added to the role succesfully !";
                }
                objvm.RolesList = GetAll_Roles();
                objvm.Userlist = GetAll_Users();

                return View(objvm);
            }
            else
            {
                objvm.RolesList = GetAll_Roles();
                objvm.Userlist = GetAll_Users();
                ModelState.AddModelError("Error", "Please enter Username and Password");
            }

            return View(objvm);
        }


        [HttpPost]
        /////[MyExceptionHandler]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(AssignRoleVM objvm)
        {
            if (objvm.RoleName == "0")
            {
                ModelState.AddModelError("RoleName", "Please select RoleName");
            }

            if (objvm.UserName == "0")
            {
                ModelState.AddModelError("UserName", "Please select Username");
            }

            objvm.RolesList = GetAll_Roles();
            objvm.Userlist = GetAll_Users();

            if (ModelState.IsValid)
            {
                if (objIAccountData.Get_CheckUserRoles(objvm.UserName) == true)
                {
                    var user = objIAccountData.GetUserByUserName(objvm.UserName);
                    ////var UserName = objIAccountData.GetUserName_BY_UserID(objvm.UserName);
                    _userManager.RemoveFromRoleAsync(user, objvm.RoleName);
                    //////Roles.RemoveUserFromRole(UserName, objvm.RoleName);
                    ViewBag.ResultMessage = "Role removed from this user successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }
            }
            return View(objvm);
        }

        [HttpGet]
        /////[MyExceptionHandler]
        public ActionResult DeleteRoleForUser()
        {
            AssignRoleVM objvm = new AssignRoleVM();
            objvm.RolesList = GetAll_Roles();
            objvm.Userlist = GetAll_Users();
            return View(objvm);
        }


        [HttpGet]
        /////[MyExceptionHandler]
        public ActionResult DisplayAllUserroles()
        {
            AllroleandUser objru = new AllroleandUser();
            objru.AllDetailsUserlist = objIAccountData.DisplayAllUser_And_Roles();
            return View(objru);
        }

        /////[MyExceptionHandler]
        public ActionResult RoleIndex()
        {
            var roles = objIAccountData.GetRoles();
            return View(roles);
        }

        ///////////[MyExceptionHandler]
        //////public async Task<IActionResult> RoleDelete(string RoleName)
        //////{
        //////    var roleToDelete = await _roleManager.FindByNameAsync(RoleName);
        //////   await _roleManager.DeleteAsync(roleToDelete);
        //////    ////Roles.DeleteRole(RoleName);
        //////    return RedirectToAction("RoleIndex", "Account");
        //////}

        ////[HttpPost]
        /////////[MyExceptionHandler]
        ////[ValidateAntiForgeryToken]
        ////public ActionResult GetRoles(string UserName)
        ////{
        ////    if (!string.IsNullOrWhiteSpace(UserName))
        ////    {
        ////        ViewBag.RolesForThisUser = Roles.GetRolesForUser(UserName);
        ////        SelectList list = new SelectList(Roles.GetAllRoles());
        ////        ViewBag.Roles = list;
        ////    }
        ////    return View("RoleAddToUser");
        ////}

        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            /////WebSecurity.Logout();
            ////Response.Redirect("~/account/login");
            RedirectToAction("Login", "Account");
            return View();
        }

        [HttpGet]
        /////[MyExceptionHandler]
        public ActionResult Changepassword()
        {
            return View(new ChangepasswordVM());
        }

        [HttpPost]
        /////[MyExceptionHandler]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Changepassword(ChangepasswordVM VM)
        {
            if (ModelState.IsValid)
            {
               
                if (string.IsNullOrEmpty(objIAccountData.Get_checkUsernameExits(loggedInUser.Email)))
                {
                    ModelState.AddModelError("Error", "UserName ");

                }
                else
                {
                    //var token = WebSecurity.GeneratePasswordResetToken(Convert.ToString(Session["Name"]));
                    //WebSecurity.ResetPassword(token, VM.password);
                    //ViewBag = "Password Changed";
                    var value = await _userManager.ChangePasswordAsync(loggedInUser, VM.OldPassword, VM.Newpassword);
                    ////var value = WebSecurity.ChangePassword(Session["Name"].ToString(), VM.OldPassword, VM.Newpassword);

                    if (!value.Succeeded)
                    {
                        ModelState.AddModelError("Error", "Incorrect Old Password");
                        return View(VM);
                    }
                    else
                    {
                        ViewBag.ResultMessage = "Password Changed Successfully";
                    }

                }
            }
            else
            {
                ModelState.AddModelError("Error", "Fill on Fields");
            }
            return View(VM);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AllRegisterUserDetails()
        {
            var Users = objIAccountData.GetAllUsers();
            return View(Users);
        }

        [NonAction]
        public string GetRoleBy_UserID(string UserId)
        {
            return objIAccountData.GetRoleByUserID(UserId);
        }

        [NonAction]
        public ApplicationUser GetUserBy_UserID(string UserId)
        {
            return objIAccountData.GetUserByUserId(UserId);
        }

        [NonAction]
        public string GetUserID_By_UserName(string UserName)
        {
            return objIAccountData.GetUserID_By_UserName(UserName);
        }

        //////[HttpGet]
        //////[AllowAnonymous]
        //////public ActionResult CheckUserNameExists(string username)
        //////{
        //////    bool UserExists = false;

        //////    try
        //////    {
        //////        var nameexits = objIAccountData.Get_checkUsernameExits(username);

        //////        if (string.Equals(nameexits, "1"))
        //////        {
        //////            UserExists = true;
        //////        }
        //////        else
        //////        {
        //////            UserExists = false;
        //////        }
        //////        return Json(!UserExists, JsonRequestBehavior.AllowGet);
        //////    }
        //////    catch (Exception)
        //////    {
        //////        return Json(false, JsonRequestBehavior.AllowGet);
        //////    }
        //////}

        [NonAction]
        public List<SelectListItem> GetAll_Roles()
        {
            List<SelectListItem> listrole = new List<SelectListItem>();

            listrole.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in objIAccountData.GetRoles())
            {
                listrole.Add(new SelectListItem { Text = item.RoleName, Value = item.RoleName });
            }

            return listrole;
        }

        [NonAction]
        public List<SelectListItem> GetAll_Users()
        {
            var Userlist = objIAccountData.GetAllUsers();
            List<SelectListItem> listuser = new List<SelectListItem>();
            listuser.Add(new SelectListItem { Text = "Select", Value = "0" });
            foreach (var item in Userlist)
            {
                listuser.Add(new SelectListItem { Text = item.UserName, Value = item.Id });
            }

            return listuser;
        }

    }
}
