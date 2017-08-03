using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestSender
{
    public class CustomerJson
    {
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        [JsonProperty("first_name")]
        public string Firstname { get; set; }

        [JsonProperty("last_name")]
        public string Lastname { get; set; }

}
}
