using BuildingSystem2D.Core.Algorithms;

namespace BuildingSystem2D.Core.Restrictions.RemoveRestrictions
{
    public class ConnectivityRestriction : RemoveRestriction
    {
        private readonly ConstructionGraph _graph;
        private readonly Block _root;

        public ConnectivityRestriction(Block root, ConstructionGraph graph)
        {
            _root = root;
            _graph = graph;
        }

        public override bool CanRemove(Block block)
        {
            var connectivityChecker = new ConnectivityChecker(_graph.Graph, _root);
            return connectivityChecker.CanRemoveBlock(block);
        }
    }
}