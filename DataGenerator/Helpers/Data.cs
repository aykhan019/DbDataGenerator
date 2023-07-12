using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using DataGenerator.Entities;
using DataGenerator.Helpers;
using Microsoft.AspNet.Identity;
using Zust.Entities.Models;

namespace DataGenerator.Helpers
{
    public class Data
    {
        private static string[] RandomInterests = { "Reading", "Traveling", "Photography", "Cooking", "Sports", "Music", "Gardening", "Painting", "Coding", "Hiking", "Writing", "Dancing", "Yoga", "Gaming", "Watching Movies", "Crafting", "Singing", "Swimming", "Cycling", "Collecting", "Volunteering", "Solving Puzzles", "Fashion", "Soccer", "Basketball", "Learning Languages", "Investing", "Fishing", "Chess", "Golf" };
        private static string[] Genders = { "Male", "Female" };
        private static string[] RelationshipStatuses = { "Single", "In a relationship", "Married" };
        private static string[] BloodGroups = { "A+", "B+", "AB+", "O+", "A-", "B-", "AB-", "O-" };
        private static string[] EmailDomains = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "mail.com", "mail.ru", "Inbox.com" };
        private static string[] RandomLanguages = { "English", "Spanish", "French", "German", "Chinese", "Japanese", "Korean", "Italian", "Russian", "Portuguese", "Arabic", "Hindi", "Bengali", "Dutch", "Swedish", "Polish", "Greek", "Turkish", "Hebrew", "Thai" };
        private static DateTime startDate = new DateTime(1940, 1, 1);
        private static DateTime endDate = new DateTime(2005, 1, 1);

        public static void GenerateUserData()
        {
            const int userCount = 500;
            var resultFilePath = Path.Combine(DataGenerator.Helpers.Constants.FilesFolder, DataGenerator.Helpers.Constants.ResultFile);

            // Show loading message before getting users' data
            Console.WriteLine("Getting users' data...");
            var users = GetUsersData(userCount);
            Console.WriteLine("Users' data retrieved successfully!");

            // Show loading message before generating insert statements
            Console.WriteLine("Generating insert statements...");
            var statements = SqlHelper.GenerateInsertStatements(users);
            Console.WriteLine("Insert statements generated successfully!");

            // Show loading message before writing to the file
            Console.WriteLine("Writing insert statements to file...");
            FileHelper<string>.WriteToFile(resultFilePath, statements);
            Console.WriteLine("Insert statements written to file successfully!");
            Console.WriteLine("File Path " + Path.GetFullPath(resultFilePath));

            // Show completion message
            Console.WriteLine("Process completed!");
        }

        private static List<User> GetUsersData(int count)
        {
            var users = new List<User>();
            var faker = new Faker();
            var usernames = FileHelper<Username>
                            .GetDataFromJson(Path.Combine(DataGenerator.Helpers.Constants.FilesFolder,
                                                          DataGenerator.Helpers.Constants.UsernamesFile))
                            .Take(count)
                            .Select(u => u.Name)
                            .ToList();
            // var emails = FileHelper<Email>.GetData(Path.Combine(Constants.FilesFolder, Constants.EmailsFile)).ToList().Take(count).ToList();
            int numLanguages = faker.Random.Int(1, 4);
            int numInterests = faker.Random.Int(1, 6);
            var covers = FileHelper<string>
                         .GetDataFromTxtFile(Path.Combine(DataGenerator.Helpers.Constants.FilesFolder,
                                                          DataGenerator.Helpers.Constants.CoversFile));
            var passwordHasher = new PasswordHasher();

            for (int i = 0; i < count; i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = usernames[i],
                    Email = $"{(new Faker().Random.Bool() ? $"{usernames[i].ToLower().Replace(" ", string.Empty)}{(faker.Random.Bool() ? faker.Random.Number(100, 999).ToString() : string.Empty)}" : $"{usernames[i].ToLower().Split(' ')[0]}{(faker.Random.Bool() ? faker.Name.LastName().ToLower() : string.Empty)}")}@{faker.Random.ArrayElement(EmailDomains)}",
                    PasswordHash = passwordHasher.HashPassword(usernames[i].Split(' ').ElementAt(0) + "123" + "-"),
                    PhoneNumber = faker.Phone.PhoneNumber(),
                    ImageUrl = faker.Internet.Avatar(),
                    CoverImage = faker.PickRandom(covers),
                    Birthday = faker.Date.Between(startDate, endDate).ToLongDateString(),
                    Occupation = faker.Name.JobTitle(),
                    Birthplace = faker.Address.City(),
                    Gender = faker.PickRandom(Genders),
                    RelationshipStatus = faker.PickRandom(RelationshipStatuses),
                    BloodGroup = faker.PickRandom(BloodGroups),
                    Website = faker.Internet.Url(),
                    SocialLink = faker.Internet.Url(),
                    Languages = string.Join(", ", faker.Random.ArrayElements(RandomLanguages, numLanguages)),
                    AboutMe = faker.Lorem.Paragraph(5),
                    EducationWork = faker.Lorem.Paragraph(),
                    Interests = string.Join(", ", faker.Random.ArrayElements(RandomInterests, numInterests))
                };
                users.Add(user);
            }
            return users;
        }
    }
}
