using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

namespace RoundOne
{
    internal class AStarSearcher<T> where T : SearchNode<T>
    {
        private readonly ISearchHeuristic<T> _searchHeuristic;
        public T StartNode { get; }
        public int DepthLimit { get; }
        //OptimizedPriorityQueue on NuGet.org
        public IPriorityQueue<T, float> SearchQueue { get; } = new SimplePriorityQueue<T>();

        public AStarSearcher(T startNode, ISearchHeuristic<T> searchHeuristic)
        {
            _searchHeuristic = searchHeuristic;
            StartNode = startNode;
        }

        public virtual IList<T> GetShortestPathToNode(T targetNode)
        {
            SearchQueue.Enqueue(StartNode, (float)EstimateCost(StartNode, targetNode));

            while (SearchQueue.Any())
            {
                var currentNode = SearchQueue.Dequeue();

                if (NodeIsBeyondDepthLimit(currentNode))
                    break;

                if (currentNode.Equals(targetNode))
                {
                    return currentNode.GetPathToHere();
                }

                var children = currentNode.GetChildren();

                foreach (var child in children)
                {
                    child.StoreParentNode(currentNode);
                    var priority = child.LengthOfPathToHere + EstimateCost(child, targetNode);
                    SearchQueue.Enqueue(child, (float)priority);
                }
            }

            return new List<T>();
        }

        private double EstimateCost(T searchNode, T targetNode)
        {
            return _searchHeuristic.EstimateSearchCost(searchNode, targetNode);
        }

        private bool NodeIsBeyondDepthLimit(T currentNode)
        {
            if (DepthLimit == 0)
                return false;

            return currentNode.LengthOfPathToHere > DepthLimit + 1;
        }
    }
}