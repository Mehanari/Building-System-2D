namespace BuildingSystem2D.Core.Restrictions.AttachRestrictions
{
    /// <summary>
    /// Used in Builder to define rules of attaching new blocks to a construction.
    /// You are free to create your own restrictions by extending this class.
    /// </summary>
    public abstract class AttachRestriction
    {
        public abstract bool CanAttach(Block block);
    }
}