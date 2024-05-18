using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HUSP_API.BL;
using HUSP_API.Data;
using HUSP_API.Models.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HUSP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class UserLogInController : ControllerBase
    {

        private readonly AuthenticationContext _authcontext;
        private readonly SubProjectContext _spcontext;


        public UserLogInController(AuthenticationContext authcontext,SubProjectContext spContext)
        {
            _authcontext = authcontext;
            _spcontext = spContext;
        }

        [HttpGet]
        [Route("GetLogInUsers")]
        public List<User>GetLogInUsers()
        {
            return new UserLogInBL().GetLogInUsers(_authcontext);
        }

        [HttpGet]
        [Route("GetLogInUserRole")]
        public List<UserRole> GetLogInUserRole()
        {
            return new UserLogInBL().GetLogInUserRole(_authcontext);
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public List<Role> GetAllRoles()
        {
            return new UserLogInBL().GetAllRoles(_authcontext);
        }


        [HttpPut]
        [Route("UpdatePassword")]
        public ActionResult UpdateMrSubProject(User user)
        {
            var result = new UserLogInBL().GetLoggedUserIdtoRestPass(user, _authcontext);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Password Reset Failed");
            }

        }

        [HttpPost]
        [Route("UserLogIn")]

        public ActionResult userLogIn(User login) 
        {
            var user = new UserLogInBL().GetLoggedInUser(login.UserName, login.Password, _authcontext);
            
            var result = new PassLoginDTO();

            if (user != null)
            {
               
                var empName = new UserLogInBL().GetLoggedInEmployeeName(user.EmployeeId, _spcontext);
                var empId = new UserLogInBL().GetEmployeeEmail(user.EmployeeId, _authcontext);
                var role = new UserLogInBL().GetUserRole(user.UserId, _authcontext);
                var userId = new UserLogInBL().GetLoggedUserId(user.UserId, _authcontext);
                var uspassword = new UserLogInBL().GetLogInPassword(user.EmployeeId, _authcontext);




                result.Msg = "LogIn Sucess";
                result.LoggedInName = empName;
                result.LogInEmployeeId = empId;
                result.UserId = userId;
                result.LoggedInPassword = uspassword;




                result.UserRole = role;

                return Ok(result);
            }
            else
            {
                result.Msg = "Username or password is incorrect";
                return BadRequest(result);
            }
        }
    }
}

public class PassLoginDTO
{
    public string Msg { get; set; }
    public string LoggedInName { get; set; }
    public string LoggedInPassword { get; set; }
    public Guid LogInEmployeeId { get; set; }

    public Guid UserId { get; set; }
    public string logInEmial { get; set; }
    public int UserRole { get; set; }


}




