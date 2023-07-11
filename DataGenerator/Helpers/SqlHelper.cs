using System;
using System.Collections.Generic;
using Zust.Entities.Models;

namespace DataGenerator.Helpers
{
    public class SqlHelper
    {
        public static string[] GenerateInsertStatements(List<User> users)
        {
            var statements = new List<string>();
            foreach (var user in users)
            {
                var handledUser = HandleSingleQuotes(user);
                var insertStatement = $"INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash, PhoneNumber, ImageUrl, CoverImage, Birthday, Occupation, Birthplace, Gender, RelationshipStatus, BloodGroup, Website, SocialLink, Languages, AboutMe, EducationWork, Interests, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount) " +
                    $"VALUES ('{Guid.NewGuid()}', '{handledUser.UserName}', '{handledUser.UserName.ToUpper()}', '{handledUser.Email}', '{handledUser.Email.ToUpper()}', '{handledUser.PasswordHash}', '{handledUser.PhoneNumber}', '{handledUser.ImageUrl}', '{handledUser.CoverImage}', '{handledUser.Birthday}', " +
                    $"'{handledUser.Occupation}', '{handledUser.Birthplace}', '{handledUser.Gender}', '{handledUser.RelationshipStatus}', '{handledUser.BloodGroup}', '{handledUser.Website}', '{handledUser.SocialLink}', '{handledUser.Languages}', '{handledUser.AboutMe}', '{handledUser.EducationWork}', '{handledUser.Interests}', " +
                    $"0, 0, 0, 0, 0);";
                statements.Add(insertStatement);
            }
            return statements.ToArray();
        }

        private static User HandleSingleQuotes(User user)
        {
            return new User
            {
                Id = user.Id,
                UserName = user.UserName?.Replace("'", "''"),
                Email = user.Email?.Replace("'", "''"),
                PasswordHash = user.PasswordHash,
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
