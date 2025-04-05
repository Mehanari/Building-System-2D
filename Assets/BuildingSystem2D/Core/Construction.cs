using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace BuildingSystem2D.Core
{
    /// <summary>
    /// The construction is a collection of blocks.
    /// </summary>
    [Serializable]
    public class Construction
    {
        [JsonProperty("blocks")] private List<Block> _blocks = new();
        [JsonIgnore] private Dictionary<Vector2, Block> _blocksDict = new(); //Used for fast search by position.

        [JsonIgnore] public IReadOnlyCollection<Block> Blocks => _blocks;

        public event Action<Block> BlockAdded;
        public event Action<Block> BlockRemoved;

        public Construction()
        {
        }

        public Construction(params Block[] blocks)
        {
            _blocks = new List<Block>(blocks);
            InitializeDictionary();
        }

        [JsonConstructor]
        public Construction(List<Block> blocks)
        {
            _blocks = new List<Block>(blocks);
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            foreach (var block in _blocks)
            {
                _blocksDict.Add(block.Position, block);
            }
        }
        
        public bool TryGetBlock(Vector2 position, out Block block)
        {
            return _blocksDict.TryGetValue(position, out block);
        }

        public void AddBlock(Block block)
        {
            _blocks.Add(block);
            _blocksDict.Add(block.Position, block);
            BlockAdded?.Invoke(block);
        }

        /// <summary>
        /// Will not invoke BlockRemoved is block is absent in the construction.
        /// </summary>
        /// <param name="block"></param>
        public void RemoveBlock(Block block)
        {
            if (_blocks.Remove(block))
            {
                _blocksDict.Remove(block.Position);
                BlockRemoved?.Invoke(block);
            }
        }
    }
}
