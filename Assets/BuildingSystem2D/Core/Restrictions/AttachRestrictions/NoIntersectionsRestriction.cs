using UnityEngine;

namespace BuildingSystem2D.Core.Restrictions.AttachRestrictions
{
    /// <summary>
    /// Checks if the block intersects with other blocks of a construction.
    /// If the construction contains no blocks, returns true.
    /// WARNING: The block size is set in relative coordinates.
    /// </summary>
    public class NoIntersectionsRestriction : AttachRestriction
    {
        private readonly Vector2 _blockSize;
        private readonly Construction _construction;

        public NoIntersectionsRestriction(Vector2 blockSize, Construction construction)
        {
            _blockSize = blockSize;
            _construction = construction;
        }

        public override bool CanAttach(Block block)
        {
            foreach (var constructionBlock in _construction.Blocks)  
            {
                if (AreIntersecting(constructionBlock, block))
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreIntersecting(Block first, Block second)
        {
            var xDistance = Mathf.Abs(first.Position.x - second.Position.x);
            var yDistance = Mathf.Abs(first.Position.y - second.Position.y);
            return !(xDistance >= _blockSize.x || yDistance >= _blockSize.y);
        }
    }
}