using DataGenerator.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zust.Entities.Models;

namespace DataGenerator.Helpers
{
    public class Like
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class LikeGenerator
    {
        static Random random = new Random();


        public static List<string> ReadIdsFromFile(string fileName)
        {
            List<string> ids = new List<string>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    ids.Add(line);
                }
            }
            return ids;
        }

        public static List<Like> GenerateRandomLikes(List<string> postIds, List<string> userIds)
        {
            List<Like> likes = new List<Like>();
            foreach (string postId in postIds)
            {
                int numLikes = random.Next(1, userIds.Count / 3);
                Console.WriteLine(numLikes);
                for (int i = 0; i < numLikes; i++)
                {
                    string randomUserId = userIds[random.Next(userIds.Count)];
                    likes.Add(new Like
                    {
                        Id = Guid.NewGuid().ToString(),
                        PostId = postId,
                        UserId = randomUserId
                    });
                }
            }
            return likes;
        }

        public static string GenerateInsertStatement(Like like)
        {
            return $"INSERT INTO Likes (Id, PostId, UserId) VALUES ('{like.Id}', '{like.PostId}', '{like.UserId}');";
        }
    }
}
