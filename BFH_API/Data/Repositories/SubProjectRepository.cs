using CECBERP.CMN.Business.Entities;
using CECBERP.CMN.Business.Entities.FIN;
using CECBERP.CMN.Business.Entities.PM;
using CECBERP.CMN.Business.Entities.SP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static CECBERP.CMN.Business.Entities.WorkSpace;

namespace HUSP_API.Data.Repositories
{
    public class SubProjectRepository
    {
        SubProjectContext _context = null;
        //AuthenticationContext _authcontext = null;

        public SubProjectRepository()
        {

        }

        public SubProjectRepository(SubProjectContext dbCtx)
        {
            _context = dbCtx;
        }


        public bool AddSubProject(SubProject subProj)
        {
            _context.SubProjects.Add(subProj);
            return true;
        }

        public bool AddProjectProgress(SubProjectProgress subprpro)
        {
            _context.subProjectProgresses.Add(subprpro);
            return true;
        }

        public bool AddMrNewJobNo(SubProjectMRJobs sumrjobcount)
        {
            _context.subProjectMRJobs.Add(sumrjobcount);
            return true;
        }
        public bool AddWorkSpace(WorkSpace ws)
        {
            _context.WorkSpaces.Add(ws);
            return true;
        }


        public bool AddServices(List<SubProjectServiceByOtherUnits> subServices)
        {
            _context.SProjectServiceByOtherUnits.AddRange(subServices);
            return true;
        }

        public bool DeleteServices(List<SubProjectServiceByOtherUnits> subServices)
        {
            _context.SProjectServiceByOtherUnits.RemoveRange(subServices);
            return true;
        }

        public bool AddSubPCode(SubProjectCodeCounter spcc)
        {
            _context.SubProjectCodeCounters.Add(spcc);
            return true;
        }

        public List<SubProjectServiceByOtherUnits> GetExistingServiceUnits(Guid subProjectId)
        {
            var exSUs = (from su in _context.SProjectServiceByOtherUnits
                        where su.SubProjectId == subProjectId
                         select su).ToList();
            return exSUs;
        }


        public Guid GetParentWorkSpaceId(Guid projId)
        {
            var wsId = (from proj in _context.Projects
                        where proj.ProjectId == projId
                        select proj.ProjectWorkSpaceId).FirstOrDefault();
            return wsId;
        }

        public SubProjectMRJobCounter GetMRCounter(Guid SubProjectId)
        {
            var mrcounter = (from mrcount in _context.SubProjectMRJobCounters
                             where mrcount.SubProjectId == SubProjectId
                             select mrcount).FirstOrDefault();
            return mrcounter;

        }

        public SubProjectCodeCounter GetCounter(Guid subProjectTypeId,Guid mainProjectId)
        {
            var currentYear = DateTime.Now.Year;
            var counter = (from cnter in _context.SubProjectCodeCounters
                        where cnter.SubProjectTypeId == subProjectTypeId && cnter.Year== currentYear && cnter.ProjectId== mainProjectId
                           select cnter).FirstOrDefault();
            return counter;

        }

        public bool UpdateMRCounter(SubProjectMRJobCounter mrcounter)
        {
            _context.SubProjectMRJobCounters.Attach(mrcounter);
            _context.Entry(mrcounter).State = EntityState.Modified;
            return true;

        }

        public bool UpdateCounter(SubProjectCodeCounter counter)
        {
            _context.SubProjectCodeCounters.Attach(counter);
            _context.Entry(counter).State = EntityState.Modified;        
            return true;

        }

        public SubProjectCodeCounter CheckIfMRExists(int year,Guid projectId)
        {
            var mrTypeId = Guid.Parse("b6bd91ed-7b16-447d-911e-9102eec77bcb");
            var mr = (from subCounter in _context.SubProjectCodeCounters
                           where subCounter.SubProjectTypeId == mrTypeId
                           && subCounter.Year== year
                           && subCounter.ProjectId== projectId
                      select subCounter).FirstOrDefault();
            return mr;
        }

        public bool AddMRCounter(SubProjectMRJobCounter mRcounter)
        {
            _context.SubProjectMRJobCounters.Add(mRcounter);
            return true;
        }

        public bool AddCounter(SubProjectCodeCounter counter)
        {
            _context.SubProjectCodeCounters.Add(counter);
            return true;
        }



        public SubprojectType GetSubProjectType(Guid subProjTypeId)
        {
            var subType = (from subProjType in _context.SubProjectTypes
                           where subProjType.SubProjectTypeId == subProjTypeId
                           select subProjType).FirstOrDefault();
            return subType;
        }

        public SubProjectMRJobs GettoEditMrJobs(Guid MrJobId)
        {
            var mrEditJobs = (from mrJoblist in _context.subProjectMRJobs
                          where mrJoblist.MrJobId == MrJobId
                          select mrJoblist).FirstOrDefault();

            return mrEditJobs;
        }

        public List<Project> GetProjectByCode(string ProjectCode)
        {

            var mainProjectCode = (from proj in _context.Projects
                                   where proj.ProjectCode.Contains(ProjectCode) && proj.ProjectCode.Contains("C802")
                                   select proj).ToList();

            foreach (var proj in mainProjectCode)
            {
                proj.ClientName = GetClientName(proj.ClientId);
                if (proj.AGMWorkSpaceId != null)
                    proj.AGMsectionName = GetWorkSpaceName(proj.AGMWorkSpaceId.Value);
                if (proj.DGMWorkSpaceId != null)
                    proj.DGMunitName = GetWorkSpaceName(proj.DGMWorkSpaceId.Value);
            }

            return mainProjectCode;
        }

        public List<SubProjectProgress> GetSubProjectProgressHistory(Guid SubProjectId)
        {
            var subProgressHistory = (from subProHistory in _context.subProjectProgresses 
                                      where subProHistory.SubProjectId == SubProjectId
                                      select subProHistory).ToList();

            return subProgressHistory;
        }

        public List<SubProjectMRJobs>GetSubProjectMRJobs(Guid SubProjectId)
        {
            var SubProMRJobs = (from subProMRJobs in _context.subProjectMRJobs
                                      where subProMRJobs.SubProjectId == SubProjectId
                                      select subProMRJobs).ToList();

            return SubProMRJobs;
        }

        public List<SubProjectProgress>GetSpProgressById(Guid ProgressId)
        {
            var subProgressById = (from subProgressByid in _context.subProjectProgresses
                                      where subProgressByid.ProgressId == ProgressId
                                   select subProgressByid).ToList();

            return subProgressById;
        }

        public List<SubProjectMRJobs>GetMrJobs(Guid MrJobId)
        {
            var mrJobs = (from mrJoblist in _context.subProjectMRJobs
                          where mrJoblist.MrJobId == MrJobId
                          select mrJoblist).ToList();

            return mrJobs;
        }

        public List<SubProject>GetApprovedSubProjectByCode(string SubProjectCode)
        {

            var approvedSubProjectCode = (from asc in _context.SubProjects
                                   where asc.SubProjectCode.Contains(SubProjectCode)
                                   select asc).ToList();

            return approvedSubProjectCode;
        }
        //public List<SubProjectDTO> GetSubProjectServicesOU(Guid ServiceId)
        //{
        //    var subProjectServicesOU = (from subser in _context.SubProjectsServices
        //                                join workSpaceIds in _context.WorkSpaces on subser.WorkSpaceId equals workSpaceIds.WorkSpaceId
        //                                where subser.ServiceId == Guid.Parse("BDF65527-B06C-4B59-A77F-0ACD801D519F")
        //                                );.ToList();

        //    return subProjectServicesOU;
        //}


        public List<SubProject>GetMrJobCode(string SubProjectCode)
        {

            var mrJobCode = (from mrjc in _context.SubProjects
                             join SubProjectTypeId in _context.SubProjectTypes on mrjc.SubProjectTypeId equals Guid.Parse("B6BD91ED-7B16-447D-911E-9102EEC77BCB")
                             where mrjc.SubProjectCode == SubProjectCode 
                             select mrjc).ToList();

            return mrJobCode;
        }


            public List<Project> GetProjectByLocation(string CityName)
        {
           
            var mainProjectLocation = (from proj in _context.Projects
                                       join projLoc in _context.ProjectLocations
                                       on proj.ProjectLocationId equals projLoc.ProjectLocationId
                                       where projLoc.CityName.Contains(CityName) && proj.ProjectCode.Contains("C802")
                                       select proj).ToList();

            foreach (var proj in mainProjectLocation)
            {
               
                proj.ClientName = GetClientName(proj.ClientId);
                if (proj.AGMWorkSpaceId != null)
                    proj.AGMsectionName = GetWorkSpaceName(proj.AGMWorkSpaceId.Value);
                if (proj.DGMWorkSpaceId != null)
                    proj.DGMunitName = GetWorkSpaceName(proj.DGMWorkSpaceId.Value);
            }

            return mainProjectLocation;
        }

        //public List<Project> GetProjectByCodeoRLocation(string ProjectCode)
        //{

        //    var mainProjectCode = (from proj in _context.Projects
        //                           where proj.ProjectCode.Contains(ProjectCode)
        //                           select proj).ToList();

        //    foreach (var proj in mainProjectCode)
        //    {
        //        proj.ClientName = GetClientName(proj.ClientId);
        //        if (proj.AGMWorkSpaceId != null)
        //            proj.AGMsectionName = GetWorkSpaceName(proj.AGMWorkSpaceId.Value);
        //        if (proj.DGMWorkSpaceId != null)
        //            proj.DGMunitName = GetWorkSpaceName(proj.DGMWorkSpaceId.Value);
        //    }

        //    return mainProjectCode;
        //}

        public SubProjectDTO GetSubProjectDetailsById(Guid SubProjectId)
        {

            var getallSP = (from sp in _context.SubProjects
                            join ev in _context.EmployeeVersions on sp.ProjectManagerId equals ev.EmployeeId
                            //join sps in _context.SProjectServiceByOtherUnits on sp.SubProjectId equals sps.SubProjectId
                            join p in _context.Projects on sp.ProjectId equals p.ProjectId
                            join ws in _context.WorkSpaces on p.AGMWorkSpaceId equals ws.WorkSpaceId into wsAGM
                            from ws in wsAGM.DefaultIfEmpty()
                            join ws1 in _context.WorkSpaces on p.DGMWorkSpaceId equals ws1.WorkSpaceId into wsDGM
                            from ws1 in wsDGM.DefaultIfEmpty()
                            join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                            where ev.IsActive == true && sp.SubProjectId == SubProjectId
                            select new SubProjectDTO
                            {
                                SubProjectId = sp.SubProjectId,
                                SubProjectTypeId = sp.SubProjectTypeId,
                                SubProjectCode = sp.SubProjectCode,
                                SubProjectName = sp.SubProjectName,
                                SPEstimatedValue = sp.SPEstimatedValue,
                                ServiceByHU = sp.ServiceByHU,
                                ConsultancyPercentage = sp.ConsultancyPercentage,
                                PercentageA = sp.PercentageA,
                                PercentageB = sp.PercentageB,
                                PercentageC = sp.PercentageC,
                                //HandledByHospitalWorks = sp.HandledByHospitalWorks,
                                SPContractValue = sp.SPContractValue,
                                SPConsultancyFee = sp.SPConsultancyFee,
                                FinalBillValue = sp.FinalBillValue,
                                DateOfCommencement = sp != null ? sp.DateOfCommencement : null,
                                ShedulledCompletionDate = sp != null ? sp.ShedulledCompletionDate : null,
                                ProjectManagerId = sp.ProjectManagerId,
                                RejectedReason = sp.RejectedReason,
                                ProjectId = p.ProjectId,
                                MainProjectName = p.ProjectName,
                                MainProjectCode = p.ProjectCode,
                                MainProjectContractValue = p.EstimatedValue,
                                ContractConsultancyFee = p.ContractConsultancyFee.Value,
                                AGMsectionName = ws != null ? ws.WorkSpaceName : String.Empty,
                                DGMunitName = ws1 != null ? ws1.WorkSpaceName : String.Empty,
                                ClientName = cnt.ClientName,
                                // ServiceIds = GetServiceIds(sp.SubProjectId),//sps.ServiceId,
                                SubProjectPMName = ev.NameWithInitial
                            }).FirstOrDefault();

            return getallSP;
        }

        public List<SubProjectServiceByOtherUnits> GetServiceIds(Guid subProjectId)
        {
            List<SubProjectServiceByOtherUnits> services = new List<SubProjectServiceByOtherUnits>();

            var spServices = (from sServ in _context.SProjectServiceByOtherUnits
                              where sServ.SubProjectId == subProjectId
                              select sServ).ToList();
            if (spServices.Count > 0)
            {
                foreach (var serv in spServices)
                {
                    services.Add(serv);
                }
            }


            foreach (var service in services)
            {
                service.WorkSpaceName = GetWorkSpaceName(service.WorkSpaceId);
                service.ServiceName = GetServiceName(service.ServiceId);

            }

            return services;

        }

        public List<SubprojectProgressQuarterlyReportDTO> SubprojectProgressQuarterlyReport(DateTime CurrentDate,string SubProjectCode)
        {
         
            var ProgressQuarterlyReport = (from sp in _context.SubProjects
                                           join ev in _context.EmployeeVersions on sp.ProjectManagerId equals ev.EmployeeId
                                           join p in _context.Projects on sp.ProjectId equals p.ProjectId
                                           join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                                           join pl in _context.ProjectLocations on p.ProjectLocationId equals pl.ProjectLocationId
                                           join pr in _context.subProjectProgresses on sp.SubProjectId equals pr.SubProjectId
                                           where ev.IsActive == true && pr.CurrentDate == CurrentDate && sp.SubProjectCode == SubProjectCode
                                           select new SubprojectProgressQuarterlyReportDTO
                                           {
                                               //SubProjectId = sp.SubProjectId,
                                               SubProjectPMName = ev.NameWithInitial,
                                               SubProjectCode = sp.SubProjectCode,
                                               SubProjectName = sp.SubProjectName,
                                               CityName = pl.CityName,
                                               ClientName = cnt.ClientName,
                                               DateOfCommencement = sp != null ? sp.DateOfCommencement : null,
                                               ShedulledCompletionDate = sp != null ? sp.ShedulledCompletionDate : null,
                                               SPEstimatedValue = sp.SPEstimatedValue,
                                               CurrentDate = pr.CurrentDate
        }).ToList();


            return ProgressQuarterlyReport;
        }

        public List<SubprojectWorkVsFinancialPhysicalProgressReportDTO> SubprojectWorkVsFinancialPhysicalProgressReport(DateTime CurrentDate, string SubProjectCode)
        {
            var WorkVsFinancialReport = (from sp in _context.SubProjects
                                           join ev in _context.EmployeeVersions on sp.ProjectManagerId equals ev.EmployeeId
                                           join p in _context.Projects on sp.ProjectId equals p.ProjectId
                                           join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                                           join pl in _context.ProjectLocations on p.ProjectLocationId equals pl.ProjectLocationId
                                           join pr in _context.subProjectProgresses on sp.SubProjectId equals pr.SubProjectId
                                           where ev.IsActive == true && pr.CurrentDate == CurrentDate && sp.SubProjectCode == SubProjectCode
                                           select new SubprojectWorkVsFinancialPhysicalProgressReportDTO
                                           {
                                               //SubProjectId = sp.SubProjectId,
                                               //SubProjectPMName = ev.NameWithInitial,
                                               SubProjectCode = sp.SubProjectCode,
                                               SubProjectName = sp.SubProjectName,
                                               CityName = pl.CityName,
                                               ClientName = cnt.ClientName,
                                               SPContractValue = sp.SPContractValue,
                                               PhysicalProgressOfConstructionWork = pr.PhysicalProgressOfConstructionWork,
                                               FinancialProgressOfConstructionWork = pr.FinancialProgressOfConstructionWork,
                                               CurrentDate = pr.CurrentDate
                                           }).ToList();

            return WorkVsFinancialReport;
        }

        public List<SubprojectWorkVsIncomeReportDTO> SubprojectWorkVsIncomeReport(DateTime CurrentDate, string SubProjectCode)
        {
            var WorkVsIncomeReport = (from sp in _context.SubProjects
                                         join ev in _context.EmployeeVersions on sp.ProjectManagerId equals ev.EmployeeId
                                         join p in _context.Projects on sp.ProjectId equals p.ProjectId
                                         join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                                         join pl in _context.ProjectLocations on p.ProjectLocationId equals pl.ProjectLocationId
                                         join pr in _context.subProjectProgresses on sp.SubProjectId equals pr.SubProjectId
                                         where ev.IsActive == true && pr.CurrentDate == CurrentDate && sp.SubProjectCode == SubProjectCode
                                         select new SubprojectWorkVsIncomeReportDTO
                                         {
                                             //SubProjectId = sp.SubProjectId,
                                             //SubProjectPMName = ev.NameWithInitial,
                                             SubProjectCode = sp.SubProjectCode,
                                             SubProjectName = sp.SubProjectName,
                                             CityName = pl.CityName,
                                             ClientName = cnt.ClientName,
                                             SPContractValue = sp.SPContractValue,
                                             FinancialProgressOfConsultancyWork = pr.FinancialProgressOfConsultancyWork,
                                             TotalWorkDoneUptoNow = pr.TotalWorkDoneUptoNow,
                                             CurrentDate = pr.CurrentDate
                                         }).ToList();

            return WorkVsIncomeReport;
        }


     

        public bool UpdateSubProject(SubProject sp)
        {
            _context.SubProjects.Attach(sp);
            _context.Entry(sp).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        //update after the Sub Project code change
        public bool UpdateSubProjectCode(SubProject sp) 
        {
            _context.SubProjects.Attach(sp);
            _context.Entry(sp).State = EntityState.Modified;
            //_context.SaveChanges();
            return true;
        }
        public SubProject GetSubProject(Guid SubProjectId)
        {
            var sp = (from sub in _context.SubProjects
                                where sub.SubProjectId == SubProjectId
                                select sub).FirstOrDefault();
            return sp;
        }

        //public SubProjectMRJobs GetMrSubProject(Guid SubProjectId)
        //{
        //    var sp = (from sub in _context.SubProjects
        //              where sub.SubProjectId == SubProjectId
        //              select sub).FirstOrDefault();
        //    return sp;
        //}

        public EmployeeVersion GetEmployeeVersion (Guid EmployeeVersionId)
        {

            var employeeepf = (from emp in _context.EmployeeVersions
                               where emp.EmployeeVersionId == EmployeeVersionId
                               select emp).FirstOrDefault();
            return employeeepf;
        }
        public List<EmployeeVersion>GetPMByName(string FullName)
        {
            var employeeepf = (from emp in _context.EmployeeVersions
                               join dn in _context.Designation  on emp.DesignationId equals dn.DesignationId
                               where emp.IsActive==true && emp.FullName.Contains(FullName)
                               && dn.DesignationName.EndsWith("Engineer")
                               && (emp.GradeId==Guid.Parse("24953b57-73a7-4ad2-a44f-c4a7a73fd09d")||
                               emp.GradeId == Guid.Parse("f5c7ef58-f193-4097-816a-eafccd3c3cbb")||
                               emp.GradeId == Guid.Parse("cf5c0092-ad39-4d63-bf61-6a6b99cb155b")||
                               emp.GradeId == Guid.Parse("c793a0b4-c162-4469-8963-318793962218") ||
                               emp.GradeId == Guid.Parse("e5959d97-a303-4c0b-a0de-e55b359bb7a1") ||
                               emp.GradeId == Guid.Parse("a135fb6c-d93e-4102-bab4-ede010d61190")) 
                               && emp.AGMWorkSpaceId == Guid.Parse("a8df8976-b9f4-4f3f-8572-7c0db3d82331") //AGMWorkSpaceId
                               select emp).ToList();
            return employeeepf;

        }


        public List<SubProject> GetSubProjectDetailsByStatus(int DataStatus, int RoleId,Guid EmployeeId)
        {
            if(RoleId == 3) //SiteEngineer
            {
                var spd = (from se in _context.SubProjects
                                  where se.DataStatus == DataStatus && se.RequesterId == EmployeeId
                                  select se).ToList();
                return spd;

            }
            else if(RoleId == 2) //ProjectManager
            {
                var pmd = (from pm in _context.SubProjects
                                  where pm.DataStatus == DataStatus && pm.ProjectManagerId == EmployeeId
                                  select pm).ToList();
                return pmd;
            }
            else if (RoleId == 1) //Admin
            {
                var amd = (from ad in _context.SubProjects
                                  where ad.DataStatus == DataStatus
                                  select ad).ToList();
                return amd;
            }
            else if (RoleId == 4) //DGM
            {
                var dgmd = (from dgm in _context.SubProjects
                                  where dgm.DataStatus == DataStatus
                                  select dgm).ToList();
                return dgmd;
            }
            else if (RoleId == 5) //AGM
            {
                var agmd = (from agm in _context.SubProjects
                                  where agm.DataStatus == DataStatus
                                  select agm).ToList();
                return agmd;
            }
            else if (RoleId == 6) //PM+SE
            {
                var pmdspd = (from pmse in _context.SubProjects
                              where (pmse.DataStatus == DataStatus && pmse.ProjectManagerId == EmployeeId)|| (pmse.DataStatus == DataStatus && pmse.RequesterId == EmployeeId)
                              select pmse).ToList();
                return pmdspd;
            }
            else
            {
                return new List<SubProject>();
            }

           
        }

        public List<SubProject> GetPendingApprovalSubProjectDetails(int RoleId, Guid EmployeeId)
        {
            if (RoleId == 3) //SiteEngineer
            {
                var spd = (from se in _context.SubProjects
                           where se.DataStatus == 1 && se.RequesterId == EmployeeId
                           select se).ToList();
                return spd;

            }
            else if (RoleId == 2) //ProjectManager
            {
                var pmd = (from pm in _context.SubProjects
                           where pm.DataStatus == 1 && pm.ProjectManagerId == EmployeeId
                           select pm).ToList();
                return pmd;
            }
            else if (RoleId == 1) //Admin
            {
                var amd = (from ad in _context.SubProjects
                           where ad.DataStatus == 1
                           select ad).ToList();
                return amd;
            }
            else if (RoleId == 4) //DGM
            {
                var dgmd = (from dgm in _context.SubProjects
                            where dgm.DataStatus == 1
                            select dgm).ToList();
                return dgmd;
            }
            else if (RoleId == 5) //AGM
            {
                var agmd = (from agm in _context.SubProjects
                            where agm.DataStatus == 1
                            select agm).ToList();
                return agmd;
            }
            else if (RoleId == 6) //PM+SE
            {
                var pmdspd = (from pmse in _context.SubProjects
                              where pmse.DataStatus == 1 && pmse.ProjectManagerId == EmployeeId
                              select pmse).ToList();
                return pmdspd;
            }
            else
            {
                return new List<SubProject>();
            }
      
            //where pa.DataStatus == 1 && pa.RequesterId ==
        }

        public List<SubProjectServices> GetAllSubProjectServices()
        {
            var subProjectServices = (from spservice in _context.SubProjectsServices select spservice).ToList();

            return subProjectServices;
        }
        public List<WorkSpaceServiceDTO> GetWorkSpaces(Guid ServiceId)
        {
            if (ServiceId == Guid.Parse("849E110B-3CF4-4E17-9BEB-A06F139817C0"))
            {
                var ws= (from wa in _context.WorkSpaces
                         where wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82324") ||
                               wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82331") 
                             
                         select new WorkSpaceServiceDTO
                         {
                             WorkSpaceId = wa.WorkSpaceId,
                             WorkSpaceName = wa.WorkSpaceName
                         }).ToList();
                return ws;
            }
            else if(ServiceId == Guid.Parse("BDF65527-B06C-4B59-A77F-0ACD801D519F"))
            {
                var ws = (from wa in _context.WorkSpaces
                          where wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82314") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82315")

                          select new WorkSpaceServiceDTO
                          {
                              WorkSpaceId = wa.WorkSpaceId,
                              WorkSpaceName = wa.WorkSpaceName
                          }).ToList();
                return ws;
            }
            else if(ServiceId == Guid.Parse("8C34AA54-06B3-47A5-81F9-57F2F4DE7151"))
            {
                var ws = (from wa in _context.WorkSpaces
                          where wa.WorkSpaceId == Guid.Parse("E406C899-A930-46FB-80E0-C59E5076E141")
                                

                          select new WorkSpaceServiceDTO
                          {
                              WorkSpaceId = wa.WorkSpaceId,
                              WorkSpaceName = wa.WorkSpaceName
                          }).ToList();
                return ws;
            }
            else if (ServiceId == Guid.Parse("1324E25A-C524-41DE-919F-A4E4672DA879"))
            {
                var ws = (from wa in _context.WorkSpaces
                          where wa.WorkSpaceId == Guid.Parse("e406c899-a930-46fb-80e0-c59e5076e196") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82331")

                          select new WorkSpaceServiceDTO
                          {
                              WorkSpaceId = wa.WorkSpaceId,
                              WorkSpaceName = wa.WorkSpaceName
                          }).ToList();
                return ws;
            }
            else if (ServiceId == Guid.Parse("1BD22DC3-ED36-4C4C-BAD8-B9DEA99536F5"))
            {
                var ws = (from wa in _context.WorkSpaces
                          where wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82323") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82331")

                          select new WorkSpaceServiceDTO
                          {
                              WorkSpaceId = wa.WorkSpaceId,
                              WorkSpaceName = wa.WorkSpaceName
                          }).ToList();
                return ws;
            }
            else if (ServiceId == Guid.Parse("A16BD098-43B1-425F-9790-CF8445A2836A"))
            {
                var ws = (from wa in _context.WorkSpaces
                          where wa.WorkSpaceId == Guid.Parse("00858DDE-8CB2-4746-A742-F1B1747C8219") ||
                                wa.WorkSpaceId == Guid.Parse("E406C899-A930-46FB-80E0-C59E5076E220") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82340") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82338") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82335") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82331") ||
                                wa.WorkSpaceId == Guid.Parse("E406C899-A930-46FB-80E0-C59E5076E219")


                          select new WorkSpaceServiceDTO
                          {
                              WorkSpaceId = wa.WorkSpaceId,
                              WorkSpaceName = wa.WorkSpaceName
                          }).ToList();
                return ws;
            }
            else if (ServiceId == Guid.Parse("A0B9B592-D784-44EB-80B3-D48F8D80F719"))
            {
                var ws = (from wa in _context.WorkSpaces
                          where wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82324") ||
                                wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82331") 
                 
                          select new WorkSpaceServiceDTO
                          {
                              WorkSpaceId = wa.WorkSpaceId,
                              WorkSpaceName = wa.WorkSpaceName
                          }).ToList();
                return ws;
            }
            else
            {
                return new List<WorkSpaceServiceDTO>();
            }

            //var workSpaceId = (from wa in _context.WorkSpaces
            //                   where wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82314") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82315") ||
            //                         wa.WorkSpaceId == Guid.Parse("E406C899-A930-46FB-80E0-C59E5076E141") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82324") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82331") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82326") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82323") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82338") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82340") ||
            //                         wa.WorkSpaceId == Guid.Parse("A8DF8976-B9F4-4F3F-8572-7C0DB3D82335") ||
            //                         wa.WorkSpaceId == Guid.Parse("00858DDE-8CB2-4746-A742-F1B1747C8219") ||
            //                         wa.WorkSpaceId == Guid.Parse("E406C899-A930-46FB-80E0-C59E5076E219") ||
            //                         wa.WorkSpaceId == Guid.Parse("E406C899-A930-46FB-80E0-C59E5076E220") 

            //                   select new WorkSpaceServiceDTO
            //                   {
            //                       WorkSpaceId = wa.WorkSpaceId,
            //                       WorkSpaceName = wa.WorkSpaceName
            //                   }).ToList();
            //return workSpaceId;
        }



        public IQueryable<Object> GetSubProjectDetails(int DataStatus)
        {
            var dataStatus = (from sp in _context.SubProjects
                              join ev in _context.EmployeeVersions on sp.ProjectManagerId equals ev.EmployeeId
                              join sps in _context.SProjectServiceByOtherUnits on sp.SubProjectId equals sps.SubProjectId
                              join p in _context.Projects on sp.ProjectId equals p.ProjectId
                              join ws in _context.WorkSpaces on sp.WorkSpaceId equals ws.WorkSpaceId
                              join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                              where ev.IsActive == true && sp.DataStatus == DataStatus
                              select new
                              {
                                  SubProjectId = sp.SubProjectId,
                                  SubProjectTypeId = sp.SubProjectTypeId,
                                  SubProjectCode = sp.SubProjectCode,
                                  SubProjectName = sp.SubProjectName,
                                  ServiceByHU = sp.ServiceByHU,
                                  SPEstimatedValue = sp.SPEstimatedValue,
                                  ConsultancyPercentage = sp.ConsultancyPercentage,
                                  PercentageA = sp.PercentageA,
                                  PercentageB = sp.PercentageB,
                                  PercentageC = sp.PercentageC,
                                  //HandledByHospitalWorks = sp.HandledByHospitalWorks,
                                  SPContractValue = sp.SPContractValue,
                                  SPConsultancyFee = sp.SPConsultancyFee,
                                  FinalBillValue = sp.FinalBillValue,
                                  DateOfCommencement = sp.DateOfCommencement,
                                  ShedulledCompletionDate = sp.ShedulledCompletionDate,
                                  ProjectManagerId = sp.ProjectManagerId,
                                  EmployeeId = sp.ProjectManagerId,
                                  ProjectId = p.ProjectId,
                                  ProjectName = p.ProjectName,
                                  ProjectCode = p.ProjectCode,
                                  EstimatedValue = p.EstimatedValue,
                                  ContractConsultancyFee = p.ContractConsultancyFee,
                                  AGMsectionName = sp.AGMsectionName,
                                  DGMunitName = sp.DGMunitName,
                                  ClientName = sp.ClientName,
                                  ServiceId = sps.ServiceId,
                                  RejectedReason = sp.RejectedReason,
                                  FullName = ev.FullName,
                              });

            return dataStatus;
        }

        //public bool UpdateSubProjectDetails(Guid SubProjectId, SubProjectContext _context)
        //{
        //    var result = true;
        //    var sp = new SubProjectRepository(_context).GetSubProject(SubProjectId);
        //    //var sp = _context.SubProjects.FirstOrDefault(a => a.SubProjectId == subProj.SubProjectId);


        //    sp.SubProjectTypeId = subProj.SubProjectTypeId;
        //    sp.SubProjectCode = subProj.SubProjectCode;
        //    sp.SubProjectName = subProj.SubProjectName;
        //    sp.SPEstimatedValue = subProj.SPEstimatedValue;
        //    sp.ConsultancyPercentage = subProj.ConsultancyPercentage;
        //    sp.SPContractValue = subProj.SPContractValue;
        //    sp.SPConsultancyFee = subProj.SPConsultancyFee;
        //    sp.FinalBillValue = subProj.FinalBillValue;
        //    sp.DateOfCommencement = subProj.DateOfCommencement;
        //    sp.ShedulledCompletionDate = subProj.ShedulledCompletionDate;
        //    sp.ProjectManagerId = subProj.ProjectManagerId;
        //    //sp.EmployeeId = subProj.ProjectManagerId;
        //    sp.ProjectId = subProj.ProjectId;
        //    sp.ServiceId = subProj.ServiceId;
        //    sp.RejectedReason = subProj.RejectedReason;
        //    return result;
        //}


        public Project GetProject(Guid ProjectId)
        {

            var mainProject = (from proj in _context.Projects
                               where proj.ProjectId == ProjectId
                               select proj).FirstOrDefault();
           
            return mainProject;
        }


        public List<SubprojectType>GetSubProjectNames()
        {
            var subProjecttypeName = (from sptname in _context.SubProjectTypes select sptname).ToList();

            return subProjecttypeName;
        }

        public List<Project> GetProjects()
        {
            var mainProjects = (from proj in _context.Projects select proj).ToList();
           
                foreach (var proj in mainProjects)
            {
                proj.ClientName= GetClientName(proj.ClientId);
                if (proj.AGMWorkSpaceId != null)
                    proj.AGMsectionName = GetWorkSpaceName(proj.AGMWorkSpaceId.Value);
                if (proj.DGMWorkSpaceId != null)
                    proj.DGMunitName = GetWorkSpaceName(proj.DGMWorkSpaceId.Value);
            }
            
            return mainProjects;
        }

        public string GetPMEmail(Guid empId)
        {
            var email = (from emp in _context.EmployeeVersions
                              where emp.EmployeeId == empId
                         select emp.OfficeEmail).FirstOrDefault();
            return email;
        }

        public string GetEmployeeEmail(Guid empId)
        {
            var email = (from emp in _context.EmployeeVersions
                         where emp.EmployeeId == empId
                         select emp.OfficeEmail).FirstOrDefault();
            return email;
        }

        private string GetClientName(Guid ClientId)
        {
            var ClientName = (from cnt in _context.Clients
                              where cnt.ClientId == ClientId
                              select cnt.ClientName).FirstOrDefault();
            return ClientName;
        }

        private string GetProjLocation(Guid ProjectLocationId)
        {
            var CityName = (from ploc in _context.ProjectLocations
                                 where ploc.ProjectLocationId == ProjectLocationId
                                 select ploc.CityName).FirstOrDefault();
            return CityName;
        }


        private string GetWorkSpaceName(Guid WorkSpaceId)
        {
            var WorkSpaceName = (from ws in _context.WorkSpaces
                              where ws.WorkSpaceId == WorkSpaceId
                          select ws.WorkSpaceName).FirstOrDefault();
            return WorkSpaceName;
        }

        private string GetServiceName(Guid serviceId)
        {
            var serviceName = (from serv in _context.SubProjectsServices
                                 where serv.ServiceId == serviceId
                               select serv.ServiceName).FirstOrDefault();
            return serviceName;
        }


        public List<SubProjectDTO>GetSubProjectDetailsByCodeToInvoice(string SubProjectCode)
        {
            var sbprojectCode = (from sp in _context.SubProjects
                            join ev in _context.EmployeeVersions on sp.ProjectManagerId equals ev.EmployeeId
                            //join sps in _context.SProjectServiceByOtherUnits on sp.SubProjectId equals sps.SubProjectId
                            join p in _context.Projects on sp.ProjectId equals p.ProjectId
                            join ws in _context.WorkSpaces on p.AGMWorkSpaceId equals ws.WorkSpaceId into wsAGM
                            from ws in wsAGM.DefaultIfEmpty()
                            join ws1 in _context.WorkSpaces on p.DGMWorkSpaceId equals ws1.WorkSpaceId into wsDGM
                            from ws1 in wsDGM.DefaultIfEmpty()
                            join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                            where ev.IsActive == true && sp.SubProjectCode == SubProjectCode
                            select new SubProjectDTO
                            {
                                SubProjectId = sp.SubProjectId,
                                SubProjectTypeId = sp.SubProjectTypeId,
                                SubProjectCode = sp.SubProjectCode,
                                SubProjectName = sp.SubProjectName,
                                SPEstimatedValue = sp.SPEstimatedValue,
                                ServiceByHU = sp.ServiceByHU,
                                ConsultancyPercentage = sp.ConsultancyPercentage,
                                PercentageA = sp.PercentageA,
                                PercentageB = sp.PercentageB,
                                PercentageC = sp.PercentageC,
                                //HandledByHospitalWorks = sp.HandledByHospitalWorks,
                                SPContractValue = sp.SPContractValue,
                                SPConsultancyFee = sp.SPConsultancyFee,
                                FinalBillValue = sp.FinalBillValue,
                                DateOfCommencement = sp != null ? sp.DateOfCommencement : null,
                                ShedulledCompletionDate = sp != null ? sp.ShedulledCompletionDate : null,
                                ProjectManagerId = sp.ProjectManagerId,
                                RejectedReason = sp.RejectedReason,
                                ProjectId = p.ProjectId,
                                MainProjectName = p.ProjectName,
                                MainProjectCode = p.ProjectCode,
                                MainProjectContractValue = p.EstimatedValue,
                                ContractConsultancyFee = p.ContractConsultancyFee.Value,
                                AGMsectionName = ws != null ? ws.WorkSpaceName : String.Empty,
                                DGMunitName = ws1 != null ? ws1.WorkSpaceName : String.Empty,
                                ClientName = cnt.ClientName,
                                // ServiceIds = GetServiceIds(sp.SubProjectId),//sps.ServiceId,
                                SubProjectPMName = ev.NameWithInitial,
                                //CheckedEmployeeByName = ev.NameWithInitial,
                                //CheckedEmployeeBy = sp.ProjectManagerId,
                                ApprovedEmployeeByName = ev.NameWithInitial,
                                ApprovedEmployeeBy = sp.ProjectManagerId

                            }).ToList();
        
                return sbprojectCode;
     

        }

        internal bool AddSubProjectPreviousInvoiceDeatails(SubProjectInvoice subProjInvoice)
        {
            throw new NotImplementedException();
        }

        public List<EmployeeVersion> GetCheckedOrApprovedByName(string FullName)
        {
            var employeeepf = (from emp in _context.EmployeeVersions
                               join dn in _context.Designation on emp.DesignationId equals dn.DesignationId
                               where emp.IsActive == true && emp.FullName.Contains(FullName)
                               && dn.DesignationName.EndsWith("Engineer") select emp).ToList();
            return employeeepf;

        }

        public bool AddSubProjectInvoiceDetails(SubProjectInvoice subProjInvoice)
        {
            _context.SubProjectInvoices.Add(subProjInvoice);
            return true;
        }
        
        public bool AddSubProjectInvoiceDetail(List<SubProjectInvoiceDetail> obj)
        {
            _context.SubProjectInvoiceDetails.AddRange(obj);
            return true;
        }

        public bool AddSubProjectResivedPayment(List<SubProjectResivedPayments> obj)
        {
            _context.SubProjectResivedPayment.AddRange(obj);
            return true;
        }

        public bool AddSubProjectPreviousInvoiceDeatails(List<SubProjectPreviousInvoiceDeatails> objt)
        {
            _context.SPPreviousInvoiceDeatails.AddRange(objt);
            return true;
        }

        public List<SubProjectInvoice> GetInvoiceBySubProjectCode(string SubProjectCode)
        {

            var editInvoice = (from einvoice in _context.SubProjectInvoices
                               where einvoice.SubProjectCode.Contains(SubProjectCode)
                                          select einvoice).ToList();

            return editInvoice;
        }

        //public List<SubProjectInvoice> GetTotalApprovedInvoice(string SubProjectCode, Guid InvoiceId)
        //{
        //    var approvedInvoice = new List<SubProjectInvoice>();
        //    if (InvoiceId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
        //    {
        //         approvedInvoice = (from apIn in _context.SubProjectInvoices
        //                               where apIn.InvoiceId != InvoiceId && apIn.DataStatus == 3 && apIn.SubProjectCode.Contains(SubProjectCode)
        //                               select new SubProjectInvoice
        //                               {
        //                                   InvoiceId = apIn.InvoiceId,
        //                                   SubProjectCode = apIn.SubProjectCode,
        //                                   InvoiceNo = apIn.InvoiceNo,
        //                                   DataStatus = apIn.DataStatus,
        //                                   TotalAmountExcludingTax = apIn.TotalAmountExcludingTax,
        //                               }).ToList();
        //    }
        //    else
        //    {
        //        approvedInvoice = (from apIn in _context.SubProjectInvoices
        //                           where apIn.DataStatus == 3 && apIn.SubProjectCode.Contains(SubProjectCode)
        //                           select new SubProjectInvoice
        //                           {
        //                               InvoiceId = apIn.InvoiceId,
        //                               SubProjectCode = apIn.SubProjectCode,
        //                               InvoiceNo = apIn.InvoiceNo,
        //                               DataStatus = apIn.DataStatus,
        //                               TotalAmountExcludingTax = apIn.TotalAmountExcludingTax,
        //                           }).ToList();

        //    }

        //    return approvedInvoice;

        //}

        //public List<TotalInvoiceTableDataDTO> GetTotalApprovedInvoice(string SubProjectCode, Guid InvoiceId)
        //{

        //    List<TotalInvoiceTableDataDTO> approvedInvoice = new List<TotalInvoiceTableDataDTO>();
        //    if (InvoiceId != Guid.Empty)
        //    {

        //        approvedInvoice = (from apIn in _context.SubProjectInvoices
        //                           where apIn.InvoiceId != InvoiceId && apIn.DataStatus == 3 && apIn.SubProjectCode.Contains(SubProjectCode))
        //                           Select
        //                           select apIn).ToList();

        //    }
        //    else
        //    {
        //        approvedInvoice = (from apIn in _context.SubProjectInvoices
        //                           where apIn.InvoiceId != InvoiceId && apIn.DataStatus == 3 && apIn.SubProjectCode.Contains(SubProjectCode)
        //                           select apIn).ToList();

        //    }
        //    return approvedInvoice;

        //}



       public List<TotalInvoiceTableDataDTO>GetTotalApprovedInvoice(string SubProjectCode, Guid InvoiceId, SubProjectContext ctx)
       
        {
                List<TotalInvoiceTableDataDTO> approvedInvoice = new List<TotalInvoiceTableDataDTO>();

            if (InvoiceId != Guid.Empty)
            {
                var invoice = new SubProjectRepository(ctx).GetDLInvoice(InvoiceId);
                var approvedDate = invoice.ApprovedDate;
                var createdDate = invoice.CreatedDate;
                var checkedDate = invoice.CheckedDate;
                var finishedDate = invoice.FinishedDate;

                if (approvedDate != null)
                {

                    approvedInvoice = ctx.SubProjectInvoices
                        .Where(apIn => apIn.InvoiceId != InvoiceId && apIn.DataStatus == 3 && apIn.ApprovedDate < approvedDate && apIn.SubProjectCode.Contains(SubProjectCode))
                        .Select(apIn => new TotalInvoiceTableDataDTO
                        {
                            InvoiceId = apIn.InvoiceId,
                            InvoiceNo = apIn.InvoiceNo,
                            AmountDueExcludingTax = apIn.AmountDueExcludingTax,
                            SubProjectCode = apIn.SubProjectCode,
                            DataStatus = apIn.DataStatus
                        })
                        .ToList();

                }

                else
                {
                    approvedInvoice = ctx.SubProjectInvoices
                       .Where(apIn => apIn.InvoiceId != InvoiceId && apIn.DataStatus == 3 && apIn.SubProjectCode.Contains(SubProjectCode))
                       .Select(apIn => new TotalInvoiceTableDataDTO
                       {
                           InvoiceId = apIn.InvoiceId,
                           InvoiceNo = apIn.InvoiceNo,
                           AmountDueExcludingTax = apIn.AmountDueExcludingTax,
                           SubProjectCode = apIn.SubProjectCode,
                           DataStatus = apIn.DataStatus
                       })
                       .ToList();
                }
            }
            else
            {
                approvedInvoice = ctx.SubProjectInvoices
                    .Where(apIn => apIn.DataStatus == 3 && apIn.SubProjectCode.Contains(SubProjectCode))
                    .Select(apIn => new TotalInvoiceTableDataDTO
                    {
                        InvoiceId = apIn.InvoiceId,
                        InvoiceNo = apIn.InvoiceNo,
                        AmountDueExcludingTax = apIn.AmountDueExcludingTax,
                        SubProjectCode = apIn.SubProjectCode,
                        DataStatus = apIn.DataStatus
                    })
                    .ToList();
            }

            return approvedInvoice;
        }

        //public List<SubProjectResivedPayments> GetInitiallyResivedPayments(string SubProjectCode, SubProjectContext ctx)
        //{
        //    List<SubProjectResivedPayments> initiallyResivedPayments = new List<SubProjectResivedPayments>();


        //    initiallyResivedPayments = ctx.SubProjectResivedPayment
        //    .Where(apIn => apIn.DataStatus == 3 && apIn.SubProjectCode == SubProjectCode)
        //    .Select(apIn => new SubProjectResivedPayments
        //    {
        //        ResivedPaymentId = apIn.ResivedPaymentId,
        //        InvoiceId = apIn.InvoiceId,
        //        SubProjectId = apIn.SubProjectId,
        //        InvoiceNo = apIn.InvoiceNo,
        //        DateReceived = apIn.DateReceived,
        //        ChequeNo = apIn.ChequeNo,
        //        ResivedAmount = apIn.ResivedAmount,
        //        DataStatus = apIn.DataStatus,
        //        SubProjectCode = apIn.SubProjectCode
        //    })
        //    .ToList();

        //    return initiallyResivedPayments;
        //}




        public List<SubProjectResivedPayments> GetInitiallyResivedPayments(string SubProjectCode , Guid InvoiceId, SubProjectContext ctx)
        {
            var invoice = new SubProjectRepository(ctx).GetDLInvoiceBySC(SubProjectCode);
            if (invoice != null)
            {
                var resivedDate = invoice.CreatedDate;
                var DataStatus = invoice.DataStatus;
              

                //var initiallyResivedPayments = (from apIn in _context.SubProjectResivedPayments
                //                                where apIn.ResivedDate <= resivedDate && apIn.SubProjectCode == SubProjectCode
                //                                select apIn).ToList();

                var initiallyResivedPayments = (from apIn in _context.SubProjectResivedPayments
                                                where apIn.InvoiceId == InvoiceId && apIn.SubProjectCode == SubProjectCode
                                                select apIn).ToList();


                return initiallyResivedPayments;
            }
            return new List<SubProjectResivedPayments>();

        }


        public List<SubProjectInvoice> GetSubProjectInvoiceDetailsByStatus(int DataStatus, int IRoleId, Guid EmployeeId)
        {
            if (IRoleId == 1) //Admin
            {
                var amd = (from ad in _context.SubProjectInvoices
                           where ad.DataStatus == DataStatus
                           select ad).ToList();
                return amd;

            }
            else if (IRoleId == 2) //PreparedBy
            {
                var pmd = (from pB in _context.SubProjectInvoices
                           where pB.DataStatus == DataStatus && pB.PreparedBy == EmployeeId
                           select pB).ToList();
                return pmd;
            }
            else if (IRoleId == 3) //CheckedEmployeeBy
            {
                var spd = (from ceBy in _context.SubProjectInvoices
                           where ceBy.DataStatus == DataStatus && ceBy.CheckedEmployeeBy == EmployeeId
                           select ceBy).ToList();
                return spd;
               
            }
            else if (IRoleId == 4) //ApprovedEmployeeBy
            {
                var dgmd = (from appEmBy in _context.SubProjectInvoices
                            where appEmBy.DataStatus == DataStatus && appEmBy.ApprovedEmployeeBy == EmployeeId
                            select appEmBy).ToList();
                return dgmd;
            }
           
            else
            {
                return new List<SubProjectInvoice>();
            }


        }

        public List<SubProjectInvoice> GetSubProjectInvoiceDetailsById(Guid InvoiceId)
        {
            var invoiceDetails = (from ind in _context.SubProjectInvoices
                                  join sp in _context.SubProjects on ind.SubProjectId equals sp.SubProjectId
                                  join emche in _context.EmployeeVersions on ind.CheckedEmployeeBy equals emche.EmployeeId
                                  join emape in _context.EmployeeVersions on ind.ApprovedEmployeeBy equals emape.EmployeeId
                                  join empre in _context.EmployeeVersions on ind.PreparedBy equals empre.EmployeeId
                                  join proj in _context.Projects on ind.ProjectId equals proj.ProjectId
                                  join cnt in _context.Clients on proj.ClientId equals cnt.ClientId
                                  where ind.InvoiceId == InvoiceId
                                  select new SubProjectInvoice
                                  {
                                      InvoiceId = ind.InvoiceId,
                                      SubProjectId = ind.SubProjectId,
                                      ProjectId = ind.ProjectId,
                                      CheckedEmployeeBy = ind.CheckedEmployeeBy,
                                      ApprovedEmployeeBy = ind.ApprovedEmployeeBy,
                                      PreparedBy = ind.PreparedBy,
                                      SubProjectCode = ind.SubProjectCode,
                                      InvoiceNo = ind.InvoiceNo,
                                      InvoiceType = ind.InvoiceType,
                                      ProjectInvoiceNo = ind.ProjectInvoiceNo,
                                      InvoiceStage = ind.InvoiceStage,
                                      DataStatus = ind.DataStatus,
                                      AccumulatedAmountClaimed = ind.AccumulatedAmountClaimed,
                                      ServiceToBeRendered = ind.ServiceToBeRendered,
                                      VATPercentage = ind.VATPercentage,
                                      VATAmount = ind.VATAmount,
                                      CreatedDate = ind.CreatedDate,
                                      TotalAmountExcludingTax = ind.TotalAmountExcludingTax,
                                      AmountClaimedInThisInvoice = ind.AmountClaimedInThisInvoice,
                                      TotalAmountPaidExcludingTax = ind.TotalAmountPaidExcludingTax,
                                      AmountDueExcludingTax = ind.AmountDueExcludingTax,
                                      TotalAmountDueIncludingTax = ind.TotalAmountDueIncludingTax,
                                      SubProjectInvoiceDetails = ind.SubProjectInvoiceDetails,
                                      PreparedByName = empre.NameWithInitial,
                                      ApprovedEmployeeByName = emape.NameWithInitial,
                                      CheckedEmployeeByName = emche.NameWithInitial,
                                      ClientName = cnt.ClientName,
                                      ProjectCode = proj.ProjectCode,
                                      ProjectName = proj.ProjectName,
                                      MainProjectName = proj.ProjectName,
                                      SubProjectName = sp.SubProjectName,
                                      SPEstimatedValue = sp.SPEstimatedValue,
                                      SPContractValue = sp.SPContractValue,
                                      FinalBillValue = sp.FinalBillValue,
                                      ConsultancyPercentage = sp.ConsultancyPercentage,
                                      PercentageA = sp.PercentageA,
                                      PercentageB = sp.PercentageB,
                                      PercentageC = sp.PercentageC,
                                      SPConsultancyFee = sp.SPConsultancyFee,
                                      EstimatedValue = proj.EstimatedValue,
                                      ContractConsultancyFee = proj.ContractConsultancyFee
                                  }).ToList();

            return invoiceDetails;

        }

        public SubProjectInvoice GetDLInvoice(Guid InvoiceId)
        {
            var sp = (from Iid in _context.SubProjectInvoices
                      where Iid.InvoiceId == InvoiceId
                      select Iid).FirstOrDefault();
            return sp;
        }

        public SubProjectInvoice GetDLInvoiceBySC(string SubProjectCode)
        {
            var sp = (from Iid in _context.SubProjectInvoices
                      where Iid.SubProjectCode == SubProjectCode
                      select Iid).FirstOrDefault();
            return sp;
        }

        public string GetCheckedByEmail(Guid empId)
        {
            var email = (from emp in _context.EmployeeVersions
                         where emp.EmployeeId == empId
                         select emp.OfficeEmail).FirstOrDefault();
            return email;
        }



        public bool UpdateSubProjectSubProjectInvoice(SubProjectInvoice spIn)
        {
            _context.SubProjectInvoices.Attach(spIn);
            _context.Entry(spIn).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }



        public TaxDetail getTaxRate()
        {
            var taxRate = (from td in _context.TaxDetails
                         where td.TaxTypeId == Guid.Parse("4becb06b-5a81-4fa5-a6a6-9bb0828e118b") && td.IsActive == true
                            select td).FirstOrDefault();
            return taxRate;
        }


        public List<SubprojectExternalInvoiceDTO> SubprojectExternalInvoice(Guid InvoiceId)
        {
            var ExternalInvoiceDTO  = (from sI in _context.SubProjectInvoices
                                      join idet in _context.SubProjectInvoiceDetails on sI.InvoiceId equals idet.InvoiceId
                                       join sp in _context.SubProjects on sI.SubProjectId equals sp.SubProjectId
                                       join ev in _context.EmployeeVersions on sI.PreparedBy equals ev.EmployeeId
                                       join ec in _context.EmployeeVersions on sI.CheckedEmployeeBy equals ec.EmployeeId
                                       join ea in _context.EmployeeVersions on sI.ApprovedEmployeeBy equals ea.EmployeeId
                                       join p in _context.Projects on sI.ProjectId equals p.ProjectId
                                      join cnt in _context.Clients on p.ClientId equals cnt.ClientId
                                      join pl in _context.ProjectLocations on p.ProjectLocationId equals pl.ProjectLocationId
                                      where sI.InvoiceId == InvoiceId
                                      select new SubprojectExternalInvoiceDTO
                                      {
                                          ClientName = cnt.ClientName,
                                          ProjectName = p.ProjectName,
                                          InvoiceDirectedTo = p.ProjectName,
                                          SubProjectName = sp.SubProjectName,
                                          SPEstimatedValue = sp.SPEstimatedValue,
                                          SPContractValue = sp.SPContractValue,
                                          FinalBillValue = sp.FinalBillValue,
                                          ServiceToBeRendered = sI.ServiceToBeRendered,
                                          SubProjectCode = sp.SubProjectCode,
                                         // ConsultancyEstimatedValue = idet.ConsultancyEstimatedValue,
                                          ProjectCode = p.ProjectCode,
                                          ProjectInvoiceNo = sI.ProjectInvoiceNo,
                                          CreatedDate = sI.CreatedDate,
                                          InvoiceNo = sI.InvoiceNo,
                                          InvoiceType = sI.InvoiceType,
                                          InvoiceStage = sI.InvoiceStage,
                                          ServiceProvided = idet.ServiceProvided,
                                          Basis = idet.Basis,
                                          Amount =idet.Amount,
                                          WorkCompleted = idet.WorkCompleted,
                                          FinalAmount = idet.FinalAmount,
                                          TotalAmountExcludingTax = sI.TotalAmountExcludingTax,
                                          AmountDueExcludingTax = sI.AmountDueExcludingTax,
                                          AmountClaimedInThisInvoice = sI.AmountClaimedInThisInvoice,
                                          VATPercentage = sI.VATPercentage,
                                          TotalAmountDueIncludingTax = sI.TotalAmountDueIncludingTax,
                                          PreparedByName = ev.NameWithInitial,
                                          CheckedEmployeeByName = ec.NameWithInitial,
                                          ApprovedEmployeeByName = ea.NameWithInitial
                                      }).ToList();

            return ExternalInvoiceDTO;
        }

        /*public SubProjectCodeCounter GetCounter(Guid subProjectTypeId, Guid mainProjectId)
        {
            var currentYear = DateTime.Now.Year;
            var counter = (from cnter in _context.SubProjectCodeCounters
                           where cnter.SubProjectTypeId == subProjectTypeId && cnter.Year == currentYear && cnter.ProjectId == mainProjectId
                           select cnter).FirstOrDefault();
            return counter;

        }*/




            public SubProjectInvoiceCounter GetProjectInvoiceCounter(string Prefix)
        {

            var picounter = (from countPrefix in _context.SubProjectInvoiceCount
                             where countPrefix.Prefix == Prefix 
                             select countPrefix).FirstOrDefault();
            return picounter;

        }


        
           public SubProjectInvoiceNoCounter GetProjectInvoiceNoCounter(string Prefix)
        {

            var piNocounter = (from countPrefix in _context.SubProjectInvoiceNoCounters
                             where countPrefix.Prefix == Prefix
                             select countPrefix).FirstOrDefault();
            return piNocounter;

        }

        public bool AddNewProjectInvoice(SubProjectInvoiceCounter subProjectInvoiceCounter)
        {
            _context.SubProjectInvoiceCount.Add(subProjectInvoiceCounter);
            return true;
        }

        public bool AddProjectInvoice(SubProjectInvoiceCounter subProjectInvoiceCounter)
        {
            _context.SubProjectInvoiceCount.Add(subProjectInvoiceCounter);
            return true;
        }

        public bool UpdateProjInvoiceCount(SubProjectInvoiceCounter subProjectInvoiceCounter)
        {
            _context.SubProjectInvoiceCount.Attach(subProjectInvoiceCounter);
            _context.Entry(subProjectInvoiceCounter).State = EntityState.Modified;
            //_context.SaveChanges();
            return true;

        }

  




        //public SubProjectInvoiceCounter GetProjectInvoiceCounter(Guid subProjectTypeId, Guid mainProjectId)
        //{
        //    var currentYear = DateTime.Now.Year;
        //    var counter = (from cnter in _context.SubProjectCodeCounters
        //                   where cnter.SubProjectTypeId == subProjectTypeId && cnter.Year == currentYear && cnter.ProjectId == mainProjectId
        //                   select cnter).FirstOrDefault();
        //    return counter;

        //}




        //public bool AddSubProjectInvoiceDetail(SubProjectInvoiceDetail SubProjectInvoiceDetails)
        //{
        //    _context.SubProjectInvoiceDetails.AddRange(SubProjectInvoiceDetails);
        //    return true;
        //}

        public List<SubProjectInvoiceDetail> GetInvoiceDetailsIds(Guid InvoiceId)
        {
            List<SubProjectInvoiceDetail> invoice = new List<SubProjectInvoiceDetail>();

            var invoiceDetail = (from invoicedetail in _context.SubProjectInvoiceDetails
                                 where invoicedetail.InvoiceId == InvoiceId
                                 select invoicedetail).ToList();
            if (invoiceDetail.Count > 0)
            {
                foreach (var subInvoice in invoiceDetail)
                {
                    invoice.Add(subInvoice);
               }
            }

            return invoice;

        }


        public List<SubProjectInvoice> GetPreviousInvoiceBySubProjectCode(string SubProjectCode, Guid InvoiceId)
        {
            List<SubProjectInvoice> invoice = new List<SubProjectInvoice>();

            var PreviousInvoice = (from pinvoice in _context.SubProjectInvoices
                               where pinvoice.SubProjectCode == SubProjectCode && pinvoice.DataStatus == 3 && pinvoice.InvoiceId != InvoiceId
                                   select pinvoice).ToList();

            if (PreviousInvoice.Count > 0)
            {
                foreach (var subInvoice in PreviousInvoice)
                {
                    invoice.Add(subInvoice);
                }
            }

            return PreviousInvoice;
        }

        public List<SubProjectPreviousInvoiceDeatails>GetPreviousInvoice(Guid InvoiceId)
        {
            var PreviousInvoice = (from pinvoice in _context.SPPreviousInvoiceDeatails
                                   where pinvoice.InvoiceId == InvoiceId
                                   select pinvoice).ToList();
            return PreviousInvoice;
        }

        public List<SubProjectResivedPayments> GetResivedPayments(Guid InvoiceId)
        {
            var ResivedPayments = (from resivedPayments in _context.SubProjectResivedPayment
                                   where resivedPayments.InvoiceId == InvoiceId
                                   select resivedPayments).ToList();
            return ResivedPayments;
        }

        public bool UpdateInvoiceNoCount(SubProjectInvoiceNoCounter subProjectInvoiceNoCounter)
        {
            _context.SubProjectInvoiceNoCounters.Attach(subProjectInvoiceNoCounter);
            _context.Entry(subProjectInvoiceNoCounter).State = EntityState.Modified;
            //_context.SaveChanges();
            return true;

        }

        public bool AddProjectInvoice(SubProjectInvoiceNoCounter SubProjectInvoiceNoCounters)
        {
            _context.SubProjectInvoiceNoCounters.Add(SubProjectInvoiceNoCounters);
            return true;
        }

        public bool UpdateProjectInvoice(SubProjectInvoiceNoCounter SubProjectInvoiceNoCounters)
        {
            _context.SubProjectInvoiceNoCounters.Attach(SubProjectInvoiceNoCounters);
            _context.Entry(SubProjectInvoiceNoCounters).State = EntityState.Modified;
            //_context.SaveChanges();
            return true;

        }

    }
}
