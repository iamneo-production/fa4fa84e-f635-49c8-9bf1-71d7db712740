using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace dotnetapp.Models
{
    public class LoginModel
    {
        [Key]
        public int ID { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}