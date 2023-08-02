using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataGenerator.Entities;
using DataGenerator.Helpers;
using Newtonsoft.Json;

namespace DataGenerator.Services
{
    public class PostData
    {
        public string MediaUrl { get; set; }
        public string RelatedText { get; set; }
    }

    public static class PostDataService
    {
        private static readonly Cloudinary _cloudinary;

        static PostDataService()
        {
            // Configure Cloudinary with your cloud name, API key, and API secret
            Account account = new Account(
                "your_cloud_name",
                "your_api_key",
                "your_api_secret");

            _cloudinary = new Cloudinary(account);
        }

        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly Random _random = new Random();
        private static readonly string _pexelsApiKey = "563492ad6f917000010000018a22158dcfd14500b388238b3ce86808"; // Replace with your Pexels API key

        public static async Task GeneratePostData(int numberOfPosts)
        {
            var faker = new Faker();

            var path = Path.Combine(DataGenerator.Helpers.Constants.FilesFolder, DataGenerator.Helpers.Constants.UserIdsFile);
            var resultPath = Path.Combine(DataGenerator.Helpers.Constants.FilesFolder, DataGenerator.Helpers.Constants.ResultFile);
            var ids = FileHelper<string>.GetDataFromTxtFile(path);
            var images = FileHelper<string>.GetDataFromTxtFile(Path.Combine(DataGenerator.Helpers.Constants.FilesFolder, "images.txt"));

            for (int i = 0; i < numberOfPosts; i++)
            {
                Console.WriteLine("Image URL : " + images[i]);

                await Console.Out.WriteAsync($"Uploading image ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                await Console.Out.WriteAsync(i.ToString() + " ");
                Console.ResetColor();
                await Console.Out.WriteLineAsync($"to cloudinary");

                var uploadResult = await UploadImageToCloudinary(images[i]);

                Console.ForegroundColor = ConsoleColor.Green;

                await Console.Out.WriteLineAsync("Uploaded to " + uploadResult.SecureUrl.AbsoluteUri);

                var post = new Post
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = faker.Lorem.Sentences(),
                    HasMediaContent = true,
                    ContentUrl = uploadResult.SecureUrl.AbsoluteUri,
                    IsVideo = false,
                    CreatedAt = faker.Date.Past(),
                    UserId = faker.PickRandom(ids)
                };

                var insertStatement = SqlHelper.GenerateInsertSqlStatement(post);

                FileHelper<string>.AppendTextToFile(resultPath, insertStatement);

                Console.ResetColor();
            }
        }

        private static async Task<ImageUploadResult> UploadImageToCloudinary(string imageUrl)
        {
            // Configure Cloudinary with your cloud name, API key, and API secret
            Account account = new Account(
                "dax9yhk8g",
                "881374158784918",
                "C0qdG2p4fB8Tu4EFk1XjYKjrHyQ");

            var cloudinary = new Cloudinary(account);

            // Create an upload parameter with the image URL
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageUrl),
                UseFilename = true,
                UniqueFilename = false
            };

            // Upload the image to Cloudinary
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }
    }
}
