using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.Models.Authentication
{
    [Table("au_UserTask")]
    public class UserTask
    {
        [Key]
        public Guid UserTaskId { get; set; }
        public string UserTaskName { get; set; }
    }
}
