using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFProject.Models
{
    public class PersonModel
    {
        public string User_id { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public System.DateTime Joined { get; set; }
        public bool Active { get; set; }
        public bool IsFollowing { get; set; }
    }
    public partial class TweetModel
    {
        public int Tweet_id { get; set; }
        public string User_id { get; set; }
        public string UserName { get; set; }

        [Required]
        [StringLength(140, ErrorMessage = "Please enter minimum of 130 and maximum of 140 characters", MinimumLength =130)]
        public string Message { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public bool CanEdit { get; set; }

    }
}