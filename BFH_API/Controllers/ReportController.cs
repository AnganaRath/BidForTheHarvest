using AspNetCore.Reporting;
using CECBERP.CMN.Business.Entities.SP;
using HUSP_API.BL;
using HUSP_API.Data;
using HUSP_API.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HUSP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
       // private IReportService _reportService;

        private readonly SubProjectContext _context;
        private readonly ILogger<SubProjectController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(SubProjectContext spContext, ILogger<SubProjectController> logger, IWebHostEnvironment webHostEnvironment)
        { 
            _context = spContext;

            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }


/*-----------------------------------------SubprojectProgressQuarterlyReport------------------------------------------------------------------------*/

        [HttpGet]
        [Route("SubprojectPQReport/{CurrentDate}/{SubProjectCode}/{rType}")]
        public IActionResult SubprojectProgressQuarterlyReport(DateTime CurrentDate, string SubProjectCode, int rType)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;

            var SpDataSets = new SubProjectBL().SubprojectProgressQuarterlyReport(CurrentDate, SubProjectCode, rType, _context);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\SubprojectProgressQuarterlyReport.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("SPQRDataSet", SpDataSets);
            //var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
            //return File(result.MainStream, "application/pdf", "Report.pdf");

            if (rType == 1)
            {
                var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
                return File(result.MainStream, "application/pdf", "SubprojectProgressQuarterlyReport.pdf");
            }
            else
            {
                var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
                return File(result.MainStream, "application/xls", "SubprojectProgressQuarterlyReport.xls");
            }


        }

        /*--------------------------------------SubprojectProgressQuarterlyReport End-------------------------------------------------------------------*/



        /*-----------------------------------------SubprojectWorkVsFinancialAndPhysicalProgressReport----------------------------------------------------*/

        [HttpGet]
        [Route("SubprojectWFPReport/{CurrentDate}/{SubProjectCode}/{rType}")]
        public IActionResult SubprojectWFPReport(DateTime CurrentDate, string SubProjectCode, int rType)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;

            var SpDataSets = new SubProjectBL().SubprojectWorkVsFinancialPhysicalProgressReport(CurrentDate, SubProjectCode, rType, _context);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\SubprojectWorkVsFinancialAndPhysicalProgressReport.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("SWFPPRDataSet", SpDataSets);
            //var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
            //return File(result.MainStream, "application/pdf", "Report.pdf");

            if (rType == 1)
            {
                var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
                return File(result.MainStream, "application/pdf", "SubprojectWorkVsFinancialAndPhysicalProgressReport.pdf");
            }
            else
            {
                var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
                return File(result.MainStream, "application/xls", "SubprojectWorkVsFinancialAndPhysicalProgressReport.xls");
            }


        }

        /*--------------------------------------SubprojectWorkVsFinancialAndPhysicalProgressReport End---------------------------------------------------------*/


        /*--------------------------------------------------SubprojectWorkVsIncomeReport--------------------------------------------------------*/

        [HttpGet]
        [Route("SubprojectWorkVsIncomeReport/{CurrentDate}/{SubProjectCode}/{rType}")]
        public IActionResult SubprojectWIReport(DateTime CurrentDate, string SubProjectCode, int rType)
        {
            var replacement = SubProjectCode.Replace("%2F", "/");
            SubProjectCode = replacement;

            var SpDataSets = new SubProjectBL().SubprojectWorkVsIncomeReport(CurrentDate, SubProjectCode, rType, _context);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\SubprojectWorkVsIncomeReport.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("SWIRDataSet", SpDataSets);
            //var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
            //return File(result.MainStream, "application/pdf", "Report.pdf");

            if (rType == 1)
            {
                var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
                return File(result.MainStream, "application/pdf", "SubprojectWorkVsIncomeReport.pdf");
            }
            else
            {
                var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
                return File(result.MainStream, "application/xls", "SubprojectWorkVsIncomeReport.xls");
            }


        }

        /*--------------------------------------SubprojectWorkVsIncomeReport End---------------------------------------------------------------*/


        /*--------------------------------------------------HWSubprojectExternalInvoices--------------------------------------------------------*/

        [HttpGet]
        [Route("SubprojectExternalInvoiceDTO/{InvoiceId}/{rType}")]
        public IActionResult SubprojectExternalInvoice(Guid InvoiceId, int rType)
        {
          
            var SpExternalInvoiceDataSets = new SubProjectBL().SubprojectExternalInvoice(InvoiceId, rType,_context);
            var SpExternalInvoiceDataSets1 = new SubProjectBL().SubprojectExternalInvoiceDetails(InvoiceId, rType, _context);
            var SpExternalInvoiceDataSets2 = new SubProjectBL().SubProjectPreviousInvoiceDeatails(InvoiceId, rType, _context);
            var SpExternalInvoiceDataSets3 = new SubProjectBL().GetResivedPayments(InvoiceId, rType, _context);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\HWSubprojectExternalInvoices.rdlc";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport lr = new LocalReport(path);
            lr.AddDataSource("DataSet1", SpExternalInvoiceDataSets);
            lr.AddDataSource("DataSet2", SpExternalInvoiceDataSets1);
            lr.AddDataSource("DataSet3", SpExternalInvoiceDataSets2);
            lr.AddDataSource("DataSet4", SpExternalInvoiceDataSets3);
            if (rType == 1)
            {
                var result = lr.Execute(RenderType.Pdf, extension, parameters, mimetype);
                return File(result.MainStream, "application/pdf", "HWSubprojectExternalInvoices.pdf");
            }
            else
            {
                var result = lr.Execute(RenderType.Excel, extension, parameters, mimetype);
                return File(result.MainStream, "application/xls", "HWSubprojectExternalInvoices.xls");
            }

        }

        /*--------------------------------------HWSubprojectExternalInvoices End---------------------------------------------------------------*/



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

    }
}




