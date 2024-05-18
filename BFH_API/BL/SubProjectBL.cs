using CECBERP.CMN.Business.Entities;
using CECBERP.CMN.Business.Entities.FIN;
using CECBERP.CMN.Business.Entities.PM;
using CECBERP.CMN.Business.Entities.SP;
using HUSP_API.Data;
using HUSP_API.Data.Repositories;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
//using System.Net.Mail;
using System.Threading.Tasks;
using static CECBERP.CMN.Business.Entities.WorkSpace;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Globalization;

namespace HUSP_API.BL
{
    public class SubProjectBL
    {
        private readonly SubProjectInvoice subProjInvoice;

        public bool SaveSubProject(SubProject subProj, out Guid subProjId, out string saveMessage, SubProjectContext _context)
        {
            var result = true;
            subProjId = Guid.Empty;
            saveMessage = String.Empty;
            var mainProjId = new SubProjectRepository(_context).GetProject(subProj.ProjectId);
            if ((subProj.SubProjectTypeId == Guid.Parse("b6bd91ed-7b16-447d-911e-9102eec77bcb")))
            {
                if (CheckIfMRExists(_context, subProj.ProjectId))
                {
                    saveMessage = "MR code for the current year exists";
                    return false;
                }

            }

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {

                    subProj.SubProjectId = Guid.NewGuid();
                    subProjId = subProj.SubProjectId;
                    //subProj.RequesterId = Guid.Parse("38618B95-8CDB-4D83-BDD2-95DB4FD7B7CA");
                    subProj.RequestedDate = DateTime.Now;
                    Guid workSpaceId = Guid.Empty;
                    result = SaveWorkSpace(subProj, _context, out workSpaceId);

                    if (result)
                    {
                        subProj.WorkSpaceId = workSpaceId;
                        result = new SubProjectRepository(_context).AddSubProject(subProj);
                        if (result)
                        {
                            result = SaveSubProjectServices(subProj, _context);
                        }

                    }


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }
            }
            return result;
        }

        public bool SaveProjectProgress(SubProjectProgress subprpro, SubProjectContext _context)
        {
            var result = true;

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {

                    subprpro.ProgressId = Guid.NewGuid();

                    if (result)
                    {
                        result = new SubProjectRepository(_context).AddProjectProgress(subprpro);

                    }

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }

            }
            return result;
        }

        public bool SaveMrnewJobCodes(SubProjectMRJobs sumr, SubProjectContext _context)
        {

            var result = true;
            var subId = sumr.SubProjectId;
            int lastNo = 0;
            sumr.JobNo = SetMRJobCode(subId, out lastNo, _context);
            using (var ts = _context.Database.BeginTransaction())
            {
                new SubProjectRepository(_context).AddMrNewJobNo(sumr);
                try
                {

                    if (result)
                    {
                        if (lastNo == 1)
                        {
                            var mrcntr = new SubProjectMRJobCounter();
                            mrcntr.MrJobCounterId = Guid.NewGuid();
                            mrcntr.SubProjectId = subId;
                            mrcntr.LastJobNo = 1;
                            new SubProjectRepository(_context).AddMRCounter(mrcntr);
                        }
                        else
                        {
                            var mrspCounter = new SubProjectRepository(_context).GetMRCounter(subId);
                            mrspCounter.LastJobNo = lastNo;
                            new SubProjectRepository(_context).UpdateMRCounter(mrspCounter);
                        }
                    }

                }


                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }

            }
            return result;
        }


        public bool CheckIfMRExists(SubProjectContext _context, Guid projectId)
        {
            var currentYear = DateTime.Now.Year;
            var subCounter = new SubProjectRepository(_context).CheckIfMRExists(currentYear, projectId);
            if (subCounter != null)
            {
                return true;
            }
            return false;
        }

        public bool UpdateSubProjectDetails(SubProject subProj, SubProjectContext _context, out string updateMessage)
        {
            var result = true;
            updateMessage = String.Empty;


            if (subProj.SubProjectTypeId == Guid.Parse("b6bd91ed-7b16-447d-911e-9102eec77bcb"))
            {
                if (CheckIfMRExists(_context, subProj.ProjectId))
                {
                    updateMessage = "MR code for the current year exists";
                    return false;
                }

            }

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {


                    var sp = new SubProjectRepository(_context).GetSubProject(subProj.SubProjectId);
                    if (sp != null)
                    {
                        sp.SubProjectTypeId = subProj.SubProjectTypeId;
                        sp.SubProjectName = subProj.SubProjectName;
                        sp.SPEstimatedValue = subProj.SPEstimatedValue;
                        sp.ServiceByHU = subProj.ServiceByHU;
                        sp.ConsultancyPercentage = subProj.ConsultancyPercentage;
                        sp.PercentageA = subProj.PercentageA;
                        sp.PercentageB = subProj.PercentageB;
                        sp.PercentageC = subProj.PercentageC;
                        //sp.HandledByHospitalWorks = subProj.HandledByHospitalWorks;
                        sp.SPContractValue = subProj.SPContractValue;
                        sp.SPConsultancyFee = subProj.SPConsultancyFee;
                        sp.FinalBillValue = subProj.FinalBillValue;
                        sp.DateOfCommencement = subProj.DateOfCommencement;
                        sp.ShedulledCompletionDate = subProj.ShedulledCompletionDate;
                        sp.ProjectManagerId = subProj.ProjectManagerId;
                        sp.ProjectId = subProj.ProjectId;
                    }


                    if (result)
                    {
                        result = new SubProjectRepository(_context).UpdateSubProjectCode(sp);
                        if (result)
                        {
                            result = SaveSubProjectServices(subProj, _context);
                        }
                    }


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }
            }

            return result;
        }

        public bool UpdateApprovedSubProject(SubProject subProj, SubProjectContext _context)
        {
            var result = true;

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {


                    var sp = new SubProjectRepository(_context).GetSubProject(subProj.SubProjectId);
                    if (sp != null)
                    {
                        sp.SubProjectCode = subProj.SubProjectCode;
                        sp.SubProjectName = subProj.SubProjectName;
                        sp.SPEstimatedValue = subProj.SPEstimatedValue;
                        sp.ConsultancyPercentage = subProj.ConsultancyPercentage;
                        sp.PercentageA = subProj.PercentageA;
                        sp.PercentageB = subProj.PercentageB;
                        sp.PercentageC = subProj.PercentageC;
                        sp.SPContractValue = subProj.SPContractValue;
                        sp.SPConsultancyFee = subProj.SPConsultancyFee;
                        sp.FinalBillValue = subProj.FinalBillValue;

                        sp.TerminationDate = subProj.TerminationDate;
                        sp.TReason = subProj.TReason;
                        if (sp.TerminationDate != null)
                        {
                            subProj.ProjectStatus = 2;
                        }
                        sp.ProjectStatus = subProj.ProjectStatus;
                    }

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }
            }

            return result;
        }


        public bool UpdateMrSubProject(SubProjectMRJobs mrsubProj, SubProjectContext _context)
        {
            var result = true;

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {

                    var mrsp = new SubProjectRepository(_context).GettoEditMrJobs(mrsubProj.MrJobId);
                    if (mrsp != null)
                    {
                        mrsp.SubProjectCode = mrsubProj.SubProjectCode;
                        mrsp.RequestDate = mrsubProj.RequestDate;
                        mrsp.EstimatedDate = mrsubProj.EstimatedDate;
                        mrsp.EstimatedValue = mrsubProj.EstimatedValue;
                        mrsp.WorkDoneDate = mrsubProj.WorkDoneDate;
                        mrsp.WorkDoneValue = mrsubProj.WorkDoneValue;
                        mrsp.Description = mrsubProj.Description;
                    }
                }

                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }
            }

            return result;
        }


        public bool FinishSubProject(Guid SubProjectId, SubProjectContext _context)
        {
            var result = true;
            var sp = new SubProjectRepository(_context).GetSubProject(SubProjectId);
            sp.DataStatus = 1;
            result = new SubProjectRepository(_context).UpdateSubProject(sp);
            if (result)
            {
                var subProjectName = sp.SubProjectName;
                var pmEmail = new SubProjectRepository(_context).GetPMEmail(sp.ProjectManagerId);
                //var pmEmail = "ajr.office2021@gmail.com";
                if (!String.IsNullOrEmpty(pmEmail))
                {
                    var mailResult = SendFinishedEmail(pmEmail, subProjectName);

                }
            }
            return result;
        }


        public bool ApprovedSubProject(Guid SubProjectId, Guid SubProjectTypeId, SubProjectContext _context, Guid EmployeeId, AuthenticationContext _authcontext, out string updateMessage)
        {

            var result = true;
            updateMessage = String.Empty;
            var sp = new SubProjectRepository(_context).GetSubProject(SubProjectId);

            if (sp.SubProjectTypeId == Guid.Parse("b6bd91ed-7b16-447d-911e-9102eec77bcb"))
            {
                if (CheckIfMRExists(_context, sp.ProjectId))
                {
                    updateMessage = "MR code for the current year exists";
                    return false;
                }

            }



            var mainProj = new SubProjectRepository(_context).GetProject(sp.ProjectId);
            var projectId = sp.ProjectId;
            sp.DataStatus = 2;
            var spTypeId = sp.SubProjectTypeId;
            sp.ApproverId = Guid.Parse("29A97497-C527-4161-BD70-023F5071BF1C");
            sp.ApprovedDate = DateTime.Now;


            if (spTypeId != Guid.Parse("b6bd91ed-7b16-447d-911e-9102eec77bcb"))
            {
                int lastNo = 0;
                sp.SubProjectCode = SetSubProjectCode(spTypeId, sp.ProjectId, mainProj.ProjectCode, out lastNo, _context);



                using (var ts = _context.Database.BeginTransaction())
                {
                    try
                    {
                        result = new SubProjectRepository(_context).UpdateSubProjectCode(sp);
                        if (result)
                        {
                            if (lastNo == 1)
                            {
                                var cntr = new SubProjectCodeCounter();
                                cntr.SPCodeCounterId = Guid.NewGuid();
                                cntr.SubProjectTypeId = spTypeId;
                                cntr.ProjectId = projectId;
                                cntr.Year = DateTime.Now.Year;
                                cntr.LastJobNo = 1;
                                new SubProjectRepository(_context).AddCounter(cntr);
                            }
                            else
                            {
                                var spCounter = new SubProjectRepository(_context).GetCounter(spTypeId, projectId);
                                spCounter.LastJobNo = lastNo;
                                new SubProjectRepository(_context).UpdateCounter(spCounter);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ts.Rollback();
                        result = false;
                    }
                    _context.SaveChanges();
                    ts.Commit();
                }
            }
            else //Adding MR jOb code
            {
                sp.SubProjectCode = SetSubProjectCodeForMr(spTypeId, mainProj.ProjectId, mainProj.ProjectCode, _context);

                using (var ts = _context.Database.BeginTransaction())
                {
                    try
                    {
                        result = new SubProjectRepository(_context).UpdateSubProjectCode(sp);
                        if (result)
                        {
                            var cntr = new SubProjectCodeCounter();
                            cntr.SPCodeCounterId = Guid.NewGuid();
                            cntr.SubProjectTypeId = spTypeId;
                            cntr.ProjectId = projectId;
                            cntr.Year = DateTime.Now.Year;
                            cntr.LastJobNo = 1;
                            new SubProjectRepository(_context).AddCounter(cntr);
                        }
                    }
                    catch (Exception ex)
                    {
                        ts.Rollback();
                        result = false;
                    }
                    _context.SaveChanges();
                    ts.Commit();
                }
            }
            if (result)
            {
                var subProjectName = sp.SubProjectName;
                var subProjectCode = sp.SubProjectCode;
                var requesterEmail = new SubProjectRepository(_context).GetEmployeeEmail(sp.RequesterId);
                if (!String.IsNullOrEmpty(requesterEmail))
                {
                    var mailResult = SendApprovedEmailAsync(requesterEmail, subProjectName, subProjectCode);
                }
            }

            return result;
        }

        private string SetSubProjectCode(Guid subProjTypeId, Guid mainProjectId, string mainProjCode, out int lastJobNo, SubProjectContext _context)
        {
            var subCode = String.Empty;
            var subType = new SubProjectRepository(_context).GetSubProjectType(subProjTypeId);
            var shortName = subType.ShortName;
            var currentYear = DateTime.Now.Year;
            var spCounter = new SubProjectRepository(_context).GetCounter(subProjTypeId, mainProjectId);

            if (spCounter != null)
            {
                var nextNo = spCounter.LastJobNo + 1;
                subCode = mainProjCode + "/" + shortName + "/" + currentYear + "/" + nextNo.ToString();
                lastJobNo = nextNo;

            }
            else
            {

                subCode = mainProjCode + "/" + shortName + "/" + currentYear + "/1";
                lastJobNo = 1;

            }



            return subCode;
        }


        //Adding MR jOb code
        private string SetSubProjectCodeForMr(Guid subProjTypeId, Guid mainProjectId, string mainProjCode, SubProjectContext _context)
        {
            var subCodeMr = String.Empty;
            var subType = new SubProjectRepository(_context).GetSubProjectType(subProjTypeId);
            var shortName = subType.ShortName;
            var currentYear = DateTime.Now.Year;
            var spCounter = new SubProjectRepository(_context).GetCounter(subProjTypeId, mainProjectId);

            subCodeMr = mainProjCode + "/" + shortName + "/" + currentYear;

            return subCodeMr;
        }


        public bool RejectedSubProject(Guid SubProjectId, string rejectReason, SubProjectContext _context)
        {
            var result = true;
            var sp = new SubProjectRepository(_context).GetSubProject(SubProjectId);
            sp.DataStatus = 3;
            sp.RejectedDate = DateTime.Now;
            sp.RejectedReason = rejectReason;

            result = new SubProjectRepository(_context).UpdateSubProject(sp);

            if (result)
            {
                var subProjectName = sp.SubProjectName;
                var subProjectRejectReason = sp.RejectedReason;
                var requesterEmail = new SubProjectRepository(_context).GetEmployeeEmail(sp.RequesterId);
                if (!String.IsNullOrEmpty(requesterEmail))
                {
                    var mailResult = SendRejectEmailAsync(requesterEmail, subProjectName, subProjectRejectReason);
                }
            }

            return result;
        }




        private bool SaveSubProjectServices(SubProject subProject, SubProjectContext _context)
        {
            var result = true;
            var services = subProject.ServiceId;
            var serviceUnits = subProject.ServiceWorkSpaceIds;
            var existingServiceUnits = new SubProjectRepository(_context).GetExistingServiceUnits(subProject.SubProjectId);
            if (existingServiceUnits != null && existingServiceUnits.Count > 0)
            {
                result = new SubProjectRepository(_context).DeleteServices(existingServiceUnits);
            }
            if (result)
            {
                if (services != null && services.Length > 0)
                {
                    List<SubProjectServiceByOtherUnits> selectedServs = new List<SubProjectServiceByOtherUnits>();
                    for (int i = 0; i < services.Length; i++)
                    {
                        var selServ = new SubProjectServiceByOtherUnits();
                        selServ.ServiceId = services[i];
                        selServ.WorkSpaceId = serviceUnits[i];
                        selServ.SubProjectId = subProject.SubProjectId;
                        selServ.SubProjectSOUId = new Guid();
                        selectedServs.Add(selServ);
                    }
                    result = new SubProjectRepository(_context).AddServices(selectedServs);
                }
            }
            return result;
        }




        private bool SaveSubProjectCode(SubProjectCodeCounter spcc, Guid spCodeCounterId, SubProjectContext _context)
        {
            var result = true;
            spCodeCounterId = Guid.Empty;
            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {
                    spcc.SPCodeCounterId = Guid.NewGuid();
                    spCodeCounterId = spcc.SPCodeCounterId;
                    spcc.Year = DateTime.Now.Year;
                    //spcc.LastJobNo =
                    //subProj.SubProjectCode = SetSubProjectCode(subProj.SubProjectTypeId, _context);
                    Guid SubProjectTypeId = Guid.Empty;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                _context.SaveChanges();
                ts.Commit();
            }

            return result;
        }

        private int SetMRJobCode(Guid SubProjectId, out int lastJobNo, SubProjectContext _context)
        {
            var subCode = String.Empty;
            var MrspCounter = new SubProjectRepository(_context).GetMRCounter(SubProjectId);

            if (MrspCounter != null)
            {
                var nextNo = MrspCounter.LastJobNo + 1;
                lastJobNo = nextNo;

            }
            else
            {
                lastJobNo = 1;
            }

            return lastJobNo;
        }



        private bool SaveWorkSpace(SubProject subProj, SubProjectContext _context, out Guid workSpaceId)
        {
            WorkSpace ws = new WorkSpace();
            ws.WorkSpaceId = Guid.NewGuid();
            workSpaceId = ws.WorkSpaceId;
            ws.WorkSpaceName = subProj.SubProjectName;
            ws.WorkSpaceTypeId = Guid.Parse("bd1cf5f7-0a8c-4880-a44a-71650416d33c");
            ws.ParentWorkSpaceId = new SubProjectRepository(_context).GetParentWorkSpaceId(subProj.ProjectId);
            ws.BusinessUnitId = Guid.Parse("f8ad62ee-8fc1-41c8-bf8d-2f1dfebd1d0a");
            ws.OrganizationId = Guid.Parse("bae027a7-2fa5-41ba-8753-1450fd21b181");
            ws.CreatedDateTime = DateTime.Now;
            ws.CreatedUserId = Guid.NewGuid();
            ws.AccountCode = Guid.Empty;
            var result = new SubProjectRepository(_context).AddWorkSpace(ws);
            return result;
        }

        public SubProjectDTO GetSubProjectDetailsById(Guid SubProjectId, SubProjectContext _context)
        {
            try
            {
                var subProject = new SubProjectRepository(_context).GetSubProjectDetailsById(SubProjectId);
                subProject.ServicesByOtherUnits = new SubProjectRepository(_context).GetServiceIds(subProject.SubProjectId);
                return subProject;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubprojectProgressQuarterlyReportDTO> SubprojectProgressQuarterlyReport(DateTime CurrentDate, string SubProjectCode, int rType, SubProjectContext _context)
        {
            try
            {
                var qreport = new SubProjectRepository(_context).SubprojectProgressQuarterlyReport(CurrentDate, SubProjectCode);
                foreach (var qr in qreport)
                {
                    qr.CurrentDate = qr.CurrentDate.Value.Date;
                    qr.SubProjectCode = qr.SubProjectCode;
                    //qr.CurrentDateString = qr.CurrentDate.Value.Year + " /" + qr.CurrentDate.Value.Month + " /" + qr.CurrentDate.Value.Day;
                    //qr.DateOfCommencementString = qr.DateOfCommencement.Value.Year + " /" + qr.DateOfCommencement.Value.Month + " /" + qr.DateOfCommencement.Value.Day;
                    //qr.ShedulledCompletionDateString = qr.ShedulledCompletionDate.Value.Year + " /" + qr.ShedulledCompletionDate.Value.Month + " /" + qr.ShedulledCompletionDate.Value.Day;
                }

                return qreport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubprojectWorkVsFinancialPhysicalProgressReportDTO> SubprojectWorkVsFinancialPhysicalProgressReport(DateTime CurrentDate, string SubProjectCode, int rType, SubProjectContext _context)
        {
            try
            {
                var wfreport = new SubProjectRepository(_context).SubprojectWorkVsFinancialPhysicalProgressReport(CurrentDate, SubProjectCode);

                foreach (var wf in wfreport)
                {
                    wf.CurrentDate = wf.CurrentDate.Value.Date;
                    wf.SubProjectCode = wf.SubProjectCode;
                    //qr.CurrentDate = qr.CurrentDate.Value.Date;
                    //wf.CurrentDateString = wf.CurrentDate.Value.Year + " /" + wf.CurrentDate.Value.Month + " /" + wf.CurrentDate.Value.Day;

                }

                return wfreport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubprojectWorkVsIncomeReportDTO> SubprojectWorkVsIncomeReport(DateTime CurrentDate, string SubProjectCode, int rType, SubProjectContext _context)
        {
            try
            {
                var wireport = new SubProjectRepository(_context).SubprojectWorkVsIncomeReport(CurrentDate, SubProjectCode);

                foreach (var wi in wireport)
                {
                    wi.CurrentDate = wi.CurrentDate.Value.Date;
                    wi.SubProjectCode = wi.SubProjectCode;
                    //qr.CurrentDate = qr.CurrentDate.Value.Date;
                    //wi.CurrentDateString = wi.CurrentDate.Value.Year + " /" + wi.CurrentDate.Value.Month + " /" + wi.CurrentDate.Value.Day;

                }

                return wireport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Project GetProject(Guid ProjectId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetProject(ProjectId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectMRJobs> GetSubProjectMRJobs(Guid SubProjectId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSubProjectMRJobs(SubProjectId);
            }

            catch (Exception ex)
            {
                throw;
            }
        }


        public List<SubProjectServices> GetAllSubProjectServices(SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetAllSubProjectServices();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubprojectType> GetSubProjectNames(SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSubProjectNames();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Project> GetProjects(SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetProjects();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Project> GetProjectByCode(string ProjectCode, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetProjectByCode(ProjectCode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProject> GetMrJobCode(string SubProjectCode, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetMrJobCode(SubProjectCode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectProgress> GetSubProjectProgressHistory(Guid SubProjectId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSubProjectProgressHistory(SubProjectId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectProgress> GetSpProgressById(Guid ProgressId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSpProgressById(ProgressId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectMRJobs> GetMrJobs(Guid MrJobId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetMrJobs(MrJobId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProject> GetApprovedSubProjectByCode(string SubProjectCode, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetApprovedSubProjectByCode(SubProjectCode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Project> GetProjectByLocation(string CityName, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetProjectByLocation(CityName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<EmployeeVersion> GetPMByName(string FullName, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetPMByName(FullName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public List<SubProject> GetPendingApprovalSubProjectDetails(int RoleId, Guid EmployeeId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetPendingApprovalSubProjectDetails(RoleId, EmployeeId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<WorkSpaceServiceDTO> GetWorkSpaces(Guid ServiceId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetWorkSpaces(ServiceId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<SubProject> GetSubProjectDetailsByStatus(int DataStatus, int RoleId, Guid EmployeeId, SubProjectContext _context)
        {

            try
            {
                return new SubProjectRepository(_context).GetSubProjectDetailsByStatus(DataStatus, RoleId, EmployeeId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IQueryable<Object> GetSubProjectDetails(int DataStatus, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSubProjectDetails(DataStatus);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        //Send email when finish
        public async Task SendFinishedEmail(string toEMail, string spName)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEMail));
            email.Subject = "Subproject to  Approve";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
              "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
              "<title>Untitled Document</title>" +
              "</head><body>" +

              "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
              "Dear Sir/Madam,</strong></p>" +

              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "You have following Subproject Request to approve.</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Name     :-" + spName + "</p>" +

               "<b>" + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
               "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a> to approve." + ".</p>" +
              "<br />" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "Thank You,</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
              "This email was auto-generated by the CECB-ERP System.</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        //send email when approved
        public async Task SendApprovedEmailAsync(string requesterEmail, string spName, string spCode)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(requesterEmail));
            email.Subject = "Subproject Request Approved";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
                 "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
                  "<title>Untitled Document</title>" +
                  "</head><body>" +

                  "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
                  "Dear Sir/Madam,</strong></p>" +

                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Following Subproject Request was approved.</p>" +

                  "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Code     :-" + spCode + "</p>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Name     :-" + spName + "</p>" +

                    "<b>" +

                    "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                      "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a>" +
                      ".</p>" +

                  "<br />" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Thank You,</p>" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "</p>" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
                  "This email was auto-generated by the CECB-ERP System.</p>" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }



        //send email when rejected
        public async Task SendRejectEmailAsync(string requesterEmail, string spName, string reason)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(requesterEmail));
            email.Subject = "Subproject Request Approved";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
                 "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
                  "<title>Untitled Document</title>" +
                  "</head><body>" +

                  "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
                  "Dear Sir/Madam,</strong></p>" +

                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Following Subproject Request was rejected.</p>" +

                  "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Name     :-" + spName + "</p>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Rejected Reason     :-" + reason + "</p>" +

                    "<b>" +

                    "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                      "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a>" +
                      ".</p>" +

                  "<br />" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Thank You,</p>" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "</p>" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
                  "This email was auto-generated by the CECB-ERP System.</p>" +
                  "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public List<SubProjectDTO> GetSubProjectDetailsByCodeToInvoice(string SubProjectCode, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSubProjectDetailsByCodeToInvoice(SubProjectCode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<EmployeeVersion> GetCheckedOrApprovedByName(string FullName, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetCheckedOrApprovedByName(FullName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //private bool SaveSubProjectInvoiceDetail(SubProjectInvoice subProjInvoice, SubProjectContext _context)
        //{
        //    var result = true;
        //    var invoice = subProjInvoice.InvoiceId;

        //    if (result)
        //    {
        //                var savedInvoice = new List<SubProjectInvoiceDetail>();

        //                var addinvoice = new SubProjectInvoiceDetail();
        //                addinvoice.InvoiceId = invoice;
        //                addinvoice.InvoiceDetailId = new Guid();
        //                addinvoice.ServiceProvided = subProjInvoice.ServiceProvided;
        //                addinvoice.Basis = subProjInvoice.Basis;
        //                addinvoice.WorkCompleted = subProjInvoice.WorkCompleted;
        //                addinvoice.Amount = subProjInvoice.Amount;
        //                addinvoice.FinalAmount = subProjInvoice.FinalAmount;
        //                savedInvoice.Add(addinvoice);

        //            result = new SubProjectRepository(_context).AddSubProjectInvoiceDetail(savedInvoice);

        //    }
        //    return result;
        //}




        public bool SaveSubProjectInvoiceDetails(SubProjectInvoice subProjInvoice, out Guid InvoiceId, out string saveInvoiceMsg, SubProjectContext _context)
        {
            var result = true;
            InvoiceId = Guid.Empty;
            saveInvoiceMsg = String.Empty;
            var SubProjectId = subProjInvoice.SubProjectId;
        

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {

                    subProjInvoice.InvoiceId = Guid.NewGuid();
                    InvoiceId = subProjInvoice.InvoiceId;


                    if (result)
                    {

                        result = new SubProjectRepository(_context).AddSubProjectInvoiceDetails(subProjInvoice);
                        if (result)
                        {

                            var obj = new List<SubProjectInvoiceDetail>();
                            for (int i = 0; i < obj.Count; i++)
                            {
                                var subInDetails = new SubProjectInvoiceDetail();
                                subInDetails.InvoiceId = InvoiceId;
                                subInDetails.InvoiceDetailId = Guid.NewGuid();
                                obj.Add(subInDetails);
                            }
                            result = new SubProjectRepository(_context).AddSubProjectInvoiceDetail(obj);
                        }

                        if (result)
                        {
                            var objRP = new List<SubProjectResivedPayments>();

                            for (int i = 0; i < objRP.Count; i++)
                            {
                                var paymentResived = new SubProjectResivedPayments();
                                paymentResived.InvoiceId = InvoiceId;
                                paymentResived.SubProjectId = subProjInvoice.SubProjectId;
                                paymentResived.SubProjectCode = subProjInvoice.SubProjectCode;
                                paymentResived.ResivedDate = DateTime.Now;
                                paymentResived.DataStatus = subProjInvoice.DataStatus;
                                paymentResived.ResivedPaymentId = Guid.NewGuid();
                                objRP.Add(paymentResived);
                            }
                            result = new SubProjectRepository(_context).AddSubProjectResivedPayment(objRP);
                        }

                        if (result){
                            var subprObj = new List<SubProjectPreviousInvoiceDeatails>();
 

                            for (int i = 0; i < subprObj.Count; i++)
                            {
                                var subPreInDetails = new SubProjectPreviousInvoiceDeatails();
                                subPreInDetails.InvoiceId = InvoiceId;
                                subPreInDetails.PreviousInvoiceDeatailsId = Guid.NewGuid();
                                subPreInDetails.InvoiceAddedDate = DateTime.Now;
                                subPreInDetails.DataStatus = subProjInvoice.DataStatus;
                                subPreInDetails.SubProjectCode = subProjInvoice.SubProjectCode;

                                subprObj.Add(subPreInDetails);
                            }
                            new SubProjectRepository(_context).AddSubProjectPreviousInvoiceDeatails(subprObj);

                        }
                    }


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }

            }
            return result;
        }


        public List<SubProjectInvoice> GetInvoiceBySubProjectCode(string SubProjectCode, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetInvoiceBySubProjectCode(SubProjectCode);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TotalInvoiceTableDataDTO> GetTotalApprovedInvoice(string SubProjectCode, Guid InvoiceId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetTotalApprovedInvoice(SubProjectCode, InvoiceId, _context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectResivedPayments> GetInitiallyResivedPayments(string SubProjectCode, Guid InvoiceId, SubProjectContext _context)
        {

            try
            {
                return new SubProjectRepository(_context).GetInitiallyResivedPayments(SubProjectCode, InvoiceId, _context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public List<SubProjectInvoice> GetTotalApprovedInvoice(string SubProjectCode, SubProjectContext _context)
        //{
        //    try
        //    {
        //        return new SubProjectRepository(_context).GetTotalApprovedInvoice(SubProjectCode);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public List<SubProjectInvoice> GetSubProjectInvoiceDetailsByStatus(int DataStatus, int IRoleId, Guid EmployeeId, SubProjectContext _context)
        {

            try
            {
                return new SubProjectRepository(_context).GetSubProjectInvoiceDetailsByStatus(DataStatus, IRoleId, EmployeeId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectInvoice> GetSubProjectInvoiceDetailsById(Guid InvoiceId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetSubProjectInvoiceDetailsById(InvoiceId);


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UpdateInvoiceDetails(SubProjectInvoice invoiceDetails, SubProjectContext _context, out string upMsg)
        {
            var result = true;
            upMsg = String.Empty;

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {
                    var IND = new SubProjectRepository(_context).GetDLInvoice(invoiceDetails.InvoiceId);
                    if (IND != null)
                    {
                        IND.InvoiceId = invoiceDetails.InvoiceId;
                        IND.SubProjectId = invoiceDetails.SubProjectId;
                        IND.ProjectId = invoiceDetails.ProjectId;
                        IND.CheckedEmployeeBy = invoiceDetails.CheckedEmployeeBy;
                        IND.ApprovedEmployeeBy = invoiceDetails.ApprovedEmployeeBy;
                        IND.PreparedBy = invoiceDetails.PreparedBy;
                        IND.SubProjectCode = invoiceDetails.SubProjectCode;
                        IND.InvoiceNo = invoiceDetails.InvoiceNo;
                        IND.ProjectInvoiceNo = invoiceDetails.ProjectInvoiceNo;
                        IND.InvoiceStage = invoiceDetails.InvoiceStage;
                        IND.DataStatus = invoiceDetails.DataStatus;
                        IND.AccumulatedAmountClaimed = invoiceDetails.AccumulatedAmountClaimed;
                        IND.ServiceToBeRendered = invoiceDetails.ServiceToBeRendered;
                        IND.VATPercentage = invoiceDetails.VATPercentage;
                        IND.VATAmount = invoiceDetails.VATAmount;
                        IND.CreatedDate = invoiceDetails.CreatedDate;
                        IND.TotalAmountExcludingTax = invoiceDetails.TotalAmountExcludingTax;
                        IND.AmountClaimedInThisInvoice = invoiceDetails.AmountClaimedInThisInvoice;
                        IND.TotalAmountPaidExcludingTax = invoiceDetails.TotalAmountPaidExcludingTax;
                        IND.AmountDueExcludingTax = invoiceDetails.AmountDueExcludingTax;
                        IND.TotalAmountDueIncludingTax = invoiceDetails.TotalAmountDueIncludingTax;
                        IND.SubProjectInvoiceDetails = invoiceDetails.SubProjectInvoiceDetails;
                        IND.ClientName = invoiceDetails.ClientName;
                        IND.ProjectCode = invoiceDetails.ProjectCode;
                        IND.ProjectName = invoiceDetails.ProjectName;
                        IND.MainProjectName = invoiceDetails.ProjectName;
                        IND.SubProjectName = invoiceDetails.SubProjectName;
                        IND.SPEstimatedValue = invoiceDetails.SPEstimatedValue;
                        IND.SPContractValue = invoiceDetails.SPContractValue;
                        IND.FinalBillValue = invoiceDetails.FinalBillValue;
                        IND.ConsultancyPercentage = invoiceDetails.ConsultancyPercentage;
                        IND.PercentageA = invoiceDetails.PercentageA;
                        IND.PercentageB = invoiceDetails.PercentageB;
                        IND.PercentageC = invoiceDetails.PercentageC;
                        IND.SPConsultancyFee = invoiceDetails.SPConsultancyFee;
                        IND.EstimatedValue = invoiceDetails.EstimatedValue;
                        IND.ContractConsultancyFee = invoiceDetails.ContractConsultancyFee;
                        IND.InvoiceType = invoiceDetails.InvoiceType;
                        IND.SubProjectResivedPayment = invoiceDetails.SubProjectResivedPayment;
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                if (result)
                {
                    _context.SaveChanges();
                    ts.Commit();
                }
            }

            return result;

        }


        //Send email when finish

        public async Task SendFinishedEmail(string toEMail, string spCode, string InNo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEMail));
            email.Subject = "Subproject Invoice to  Check";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
              "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
              "<title>Untitled Document</title>" +
              "</head><body>" +

              "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
              "Dear Sir/Madam,</strong></p>" +

              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "You have following Subproject Invoice to check.</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Code     :-" + spCode + "</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Invoice No     :-" + InNo + "</p>" +

               "<b>" + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
               "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a> to check." + ".</p>" +
              "<br />" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "Thank You,</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
              "This email was auto-generated by the CECB-ERP System.</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        //private string SPInvoiceNo(out int lastJobNo, SubProjectContext _context)
        //{
        //    var invoiceNo = String.Empty;
        //    var currentYear = DateTime.Now.Year;
        //    var hospitalWorks = "HW";

        //    var prifix = hospitalWorks + "/" + currentYear;

        //    var projectInvoiceNoCount = new SubProjectRepository(_context).GetProjectInvoiceNoCounter(prifix);

        //    if (projectInvoiceNoCount != null)
        //    {
        //        var nextNo = projectInvoiceNoCount.NextNo + 1;
        //        invoiceNo = prifix + "/" + nextNo.ToString();
        //        lastJobNo = nextNo;

        //    }
        //    else
        //    {
        //        invoiceNo = prifix + "/1";
        //        lastJobNo = 1;

        //    }

        //    return invoiceNo;
        //}

        public bool finishdInvoice(Guid InvoiceId, SubProjectContext _context)
        {
            var result = true;
            int lastJobNo; // Define the variable to capture the output from SPInvoiceNo
            var InId = new SubProjectRepository(_context).GetDLInvoice(InvoiceId);
            InId.DataStatus = 1;
            InId.FinishedDate = DateTime.Now;
            var currentYear = DateTime.Now.Year;
            var hospitalWorks = "HW";

            var prifix = hospitalWorks + "/" + currentYear;
            InId.InvoiceNo = SPInvoiceNo(out lastJobNo, _context);

            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {
                    result = new SubProjectRepository(_context).UpdateSubProjectSubProjectInvoice(InId);
                    if (result)
                    {
                        var subProjectCode = InId.SubProjectCode;
                        var invoiceNumber = InId.InvoiceNo;
                        var finishemail = new SubProjectRepository(_context).GetEmployeeEmail(InId.CheckedEmployeeBy);
                        //var checkedEmail = "anganarathnayake10@gmail.com";
                        if (!String.IsNullOrEmpty(finishemail))
                        {         
                            if (lastJobNo == 1)
                            {
                                var InNoCount = new SubProjectInvoiceNoCounter();
                                InNoCount.SubProjectInvoiceNoCounterId = Guid.NewGuid();
                                InNoCount.Prefix = prifix;
                                InNoCount.NextNo = 1;
                                new SubProjectRepository(_context).AddProjectInvoice(InNoCount);
                            }
                            else
                            {
                                var projectInvoiceNoCount = new SubProjectRepository(_context).GetProjectInvoiceNoCounter(prifix);
                                projectInvoiceNoCount.NextNo = lastJobNo;
                                new SubProjectRepository(_context).UpdateProjectInvoice(projectInvoiceNoCount);
                            }

                        }
                        var mailResult = SendFinishedEmail(finishemail, subProjectCode, invoiceNumber);
                    }

                    }
            
                 catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }
                _context.SaveChanges();
                    ts.Commit();
                }

            return result;
        }



        //Send email when checked
        public async Task SendCheckedEmail(string toEMail, string spCode, string InNo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEMail));
            email.Subject = "Subproject Invoice to  Approve";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
              "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
              "<title>Untitled Document</title>" +
              "</head><body>" +

              "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
              "Dear Sir/Madam,</strong></p>" +

              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "You have following Subproject Invoice to Approve.</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Code     :-" + spCode + "</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Invoice No     :-" + InNo + "</p>" +

               "<b>" + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
               "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a> to check." + ".</p>" +
              "<br />" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "Thank You,</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
              "This email was auto-generated by the CECB-ERP System.</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };


            //using (var smtp = new SmtpClient())
            //{
            //    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //    smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            //    smtp.Send(email);
            //    smtp.Disconnect(true);
            //}



            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public bool CheckedInvoice(Guid InvoiceId, SubProjectContext _context)
        {
            var result = true;
            var InId = new SubProjectRepository(_context).GetDLInvoice(InvoiceId);
            InId.DataStatus = 2;
            InId.CheckedDate = DateTime.Now;
            result = new SubProjectRepository(_context).UpdateSubProjectSubProjectInvoice(InId);
            if (result)
            {
                var subProjectCode = InId.SubProjectCode;
                var invoiceNumber = InId.InvoiceNo;
               // var checkedEmail = new SubProjectRepository(_context).GetEmployeeEmail(InId.ApprovedEmployeeBy);
                var checkedEmail = "anganarathnayake10@gmail.com";
                if (!String.IsNullOrEmpty(checkedEmail))
                {
                    var mailResult = SendCheckedEmail(checkedEmail, subProjectCode, invoiceNumber);

                }
            }
            return result;
        }

        //Send email when Approved Invoice
        public async Task ApprovedInvoiceEmail(string toEMail, string spCode, string InNo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEMail));
            email.Subject = "Approved Subproject Invoice";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
              "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
              "<title>Untitled Document</title>" +
              "</head><body>" +

              "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
              "Dear Sir/Madam,</strong></p>" +

              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "Fllowing Subproject Invoice was Approved.</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Code     :-" + spCode + "</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Invoice No     :-" + InNo + "</p>" +

               "<b>" + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
               "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a> to check." + ".</p>" +
              "<br />" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "Thank You,</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
              "This email was auto-generated by the CECB-ERP System.</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        //public bool approvedInvoice(Guid InvoiceId, SubProjectPreviousInvoiceDeatails SPPreviousInvoiceDeatails, SubProjectContext _context, Guid EmployeeId, string SubProjectCode, int InvoiceStage, AuthenticationContext _authcontext)
        public bool approvedInvoice(Guid InvoiceId, SubProjectContext _context, Guid EmployeeId, string SubProjectCode, int InvoiceStage, List<SubProjectPreviousInvoiceDeatails> SPPreviousInvoiceDeatails, AuthenticationContext _authcontext)

        {
            var result = true;
            var projectInvoiceCode = String.Empty;
            var InId = new SubProjectRepository(_context).GetDLInvoice(InvoiceId);
            InId.DataStatus = 3;
            InId.ApprovedDate = DateTime.Now;
            var SubCode = InId.SubProjectCode;
            int endIndex = SubCode.IndexOf('/', SubCode.IndexOf('/') + 1);
            string SPcode = SubCode.Substring(0, endIndex); // retrieves "C802-65/D"
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            string abbreviatedMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth);
            int InvStage = InId.InvoiceStage;
            var InStageString = "";
            if (InvStage == 1)
            {
                InStageString = "A";
            }
            else if (InvStage == 2)
            {
                InStageString = "B";
            }
            else if (InvStage == 3)
            {
                InStageString = "C";
            }

            var prifix = SPcode + "/" + InStageString + "/" + currentYear + "/" + abbreviatedMonthName;

            //int lastInNo = 0;
            InId.ProjectInvoiceNo = SetProjectInvoiceCode(InvoiceId, SubProjectCode, InvoiceStage, out int lastInNo, _context);
            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {
                    result = new SubProjectRepository(_context).UpdateSubProjectSubProjectInvoice(InId);
                    if (result)
                    {
                        if (lastInNo == 1)
                        {
                            var spInvoice = new SubProjectInvoiceCounter();
                            spInvoice.SubProjectInvoiceCodeCounterId = Guid.NewGuid();
                            spInvoice.Prefix = prifix;
                            spInvoice.NextNo = 1;
                            new SubProjectRepository(_context).AddProjectInvoice(spInvoice);
                        }
                        else
                        {

                            var ProjInvoiceCounter = new SubProjectRepository(_context).GetProjectInvoiceCounter(prifix);
                            ProjInvoiceCounter.NextNo = lastInNo;
                            new SubProjectRepository(_context).UpdateProjInvoiceCount(ProjInvoiceCounter);

                        }

                        List<SubProjectPreviousInvoiceDeatails> subPreviousInDetails = SPPreviousInvoiceDeatails;

                        for (int i = 0; i < subPreviousInDetails.Count; i++)
                        {
                            //var subPreInDetails = new SubProjectPreviousInvoiceDeatails();
                            subPreviousInDetails[i].InvoiceId = InvoiceId;
                            subPreviousInDetails[i].PreviousInvoiceDeatailsId = Guid.NewGuid();
                            subPreviousInDetails[i].InvoiceAddedDate = DateTime.Now;
                            subPreviousInDetails[i].DataStatus = InId.DataStatus;
                            subPreviousInDetails[i].SubProjectCode = SubProjectCode;

                            // subPreviousInDetails.Add(subPreInDetails);
                        }
                        new SubProjectRepository(_context).AddSubProjectPreviousInvoiceDeatails(subPreviousInDetails);
                    }


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }

                _context.SaveChanges();
                ts.Commit();
            }

            if (result)
            {
                var subProjectCode = InId.SubProjectCode;
                var invoiceNumber = InId.InvoiceNo;
                //var checkedEmail = new SubProjectRepository(_context).GetEmployeeEmail(InId.PreparedBy);
                var checkedEmail = "anganarathnayake10@gmail.com";
                if (!String.IsNullOrEmpty(checkedEmail))
                {
                    var mailResult = ApprovedInvoiceEmail(checkedEmail, subProjectCode, invoiceNumber);

                }
            }
            return result;
        }

        public bool noApprovedInvoice(Guid InvoiceId, SubProjectContext _context, Guid EmployeeId, string SubProjectCode, int InvoiceStage, AuthenticationContext _authcontext)
        {
            var result = true;
            var projectInvoiceCode = String.Empty;
            var InId = new SubProjectRepository(_context).GetDLInvoice(InvoiceId);
            InId.DataStatus = 3;
            InId.ApprovedDate = DateTime.Now;
            var SubCode = InId.SubProjectCode;
            int endIndex = SubCode.IndexOf('/', SubCode.IndexOf('/') + 1);
            string SPcode = SubCode.Substring(0, endIndex); // retrieves "C802-65/D"
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            string abbreviatedMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth);
            int InvStage = InId.InvoiceStage;
            var InStageString = "";
            if (InvStage == 1)
            {
                InStageString = "A";
            }
            else if (InvStage == 2)
            {
                InStageString = "B";
            }
            else if (InvStage == 3)
            {
                InStageString = "C";
            }

            var prifix = SPcode + "/" + InStageString + "/" + currentYear + "/" + abbreviatedMonthName;

            //int lastInNo = 0;
            InId.ProjectInvoiceNo = SetProjectInvoiceCode(InvoiceId, SubProjectCode, InvoiceStage, out int lastInNo, _context);
            using (var ts = _context.Database.BeginTransaction())
            {
                try
                {
                    result = new SubProjectRepository(_context).UpdateSubProjectSubProjectInvoice(InId);
                    if (result)
                    {
                        if (lastInNo == 1)
                        {
                            var spInvoice = new SubProjectInvoiceCounter();
                            spInvoice.SubProjectInvoiceCodeCounterId = Guid.NewGuid();
                            spInvoice.Prefix = prifix;
                            spInvoice.NextNo = 1;
                            new SubProjectRepository(_context).AddProjectInvoice(spInvoice);
                        }
                        else
                        {

                            var ProjInvoiceCounter = new SubProjectRepository(_context).GetProjectInvoiceCounter(prifix);
                            ProjInvoiceCounter.NextNo = lastInNo;
                            new SubProjectRepository(_context).UpdateProjInvoiceCount(ProjInvoiceCounter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    result = false;
                }

                _context.SaveChanges();
                ts.Commit();
            }

            if (result)
            {
                var subProjectCode = InId.SubProjectCode;
                var invoiceNumber = InId.InvoiceNo;
                //var checkedEmail = new SubProjectRepository(_context).GetEmployeeEmail(InId.PreparedBy);
                var checkedEmail = "anganarathnayake10@gmail.com";
                if (!String.IsNullOrEmpty(checkedEmail))
                {
                    var mailResult = ApprovedInvoiceEmail(checkedEmail, subProjectCode, invoiceNumber);

                }
            }
            return result;
        }






        private string SetProjectInvoiceCode(Guid InvoiceId, string SubProjectCode, int InvoiceStage, out int lastJobNo, SubProjectContext _context)
        {
            var projectInvoiceCode = String.Empty;
            var InId = new SubProjectRepository(_context).GetDLInvoice(InvoiceId);
            var SubCode = InId.SubProjectCode;
            int endIndex = SubCode.IndexOf('/', SubCode.IndexOf('/') + 1);
            string SPcode = SubCode.Substring(0, endIndex); // retrieves "C802-65/D"
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            string abbreviatedMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(currentMonth);
            int InvStage = InId.InvoiceStage;
            var InStageString = "";
            if (InvStage == 1)
            {
                InStageString = "A";
            }
            else if (InvStage == 2)
            {
                InStageString = "B";
            }
            else if (InvStage == 3)
            {
                InStageString = "C";
            }

            var prifix = SPcode + "/" + InStageString + "/" + currentYear + "/" + abbreviatedMonthName;

            var projectInvoiceCount = new SubProjectRepository(_context).GetProjectInvoiceCounter(prifix);

            if (projectInvoiceCount != null)
            {
                var nextNo = projectInvoiceCount.NextNo + 1;
                projectInvoiceCode = prifix + "/" + nextNo.ToString();
                lastJobNo = nextNo;

            }
            else
            {
                projectInvoiceCode = prifix + "/1";
                lastJobNo = 1;

            }

            return projectInvoiceCode;
        }


        private string SPInvoiceNo( out int lastJobNo, SubProjectContext _context)
        {
            var invoiceNo = String.Empty;
            var currentYear = DateTime.Now.Year;
            var hospitalWorks = "HW";

            var prifix = hospitalWorks +"/" + currentYear;

            var projectInvoiceNoCount = new SubProjectRepository(_context).GetProjectInvoiceNoCounter(prifix);

            if (projectInvoiceNoCount != null)
            {
                var nextNo = projectInvoiceNoCount.NextNo + 1;
                invoiceNo = prifix + "/" + nextNo.ToString();
                lastJobNo = nextNo;

            }
            else
            {
                invoiceNo = prifix + "/1";
                lastJobNo = 1;

            }

            return invoiceNo;
        }


        //Send email when Rejected Invoice
        public async Task RejectedInvoiceEmail(string toEMail, string spCode, string InNo)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("notificationserp@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEMail));
            email.Subject = "Rejected Subproject Invoice";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<html xmlns=http://www.w3.org/1999/xhtml><head>" +
              "<meta http-equiv=Content-Type content=text/html; charset=utf-8 />" +
              "<title>Untitled Document</title>" +
              "</head><body>" +

              "<p align=left><strong style=font-family:Arial, Helvetica, sans-serif; font-weight:bold; font-size:20px;>" +
              "Dear Sir/Madam,</strong></p>" +

              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "You have following Subproject Invoice was Rejected.</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Subproject Code     :-" + spCode + "</p>" +

               "<b>"
                  + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
                  "Invoice No     :-" + InNo + "</p>" +

               "<b>" + "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
               "Please log into<a href=https://www.cecberp.lk:3075/> https://www.cecberp.lk:3075/ </a> to check." + ".</p>" +
              "<br />" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "Thank You,</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;>" +
              "</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:12px;>" +
              "This email was auto-generated by the CECB-ERP System.</p>" +
              "<p align=left style=font-family: Georgia, 'Times New Roman', Times, serif; font-size:14px;> (Please do not reply for this email message)</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("notificationserp@gmail.com", "eiifrcmchjvhgdvb");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public bool rejectedInvoice(Guid InvoiceId, SubProjectContext _context)
        {
            var result = true;
            var InId = new SubProjectRepository(_context).GetDLInvoice(InvoiceId);
            InId.DataStatus = 0;
            InId.RejectedDate = DateTime.Now;
            result = new SubProjectRepository(_context).UpdateSubProjectSubProjectInvoice(InId);
            if (result)
            {
                var subProjectCode = InId.SubProjectCode;
                var invoiceNumber = InId.InvoiceNo;
                //var checkedEmail = new SubProjectRepository(_context).GetEmployeeEmail(InId.PreparedBy);
                var checkedEmail = "anganarathnayake10@gmail.com";
                if (!String.IsNullOrEmpty(checkedEmail))
                {
                    var mailResult = ApprovedInvoiceEmail(checkedEmail, subProjectCode, invoiceNumber);

                }
            }
            return result;
        }

        public TaxDetail getTaxRate(SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).getTaxRate();


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubprojectExternalInvoiceDTO> SubprojectExternalInvoice(Guid InvoiceId, int rType, SubProjectContext _context)
        {
            try
            {
                var wireport = new SubProjectRepository(_context).SubprojectExternalInvoice(InvoiceId);

                foreach (var wi in wireport)
                {
                    wi.InvoiceId = wi.InvoiceId;
                    //qr.CurrentDate = qr.CurrentDate.Value.Date;
                    //wi.CurrentDateString = wi.CurrentDate.Value.Year + " /" + wi.CurrentDate.Value.Month + " /" + wi.CurrentDate.Value.Day;

                }

                return wireport;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectInvoiceDetail> SubprojectExternalInvoiceDetails(Guid InvoiceId, int rType, SubProjectContext _context)
        {
            try
            {
                var invoiceDetails = new SubProjectRepository(_context).GetInvoiceDetailsIds(InvoiceId);

                foreach (var ind in invoiceDetails)
                {
                    ind.InvoiceId = ind.InvoiceId;
                    //qr.CurrentDate = qr.CurrentDate.Value.Date;
                    //wi.CurrentDateString = wi.CurrentDate.Value.Year + " /" + wi.CurrentDate.Value.Month + " /" + wi.CurrentDate.Value.Day;

                }

                return invoiceDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectPreviousInvoiceDeatails> SubProjectPreviousInvoiceDeatails(Guid InvoiceId, int rType, SubProjectContext _context)
        {
            try
            {
                var previousinvoiceDetails = new SubProjectRepository(_context).GetPreviousInvoice(InvoiceId);

                foreach (var pind in previousinvoiceDetails)
                {
                    pind.InvoiceId = pind.InvoiceId;
                    //qr.CurrentDate = qr.CurrentDate.Value.Date;
                    //wi.CurrentDateString = wi.CurrentDate.Value.Year + " /" + wi.CurrentDate.Value.Month + " /" + wi.CurrentDate.Value.Day;

                }

                return previousinvoiceDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SubProjectResivedPayments>GetResivedPayments(Guid InvoiceId, int rType, SubProjectContext _context)
        {
            try
            {
                var resivedPayments = new SubProjectRepository(_context).GetResivedPayments(InvoiceId);

                foreach (var rind in resivedPayments)
                {
                    rind.InvoiceId = rind.InvoiceId;
                    //qr.CurrentDate = qr.CurrentDate.Value.Date;
                    //wi.CurrentDateString = wi.CurrentDate.Value.Year + " /" + wi.CurrentDate.Value.Month + " /" + wi.CurrentDate.Value.Day;

                }

                return resivedPayments;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        //public bool SavePreviousInvoiceDeatails(Guid InvoiceId,SubProjectPreviousInvoiceDeatails SPPreviousInvoiceDeatails, SubProjectContext _context)
        //{
        //    var result = true;


        //    //saveInvoiceMsg = String.Empty;

        //    using (var ts = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {

        //            if (result)
        //            {
        //                SPPreviousInvoiceDeatails.InvoiceId = InvoiceId;
        //                SPPreviousInvoiceDeatails.PreviousInvoiceDeatailsId = Guid.NewGuid();
        //                SPPreviousInvoiceDeatails.InvoiceAddedDate = DateTime.Now;

        //                result = new SubProjectRepository(_context).AddSubProjectPreviousInvoiceDeatails(SPPreviousInvoiceDeatails);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ts.Rollback();
        //            result = false;
        //        }
        //        if (result)
        //        {
        //            _context.SaveChanges();
        //            ts.Commit();
        //        }

        //    }
        //    return result;
        //}


        public List<SubProjectInvoice> GetPreviousInvoiceBySubProjectCode(string SubProjectCode, Guid InvoiceId, SubProjectContext _context)
        {
            try
            {
                return new SubProjectRepository(_context).GetPreviousInvoiceBySubProjectCode(SubProjectCode, InvoiceId);


            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}



