using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("\n##################################################################################\n");
        Console.WriteLine("\n           SEVGİLİ KULLANICIMIZ HAVA DURUMU SAYFASINA HOŞGELDİNİZ   \n");
        Console.WriteLine("\n   LÜTFEN UYARILARI DİKKATE ALINIZ VE ZORLU HAVA KOŞULLARINDAN ETKİLENMEYİNİZ  \n");
        Console.WriteLine("\n##################################################################################\n");

        HavaDurumu istanbulHavaDurumu = await GetWeather("istanbul");
        HavaDurumu izmirHavaDurumu = await GetWeather("izmir");
        HavaDurumu ankaraHavaDurumu = await GetWeather("ankara");

        Console.WriteLine("İstanbul Hava Durumu:");
        PrintWeatherInfo(istanbulHavaDurumu);

        Console.WriteLine("\nİzmir Hava Durumu:");
        PrintWeatherInfo(izmirHavaDurumu);

        Console.WriteLine("\nAnkara Hava Durumu:");
        PrintWeatherInfo(ankaraHavaDurumu);
    }

    static async Task<HavaDurumu> GetWeather(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"https://goweather.herokuapp.com/weather/{city}";

            try
            {
                string weatherJson = await client.GetStringAsync(apiUrl);
                return JsonConvert.DeserializeObject<HavaDurumu>(weatherJson);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"API çağrısı sırasında bir hata oluştu: {ex.Message}");
                return null;
            }
        }
    }

    static void PrintWeatherInfo(HavaDurumu havaDurumu)
    {
        if (havaDurumu != null)
        {
            Console.WriteLine($"Durum: {havaDurumu.Description}");
            Console.WriteLine($"Sıcaklık: {havaDurumu.Temperature}");
            Console.WriteLine($"Nem: {havaDurumu.Humidity}");
            Console.WriteLine($"Rüzgar: {havaDurumu.Wind}");
            Console.WriteLine($"Görünürlük: {havaDurumu.Visibility}");

            if (havaDurumu.Forecast != null && havaDurumu.Forecast.Any())
            {
                Console.WriteLine("\nTahmini Hava Durumu (Önümüzdeki 3 Gün):");
                DateTime today = DateTime.Now.Date;

                foreach (var tahmin in havaDurumu.Forecast)
                {
                    today = today.AddDays(1); // Bir sonraki günü al

                    Console.WriteLine($"Tarih: {today.ToShortDateString()}, Gün: {tahmin.Day}, Durum: {tahmin.Description}, Max Sıcaklık: {tahmin.MaxTemperature}, Min Sıcaklık: {tahmin.MinTemperature}");
                }
            }
            else
            {
                Console.WriteLine("API'den tahmin verileri alınamadı.");
            }
        }
        else
        {
            Console.WriteLine("Hava durumu bilgileri alınamadı.");
        }
    }
}

