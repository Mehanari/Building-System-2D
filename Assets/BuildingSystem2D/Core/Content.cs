using System;
using Newtonsoft.Json;

namespace BuildingSystem2D.Core
{
    [Serializable]
    public class Content
    {
        [JsonProperty("prefabId")]
        public string PrefabId { get; set; }
        /// <summary>
        /// Use this field to store states of objects from your scene.
        /// </summary>\
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}