using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace nbpTracker.Model.ApiResponses
{
    public partial class NbpTable
    {
        [JsonProperty("table")]
        public string Table { get; set; }

        [JsonProperty("no")]
        public string No { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTimeOffset EffectiveDate { get; set; }

        [JsonProperty("rates")]
        public List<Rate> Rates { get; set; }
    }
}
