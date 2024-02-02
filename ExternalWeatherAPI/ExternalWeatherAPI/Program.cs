using ExternalWeatherAPI.Classes;
using ExternalWeatherAPI.Data;

namespace ExternalWeatherAPI
{
	/*
	 * Program requires	the	following NuGet	packages:
	 * Newtonsoft.Json
	 * 
	 * Please make sure	these packages are installed before	running	the	program.
	 */
	internal class Program
	{
		private	static OpenWeatherFunctions	_owf = new OpenWeatherFunctions();
		private	static string _cachedCityName =	"";
		private	static WeatherData _cachedWeatherData;

		static void	Main(string[] args)
		{
			while (true)
			{
				// Prompt the user to enter	a city name
				Console.Write("Enter city name (type 'exit'	to quit): ");
				string cityName	= Console.ReadLine().ToLower();

				// Exit	the	loop if	the	user types 'exit'
				if (cityName.ToLower() == "exit") break;

				// If the user types the same city name	as the last	time, don't	make a new API call
				if (cityName ==	_cachedCityName)
				{
					Console.Clear();
					Console.WriteLine(_owf.BuildApiUrl(cityName));
					Console.WriteLine();

					PrintWeatherInfo(_cachedWeatherData);
					Console.WriteLine();

					continue;
				}

				// If the user types a different city name,	make a new API call
				try
				{
					Task.Run(async () =>
					{
						// await the result	of the GetWeatherDataAsync method
						_cachedWeatherData = await _owf.GetWeatherDataAsync(cityName);

						Console.Clear(); //	clear the console
						Console.WriteLine(_owf.BuildApiUrl(cityName)); // print	the	API	URL
						Console.WriteLine(); //	print a	blank line

						PrintWeatherInfo(_cachedWeatherData); // print the weather data
						Console.WriteLine(); //	print a	blank line

					}).Wait();
				}
				catch (Exception ex) //	Catch any exceptions and print the error message
				{
					Console.Clear(); //	clear the console
					Console.WriteLine(_owf.BuildApiUrl(cityName)); // print	the	API	URL
					Console.WriteLine(); //	print a	blank line

					PrintWeatherInfo(_cachedWeatherData); // print the cached weather data
					Console.WriteLine(); //	print a	blank line

					Console.WriteLine($"Error: {ex.Message}"); // print	the	error message
					Console.WriteLine(); //	print a	blank line
				}
			}
		}

		///	<summary>
		///	Print weather data to the console on separate lines.
		///	</summary>
		///	<param name="weatherData">The weather data to print</param>
		private	static void	PrintWeatherInfo(WeatherData weatherData)
		{
			Console.WriteLine($"City: {weatherData.Name}");
			// Convert the temperature from	Kelvin to Celsius
			Console.WriteLine($"Temperature: {(int)(weatherData.Main.Temp -	273.15)}°C");
			Console.WriteLine($"Weather: {weatherData.Weather[0].Description}");
			Console.WriteLine($"Humidity: {weatherData.Main.Humidity}%");
			Console.WriteLine($"Wind Speed:	{weatherData.Wind.Speed} m/s");
		}

		
	}
}