namespace ExternalWeatherAPI.Classes
{
	public class WeatherInfo
	{
		public WeatherInfo(string description)
		{
			Description = description;
		}

		public string Description { get; }
	}
}
