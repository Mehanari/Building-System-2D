using System;
using System.Collections.Generic;

namespace BuildingSystem2D.Core.Algorithms
{
    /// <summary>
    /// Checks if it is possible to remove a block from a construction so that all remaining blocks of the constructions are connected to a specified root block.
    /// </summary>
    public class ConnectivityChecker
    {
        private readonly IReadOnlyDictionary<Block, List<Block>> _adjacency;
        private readonly Block _rootBlock;
        private Dictionary<Block, int> _discoveryTime;
        private Dictionary<Block, int> _lowTime;
        private HashSet<Block> _articulationPoints;
        private int _time;
        
        public ConnectivityChecker(IReadOnlyDictionary<Block, List<Block>> graph, Block root)
        {
            _adjacency = graph;
            _rootBlock = root;
        }
        
        public bool CanRemoveBlock(Block block)
        {
            if (block == _rootBlock)
                return false;
            
            FindArticulationPoints();
            return !_articulationPoints.Contains(block);
        }
        
        private void FindArticulationPoints()
        {
            _discoveryTime = new Dictionary<Block, int>();
            _lowTime = new Dictionary<Block, int>();
            _articulationPoints = new HashSet<Block>();
            HashSet<Block> visited = new HashSet<Block>();
            _time = 0;
            
            foreach (Block block in _adjacency.Keys)
            {
                _discoveryTime[block] = -1;
                _lowTime[block] = -1;
            }
            
            DfsArticulationPoints(_rootBlock, visited, null);
        }
        
        private void DfsArticulationPoints(Block current, HashSet<Block> visited, Block parent)
        {
            int children = 0;
            visited.Add(current);
            
            _discoveryTime[current] = _lowTime[current] = ++_time;
            
            foreach (Block neighbor in _adjacency[current])
            {
                if (!visited.Contains(neighbor))
                {
                    children++;
                    DfsArticulationPoints(neighbor, visited, current);
                    
                    _lowTime[current] = Math.Min(_lowTime[current], _lowTime[neighbor]);
                    
                    if (parent != null && _lowTime[neighbor] >= _discoveryTime[current])
                    {
                        _articulationPoints.Add(current);
                    }
                }
                else if (neighbor != parent)
                {
                    _lowTime[current] = Math.Min(_lowTime[current], _discoveryTime[neighbor]);
                }
            }
            
            if (parent == null && children > 1)
            {
                _articulationPoints.Add(current);
            }
        }
    }
}