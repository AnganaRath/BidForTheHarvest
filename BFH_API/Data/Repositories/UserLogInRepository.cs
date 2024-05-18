using HUSP_API.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.Data.Repositories
{
    public class UserLogInRepository
    {
        AuthenticationContext _authcontext = null;

        public UserLogInRepository()
        {

        }

        public UserLogInRepository(AuthenticationContext dbaCtx)
        {
            _authcontext = dbaCtx;
        }

        public List<User> GetLogInUsers()
        {
            var users = (from user in _authcontext.Users select user).ToList();

            return users;
        }

        public Guid GetLogInUsersId()
        {
            var getLogInUsersId = (from uid in _authcontext.Users
                                   select uid.UserId).FirstOrDefault();
            return getLogInUsersId;
        }


        public List<UserRole> GetLogInUserRole()
        {
            var userRole = (from logInUserRole in _authcontext.UserRoles select logInUserRole).ToList();

            return userRole;
        }

        public List<Role> GetAllRoles()
        {
            var allRoles = (from roles in _authcontext.Roles select roles).ToList();

            return allRoles;
        }

        public int GetRole(Guid UserId)
        {
            var userRole = (from role in _authcontext.UserRoles
                            where role.UserId == UserId
                            select role.RoleId).FirstOrDefault();
            return userRole;
        }

        public int LoggedGetUserRoleId(Guid UserId)
        {

            var urId = (from rid in _authcontext.UserRoles
                        where rid.UserId == UserId
                        select rid.RoleId).FirstOrDefault();
            return urId;
        }

        public Guid GetLoggedUserId(Guid UserId)
        {

            var uId = (from usid in _authcontext.Users
                        where usid.UserId == UserId
                        select usid.UserId).FirstOrDefault();
            return uId;
        }


        public User ResetUserPass(Guid UserId)
        {
            var rpassuid = (from rpuId in _authcontext.Users
                              where rpuId.UserId == UserId
                            select rpuId).FirstOrDefault();

            return rpassuid;
        }


        public int GetUserRole (Guid UserId)
        {

            var urId = (from rid in _authcontext.UserRoles
                        where rid.UserId == UserId
                        select rid.RoleId).FirstOrDefault();
            return urId;
        }

        public Guid LoggedGetEmployeeId(Guid UserId)
        {

            var empId = (from emId in _authcontext.Users
                        where emId.UserId == UserId
                         select emId.EmployeeId).FirstOrDefault();
            return empId;
        }

        public Guid GetEmployeeId(Guid employeeId)
        {
            var userempId = (from empId in _authcontext.Users
                            where empId.EmployeeId == employeeId
                             select empId.EmployeeId).FirstOrDefault();
            return userempId;
        }

        public User GetLoggedInUser(string userName, string password)
        {
            var loggedInUser = (from user in _authcontext.Users
                            where user.UserName == userName && user.Password== password
                            select user).FirstOrDefault();
            return loggedInUser;
        }

        public Guid GetEmployeeEmail(Guid EmployeeId)
        {

            var empEmail = (from empemail in _authcontext.Users
                            where empemail.EmployeeId == EmployeeId
                            select empemail.EmployeeId).FirstOrDefault();
            return empEmail;
        }

        public string GetLogInPassword(Guid EmployeeId)
        {

            var pword = (from pass in _authcontext.Users
                            where pass.EmployeeId == EmployeeId
                            select pass.Password).FirstOrDefault();
            return pword;
        }


        public string GetloginEmployeeEmail(Guid EmployeeId)
        {

            var empEmail = (from emploginemail in _authcontext.Users
                            where emploginemail.EmployeeId == EmployeeId
                            select emploginemail.email).FirstOrDefault();
            return empEmail;
        }


    }
}
