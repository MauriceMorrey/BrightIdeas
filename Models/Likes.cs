using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BeltExam.Models
{
    public class Likes
    {
        [Key]
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
         public int IdeaId { get; set; }
        public Ideas Idea { get; set; }
    }
}