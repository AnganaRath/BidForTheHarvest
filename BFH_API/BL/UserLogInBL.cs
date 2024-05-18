using HUSP_API.Data;
using HUSP_API.Data.Repositories;
using HUSP_API.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.BL
{
    public class UserLogInBL
    {
        public List<User> GetLogInUsers(AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetLogInUsers();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public List<UserRole>GetLogInUserRole(AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetLogInUserRole();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public List<Role> GetAllRoles(AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetAllRoles();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int LoggedGetUserRoleId(Guid UserId, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).LoggedGetUserRoleId(UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Guid GetLoggedUserId(Guid UserId, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetLoggedUserId(UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetUserRole(Guid userId, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetUserRole(userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Guid LoggedGetEmployeeId(Guid UserId, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).LoggedGetEmployeeId(UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Guid GetUserId(AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetLogInUsersId();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool GetLoggedUserIdtoRestPass(User user, AuthenticationContext _authcontext)
        {
            var result = true;

            using (var ts = _authcontext.Database.BeginTransaction())
            {
                try
                {

                    var repUid = new UserLogInRepository(_authcontext).ResetUserPass(user.UserId);
                    if (repUid != null)
                    {
                        repUid.Password = user.Password;
                    }
                }

                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _authcontext.SaveChanges();
                    ts.Commit();
                }
            }

            return result;
        }

        public User GetLoggedInUser(string userName, string password, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetLoggedInUser(userName,password);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetLoggedInEmployeeName(Guid employeeId, SubProjectContext _spcontext)
        {
            try
            {
                return new EmployeeRepository(_spcontext).GetEmployeeName(employeeId);
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public Guid GetEmployeeEmail(Guid employeeId, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetEmployeeEmail(employeeId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetLogInPassword(Guid employeeId, AuthenticationContext _authcontext)
        {
            try
            {
                return new UserLogInRepository(_authcontext).GetLogInPassword(employeeId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
