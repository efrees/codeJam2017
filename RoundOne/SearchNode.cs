using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2016.Searchers
{
    public abstract class SearchNode<T> where T : SearchNode<T>
    {
        private T _parentNode;

        public abstract IEnumerable<T> GetChildren();

        public int LengthOfPathToHere { get; private set; } = 1;

        public virtual void StoreParentNode(T parentNode)
        {
            _parentNode = parentNode;
            LengthOfPathToHere = parentNode.LengthOfPathToHere + 1;
        }

        public IList<T> GetPathToHere()
        {
            return EnumerateParentsBackToRoot()
                .Reverse()
                .ToList();
        }

        private IEnumerable<T> EnumerateParentsBackToRoot()
        {
            yield return this as T;
            var currentNode = this;
            while (currentNode.HasParentStored())
            {
                yield return currentNode._parentNode;

                currentNode = currentNode._parentNode;
            }
        }

        private bool HasParentStored()
        {
            return _parentNode != null;
        }

        public override bool Equals(object obj)
        {
            if (obj is SearchNode<T>)
                throw new NotImplementedException("A subclass of SearchNode must override Equals() and GetHashCode()");

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException("A subclass of SearchNode must override Equals() and GetHashCode()");
        }
    }
}