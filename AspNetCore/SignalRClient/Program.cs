using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SignalRClient
{
    class Program
    {
        static readonly HttpClient _httpClient = new HttpClient();
        static readonly string baseUrl = "https://localhost:5001";

        static void Main(string[] args)
        {
            Console.Write("User name: ");
            var userName = Console.ReadLine();

            Console.Write("Password: ");
            var password = string.Empty;
            while (true)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
                Console.Write('*');
            }

            Console.WriteLine();

            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/chat", options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var stringData = JsonConvert.SerializeObject(new
                        {
                            userName,
                            password
                        });
                        var content = new StringContent(stringData);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        var response = await _httpClient.PostAsync($"{ baseUrl }/api/token", content);
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    };
                })
                .Build();

            hubConnection.On<string, string>("newMessage", (sender, message) => Console.WriteLine($"{sender}: {message}"));

            hubConnection.StartAsync().Wait();

            System.Console.WriteLine("\nConnected!");

            while (true)
            {
                var message = Console.ReadLine();
                if (message.Equals("exit"))
                {
                    break;
                }
                hubConnection.SendAsync("SendMessage", message).Wait();
            }

            System.Console.WriteLine("\nDisonnecting ...");
        }
    }
}
