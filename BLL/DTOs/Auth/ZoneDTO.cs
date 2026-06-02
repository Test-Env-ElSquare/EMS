




namespace BLL.DTOs.Auth
{
    public class ZoneDTO
    {
        public int ZoneId { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<AreaLookupDto> Areas { get; set; } = new();
    }

    public class AreaLookupDto
    {
        public int AreaId { get; set; }
        public string Name { get; set; } = string.Empty;
    }



    public class StationsPerZoneDTO
    {
        [JsonPropertyOrder(1)]
        public int SiteId { get; set; }

        [JsonPropertyOrder(2)]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyOrder(3)]
        public string Device_ID { get; set; } = string.Empty;

        [JsonPropertyOrder(4)]
        public double Latitude { get; set; }

        [JsonPropertyOrder(5)]
        public double Longitude { get; set; }

        [JsonPropertyOrder(6)]
        public bool IsActive { get; set; }

        [JsonPropertyOrder(7)]
        public bool IsInstalled { get; set; }
    }

    public class ZoneWithStationsDTO : ZoneDTO
    {
        [JsonPropertyOrder(3)]
        public List<StationsPerZoneDTO> Sites { get; set; } = new();
    }

    public class CreateZoneDto
    {
        public string Name { get; set; } = string.Empty;
        public int AreaId { get; set; }
    }
}
