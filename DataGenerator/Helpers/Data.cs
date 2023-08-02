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
using DataGenerator.Services;
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

        //public static async void GenerateBogusPosts()
        //{
        //    const int postCount = 1000;
        //    var resultFilePath = Path.Combine(DataGenerator.Helpers.Constants.FilesFolder, DataGenerator.Helpers.Constants.ResultFile);

        //    // Show loading message before getting users' data
        //    Console.WriteLine("Generating posts' data...");
        //    var posts = await GetPostData(postCount);
        //    Console.WriteLine("Posts' data generated successfully!");

        //    // Show loading message before generating insert statements
        //    Console.WriteLine("Generating insert statements...");
        //    var statements = SqlHelper.GenerateInsertStatements(posts);
        //    Console.WriteLine("Insert statements generated successfully!");

        //    // Show loading message before writing to the file
        //    Console.WriteLine("Writing insert statements to file...");
        //    FileHelper<string>.WriteToFile(resultFilePath, statements);
        //    Console.WriteLine("Insert statements written to file successfully!");
        //    Console.WriteLine("File Path " + Path.GetFullPath(resultFilePath));

        //    // Show completion message
        //    Console.WriteLine("Process completed!");
        //}

        //private static async Task<List<Post>> GetPostData(int numberOfPosts)
        //{
        //    var faker = new Faker();

        //    var posts = new List<Post>();

        //    var images = await PostDataService.GetPostImagesFromLoremFlickr(numberOfPosts);

        //    var path = Path.Combine(DataGenerator.Helpers.Constants.FilesFolder, DataGenerator.Helpers.Constants.UserIdsFile);
        //    var ids = FileHelper<string>.GetDataFromTxtFile(path);

        //    for (int i = 0; i < numberOfPosts; i++)
        //    {
        //        var post = new Post
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Description = faker.Lorem.Sentences(),
        //            HasMediaContent = true,
        //            ContentUrl = images[i],
        //            IsVideo = false,
        //            CreatedAt = faker.Date.Past(),
        //            UserId = faker.PickRandom(ids)
        //        };

        //        posts.Add(post);
        //    }

        //    return posts;
        //}

      


        public static async Task GenerateRandomDataSQL()
        {
            var random = new Random();

            var list = FileHelper<User>.Deserialize(Path.Combine(Constants.FilesFolder, "results.json"));
            Console.WriteLine(list.Count);

            //var sqlStatements = new List<string>();

            foreach (var user in list)
            {
                var allUsers = list.Where(u => u.Id != user.Id).ToList();
                var userCount = random.Next(0, allUsers.Count() / 4);
                Console.WriteLine(userCount);
                var pendingRequestCount = (int)(userCount * 0.07);
                var friendCount = userCount - pendingRequestCount;

                var users = allUsers.ToList().GetRandomElements(userCount);

                var friendshipsFile = Path.Combine(Constants.FilesFolder, "friendships.txt");
                var friendrequestsFile = Path.Combine(Constants.FilesFolder, "friendrequests.txt");
                var notificationsFile = Path.Combine(Constants.FilesFolder, "notifications.txt");

                for (int i = 0; i < friendCount; i++)
                {
                    var friendToSendRequest = users[i];

                    var requestDate = GenerateRandomDate(new DateTime(2023, 1, 1));

                    var friendRequestId = Guid.NewGuid().ToString();
                    var frSql = $"INSERT INTO FriendRequest (Id, SenderId, ReceiverId, RequestDate, Status) " +
                        $"VALUES ('{friendRequestId}', '{user.Id}', '{friendToSendRequest.Id}', '{requestDate}', 'Accepted');";
                    FileHelper<string>.AppendTextToFile(friendrequestsFile, "GO");
                    FileHelper<string>.AppendTextToFile(friendrequestsFile, frSql);
                    //sqlStatements.Add(frSql);

                    var ntfcId = Guid.NewGuid().ToString();
                    var ntfcSql = $"INSERT INTO Notifications (Id, Date, IsRead, FromUserId, ToUserId, Message) " +
                        $"VALUES ('{ntfcId}', '{requestDate}', 'true', '{user.Id}', '{friendToSendRequest.Id}', " +
                        $"'{NotificationType.GetNewFriendRequestMessage(user.UserName)}');";
                    FileHelper<string>.AppendTextToFile(notificationsFile, "GO");
                    FileHelper<string>.AppendTextToFile(notificationsFile, ntfcSql);
                    //sqlStatements.Add(ntfcSql);

                    var friendshipId = Guid.NewGuid().ToString();
                    var friendshipSql = $"INSERT INTO Friendships (FriendshipId, FriendId, UserId) " +
                        $"VALUES ('{friendshipId}', '{friendToSendRequest.Id}', '{user.Id}');";
                    FileHelper<string>.AppendTextToFile(friendshipsFile, "GO");
                    FileHelper<string>.AppendTextToFile(friendshipsFile, friendshipSql);
                    //sqlStatements.Add(friendshipSql);

                    var ntfc2Id = Guid.NewGuid().ToString();
                    var ntfc2Sql = $"INSERT INTO Notifications (Id, Date, IsRead, FromUserId, ToUserId, Message) " +
                        $"VALUES ('{ntfc2Id}', '{requestDate.AddDays(random.Next(35))}', 'true', '{friendToSendRequest.Id}', '{user.Id}', " +
                        $"'{NotificationType.GetFriendRequestAcceptedMessage(friendToSendRequest.UserName)}');";
                    FileHelper<string>.AppendTextToFile(notificationsFile, "GO");
                    FileHelper<string>.AppendTextToFile(notificationsFile, ntfc2Sql);
                    //sqlStatements.Add(ntfc2Sql);
                }

                for (int i = 0; i < pendingRequestCount; i++)
                {
                    var friendToSendRequest = users[i + friendCount];

                    var requestDate = GenerateRandomDate(new DateTime(2023, 1, 1));

                    var friendRequestId = Guid.NewGuid().ToString();
                    var frSql = $"INSERT INTO FriendRequest (Id, SenderId, ReceiverId, RequestDate, Status) " +
                        $"VALUES ('{friendRequestId}', '{user.Id}', '{friendToSendRequest.Id}', '{requestDate}', 'Pending');";
                    FileHelper<string>.AppendTextToFile(friendrequestsFile, "GO");
                    FileHelper<string>.AppendTextToFile(friendrequestsFile, frSql);

                    var ntfcId = Guid.NewGuid().ToString();
                    var ntfcSql = $"INSERT INTO Notifications (Id, Date, IsRead, FromUserId, ToUserId, Message) " +
                        $"VALUES ('{ntfcId}', '{requestDate}', 'true', '{user.Id}', '{friendToSendRequest.Id}', " +
                        $"'{NotificationType.GetNewFriendRequestMessage(user.UserName)}');";
                    FileHelper<string>.AppendTextToFile(notificationsFile, "GO");
                    FileHelper<string>.AppendTextToFile(notificationsFile, ntfcSql);
                }
            }

            // Combine all SQL statements into a single string
            //var sqlString = string.Join(Environment.NewLine, sqlStatements);

            await Console.Out.WriteLineAsync("DONE");
        }

        private static DateTime GenerateRandomDate(DateTime startDate)
        {
            DateTime endDate = DateTime.Today;
            Random random = new Random();
            int range = (endDate - startDate).Days;

            return startDate.AddDays(random.Next(range));
        }

    }

    internal class NotificationType
    {
        /// <summary>
        /// Generates a notification message for a new friend request.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetNewFriendRequestMessage(string username)
        {
            return $"You have received a friend request from {username}!";
        }

        /// <summary>
        /// Generates a notification message for a friend request being accepted.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetFriendRequestAcceptedMessage(string username)
        {
            return $"{username} accepted your friend request!";
        }

        /// <summary>
        /// Generates a notification message for a friend request being declined.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetFriendRequestDeclinedMessage(string username)
        {
            return $"{username} declined your friend request!";
        }

        /// <summary>
        /// Generates a notification message for someone liking your post.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetLikedYourPostMessage(string username)
        {
            return $"{username} liked your post!";
        }

        /// <summary>
        /// Generates a notification message for someone commenting on your post.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetCommentedOnYourPostMessage(string username)
        {
            return $"{username} commented on your post!";
        }

        /// <summary>
        /// Generates a notification message for someone sending you a message.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetSentYouMessageMessage(string username)
        {
            return $"{username} sent you a message!";
        }

        /// <summary>
        /// Generates a notification message for someone sharing a post.
        /// </summary>
        /// <param name="username">The username of the sender.</param>
        /// <returns>The notification message.</returns>
        public static string GetSharedPostMessage(string username)
        {
            return $"{username} shared a post!";
        }
    }
}
