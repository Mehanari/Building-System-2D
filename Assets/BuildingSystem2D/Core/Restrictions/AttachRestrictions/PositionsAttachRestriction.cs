using System.Collections.Generic;
using UnityEngine;

namespace BuildingSystem2D.Core.Restrictions.AttachRestrictions
{
    /// <summary>
    /// Checks if block position belongs to a list of allowed positions.
    /// </summary>
    public class PositionsAttachRestriction : AttachRestriction
    {
        private readonly List<Vector2> _positions;
        private readonly float _epsilon;

        public PositionsAttachRestriction(float epsilon, params Vector2[] positions)
        {
            _epsilon = epsilon;
            _positions = new List<Vector2>(positions);
        }

        public override bool CanAttach(Block block)
        {
            foreach (var position in _positions)    
            {
                if (Vector2.Distance(block.Position, position) <= _epsilon)
                {
                    return true;
                }
            }

            return false;
        }
    }
}