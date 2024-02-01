namespace ExternalWeatherAPI.Classes
{
	public class MainData
	{
		public MainData(float temp, int humidity)
		{
			Temp = temp;
			Humidity = humidity;
		}

		public float Temp { get; }
		public int Humidity { get; }
	}
}