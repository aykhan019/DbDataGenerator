using DataGenerator.Entities;
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

        public static string[] GenerateInsertStatements(List<Post> posts)
        {
            var statements = new List<string>();
            foreach (var post in posts)
            {
                var handledPost = HandleSingleQuotes(post);
                var insertStatement = $"INSERT INTO Posts (Id, Description, HasMediaContent, ContentUrl, IsVideo, CreatedAt, UserId) " +
                    $"VALUES ('{Guid.NewGuid()}', '{handledPost.Description}', '{handledPost.HasMediaContent}', '{handledPost.ContentUrl}', '{handledPost.IsVideo}', '{handledPost.CreatedAt}', '{handledPost.UserId}');";
                statements.Add(insertStatement);
            }
            return statements.ToArray();
        }

        public static string GenerateInsertSqlStatement(Post post)
        {
            var handledPost = HandleSingleQuotes(post);
            var insertStatement = $"INSERT INTO Posts (Id, Description, HasMediaContent, ContentUrl, IsVideo, CreatedAt, UserId) " +
                $"VALUES ('{Guid.NewGuid()}', '{handledPost.Description}', '{handledPost.HasMediaContent}', '{handledPost.ContentUrl}', '{handledPost.IsVideo}', '{handledPost.CreatedAt}', '{handledPost.UserId}');";
            return insertStatement;
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

        private static Post HandleSingleQuotes(Post post)
        {
            return new Post
            {
                Id = post.Id,
                Description = post.Description?.Replace("'", "''"),
                HasMediaContent = post.HasMediaContent,
                ContentUrl = post.ContentUrl?.Replace("'", "''"),
                IsVideo = post.IsVideo,
                CreatedAt = post.CreatedAt,
                UserId = post.UserId,
            };
        }
    }
}
