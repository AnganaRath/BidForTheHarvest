using AspNetCore.Reporting;
using CECBERP.CMN.Business.Entities.SP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HUSP_API.Services
{
    public interface IReportService
    {
        byte[] GenerateReportAsync(string reportName, string reportType);
    }

    public class ReportService : IReportService
    {
        public byte[] GenerateReportAsync(string reportName, string reportType)
        {
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("HUSP_API.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}ReportFiles\\{1}.rdlc", fileDirPath, reportName);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");

            LocalReport report = new LocalReport(rdlcFilePath);

            // prepare data for report
            List<SubProject> SpDataSets = new List<SubProject>();

            var sp1 = new SubProject { SubProjectName = "Project1", SubProjectCode = "CD123", SPConsultancyFee = 6000000};
            var sp2 = new SubProject { SubProjectName = "Project2", SubProjectCode = "CD124", SPConsultancyFee = 7000000 };




            SpDataSets.Add(sp1);
            SpDataSets.Add(sp2);



            report.AddDataSource("SpDataSet", SpDataSets);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var result = report.Execute(GetRenderType(reportType), 1, parameters);
           

            return result.MainStream;
        
        }

    

        private RenderType GetRenderType(string reportType)
        {
            var renderType = RenderType.Pdf;

            switch (reportType.ToUpper())
            {
                default:
                case "PDF":
                    renderType = RenderType.Pdf;
                   
                    break;
                case "XLS":
                    renderType = RenderType.Excel;
                    break;
                case "WORD":
                    renderType = RenderType.Word;
                    break;
            }

            return renderType;
        }

    }
}
