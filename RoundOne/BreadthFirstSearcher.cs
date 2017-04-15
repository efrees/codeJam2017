using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Searchers
{
    internal class BreadthFirstSearcher<T> where T : SearchNode<T>
    {
        public T StartNode { get; }
        public int DepthLimit { get; }
        public Queue<T> SearchQueue { get; } = new Queue<T>();
        public virtual ISet<T> VisitedNodes { get; } = new HashSet<T>();

        public BreadthFirstSearcher(T startNode)
        {
            StartNode = startNode;
        }

        public BreadthFirstSearcher(T startNode, int depthLimit)
        {
            StartNode = startNode;
            DepthLimit = depthLimit;
        }

        public virtual IList<T> GetShortestPathToNode(T targetNode)
        {
            SearchQueue.Enqueue(StartNode);

            while (SearchQueue.Any())
            {
                var currentNode = SearchQueue.Dequeue();

                if (VisitedNodes.Contains(currentNode))
                    continue;

                if (NodeIsBeyondDepthLimit(currentNode))
                    break;

                VisitedNodes.Add(currentNode);

                if (currentNode.Equals(targetNode))
                {
                    return currentNode.GetPathToHere();
                }

                var children = currentNode.GetChildren();

                foreach (var child in children)
                {
                    child.StoreParentNode(currentNode);
                    SearchQueue.Enqueue(child);
                }
            }

            return new List<T>();
        }

        private bool NodeIsBeyondDepthLimit(T currentNode)
        {
            if (DepthLimit == 0)
                return false;

            return currentNode.LengthOfPathToHere > DepthLimit + 1;
        }
    }
}