using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.Models.Authentication
{
    [Table("au_RoleTaskAction")]
    public class RoleTaskAction
    {
        [Key]
        public Guid RoleTaskActionId { get; set; }
        public int RoleId { get; set; }
        public Guid UserTaskId { get; set; }
        public Guid UserActionId { get; set; }


        [ForeignKey("RoleId")]
        public virtual Role Roles { get; set; }
        [ForeignKey("UserTaskId")]
        public virtual UserTask UserTasks { get; set; }
        [ForeignKey("UserActionId")]
        public virtual UserAction UserActions { get; set; }
    }
}
