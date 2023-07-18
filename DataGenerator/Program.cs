using System;
using System.Globalization;
using DataGenerator.Helpers;
using DataGenerator.Services;

namespace DataGenerator
{
    public class Program
    {
        static void Main()
        {
            try
            {
                //Data.GenerateUserData();
                //Data.GenerateBogusPosts();
                Call();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static async void Call()
        {
            await PostDataService.GeneratePostData(1000);
        }
    }
}
