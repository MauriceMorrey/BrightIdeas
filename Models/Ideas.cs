using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BeltExam.Models
{
    public class Ideas
    {
        [Key]
        public int IdeaId { get; set; }

        public string Idea {get; set;}
        public int UserId {get; set;}

        public Users Users { get; set; }        
        public List<Likes> Likes { get; set; }
        public Ideas()
        {
            Likes = new List<Likes>();            
        }
    }
}