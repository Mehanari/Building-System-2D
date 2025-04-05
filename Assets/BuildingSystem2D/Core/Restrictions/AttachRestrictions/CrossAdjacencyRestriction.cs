using JetBrains.Annotations;
using UnityEngine;

namespace BuildingSystem2D.Core.Restrictions.AttachRestrictions
{
    /// <summary>
    /// CanAttach returns true if the new block is cross-adjacent to at least one block of the construction, returns false otherwise.
    /// The block is cross-adjacent to another block if it is placed to the leftwards, rightwards, upwards or downwards to a reference block.
    ///
    /// If the construction contains zero blocks, this restriction will call a specified fallback restriction.
    /// If the fallback restriction is null and the construction contains no blocks, then CanAttach will return false.
    /// </summary>
    public class CrossAdjacencyRestriction : AttachRestriction
    {
        private readonly Construction _construction;
        private readonly Vector2 _offsets;
        private readonly Vector2 _epsilons;
        [CanBeNull] private readonly AttachRestriction _fallBackAttachRestriction;

        public CrossAdjacencyRestriction(Construction construction, Vector2 offsets, Vector2 epsilons, AttachRestriction fallBackAttachRestriction = null)
        {
            _construction = construction;
            _offsets = offsets;
            _epsilons = epsilons;
            _fallBackAttachRestriction = fallBackAttachRestriction;
        }

        public override bool CanAttach(Block block)
        {
            if (_construction.Blocks.Count == 0)
            {
                return _fallBackAttachRestriction?.CanAttach(block) ?? false;
            }
            foreach (var buildingBlock in _construction.Blocks)
            {
                if (IsCrossAdjacent(block, buildingBlock))
                {
                    return true;
                }
            }

            return false;
        }
        
        private bool IsCrossAdjacent(Block newBlock, Block referenceBlock)
        {
            var xDistance = Mathf.Abs(newBlock.Position.x - referenceBlock.Position.x);
            var yDistance = Mathf.Abs(newBlock.Position.y - referenceBlock.Position.y);
            if (yDistance <= _epsilons.y)
            {
                //Checking if new block is placed leftwards or rightwards.
                return Mathf.Abs(xDistance - _offsets.x) <= _epsilons.x;
            }
            if (xDistance <= _epsilons.x)
            {
                //Checking if ne block is placed upwards or downwards.
                return Mathf.Abs(yDistance - _offsets.y) <= _epsilons.y;
            }
            return false;
        }
    }
}