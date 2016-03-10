using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace Cats
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("http://agl-developer-test.azurewebsites.net/people.json").Result;
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var values = JArray.Parse(json);

                foreach (var group in values.GroupBy(people => (string) people["gender"]))
                {
                    Console.WriteLine(group.Key);
                    Console.WriteLine("============");
                    foreach (
                        var catName in
                            group.SelectMany(people => people["pets"])
                                .Where(pet => (string) pet["type"] == "Cat")
                                .Select(cat => (string) cat["name"])
                                .OrderBy(name => name))
                    {
                        Console.WriteLine(catName);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
