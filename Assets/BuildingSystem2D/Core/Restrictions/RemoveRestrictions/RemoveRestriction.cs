namespace BuildingSystem2D.Core.Restrictions.RemoveRestrictions
{
    /// <summary>
    /// Used in Builder to define rules of removing existing blocks from a construction.
    /// You are free to create your own restrictions by extending this class.
    /// </summary>
    public abstract class RemoveRestriction
    {
        public abstract bool CanRemove(Block block);
    }
}