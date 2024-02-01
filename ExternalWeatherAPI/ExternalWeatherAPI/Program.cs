using ExternalWeatherAPI.Classes;
using Newtonsoft.Json;

namespace ExternalWeatherAPI
{
	internal class Program
	{
		// use the API key provided in the project files
		// OR use your own API key from https://openweathermap.org/api
		private static readonly string _apiKey = "";
		private static readonly string _apiKeyQuery = "&appid=";
		private static readonly string _locQuery = "?q=";
		private static readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/weather";

		private static string _cachedCityName ="";
		private static WeatherData _cachedWeatherData;

		private static void Main(string[] args)
		{
			while (true)
			{
				// Prompt the user to enter a city name
				Console.Write("Enter city name (type 'exit' to quit): ");
				string cityName = Console.ReadLine().ToLower();

				// Exit the loop if the user types 'exit'
				if (cityName.ToLower() == "exit") break;

				// If the user types the same city name as the last time, don't make a new API call
				if (cityName == _cachedCityName)
				{
					Console.Clear();
					Console.WriteLine(BuildApiUrl(cityName));
					Console.WriteLine();

					PrintWeatherInfo(_cachedWeatherData);
					Console.WriteLine();

					continue;
				}

				// If the user types a different city name, make a new API call
				try
				{
					Task.Run(async () =>
					{
						// await the result of the GetWeatherDataAsync method
						_cachedWeatherData = await GetWeatherDataAsync(cityName);

						Console.Clear(); // clear the console
						Console.WriteLine(BuildApiUrl(cityName)); // print the API URL
						Console.WriteLine(); // print a blank line

						PrintWeatherInfo(_cachedWeatherData); // print the weather data
						Console.WriteLine(); // print a blank line

					}).Wait();
				}
				catch (Exception ex) // Catch any exceptions and print the error message
				{
					Console.Clear(); // clear the console
					Console.WriteLine(BuildApiUrl(cityName)); // print the API URL
					Console.WriteLine(); // print a blank line

					PrintWeatherInfo(_cachedWeatherData); // print the cached weather data
					Console.WriteLine(); // print a blank line

					Console.WriteLine($"Error: {ex.Message}"); // print the error message
					Console.WriteLine(); // print a blank line
				}
			}
		}

		/// <summary>
		/// Retrieve weather data from the OpenWeatherMap API.
		/// </summary>
		/// <param name="cityName">The name of the city to retrieve weather data for</param>
		/// <returns>The weather data for the specified city</returns>
		/// <exception cref="HttpRequestException"></exception>
		private static async Task<WeatherData> GetWeatherDataAsync(string cityName)
		{
			// Create a new HttpClient instance
			using (HttpClient httpClient = new HttpClient())
			{
				// Construct the API URL
				string apiUrl = BuildApiUrl(cityName);

				// Send a GET request to the API URL
				HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

				// If the response is successful, read the JSON response and deserialize it into a WeatherData object
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
					return weatherData;
				}
				else // If the response is not successful, throw an HttpRequestException
				{
					throw new HttpRequestException($"HTTP error: {response.StatusCode} - {response.ReasonPhrase}");
				}
			}
		}

		/// <summary>
		/// Print weather data to the console on separate lines.
		/// </summary>
		/// <param name="weatherData">The weather data to print</param>
		private static void PrintWeatherInfo(WeatherData weatherData)
		{
			Console.WriteLine($"City: {weatherData.Name}");
			Console.WriteLine($"Temperature: {(int)(weatherData.Main.Temp - 273.15)}°C");
			Console.WriteLine($"Weather: {weatherData.Weather[0].Description}");
			Console.WriteLine($"Humidity: {weatherData.Main.Humidity}%");
			Console.WriteLine($"Wind Speed: {weatherData.Wind.Speed} m/s");
		}

		/// <summary>
		/// Print the API URL to the console.
		/// </summary>
		/// <param name="cityName">The city name to include in the URL</param>
		/// <returns>A reconstructed API URL</returns>
		private static string BuildApiUrl(string cityName)
		{
			return $"{_baseUrl}{_locQuery}{cityName.ToLower()}{_apiKeyQuery}{_apiKey}";
		}
	}
}