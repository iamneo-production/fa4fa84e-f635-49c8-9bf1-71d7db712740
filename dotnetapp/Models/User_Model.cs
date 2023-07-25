using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace dotnetapp.Models
{
    public class User_Model
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
        public string username { get; set; }
        public string mobileNumber { get; set; }
        public string userRole { get; set; }
    }

    
}