using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections;

using System.IO;
using System.Net;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.Reporting.WebForms;




namespace HUSP_API
{
    public class ReportManager
    {
        public void GenerateReport(ArrayList reportParam, string reportParth, string fileName)
        {
            string SSRSReportOutputParth = "";
            string SSRSReportServerURL = "";
            string SSRSServerUsername = "";
            string SSRSServerPassword = "";
            string SSRSServerDomain = "";
            string SSRSDataSourceCredentialsName = "";
            string SSRSDataSourceCredentialsUser = "";
            string SSRSDataSourceCredentialsPassword = "";
            string SSRSConnectionString = "";

            try
            {

                SSRSReportOutputParth = "C:/Temp/OutputFile";
                SSRSConnectionString = "Data Source=172.16.255.234;Initial Catalog=CECB_ERP";
                SSRSReportServerURL = "http://172.16.255.234/ReportServer/";
                SSRSServerUsername = "006304";
                SSRSServerPassword = "wisdom4";
                SSRSServerDomain = "cecb";
                SSRSDataSourceCredentialsName = "cecbhrm_DataSource";
                SSRSDataSourceCredentialsUser = "erp";
                SSRSDataSourceCredentialsPassword = "ceslerp";


                //SSRSReportOutputParth = "C:/Temp/OutputFile";
                //SSRSReportServerURL = "http://172.16.255.234/ReportServer/";
                //SSRSServerUsername = "006304";
                //SSRSServerPassword = "wisdom4";
                //SSRSServerDomain = "cecb";
                //SSRSDataSourceCredentialsName = "cecbhrm_DataSource";
                //SSRSDataSourceCredentialsUser = "erp";
                //SSRSDataSourceCredentialsPassword = "ceslerp";
                //SSRSConnectionString = "Data Source=172.16.255.234;Initial Catalog=CECB_ERP";

            }
            catch (Exception ex)
            {
                throw;
            }

            string outputPath = SSRSReportOutputParth + fileName + ".pdf";
            //string outputPath = SSRSReportOutputParth + ".pdf";
            try
            {
                //                string dirPath = SSRSReportOutputParth.Substring(0, SSRSReportOutputParth.LastIndexOf("\\"));
                //                DeleteTempFiles(dirPath);

                reportParam.Add(CreateReportParameter("pConnectionString", SSRSConnectionString));

                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ServerReport.ReportPath = reportParth;
                reportViewer.ServerReport.ReportServerUrl = new Uri(SSRSReportServerURL);
                reportViewer.ProcessingMode = ProcessingMode.Remote;
                IReportServerCredentials irsc = new CustomReportCredentials(SSRSServerUsername, SSRSServerPassword, SSRSServerDomain);
                reportViewer.ServerReport.ReportServerCredentials = irsc;

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                reportViewer.ServerReport.SetParameters(param);
                List<DataSourceCredentials> asd = new List<DataSourceCredentials>();
                DataSourceCredentials dsc = new DataSourceCredentials();
                dsc.Name = SSRSDataSourceCredentialsName;
                dsc.Password = SSRSDataSourceCredentialsPassword;
                dsc.UserId = SSRSDataSourceCredentialsUser;
                asd.Add(dsc);

                reportViewer.ServerReport.SetDataSourceCredentials(asd);
                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Warning[] warnings;
                byte[] pdfBytes = reportViewer.ServerReport.Render("PDF", string.Empty, out mimeType,
                    out encoding, out extension, out streams, out warnings);

                // save the file
                using (FileStream fs = new FileStream(outputPath, FileMode.Create))
                {
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                    fs.Close();
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GenerateExcelReport(ArrayList reportParam, string reportParth)
        {
            string SSRSReportOutputParth = "";
            string SSRSReportServerURL = "";
            string SSRSServerUsername = "";
            string SSRSServerPassword = "";
            string SSRSServerDomain = "";
            string SSRSDataSourceCredentialsName = "";
            string SSRSDataSourceCredentialsUser = "";
            string SSRSDataSourceCredentialsPassword = "";
            string SSRSConnectionString = "";

            try
            {

                SSRSReportOutputParth = "C:/Temp/OutputFile";
                SSRSConnectionString = "Data Source=172.16.255.234;Initial Catalog=CECB_ERP";
                SSRSReportServerURL = "http://172.16.255.234/ReportServer/";
                SSRSServerUsername = "006304";
                SSRSServerPassword = "wisdom4";
                SSRSServerDomain = "cecb";
                SSRSDataSourceCredentialsName = "cecbhrm_DataSource";
                SSRSDataSourceCredentialsUser = "erp";
                SSRSDataSourceCredentialsPassword = "ceslerp";

                //SSRSReportOutputParth = "C:/Temp/OutputFile";
                //SSRSReportServerURL = "http://172.16.255.234/ReportServer/";
                //SSRSServerUsername = "006304";
                //SSRSServerPassword = "wisdom4";
                //SSRSServerDomain = "cecb";
                //SSRSDataSourceCredentialsName = "cecbhrm_DataSource";
                //SSRSDataSourceCredentialsUser = "erp";
                //SSRSDataSourceCredentialsPassword = "ceslerp";
                //SSRSConnectionString = "Data Source=172.16.255.234;Initial Catalog=CECB_ERP";

            }
            catch (Exception ex)
            {
                throw;
            }

            //string outputPath = SSRSReportOutputParth + ".xlsx";
            string outputPath = SSRSReportOutputParth + ".xls";
            try
            {
                reportParam.Add(CreateReportParameter("pConnectionString", SSRSConnectionString));

                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ServerReport.ReportPath = reportParth;
                reportViewer.ServerReport.ReportServerUrl = new Uri(SSRSReportServerURL);
                reportViewer.ProcessingMode = ProcessingMode.Remote;
                IReportServerCredentials irsc = new CustomReportCredentials(SSRSServerUsername, SSRSServerPassword, SSRSServerDomain);
                reportViewer.ServerReport.ReportServerCredentials = irsc;

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                reportViewer.ServerReport.SetParameters(param);
                List<DataSourceCredentials> asd = new List<DataSourceCredentials>();
                DataSourceCredentials dsc = new DataSourceCredentials();
                dsc.Name = SSRSDataSourceCredentialsName;
                dsc.Password = SSRSDataSourceCredentialsPassword;
                dsc.UserId = SSRSDataSourceCredentialsUser;
                asd.Add(dsc);
                reportViewer.ServerReport.SetDataSourceCredentials(asd);
                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Warning[] warnings;
                byte[] pdfBytes = reportViewer.ServerReport.Render("Excel", string.Empty, out mimeType,
                    out encoding, out extension, out streams, out warnings);

                // save the file
                using (FileStream fs = new FileStream(outputPath, FileMode.Create))
                {
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                    fs.Close();
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void GenerateWordReport(ArrayList reportParam, string reportParth)
        {
            string SSRSReportOutputParth = "";
            string SSRSReportServerURL = "";
            string SSRSServerUsername = "";
            string SSRSServerPassword = "";
            string SSRSServerDomain = "";
            string SSRSDataSourceCredentialsName = "";
            string SSRSDataSourceCredentialsUser = "";
            string SSRSDataSourceCredentialsPassword = "";
            string SSRSConnectionString = "";

            try
            {


                SSRSReportOutputParth = "C:/Temp/OutputFile";
                SSRSConnectionString = "Data Source=172.16.255.234;Initial Catalog=CECB_ERP";
                SSRSReportServerURL = "http://172.16.255.234/ReportServer/";
                SSRSServerUsername = "006304";
                SSRSServerPassword = "wisdom4";
                SSRSServerDomain = "cecb";
                SSRSDataSourceCredentialsName = "cecbhrm_DataSource";
                SSRSDataSourceCredentialsUser = "erp";
                SSRSDataSourceCredentialsPassword = "ceslerp";
          

            }
            catch (Exception ex)
            {
                throw;
            }

            string outputPath = SSRSReportOutputParth + ".docx";
            try
            {
                reportParam.Add(CreateReportParameter("pConnectionString", SSRSConnectionString));

                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ServerReport.ReportPath = reportParth;
                reportViewer.ServerReport.ReportServerUrl = new Uri(SSRSReportServerURL);
                reportViewer.ProcessingMode = ProcessingMode.Remote;
                IReportServerCredentials irsc = new CustomReportCredentials(SSRSServerUsername, SSRSServerPassword, SSRSServerDomain);
                reportViewer.ServerReport.ReportServerCredentials = irsc;

                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                reportViewer.ServerReport.SetParameters(param);
                List<DataSourceCredentials> asd = new List<DataSourceCredentials>();
                DataSourceCredentials dsc = new DataSourceCredentials();
                dsc.Name = SSRSDataSourceCredentialsName;
                dsc.Password = SSRSDataSourceCredentialsPassword;
                dsc.UserId = SSRSDataSourceCredentialsUser;
                asd.Add(dsc);
                reportViewer.ServerReport.SetDataSourceCredentials(asd);
                string mimeType;
                string encoding;
                string extension;
                string[] streams;
                Warning[] warnings;
                byte[] pdfBytes = reportViewer.ServerReport.Render("WORD", string.Empty, out mimeType,
                    out encoding, out extension, out streams, out warnings);

                // save the file
                using (FileStream fs = new FileStream(outputPath, FileMode.Create))
                {
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                    fs.Close();
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }



        private void DeleteTempFiles(string dirPath)
        {
            foreach (string file in Directory.GetFiles(dirPath))
            {
                FileInfo fi = new FileInfo(file);
                //if (fi.CreationTime < DateTime.Now.AddHours(-1))
                if (fi.CreationTime < DateTime.Now.AddMinutes(-15))
                {
                    fi.Delete();
                }
            }
        }
    }
    public class CustomReportCredentials : IReportServerCredentials
    {
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
        {
            _UserName = UserName;
            _PassWord = PassWord;
            _DomainName = DomainName;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user,
         out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}

//{
//    public class ReportManager
//    {
//        public void GenerateReport(ArrayList reportParam, string reportParth, string fileName)
//        {
//            string SSRSReportOutputParth = "";
//            string SSRSReportServerURL = "";
//            string SSRSServerUsername = "";
//            string SSRSServerPassword = "";
//            string SSRSServerDomain = "";
//            string SSRSDataSourceCredentialsName = "";
//            string SSRSDataSourceCredentialsUser = "";
//            string SSRSDataSourceCredentialsPassword = "";
//            string SSRSConnectionString = "";

//            try
//            {
//                SSRSReportOutputParth = ConfigurationManager.AppSettings["SSRSReportOutputParth"];
//                SSRSReportServerURL = ConfigurationManager.AppSettings["SSRSReportServerURL"];
//                SSRSServerUsername = ConfigurationManager.AppSettings["SSRSServerUsername"];
//                SSRSServerPassword = ConfigurationManager.AppSettings["SSRSServerPassword"];
//                SSRSServerDomain = ConfigurationManager.AppSettings["SSRSServerDomain"];
//                SSRSDataSourceCredentialsName = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsName"];
//                SSRSDataSourceCredentialsUser = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsUser"];
//                SSRSDataSourceCredentialsPassword = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsPassword"];
//                SSRSConnectionString = ConfigurationManager.AppSettings["SSRSConnectionString"];

//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//            string outputPath = SSRSReportOutputParth + fileName + ".pdf";
//            //string outputPath = SSRSReportOutputParth + ".pdf";
//            try
//            {
//                //                string dirPath = SSRSReportOutputParth.Substring(0, SSRSReportOutputParth.LastIndexOf("\\"));
//                //                DeleteTempFiles(dirPath);

//                reportParam.Add(CreateReportParameter("pConnectionString", SSRSConnectionString));

//                ReportViewer reportViewer = new ReportViewer();
//                reportViewer.ServerReport.ReportPath = reportParth;
//                reportViewer.ServerReport.ReportServerUrl = new Uri(SSRSReportServerURL);
//                reportViewer.ProcessingMode = ProcessingMode.Remote;
//                IReportServerCredentials irsc = new CustomReportCredentials(SSRSServerUsername, SSRSServerPassword, SSRSServerDomain);
//                reportViewer.ServerReport.ReportServerCredentials = irsc;

//                ReportParameter[] param = new ReportParameter[reportParam.Count];
//                for (int k = 0; k < reportParam.Count; k++)
//                {
//                    param[k] = (ReportParameter)reportParam[k];
//                }
//                reportViewer.ServerReport.SetParameters(param);
//                List<DataSourceCredentials> asd = new List<DataSourceCredentials>();
//                DataSourceCredentials dsc = new DataSourceCredentials();
//                dsc.Name = SSRSDataSourceCredentialsName;
//                dsc.Password = SSRSDataSourceCredentialsPassword;
//                dsc.UserId = SSRSDataSourceCredentialsUser;
//                asd.Add(dsc);

//                reportViewer.ServerReport.SetDataSourceCredentials(asd);
//                string mimeType;
//                string encoding;
//                string extension;
//                string[] streams;
//                Warning[] warnings;
//                byte[] pdfBytes = reportViewer.ServerReport.Render("PDF", string.Empty, out mimeType,
//                    out encoding, out extension, out streams, out warnings);

//                // save the file
//                using (FileStream fs = new FileStream(outputPath, FileMode.Create))
//                {
//                    fs.Write(pdfBytes, 0, pdfBytes.Length);
//                    fs.Close();
//                }


//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public void GenerateExcelReport(ArrayList reportParam, string reportParth)
//        {
//            string SSRSReportOutputParth = "";
//            string SSRSReportServerURL = "";
//            string SSRSServerUsername = "";
//            string SSRSServerPassword = "";
//            string SSRSServerDomain = "";
//            string SSRSDataSourceCredentialsName = "";
//            string SSRSDataSourceCredentialsUser = "";
//            string SSRSDataSourceCredentialsPassword = "";
//            string SSRSConnectionString = "";

//            try
//            {
//                SSRSReportOutputParth = ConfigurationManager.AppSettings["SSRSReportOutputParth"];
//                SSRSReportServerURL = ConfigurationManager.AppSettings["SSRSReportServerURL"];
//                SSRSServerUsername = ConfigurationManager.AppSettings["SSRSServerUsername"];
//                SSRSServerPassword = ConfigurationManager.AppSettings["SSRSServerPassword"];
//                SSRSServerDomain = ConfigurationManager.AppSettings["SSRSServerDomain"];
//                SSRSDataSourceCredentialsName = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsName"];
//                SSRSDataSourceCredentialsUser = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsUser"];
//                SSRSDataSourceCredentialsPassword = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsPassword"];
//                SSRSConnectionString = ConfigurationManager.AppSettings["SSRSConnectionString"];

//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//            //string outputPath = SSRSReportOutputParth + ".xlsx";
//            string outputPath = SSRSReportOutputParth + ".xls";
//            try
//            {
//                reportParam.Add(CreateReportParameter("pConnectionString", SSRSConnectionString));

//                ReportViewer reportViewer = new ReportViewer();
//                reportViewer.ServerReport.ReportPath = reportParth;
//                reportViewer.ServerReport.ReportServerUrl = new Uri(SSRSReportServerURL);
//                reportViewer.ProcessingMode = ProcessingMode.Remote;
//                IReportServerCredentials irsc = new CustomReportCredentials(SSRSServerUsername, SSRSServerPassword, SSRSServerDomain);
//                reportViewer.ServerReport.ReportServerCredentials = irsc;

//                ReportParameter[] param = new ReportParameter[reportParam.Count];
//                for (int k = 0; k < reportParam.Count; k++)
//                {
//                    param[k] = (ReportParameter)reportParam[k];
//                }
//                reportViewer.ServerReport.SetParameters(param);
//                List<DataSourceCredentials> asd = new List<DataSourceCredentials>();
//                DataSourceCredentials dsc = new DataSourceCredentials();
//                dsc.Name = SSRSDataSourceCredentialsName;
//                dsc.Password = SSRSDataSourceCredentialsPassword;
//                dsc.UserId = SSRSDataSourceCredentialsUser;
//                asd.Add(dsc);
//                reportViewer.ServerReport.SetDataSourceCredentials(asd);
//                string mimeType;
//                string encoding;
//                string extension;
//                string[] streams;
//                Warning[] warnings;
//                byte[] pdfBytes = reportViewer.ServerReport.Render("Excel", string.Empty, out mimeType,
//                    out encoding, out extension, out streams, out warnings);

//                // save the file
//                using (FileStream fs = new FileStream(outputPath, FileMode.Create))
//                {
//                    fs.Write(pdfBytes, 0, pdfBytes.Length);
//                    fs.Close();
//                }


//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public void GenerateWordReport(ArrayList reportParam, string reportParth)
//        {
//            string SSRSReportOutputParth = "";
//            string SSRSReportServerURL = "";
//            string SSRSServerUsername = "";
//            string SSRSServerPassword = "";
//            string SSRSServerDomain = "";
//            string SSRSDataSourceCredentialsName = "";
//            string SSRSDataSourceCredentialsUser = "";
//            string SSRSDataSourceCredentialsPassword = "";
//            string SSRSConnectionString = "";

//            try
//            {
//                SSRSReportOutputParth = ConfigurationManager.AppSettings["SSRSReportOutputParth"];
//                SSRSReportServerURL = ConfigurationManager.AppSettings["SSRSReportServerURL"];
//                SSRSServerUsername = ConfigurationManager.AppSettings["SSRSServerUsername"];
//                SSRSServerPassword = ConfigurationManager.AppSettings["SSRSServerPassword"];
//                SSRSServerDomain = ConfigurationManager.AppSettings["SSRSServerDomain"];
//                SSRSDataSourceCredentialsName = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsName"];
//                SSRSDataSourceCredentialsUser = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsUser"];
//                SSRSDataSourceCredentialsPassword = ConfigurationManager.AppSettings["SSRSDataSourceCredentialsPassword"];
//                SSRSConnectionString = ConfigurationManager.AppSettings["SSRSConnectionString"];

//            }
//            catch (Exception ex)
//            {
//                throw;
//            }

//            string outputPath = SSRSReportOutputParth + ".docx";
//            try
//            {
//                reportParam.Add(CreateReportParameter("pConnectionString", SSRSConnectionString));

//                ReportViewer reportViewer = new ReportViewer();
//                reportViewer.ServerReport.ReportPath = reportParth;
//                reportViewer.ServerReport.ReportServerUrl = new Uri(SSRSReportServerURL);
//                reportViewer.ProcessingMode = ProcessingMode.Remote;
//                IReportServerCredentials irsc = new CustomReportCredentials(SSRSServerUsername, SSRSServerPassword, SSRSServerDomain);
//                reportViewer.ServerReport.ReportServerCredentials = irsc;

//                ReportParameter[] param = new ReportParameter[reportParam.Count];
//                for (int k = 0; k < reportParam.Count; k++)
//                {
//                    param[k] = (ReportParameter)reportParam[k];
//                }
//                reportViewer.ServerReport.SetParameters(param);
//                List<DataSourceCredentials> asd = new List<DataSourceCredentials>();
//                DataSourceCredentials dsc = new DataSourceCredentials();
//                dsc.Name = SSRSDataSourceCredentialsName;
//                dsc.Password = SSRSDataSourceCredentialsPassword;
//                dsc.UserId = SSRSDataSourceCredentialsUser;
//                asd.Add(dsc);
//                reportViewer.ServerReport.SetDataSourceCredentials(asd);
//                string mimeType;
//                string encoding;
//                string extension;
//                string[] streams;
//                Warning[] warnings;
//                byte[] pdfBytes = reportViewer.ServerReport.Render("WORD", string.Empty, out mimeType,
//                    out encoding, out extension, out streams, out warnings);

//                // save the file
//                using (FileStream fs = new FileStream(outputPath, FileMode.Create))
//                {
//                    fs.Write(pdfBytes, 0, pdfBytes.Length);
//                    fs.Close();
//                }


//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }

//        public ReportParameter CreateReportParameter(string paramName, string pramValue)
//        {
//            ReportParameter aParam = new ReportParameter(paramName, pramValue);
//            return aParam;
//        }



//        private void DeleteTempFiles(string dirPath)
//        {
//            foreach (string file in Directory.GetFiles(dirPath))
//            {
//                FileInfo fi = new FileInfo(file);
//                //if (fi.CreationTime < DateTime.Now.AddHours(-1))
//                if (fi.CreationTime < DateTime.Now.AddMinutes(-15))
//                {
//                    fi.Delete();
//                }
//            }
//        }
//    }
//    public class CustomReportCredentials : IReportServerCredentials
//    {
//        private string _UserName;
//        private string _PassWord;
//        private string _DomainName;

//        public CustomReportCredentials(string UserName, string PassWord, string DomainName)
//        {
//            _UserName = UserName;
//            _PassWord = PassWord;
//            _DomainName = DomainName;
//        }

//        public System.Security.Principal.WindowsIdentity ImpersonationUser
//        {
//            get { return null; }
//        }

//        public ICredentials NetworkCredentials
//        {
//            get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
//        }

//        public bool GetFormsCredentials(out Cookie authCookie, out string user,
//         out string password, out string authority)
//        {
//            authCookie = null;
//            user = password = authority = null;
//            return false;
//        }
//    }
//}




//SSRSReportOutputParth = "C:\\Temp\\OutputFile";
//SSRSReportServerURL = "http://IT-SW30/Reports/Pages";
//SSRSServerUsername = "006941";
//SSRSServerPassword = "p@ssw0rd";
//SSRSServerDomain = "IT-SW30";
//SSRSDataSourceCredentialsName = "SubProjectDataSource";
//SSRSDataSourceCredentialsUser = "sa";
//SSRSDataSourceCredentialsPassword = "123456789";
//SSRSConnectionString = "Data Source=IT-SW30;Initial Catalog=CECB_ERP";