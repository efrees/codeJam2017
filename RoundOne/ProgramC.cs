using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundOne
{
    internal class ProgramC
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine();
            var T = Convert.ToInt32(input);

            var k = 1;
            while (k <= T)
            {
                var answer = ProcessTestCase();

                Console.WriteLine($"Case #{k}: {answer}");
                k++;
            }
        }

        internal static string ProcessTestCase()
        {
            var parameters = Console.ReadLine().Split(' ');
            return ProcessTestCaseInternal(parameters);
        }

        internal static string ProcessTestCaseInternal(string[] parameters)
        {
            var startNode = new BattleSearchNode
            {
                DragonHealth = Convert.ToInt32(parameters[0]),
                DragonAttack = Convert.ToInt32(parameters[1]),
                KnightHealth = Convert.ToInt32(parameters[2]),
                KnightAttack = Convert.ToInt32(parameters[3]),
                Buff = Convert.ToInt32(parameters[4]),
                Debuff = Convert.ToInt32(parameters[5]),
            };
            startNode.OriginalDragonHealth = startNode.DragonHealth;

            var target = new BattleSearchNode { KnightHealth = 0 };
            var searcher = new BreadthFirstSearcher<BattleSearchNode>(startNode);
            var path = searcher.GetShortestPathToNode(target);

            if (path.Count == 0)
            {
                return "IMPOSSIBLE";
            }

            //Console.WriteLine(string.Join(", ", path.Select(p => p.ActionName)));
            return (path.Count - 1).ToString();
        }
    }

    internal class BattleSearchNode : SearchNode<BattleSearchNode>
    {
        public override IEnumerable<BattleSearchNode> GetChildren()
        {
            if (DragonHealth <= 0 || KnightHealth <= 0)
                return new BattleSearchNode[0];

            return new[]
            {
                GetAttackChild(),
                GetDebuffChild(),
                GetBuffChild(),
                GetCureChild(),
            };
        }

        private BattleSearchNode GetDebuffChild()
        {
            var debuffResult = new BattleSearchNode
            {
                ActionName = "Debuff",
                OriginalDragonHealth = OriginalDragonHealth,
                DragonHealth = DragonHealth,
                DragonAttack = DragonAttack,
                KnightHealth = KnightHealth,
                KnightAttack = Math.Max(0, KnightAttack - Debuff),
                Buff = Buff,
                Debuff = Debuff
            };

            debuffResult.DragonHealth -= KnightAttack;

            return debuffResult;
        }

        public string ActionName { get; set; }

        private BattleSearchNode GetCureChild()
        {
            var cureResult = new BattleSearchNode
            {
                ActionName = "Cure",
                OriginalDragonHealth = OriginalDragonHealth,
                DragonHealth = OriginalDragonHealth,
                DragonAttack = DragonAttack,
                KnightHealth = KnightHealth,
                KnightAttack = KnightAttack,
                Buff = Buff,
                Debuff = Debuff
            };

            cureResult.DragonHealth -= KnightAttack;

            return cureResult;
        }

        private BattleSearchNode GetBuffChild()
        {
            var buffResult = new BattleSearchNode
            {
                ActionName = "Buff",
                OriginalDragonHealth = OriginalDragonHealth,
                DragonHealth = DragonHealth,
                DragonAttack = DragonAttack + Buff,
                KnightHealth = KnightHealth,
                KnightAttack = KnightAttack,
                Buff = Buff,
                Debuff = Debuff
            };

            buffResult.DragonHealth -= KnightAttack;

            return buffResult;
        }

        private BattleSearchNode GetAttackChild()
        {
            var attackResult = new BattleSearchNode
            {
                ActionName = "Attack",
                OriginalDragonHealth = OriginalDragonHealth,
                DragonHealth = DragonHealth,
                DragonAttack = DragonAttack,
                KnightHealth = Math.Max(0,KnightHealth - DragonAttack),
                KnightAttack = KnightAttack,
                Buff = Buff,
                Debuff = Debuff
            };

            if (attackResult.KnightHealth > 0)
            {
                attackResult.DragonHealth -= KnightAttack;
            }

            return attackResult;
        }

        public int DragonHealth { get; set; }
        public int DragonAttack { get; set; }
        public int KnightHealth { get; set; }
        public int KnightAttack { get; set; }
        public int Buff { get; set; }
        public int Debuff { get; set; }
        public int OriginalDragonHealth { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is BattleSearchNode)
            {
                var otherNode = obj as BattleSearchNode;

                //Make this work to recognize target. Should be cleaned up.
                if (KnightHealth <= 0 && otherNode.KnightHealth <= 0)
                    return true;

                return DragonHealth.Equals(otherNode.DragonHealth)
                       && DragonAttack.Equals(otherNode.DragonAttack)
                       && KnightHealth.Equals(otherNode.KnightHealth)
                       && KnightAttack.Equals(otherNode.KnightAttack)
                       && (ActionName ?? "").Equals(otherNode.ActionName ?? "");
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ((DragonHealth * 71 + DragonAttack) * 71 + KnightHealth) * 71 + KnightAttack + (ActionName ?? "").GetHashCode();
        }
    }

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
