namespace ExternalWeatherAPI.Data
{
	public class WeatherData
	{
		public WeatherData(string name,	MainData main, WeatherInfo[] weather, WindInfo wind)
		{
			Name = name;
			Main = main;
			Weather	= weather;
			Wind = wind;
		}

		public string Name { get; }
		public MainData	Main { get;	}
		public WeatherInfo[] Weather { get;	}
		public WindInfo	Wind { get;	}
	}
}
