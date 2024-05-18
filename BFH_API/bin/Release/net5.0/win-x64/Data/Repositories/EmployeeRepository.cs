using CECBERP.CMN.Business.Entities;
using CECBERP.CMN.Business.Entities.PM;
using CECBERP.CMN.Business.Entities.SP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static CECBERP.CMN.Business.Entities.WorkSpace;

namespace HUSP_API.Data.Repositories
{
    public class EmployeeRepository
    {
        SubProjectContext _context = null;


        public EmployeeRepository()
        {

        }

        public EmployeeRepository(SubProjectContext dbCtx)
        {
            _context = dbCtx;
        }

        public String GetEmployeeName(Guid EmployeeId)
        {
       
            var empName = (from emp in _context.EmployeeVersions
                              where emp.EmployeeId == EmployeeId
                           select emp.NameWithInitial).FirstOrDefault();
            return empName;
        }

    }
}
