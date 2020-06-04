using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ElectroSterk.Web.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassowrd { get; set; }
    }
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}