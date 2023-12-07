using System.Collections.Generic;

public class HavaDurumu
{
    public string Description { get; set; }
    public string Temperature { get; set; }
    public string Humidity { get; set; }
    public string Wind { get; set; }
    public string Visibility { get; set; }
    public List<TahminVeri> Forecast { get; set; }
}
