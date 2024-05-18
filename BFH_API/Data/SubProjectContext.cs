using CECBERP.CMN.Business.Entities;
using CECBERP.CMN.Business.Entities.CMN;
using CECBERP.CMN.Business.Entities.FIN;
using CECBERP.CMN.Business.Entities.PM;
using CECBERP.CMN.Business.Entities.SP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.Data
{
    public class SubProjectContext:DbContext
    {
   
        public SubProjectContext(DbContextOptions<SubProjectContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<SubProject>SubProjects{ get; set; }
        public DbSet<Project>Projects{ get; set; }
        public DbSet<Client>Clients{ get; set; }
        public DbSet<WorkSpace>WorkSpaces{ get;set; }
        public DbSet<ProjectLocation> ProjectLocations { get; set; }
        public DbSet<SubprojectType>SubProjectTypes{ get; set; }
        public DbSet<SubProjectCodeCounter> SubProjectCodeCounters { get; set; }
        public DbSet<EmployeeVersion> EmployeeVersions { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<SubProjectServiceByOtherUnits>SProjectServiceByOtherUnits { get; set; }
        public DbSet<SubProjectServices>SubProjectsServices { get; set; }
        public DbSet<SubProjectProgress> subProjectProgresses { get; set; }
        public DbSet<SubProjectMRJobs> subProjectMRJobs { get; set; }
        public DbSet<SubProjectMRJobCounter> SubProjectMRJobCounters { get; set; }
        public DbSet<SubProjectInvoiceDetail> SubProjectInvoiceDetails { get; set; }
        public DbSet<SubProjectInvoice> SubProjectInvoices { get; set; }

        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<TaxDetail> TaxDetails { get; set; }
        public DbSet<SubProjectInvoiceCounter> SubProjectInvoiceCount { get; set; }
        public DbSet<SubProjectPreviousInvoiceDeatails> SPPreviousInvoiceDeatails { get; set; }
        public DbSet<SubProjectResivedPayments> SubProjectResivedPayment { get; set; }
        public DbSet<SubProjectInvoiceNoCounter> SubProjectInvoiceNoCounters { get; set; }
        public DbSet<SubProjectResivedPayments> SubProjectResivedPayments { get; internal set; }
    }
}
