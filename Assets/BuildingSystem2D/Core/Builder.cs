using System.Collections.Generic;
using BuildingSystem2D.Core.Restrictions.AttachRestrictions;
using BuildingSystem2D.Core.Restrictions.RemoveRestrictions;

namespace BuildingSystem2D.Core
{
    /// <summary>
    /// Allows you to apply restrictions to a process of building a construction.
    /// </summary>
    public class Builder
    {
        public List<AttachRestriction> AttachRestrictions { get; set; } = new();
        public List<RemoveRestriction> RemoveRestrictions { get; set; } = new();
        
        private readonly Construction _construction;

        public Construction Construction => _construction;

        public Builder()
        {
            _construction = new Construction();
        }

        public Builder(Construction construction)
        {
            _construction = construction;
        }

        public bool CanAttach(Block block)
        {
            foreach (var restriction in AttachRestrictions)
            {
                if (!restriction.CanAttach(block))
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryAttach(Block block)
        {
            if (!CanAttach(block))
            {
                return false;
            }
            _construction.AddBlock(block);
            return true;
        }

        public bool CanRemove(Block block)
        {
            foreach (var restriction in RemoveRestrictions)
            {
                if (!restriction.CanRemove(block))
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryRemove(Block block)
        {
            if (!CanRemove(block))
            {
                return false;
            }
            _construction.RemoveBlock(block);
            return true;
        }
    }
}