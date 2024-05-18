using CECBERP.CMN.Business.Entities;
using CECBERP.CMN.Business.Entities.PM;
using CECBERP.CMN.Business.Entities.SP;
using HUSP_API.BL;
using HUSP_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static CECBERP.CMN.Business.Entities.WorkSpace;
using Microsoft.AspNetCore.Mvc.Rendering;
using HUSP_API.Services;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.Reflection;
using System.Text;
using CECBERP.CMN.Business.Entities.FIN;

namespace HUSP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubProjectController : ControllerBase
    {

        private readonly SubProjectContext _context;
        private readonly AuthenticationContext _authcontext;
        private IReportService _reportService;

        private readonly ILogger<SubProjectController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;



        public SubProjectController(SubProjectContext context, AuthenticationContext authcontext, IReportService reportService, ILogger<SubProjectController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _authcontext = authcontext;
            _reportService = reportService;

            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }


        // GET: api/MainProjectDetails
        [HttpGet]
        [Route("GetProjectById/{ProjectId}")]
        public Project GetMainProjectDetails(Guid ProjectId)
        {
            return new SubProjectBL().GetProject(ProjectId, _context);
        }

        //GET: api/AllMainProjectCode
        [HttpGet]
        [Route("GetProjectByCode/{ProjectCode}")]
        public List<Project> GetAllProjectCode(string ProjectCode)
        {
            return new SubProjectBL().GetProjectByCode(ProjectCode, _context);
        }

        [HttpGet]
        [Route("GetSpProgressById/{ProgressId}")]
        public List<SubProjectProgress> GetSpProgressById(Guid ProgressId)
        {
            return new SubProjectBL().GetSpProgressById(ProgressId, _context);
        }

        [HttpGet]
        [Route("GetMrProjects/{MrJobId}")]
        public List<SubProjectMRJobs> GetMrJobs(Guid MrJobId)
        {
            return new SubProjectBL().GetMrJobs(MrJobId, _context);
        }

        [HttpGet]
        [Route("GetSubProjectProgressHistory/{SubProjectId}")]
        public List<SubProjectProgress> GetSubProjectProgressHistory(Guid SubProjectId)
        {
            return new SubProjectBL().GetSubProjectProgressHistory(SubProjectId, _context);
        }

        //GET: api/approved SubProjectCode
        [HttpGet]
        [Route("GetApprovedSubProject/{SubProjectCode}")]
        public List<SubProject> GetApprovedSubProjectByCode(string SubProjectCode)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;
            return new SubProjectBL().GetApprovedSubProjectByCode(SubProjectCode, _context);
        }

        [HttpGet]
        [Route("GetProjectByLocation/{CityName}")]
        public List<Project> GetProjectByLocation(string CityName)
        {
            return new SubProjectBL().GetProjectByLocation(CityName, _context);
        }

        [HttpGet]
        [Route("GetPMName/{FullName}")]
        public List<EmployeeVersion> GetPMByName(string FullName)
        {
            return new SubProjectBL().GetPMByName(FullName, _context);
        }

        [HttpGet]
        [Route("GetSubProjectDetailsByStatus/{DataStatus}/{UserId}")]

        public List<SubProject> GetSubProjectDetailsByStatus(int DataStatus, Guid UserId)
        {
            var loggedUserRole = new UserLogInBL().LoggedGetUserRoleId(UserId, _authcontext);
            var loggedUserEmployeeId = new UserLogInBL().LoggedGetEmployeeId(UserId, _authcontext);

            return new SubProjectBL().GetSubProjectDetailsByStatus(DataStatus, loggedUserRole, loggedUserEmployeeId, _context);
        }

        [HttpGet]
        [Route("GetSubProjectDetails/{DataStatus}")]
        public IQueryable<Object> GetSubProjectDetails(int DataStatus)
        {
            return new SubProjectBL().GetSubProjectDetails(DataStatus, _context);
        }

        [HttpGet]
        [Route("GetPendingApproval/{UserId}")]
        public List<SubProject> GetPendingApprovalSubProjectDetails(Guid UserId)
        {
            var loggedUserRole = new UserLogInBL().LoggedGetUserRoleId(UserId, _authcontext);
            var loggedUserEmployeeId = new UserLogInBL().LoggedGetEmployeeId(UserId, _authcontext);

            return new SubProjectBL().GetPendingApprovalSubProjectDetails(loggedUserRole, loggedUserEmployeeId, _context);
        }

        [HttpGet]
        [Route("GetWorkSpaces/{ServiceId}")]
        public List<WorkSpaceServiceDTO> GetWorkSpaces(Guid ServiceId)
        {
            return new SubProjectBL().GetWorkSpaces(ServiceId, _context);
        }


        [HttpGet]
        [Route("GetSubProjectDetailsById/{SubProjectId}")]
        public SubProjectDTO GetSubProjectDetailsById(Guid SubProjectId)
        {
            return new SubProjectBL().GetSubProjectDetailsById(SubProjectId, _context);
        }

        /*-----------------------------------------Use for Angular Reports------------------------------------------------------------------*/

        //[HttpGet]
        //[Route("SubprojectProgressQuarterlyReport/{CurrentDate}")]
        //public List<SubprojectProgressQuarterlyReportDTO> SubprojectProgressQuarterlyReport(DateTime CurrentDate)
        //{
        //    return new SubProjectBL().SubprojectProgressQuarterlyReport(CurrentDate, _context);
        //}


        //[HttpGet]
        //[Route("SubprojectWorkVsFinancialPhysicalProgressReport/{CurrentDate}")]
        //public List<SubprojectWorkVsFinancialPhysicalProgressReportDTO> SubprojectWorkVsFinancialPhysicalProgressReport(DateTime CurrentDate)
        //{
        //    return new SubProjectBL().SubprojectWorkVsFinancialPhysicalProgressReport(CurrentDate, _context);
        //}


        //[HttpGet]
        //[Route("SubprojectWorkVsIncomeReport/{CurrentDate}")]
        //public List<SubprojectWorkVsIncomeReportDTO> SubprojectWorkVsIncomeReport(DateTime CurrentDate)
        //{
        //    return new SubProjectBL().SubprojectWorkVsIncomeReport(CurrentDate, _context);
        //}

        /*-----------------------------------------Use for Angular Reports End------------------------------------------------------------------*/


        [HttpGet]
        public List<Project> GetAllProject()
        {
            return new SubProjectBL().GetProjects(_context);
        }

        [HttpGet]
        [Route("GetSubProjectTypes")]
        public List<SubprojectType> GetAllSubProjectTypes()
        {
            return new SubProjectBL().GetSubProjectNames(_context);
        }


        [HttpGet]
        [Route("GetMrSubProjectSById/{SubProjectId}")]
        public List<SubProjectMRJobs> GetSubProjectMRJobs(Guid SubProjectId)
        {
            return new SubProjectBL().GetSubProjectMRJobs(SubProjectId, _context);
        }

        [HttpGet]
        [Route("GetMrJobCode/{SubProjectCode}")]
        public List<SubProject> GetMrJobCode(string SubProjectCode)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;
            return new SubProjectBL().GetMrJobCode(SubProjectCode, _context);
        }

        [HttpGet]
        [Route("SubProjectServices")]
        public List<SubProjectServices> GetAllSubProjectsServices()
        {
            return new SubProjectBL().GetAllSubProjectServices(_context);
        }

        [HttpPost]
        [Route("InsertSubProject")]
        public ActionResult SaveSubProject(SubProject subProj)
        {
            var passResult = new PassResultDTO();
            Guid subProjId = Guid.Empty;
            string saveMsg = String.Empty;
            var result = new SubProjectBL().SaveSubProject(subProj, out subProjId, out saveMsg, _context);
            if (result)
            {
                passResult.SubProjectId = subProjId.ToString();
                passResult.Msg = "Record Inserted Successfully";
                return Ok(passResult);
            }
            else
            {

                if (saveMsg == "MR code for the current year exists")
                {
                    passResult.Msg = "Save Failed.MR code for the current year exists.";
                    return Ok(passResult);
                }
                else
                {
                    passResult.Msg = "Record Inserted Failed.";
                    return BadRequest(passResult);
                }
            }
        }

        [HttpPost]
        [Route("SaveProjectProgress")]
        public ActionResult SaveProjectProgress(SubProjectProgress subprpro)
        {

            var result = new SubProjectBL().SaveProjectProgress(subprpro, _context);
            if (result)
            {

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [Route("SaveMrnewJobCodes")]
        public ActionResult SaveMrnewJobCodes(SubProjectMRJobs sumr)
        {

            var result = new SubProjectBL().SaveMrnewJobCodes(sumr, _context);
            if (result)
            {

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("FinishedSubProject/{SubProjectId}")]
        public ActionResult<bool> FinishSubProject(Guid SubProjectId)
        {
            return new SubProjectBL().FinishSubProject(SubProjectId, _context);
        }

        [HttpPost]
        [Route("ApprovedSubProject/{SubProjectId}")]
        public ActionResult<bool> ApprovedSubProject(Guid SubProjectId, Guid EmployeeId, Guid SubProjectTypeId, Guid ProjectId)

        {
            var passUResult = new PassUpdateResultDTO();
            string upMsg = String.Empty;
            var result = new SubProjectBL().ApprovedSubProject(SubProjectId, SubProjectTypeId, _context, EmployeeId, _authcontext, out upMsg);

            if (result)
            {
                passUResult.Mesg = "Approved Successfully";
                return Ok(passUResult);
            }
            else
            {
                if (upMsg == "MR code for the current year exists")
                {
                    passUResult.Mesg = "Save Failed.MR code for the current year exists.";
                    return Ok(passUResult);
                }
                else
                {
                    passUResult.Mesg = "Record Update Failed.";
                    return BadRequest(passUResult);
                }

            }

        }





        [HttpPost]
        [Route("RejectedSubProject/{RejectedReason}/{SubProjectId}")]
        public ActionResult<bool> RejectedSubProject(Guid SubProjectId, string RejectedReason)
        {
            return new SubProjectBL().RejectedSubProject(SubProjectId, RejectedReason, _context);
        }


        [HttpPut]
        [Route("UpdateSubProjectDetails")]
        public ActionResult UpdateSubProjectDetails(SubProject subProject)
        {
            var passUResult = new PassUpdateResultDTO();
            string upMsg = String.Empty;
            var result = new SubProjectBL().UpdateSubProjectDetails(subProject, _context, out upMsg);


            if (result)
            {
                passUResult.Mesg = "Record Updated Successfully";
                return Ok(passUResult);
            }
            else
            {
                if (upMsg == "MR code for the current year exists")
                {
                    passUResult.Mesg = "Save Failed.MR code for the current year exists.";
                    return Ok(passUResult);
                }
                else
                {
                    passUResult.Mesg = "Record Update Failed.";
                    return BadRequest(passUResult);
                }

            }

        }

        [HttpPut]
        [Route("UpdateApprovedSubProject")]
        public ActionResult UpdateApprovedSubProject(SubProject subProject)
        {
            var result = new SubProjectBL().UpdateApprovedSubProject(subProject, _context);


            if (result)
            {

                return Ok();
            }
            else
            {

                return BadRequest("Update Failed");
            }

        }

        [HttpPost]
        [Route("ApprovedInvoice/{InvoiceId}")]
        public ActionResult<bool> approvedInvoice(Guid InvoiceId, Guid EmployeeId, string SubProjectCode, int InvoiceStage, List<SubProjectPreviousInvoiceDeatails> SPPreviousInvoiceDeatails)
        {
            var passInvoiceUResult = new PassInvoiceUpdateResultDTO();
            string InUpMsg = String.Empty;
            var result = new SubProjectBL().approvedInvoice(InvoiceId, _context, EmployeeId, SubProjectCode, InvoiceStage, SPPreviousInvoiceDeatails, _authcontext);

            if (result)
            {
                passInvoiceUResult.Mesg = "Invoice Approved Successfully";
                return Ok(passInvoiceUResult);
            }
            else
            {

                passInvoiceUResult.Mesg = "Record Update Failed.";
                return BadRequest(passInvoiceUResult);
            }


        }


        [HttpPost]
        [Route("NoApprovedInvoice/{InvoiceId}")]
        public ActionResult<bool> noApprovedInvoice(Guid InvoiceId, Guid EmployeeId, string SubProjectCode, int InvoiceStage)
        {
            var passInvoiceUResult = new PassInvoiceUpdateResultDTO();
            string InUpMsg = String.Empty;
            var result = new SubProjectBL().noApprovedInvoice(InvoiceId, _context, EmployeeId, SubProjectCode, InvoiceStage, _authcontext);

            if (result)
            {
                passInvoiceUResult.Mesg = "Invoice Approved Successfully";
                return Ok(passInvoiceUResult);
            }
            else
            {

                passInvoiceUResult.Mesg = "Record Update Failed.";
                return BadRequest(passInvoiceUResult);
            }


        }




        //[HttpPost]
        //[Route("ApprovedInvoice/{InvoiceId}")]
        //public ActionResult ApprovedInvoice(Guid InvoiceId, SubProjectPreviousInvoiceDeatails SPPreviousInvoiceDeatails)
        //{
        //    var passInvoiceUResult = new PassInvoiceUpdateResultDTO();
        //    //var InvoiceId = SPPreviousInvoiceDeatails.InvoiceId;
        //    var SubProjectCode = SPPreviousInvoiceDeatails.SubProjectCode;
        //    var InvoiceStage = 0;
        //    var EmployeeId = Guid.NewGuid();
        //    string InUpMsg = String.Empty;
        //    var result = new SubProjectBL().approvedInvoice(InvoiceId, SPPreviousInvoiceDeatails, _context, EmployeeId, SubProjectCode, InvoiceStage, _authcontext);

        //    if (result)
        //    {
        //        passInvoiceUResult.Mesg = "Invoice Approved Successfully";
        //        return Ok(passInvoiceUResult);
        //    }
        //    else
        //    {

        //        passInvoiceUResult.Mesg = "Record Update Failed.";
        //        return BadRequest(passInvoiceUResult);
        //    }


        //}


        [HttpPut]
        [Route("UpdateMrSubProject")]
        public ActionResult UpdateMrSubProject(SubProjectMRJobs mrjobs)
        {
            var result = new SubProjectBL().UpdateMrSubProject(mrjobs, _context);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Update Failed");
            }

        }


        private string GetfileName()
        {
            string fileName = Guid.NewGuid().ToString();
            return fileName;
        }


        [HttpGet]
        [Route("GetSubProjectDetailsByCodeToInvoice/{SubProjectCode}")]

        public List<SubProjectDTO> GetSubProjectDetailsByCodeToInvoice(string SubProjectCode)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;
            return new SubProjectBL().GetSubProjectDetailsByCodeToInvoice(SubProjectCode, _context);
        }


        [HttpGet]
        [Route("TestGetSubProjectReport")]
        public IActionResult Print()
        {
            List<SubProject> SpDataSets = new List<SubProject>();

            var sp1 = new SubProject { SubProjectName = "Project1", SubProjectCode = "CD123", SPConsultancyFee = 6000000 };
            var sp2 = new SubProject { SubProjectName = "Project2", SubProjectCode = "CD124", SPConsultancyFee = 7000000 };


            SpDataSets.Add(sp1);
            SpDataSets.Add(sp2);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\SpReport.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("SpDataSet", SpDataSets);
            var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
            return File(result.MainStream, "application/pdf", "Report.pdf");


        }

        [HttpGet]
        [Route("GetCheckedOrApprovedByName/{FullName}")]
        public List<EmployeeVersion> GetCheckedOrApprovedByName(string FullName)
        {
            return new SubProjectBL().GetCheckedOrApprovedByName(FullName, _context);
        }

        //[HttpPost]
        //[Route("InsertSubProjectInvoiceDetails")]
        //public ActionResult InsertSubProjectInvoiceDetails(SubProjectInvoice subProjInvoice)
        //{

        //    var result = new SubProjectBL().SaveSubProjectInvoiceDetails(subProjInvoice, _context);
        //    if (result)
        //    {

        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        [HttpPost]
        [Route("InsertSubProjectInvoiceDetails")]

        public ActionResult InsertSubProjectInvoiceDetails(SubProjectInvoice subProjInvoice)
        {
            var passInvoiceResult = new InvoiceResultDTO();
            Guid InvoiceId = Guid.Empty;
            string saveInvoiceMsg = String.Empty;

            var result = new SubProjectBL().SaveSubProjectInvoiceDetails(subProjInvoice, out InvoiceId, out saveInvoiceMsg,_context);
           
            if (result)
            {
                passInvoiceResult.InvoiceId = InvoiceId.ToString();
                passInvoiceResult.Msg = "Record Inserted Successfully";
                
                return Ok(passInvoiceResult);
            }
            else
            {
                passInvoiceResult.Msg = "Record Inserted Failed.";
                return BadRequest(passInvoiceResult);
            }
        }

        [HttpGet]
        [Route("GetInvoice/{SubProjectCode}/{InvoiceId}")]
        public List<TotalInvoiceTableDataDTO>GetTotalApprovedInvoice(string SubProjectCode, Guid InvoiceId)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;
            return new SubProjectBL().GetTotalApprovedInvoice(SubProjectCode, InvoiceId, _context);
        }

        [HttpGet]
        [Route("GetInitiallyResivedPayments/{SubProjectCode}/{InvoiceId}")]
        public List<SubProjectResivedPayments> GetInitiallyResivedPayments(string SubProjectCode, Guid InvoiceId)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;
            return new SubProjectBL().GetInitiallyResivedPayments(SubProjectCode, InvoiceId, _context);
        }

        //[HttpGet]
        //[Route("GetInvoice/{SubProjectCode}")]
        //public List<SubProjectInvoice> GetTotalApprovedInvoice(string SubProjectCode)
        //{
        //    var replacement = SubProjectCode.Replace("%2F", "/");
        //    SubProjectCode = replacement;
        //    return new SubProjectBL().GetTotalApprovedInvoice(SubProjectCode, _context);
        //}

        [HttpGet]
        [Route("GetSubProjectInvoiceDetailsByStatus/{DataStatus}/{UserId}")]

        public List<SubProjectInvoice> GetSubProjectInvoiceDetailsByStatus(int DataStatus, Guid UserId)
        {
            var loggedUserRole = new UserLogInBL().LoggedGetUserRoleId(UserId, _authcontext);
            var loggedUserEmployeeId = new UserLogInBL().LoggedGetEmployeeId(UserId, _authcontext);

            return new SubProjectBL().GetSubProjectInvoiceDetailsByStatus(DataStatus, loggedUserRole, loggedUserEmployeeId, _context);
        }


        [HttpGet]
        [Route("GetSubProjectInvoiceDetailsById/{InvoiceId}")]
        public List<SubProjectInvoice> GetSubProjectInvoiceDetailsById(Guid InvoiceId)
        {
            return new SubProjectBL().GetSubProjectInvoiceDetailsById(InvoiceId, _context);
        }



        [HttpPut]
        [Route("UpdateInvoiceDetails")]
        public ActionResult UpdateInvoiceDetails(SubProjectInvoice invoiceDetails)
        {
            var invoiceUResult = new InvoiceUpdateResultDTO();
            string iupMsg = String.Empty;
            var result = new SubProjectBL().UpdateInvoiceDetails(invoiceDetails, _context, out iupMsg);


            if (result)
            {
                invoiceUResult.Mesg = "Record Updated Successfully";
                return Ok(invoiceUResult);
            }
            else
            {
                invoiceUResult.Mesg = "Record Update Failed.";
                return BadRequest(invoiceUResult);
            }

        }

        [HttpPost]
        [Route("FinishedInvoice/{InvoiceId}")]
        public ActionResult<bool>FinishedInvoice(Guid InvoiceId)
        {
            return new SubProjectBL().finishdInvoice(InvoiceId, _context);
        }

        [HttpPost]
        [Route("CheckedInvoice/{InvoiceId}")]
        public ActionResult<bool> CheckedInvoice(Guid InvoiceId)
        {
            return new SubProjectBL().CheckedInvoice(InvoiceId, _context);
        }

        //[HttpPost]
        //[Route("ApprovedInvoice/{InvoiceId}")]
        //public ActionResult<bool> approvedInvoice(Guid InvoiceId, SubProjectPreviousInvoiceDeatails SPPreviousInvoiceDeatails, Guid EmployeeId, string SubProjectCode,int InvoiceStage)
        //{
        //    var passInvoiceUResult = new PassInvoiceUpdateResultDTO();
        //    string InUpMsg = String.Empty;
        //    var result = new SubProjectBL().approvedInvoice(InvoiceId, SPPreviousInvoiceDeatails, _context, EmployeeId, SubProjectCode, InvoiceStage, _authcontext);

        //    if (result)
        //    {
        //        passInvoiceUResult.Mesg = "Invoice Approved Successfully";
        //        return Ok(passInvoiceUResult);
        //    }
        //    else
        //    {

        //        passInvoiceUResult.Mesg = "Record Update Failed.";
        //        return BadRequest(passInvoiceUResult);
        //    }

            
        //}

        [HttpPost]
        [Route("RejectedInvoice/{InvoiceId}")]
        public ActionResult<bool> rejectedInvoice(Guid InvoiceId)
        {
            return new SubProjectBL().rejectedInvoice(InvoiceId, _context);
        }

        [HttpGet]
        [Route("GetTaxRate")]
        public TaxDetail getTaxRate()
        {
            return new SubProjectBL().getTaxRate(_context);
        }

        //[HttpPost]
        //[Route("InsertPreviousInvoiceDeatails/{InvoiceId}")]

        //public ActionResult InsertPreviousInvoiceDeatails(Guid InvoiceId,SubProjectPreviousInvoiceDeatails SPPreviousInvoiceDeatails)
        //{
        //    var result = new SubProjectBL().SavePreviousInvoiceDeatails(InvoiceId, SPPreviousInvoiceDeatails, _context);
        //    if (result)
        //    {

        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }

        //}


     [HttpGet]
        [Route("GetPreviousInvoiceBySubProjectCode/{SubProjectCode}/{InvoiceId}")]
        public List<SubProjectInvoice>GetPreviousInvoiceBySubProjectCode(string SubProjectCode, Guid InvoiceId)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;
            return new SubProjectBL().GetPreviousInvoiceBySubProjectCode(SubProjectCode, InvoiceId, _context);
        }

        //[HttpGet]
        //[Route("GetInvoice")]
        //public string SPInvoiceNo()
        //{
        //    return new SubProjectBL().SPInvoiceNo(out int lastJobNo, _context);
        //}



        //[HttpPut]
        //[Route("UpdateInvoiceDetails")]
        //public ActionResult UpdateInvoiceDetails(SubProjectInvoice invoiceDetails)
        //{
        //    var passUResult = new PassUpdateResultDTO();
        //    string upMsg = String.Empty;
        //    var result = new SubProjectBL().UpdateInvoiceDetails(invoiceDetails, _context, out upMsg);


        //    if (result)
        //    {
        //        passUResult.Mesg = "Record Updated Successfully";
        //        return Ok(passUResult);
        //    }
        //    else
        //    {
        //            passUResult.Mesg = "Record Update Failed.";
        //            return BadRequest(passUResult);
        //     }

        //    }

        //}



        //[HttpPost]
        //[Route("InsertSubProjectInvoiceDetails")]
        //public ActionResult InsertSubProjectInvoiceDetails(SubProjectInvoice subProjInvoice)
        //{

        //    var result = new SubProjectBL().SaveSubProjectInvoiceDetails(subProjInvoice, _context);
        //    if (result)
        //    {

        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}

        public class PassResultDTO
        {
            public string Msg { get; set; }
            public string SubProjectId { get; set; }
        }

        public class InvoiceResultDTO
        {
            public string Msg { get; set; }
            public string InvoiceId { get; set; }
        }

        public class PassUpdateResultDTO
        {
            public string Mesg { get; set; }

        }

        public class InvoiceUpdateResultDTO
        {
            public string Mesg { get; set; }

        }

        public class PassInvoiceUpdateResultDTO
        {
            public string Mesg { get; set; }
        }


        

    }
}


        //[HttpGet]
        //[Route("GetSubProjectDetails")]
        //public ActionResult GetSubProjectDetails()
        //{
        //    try
        //    {


        //        try
        //        {
        //            var result = (from sp in _context.SubProjects
        //                          join pro in _context.Projects on sp.ProjectId equals pro.ProjectId
        //                          join spsbou in _context.SProjectServiceByOtherUnits on sp.SubProjectId equals spsbou.SubProjectId
        //                          select new
        //                          {
        //                              SubProjectId = sp.SubProjectId,
        //                              SubProjectTypeId = sp.SubProjectTypeId,
        //                              SubProjectCode = sp.SubProjectCode,
        //                              SubProjectName = sp.SubProjectName,
        //                              SPEstimatedValue = sp.SPEstimatedValue,
        //                              ConsultancyPercentage = sp.ConsultancyPercentage,
        //                              SPContractValue = sp.SPContractValue,
        //                              SPConsultancyFee = sp.SPConsultancyFee,
        //                              FinalBillValue = sp.FinalBillValue,
        //                              DateOfCommencement = sp.DateOfCommencement,
        //                              ShedulledCompletionDate = sp.ShedulledCompletionDate,
        //                              ProjectManagerId = sp.ProjectManagerId,
        //                              EmployeeId = sp.ProjectManagerId,
        //                              ProjectId = pro.ProjectId,
        //                              ProjectName = pro.ProjectName,
        //                              ProjectCode = pro.ProjectCode,
        //                              EstimatedValue = pro.EstimatedValue,
        //                              ContractConsultancyFee = pro.ContractConsultancyFee,
        //                              AGMsectionName = pro.AGMsectionName,
        //                              DGMunitName = pro.DGMunitName,
        //                              ClientName = pro.ClientName,
        //                              ServiceId = spsbou.ServiceId

        //                          }).ToList();
        //            return Ok(result);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    //}










