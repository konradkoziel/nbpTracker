using Newtonsoft.Json;

namespace nbpTracker.Model.ApiResponses
{
    public class Rate
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("mid")]
        public double Mid { get; set; }
    }
}
