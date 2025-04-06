using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BuildingSystem2D.Core
{
    /// <summary>
    /// This class describes part of your building: its position and inner state data.
    /// The position is relative and you are free to interpret it as you like.
    /// </summary>
    [Serializable]
    public class Block
    {
        [JsonProperty("position")]
        public Vector2 Position { get; set; }
        [JsonProperty("contents")]
        public List<Content> Contents { get; set; } = new();
    }
}
