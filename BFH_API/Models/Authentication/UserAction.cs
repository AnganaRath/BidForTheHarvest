using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HUSP_API.Models.Authentication
{
    [Table("au_UserAction")]
    public class UserAction
    {
        [Key]
        public Guid UserActionId { get; set; }
        public string UserActionName { get; set; }
    }
}
