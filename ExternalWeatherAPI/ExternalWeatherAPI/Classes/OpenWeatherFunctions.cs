using ExternalWeatherAPI.Data;
using Newtonsoft.Json;

namespace ExternalWeatherAPI.Classes
{
	public class OpenWeatherFunctions
	{
		// use the API key provided	in the project files
		// OR use your own API key from	https://openweathermap.org/api
		// (creating an	API	key	takes less than	a minute, however,
		// it takes	a while	to "activate" before being able	to access the API)
		private	readonly string	_apiKey	= "";

		private	readonly string	_apiKeyQuery = "&appid=";
		private	readonly string	_locQuery =	"?q=";
		private	readonly string	_baseUrl = "https://api.openweathermap.org/data/2.5/weather";

		///	<summary>
		///	Retrieve weather data from the OpenWeatherMap API.
		///	</summary>
		///	<param name="cityName">The name	of the city	to retrieve	weather	data for</param>
		///	<returns>The weather data for the specified	city</returns>
		///	<exception cref="HttpRequestException"></exception>
		public async Task<WeatherData> GetWeatherDataAsync(string cityName)
		{
			// Create a	new	HttpClient instance
			using (HttpClient httpClient = new HttpClient())
			{
				// Construct the API URL
				string apiUrl =	BuildApiUrl(cityName);

				// Send	a GET request to the API URL
				HttpResponseMessage	response = await httpClient.GetAsync(apiUrl);

				// If the response is successful, read the JSON	response and deserialize it	into a WeatherData object
				if (response.IsSuccessStatusCode)
				{
					string json	= await	response.Content.ReadAsStringAsync();
					WeatherData	weatherData	= JsonConvert.DeserializeObject<WeatherData>(json);
					return weatherData;
				}
				else //	If the response	is not successful, throw an	HttpRequestException
				{
					throw new HttpRequestException($"HTTP error: {response.StatusCode} - {response.ReasonPhrase}");
				}
			}
		}

		///	<summary>
		///	Print the API URL to the console.
		///	</summary>
		///	<param name="cityName">The city	name to	include	in the URL</param>
		///	<returns>A reconstructed API URL</returns>
		public string BuildApiUrl(string cityName)
		{
			return $"{_baseUrl}{_locQuery}{cityName.ToLower()}{_apiKeyQuery}{_apiKey}";
		}
	}
}