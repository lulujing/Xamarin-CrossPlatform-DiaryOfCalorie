using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Tabs
{
    public class FoodCalorie
    {
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime updatedAt { get; set; }

        [JsonProperty(PropertyName = "Tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "CaloriePerG")]
        public float CaloriePerG { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public float quantity { get; set; }
    }

    public class FoodCalorieM
    {
        
        public string ID { get; set; }

       
        public string updatedAt { get; set; }

       
        public string Tag { get; set; }

       
        public float CaloriePerG { get; set; }

       
        public float quantity { get; set; }
    }
}
