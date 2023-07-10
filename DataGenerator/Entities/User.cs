using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Zust.Entities.Models
{
    public class User : IdentityUser
    {
        public string ImageUrl { get; set; }

        public string CoverImage { get; set; }

        public string Birthday { get; set; }

        public string Occupation { get; set; }

        public string Birthplace { get; set; }

        public string Gender { get; set; }

        public string RelationshipStatus { get; set; }

        public string BloodGroup { get; set; }

        public string Website { get; set; }

        public string SocialLink { get; set; }

        public string Languages { get; set; }

        public string AboutMe { get; set; }

        public string EducationWork { get; set; }

        public string Interests { get; set; }


        public void DisplayData()
        {
            Console.WriteLine("========= User Data =========");
            Console.WriteLine($"User ID: {base.Id}");
            Console.WriteLine($"Username: {base.UserName}");
            Console.WriteLine($"Email: {base.Email}");
            Console.WriteLine($"Image URL: {ImageUrl}");
            Console.WriteLine($"Cover Image: {CoverImage}");
            Console.WriteLine($"Birthday: {Birthday}");
            Console.WriteLine($"Occupation: {Occupation}");
            Console.WriteLine($"Birthplace: {Birthplace}");
            Console.WriteLine($"Gender: {Gender}");
            Console.WriteLine($"Relationship Status: {RelationshipStatus}");
            Console.WriteLine($"Blood Group: {BloodGroup}");
            Console.WriteLine($"Website: {Website}");
            Console.WriteLine($"Social Link: {SocialLink}");
            Console.WriteLine($"Languages: {Languages}");
            Console.WriteLine($"About Me: {AboutMe}");
            Console.WriteLine($"Education/Work: {EducationWork}");
            Console.WriteLine($"Interests: {Interests}");
        }
    }
}

