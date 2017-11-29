namespace RoundOne
{
    internal interface ISearchHeuristic<T>
    {
        double EstimateSearchCost(T startNode, T targetNode);
    }
}