using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using static Talma.Program;

namespace Talma
{
    internal class Program
    {

        public class Coord
        {
            [JsonProperty("lon")]
            public double lon { get; set; }

            [JsonProperty("lat")]
            public double lat { get; set; }
        }

        public class Main_
        {
            [JsonProperty("temp")]
            public double temp { get; set; }
            [JsonProperty("pressure")]
            public int pressure { get; set; }
            [JsonProperty("humidity")]
            public int humidity { get; set; }
            [JsonProperty("temp_min")]
            public double temp_min { get; set; }
            [JsonProperty("temp_max")]
            public double temp_max { get; set; }
        }

        public class Wind
        {
            [JsonProperty("speed")]
            public double speed { get; set; }
            [JsonProperty("deg")]
            public double deg { get; set; }
        }

        public class Clouds
        {
            [JsonProperty("all")]
            public int all { get; set; }
        }

        public class Weather
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("main")]
            public string main_ { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
            [JsonProperty("icon")]
            public string icon { get; set; }
        }

        public class City
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("coord")]
            public Coord coord { get; set; }
            [JsonProperty("main")]
            public Main_ main_ { get; set; }
            [JsonProperty("dt")]
            public long dt { get; set; }
            [JsonProperty("wind")]
            public Wind wind { get; set; }
            [JsonProperty("clouds")]
            public Clouds clouds { get; set; }
            [JsonProperty("weather")]
            public List<Weather> weather { get; set; }
        }

        public class InputData
        {
            [JsonProperty("cities")]
            public List<City> cities { get; set; }
        }

        public class OutputCity
        {
            [JsonProperty("id")]
            public int id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
            [JsonProperty("main")]
            public Main_ main_ { get; set; }
        }

        public class OutputWeather
        {
            [JsonProperty("main")]
            public string main_ { get; set; }
            [JsonProperty("description")]
            public string description { get; set; }
            [JsonProperty("icon")]
            public string icon { get; set; }
        }

        public class Output
        {
            [JsonProperty("weather")]
            public OutputWeather weather { get; set; }
            [JsonProperty("cities")]
            public List<OutputCity> cities { get; set; }
        }


        static void Main(string[] args)
        {
            string fileNamebase = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory());
            string inputPath = @"D:\ExamenJson\Talma\input.json";
            string outputPath = @"D:\ExamenJson\Talma\output.json";

            var jsonInput = File.ReadAllText(inputPath);
            var inputData = JsonConvert.DeserializeObject<InputData>(jsonInput);

            var groupedData = new Dictionary<string, Output>();

            foreach (var city in inputData.cities)
            {
                var weatherCondition = city.weather[0].main_;
                var weatherDescription = city.weather[0].description;
                var weatherIcon = city.weather[0].icon;

                var weatherKey = $"{weatherCondition}-{weatherDescription}-{weatherIcon}";

                if (!groupedData.ContainsKey(weatherKey))
                {
                    groupedData[weatherKey] = new Output
                    {
                        weather = new OutputWeather
                        {
                            main_ = weatherCondition,
                            description = weatherDescription,
                            icon = weatherIcon
                        },
                        cities = new List<OutputCity>()
                    };
                }

                groupedData[weatherKey].cities.Add(new OutputCity
                {
                    id = city.id,
                    name = city.name,
                    main_ = city.main_
                });
            }

            var outputJson = JsonConvert.SerializeObject(groupedData.Values, Formatting.Indented);
            File.WriteAllText(outputPath, outputJson);

            Console.WriteLine("Proceso Completo");
        }
    }
}
