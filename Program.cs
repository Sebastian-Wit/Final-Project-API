using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Numerics;

namespace WebAPIClient
{
    class Zipcode
    {
        [JsonProperty("post code")]
        public string post_code { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

        [JsonProperty("country abbreviation")]
        public string country_abbreviation { get; set; }

        [JsonProperty("place name")]
        public string place_name { get; set; }

        [JsonProperty("longitude")]
        public string longitude { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("state abbreviation")]
        public string state_abbreviation { get; set; }

        [JsonProperty("latitude")]
        public string latitude { get; set; }

    }
}

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        await ProcessRepositories();
    }

    private static async Task ProcessRepositories()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Enter US zip code. Press enter without writing anything to quit");

                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                var result = await client.GetAsync($"http://api.zippopotam.us/us/{input}");
                var resultRead = await result.Content.ReadAsStringAsync();

                var zipcode = JsonConvert.DeserializedObject<Zipcode>(resultRead);

                Console.WriteLine("---");
                Console.WriteLine("Post Code: " + zipcode.post_code);
                Console.WriteLine("Country: " + zipcode.country);
                Console.WriteLine("Country Abbreviation: " + zipcode.country_abbreviation);
                Console.WriteLine("Places: ");
                Console.WriteLine(" Place Name: " + zipcode.place_name);
                Console.WriteLine(" State: " + zipcode.state);
                Console.WriteLine(" State Abbreviation: " + zipcode.state_abbreviation);
                Console.WriteLine(" Latitude: " + zipcode.latitude);
                Console.WriteLine(" Longitude: " + zipcode.longitude);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR. Please enter either a valid US zip code, or press enter to exit");

            }
        }
    }
}

