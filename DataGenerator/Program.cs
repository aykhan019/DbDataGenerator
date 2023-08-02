using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bogus;
using DataGenerator.Entities;
using DataGenerator.Helpers;
using DataGenerator.Services;
using Newtonsoft.Json;
using Zust.Entities.Models;

namespace DataGenerator
{
    public class Program
    {
        static void Main()
        {
            Data.GenerateRandomDataSQL();
            //GenerateComments();


            //try
            //{
            //    //Data.GenerateUserData();
            //    //Data.GenerateBogusPosts();
            //    //Call();
            //    //Console.ReadLine();
            //    //Console.ReadLine();
            //    //Console.ReadLine();
            //    //Console.ReadLine();
            //    //Console.ReadLine();
            //    //Console.ReadLine();
            //    //var postsPath = Path.Combine(Constants.FilesFolder, "postsIds.txt");
            //    //var usersPath = Path.Combine(Constants.FilesFolder, "usersIds2.txt");
            //    //var resultPath = Path.Combine(Constants.FilesFolder, "results.txt");


            //var usersPath = Path.Combine(Constants.FilesFolder, "userIds.txt");
            //var postIdPath = Path.Combine(Constants.FilesFolder, "postsIds.txt");

            //List<string> postIds = FileHelper<string>.GetDataFromTxtFile(postIdPath);
            //List<string> userIds = FileHelper<string>.GetDataFromTxtFile(usersPath);

            //List<Like> likes = LikeGenerator.GenerateRandomLikes(postIds, userIds);

            //var resultPath = Path.Combine(Constants.FilesFolder, "results.txt");
            //foreach (var like in likes)
            //{
            //    var st = LikeGenerator.GenerateInsertStatement(like);

            //    FileHelper<string>.AppendTextToFile(resultPath, st);
            //}
            //Console.WriteLine("DONE");


            //    //foreach (var like in likes)
            //    //{
            //    //    string insertStatement = LikeGenerator.GenerateInsertStatement(like);
            //    //    FileHelper<string>.AppendTextToFile(resultPath, insertStatement);

            //    //    Console.WriteLine(insertStatement);
            //    //}
            //    //Console.WriteLine("RESULT PATH : " + Path.GetFullPath(resultPath));
            //    //Console.ReadLine();
            //    //GenerateComments();
            //}
            //catch (Exception ex)
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine(ex.Message);
            //    throw;
            //}
        }
        public class Comment
        {
            public string Id { get; set; }

            public string PostId { get; set; }

            public virtual Post Post { get; set; }

            public string UserId { get; set; }

            public virtual User User { get; set; }

            public string Text { get; set; }
        }

        public static List<T> GetRandomElements<T>(List<T> source, int count)
        {
            Random random = new Random();
            List<T> result = new List<T>();
            HashSet<int> selectedIndices = new HashSet<int>();

            while (result.Count < count)
            {
                int index = random.Next(source.Count);

                if (!selectedIndices.Contains(index))
                {
                    result.Add(source[index]);
                    selectedIndices.Add(index);
                }
            }

            return result;
        }

        static void GenerateComments()
        {
            var usersPath = Path.Combine(Constants.FilesFolder, "userIds.txt");
            var postIdPath = Path.Combine(Constants.FilesFolder, "postsIds.txt");
            var resultPath = Path.Combine(Constants.FilesFolder, "results.txt");
            List<string> userIds = FileHelper<string>.GetDataFromTxtFile(usersPath);
            Console.WriteLine(userIds.Count);
            List<string> postIds = FileHelper<string>.GetDataFromTxtFile(postIdPath);
            Console.WriteLine(postIds.Count);

            var faker = new Faker();
            List<Comment> comments = new List<Comment>();
            int c = 0;
            foreach (var postId in postIds)
            {
                c++;
                var rand = new Random();
                var count = rand.Next(3, 21);
                var someUsersIds = GetRandomElements(userIds, count);
                for (int i = 0; i < someUsersIds.Count; i++)
                {
                    var comment = new Comment()
                    {
                        Id = Guid.NewGuid().ToString(),
                        PostId = postId,
                        UserId = someUsersIds[i],
                        Text = faker.Lorem.Sentences()
                    };
                    comments.Add(comment);
                }
            }
            Console.WriteLine(c);

            foreach (var comment in comments)
            {
                string sqlInsertStatement = $"INSERT INTO Comments (Id, PostId, UserId, Text) " +
                    $"VALUES ('{comment.Id}', '{comment.PostId}', '{comment.UserId}', '{comment.Text}');";
                Console.WriteLine(sqlInsertStatement);
                FileHelper<string>.AppendTextToFile(resultPath, sqlInsertStatement);
            }
            Console.WriteLine("RESULT PATH : " + Path.GetFullPath(resultPath));
            Console.WriteLine("DONE");
        }

        private static async void Call()
        {
            await PostDataService.GeneratePostData(1000);
            //var urls = await FetchImageUrls("VYeAe2kdOd56SBEwGLISg3FaDFKYogWjA11ubM7RDK0", 1000);
            //var path = Path.Combine(Constants.FilesFolder, Constants.ResultFile);
            //await Console.Out.WriteLineAsync(urls.Count.ToString());
            //FileHelper<string>.WriteToFile(path, urls.ToArray());
        }

        static async Task<List<string>> FetchImageUrls(string accessKey, int imageCount)
        {
            string apiUrl = $"https://api.unsplash.com/photos/random?client_id={accessKey}&count=30";
            int remainingImages = imageCount;
            List<string> imageUrls = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                int gotImages = 0;

                while (remainingImages > 0)
                {


                    int requestCount = Math.Min(remainingImages, 30);
                    string requestUrl = $"{apiUrl}&count={requestCount}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        List<UnsplashImage> images = JsonConvert.DeserializeObject<List<UnsplashImage>>(responseContent);


                        // Extract the image URLs from the response
                        foreach (UnsplashImage image in images)
                        {
                            imageUrls.Add(image.Urls.Regular);
                        }
                        gotImages += 30;
                        Console.ForegroundColor = ConsoleColor.Green;
                        await Console.Out.WriteLineAsync("Got " + gotImages + " images!");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        await Console.Out.WriteLineAsync("Remaining " + remainingImages);
                        Console.ResetColor();

                        remainingImages -= requestCount;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to fetch image URLs. Status code: {response.StatusCode}");
                        break;
                    }
                }
            }

            return imageUrls;
        }
    }

    // Model class for the Unsplash image object
    class UnsplashImage
    {
        public UnsplashUrls Urls { get; set; }
    }

    // Model class for the Unsplash image URLs
    class UnsplashUrls
    {
        public string Regular { get; set; }
    }
}