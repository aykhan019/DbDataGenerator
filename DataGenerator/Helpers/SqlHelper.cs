using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Entities.Models;

namespace DataGenerator.Helpers
{
    public class SqlHelper
    {
        public static string[] GenerateInsertStatements(List<User> users)
        {
            var statements = new List<string>();
            foreach (var u in users)
            {
                var user = HandleSingleQuotes(u);
                var insertStatement = $"INSERT INTO Users (Id, UserName, Email, PasswordHash, PhoneNumber, ImageUrl, CoverImage, Birthday, Occupation, Birthplace, Gender, RelationshipStatus, BloodGroup, Website, SocialLink, Languages, AboutMe, EducationWork, Interests) " +
                    $"VALUES ({user.Id}, '{user.UserName}', '{user.Email}', '{user.PasswordHash}', '{user.PhoneNumber}', '{user.ImageUrl}', '{user.CoverImage}', '{user.Birthday}', '{user.Occupation}', '{user.Birthplace}', '{user.Gender}', '{user.RelationshipStatus}', '{user.BloodGroup}', " +
                    $"'{user.Website}', '{user.SocialLink}', '{user.Languages}', '{user.AboutMe}', '{user.EducationWork}', '{user.Interests}');";
                statements.Add(insertStatement + "\nGO");
            }
            return statements.ToArray();
        }

        private static User HandleSingleQuotes(User user)
        {
            return new User
            {
                Id = "NEWID()",
                UserName = user.UserName?.Replace("'", "''"),
                Email = user.Email?.Replace("'", "''"),
                PasswordHash = user.PasswordHash, // PasswordHash is an exception
                PhoneNumber = user.PhoneNumber?.Replace("'", "''"),
                ImageUrl = user.ImageUrl?.Replace("'", "''"),
                CoverImage = user.CoverImage?.Replace("'", "''"),
                Birthday = user.Birthday?.Replace("'", "''"),
                Occupation = user.Occupation?.Replace("'", "''"),
                Birthplace = user.Birthplace?.Replace("'", "''"),
                Gender = user.Gender?.Replace("'", "''"),
                RelationshipStatus = user.RelationshipStatus?.Replace("'", "''"),
                BloodGroup = user.BloodGroup?.Replace("'", "''"),
                Website = user.Website?.Replace("'", "''"),
                SocialLink = user.SocialLink?.Replace("'", "''"),
                Languages = user.Languages?.Replace("'", "''"),
                AboutMe = user.AboutMe?.Replace("'", "''"),
                EducationWork = user.EducationWork?.Replace("'", "''"),
                Interests = user.Interests?.Replace("'", "''")
            };
        }
    }
}
