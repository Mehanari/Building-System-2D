using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem2D.Core.Algorithms
{
    /// <summary>
    /// Creates a cross-adjacency graph for a given construction.
    /// The block is cross-adjacent to another block if this block is placed upwards, downwards, leftward or rightwards to another.
    /// Values from offset vector are used to determine cross-adjacent blocks positions.
    ///
    /// WARNING: Blocks positions are compared strictly, so it is recommended to use this class only if your blocks have integer positions.
    /// </summary>
    public class ConstructionGraph
    {
        private readonly Dictionary<Vector2, Block> _blocks;
        private readonly Dictionary<Block, List<Block>> _graph;
        private readonly Construction _construction;
        private readonly Vector2 _offsets;

        public IReadOnlyDictionary<Block, List<Block>> Graph => _graph;

        public ConstructionGraph(Construction construction, Vector2 offsets)
        {
            _blocks = new();
            _graph = new();
            _construction = construction;
            _offsets = offsets;
            foreach (var block in _construction.Blocks)
            {
                _blocks.Add(block.Position, block);
            }

            foreach (var block in _construction.Blocks)
            {
                var adjacent = GetAdjacent(block);
                _graph.Add(block, adjacent);
            }

            _construction.BlockAdded += OnBlockAdded;
            _construction.BlockRemoved += OnBlockRemoved;
        }

        private void OnBlockRemoved(Block block)
        {
            var adjacent = GetAdjacent(block);
            foreach (var adjacentBlock in adjacent)
            {
                _graph[adjacentBlock].Remove(block);
            }
            _graph.Remove(block);
            _blocks.Remove(block.Position);
        }

        private void OnBlockAdded(Block block)
        {
            if (_blocks.ContainsKey(block.Position))
            {
                Debug.LogError("Block with a given position is already added.");
                return;
            }
            _blocks.Add(block.Position, block);
            var adjacent = GetAdjacent(block);
            foreach (var adjacentBlock in adjacent)
            {
                _graph[adjacentBlock].Add(block);
            }
            _graph.Add(block, GetAdjacent(block));
        }

        private List<Block> GetAdjacent(Block block)
        {
            var adjacentBlocks = new List<Block>();
            var adjacentPositions = new List<Vector2>
            {
                block.Position + new Vector2(_offsets.x, 0),
                block.Position + new Vector2(-_offsets.x, 0),
                block.Position + new Vector2(0, _offsets.y),
                block.Position + new Vector2(0, -_offsets.y)
            };
            foreach (var pos in adjacentPositions)
            {
                if(_blocks.TryGetValue(pos, out var adjacentBlock))
                {
                    adjacentBlocks.Add(adjacentBlock);
                }
            }

            return adjacentBlocks;
        }
    }
}