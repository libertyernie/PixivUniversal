using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixeez.Objects
{
    public class UsersWork:Work
    {
        [JsonProperty("sanity_level")]
        public string SanityLevel { get; set; }
    }
}
