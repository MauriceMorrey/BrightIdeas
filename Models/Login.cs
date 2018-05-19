using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BeltExam.Models
{
    public class Login
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name ="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
 
        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage = "Password should be at least 8 characters long.")]        
        public string Password { get; set; }

    }
}