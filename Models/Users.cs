using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BeltExam.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Alias")]
        public string Alias { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
 
        [Required]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        [MinLength(8)]        
        public string Password { get; set; }

        [Required]        
        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation must match.")]
        public string Confirm { get; set; }

        public List<Ideas> Idea { get; set; }
        public List<Likes> Likes { get; set; }        
 
        public Users()
        {
            Idea = new List<Ideas>();
            Likes = new List<Likes>();   
        }

    }
}