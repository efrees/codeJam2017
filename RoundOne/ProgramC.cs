using System;
using System.Collections.Generic;

namespace RoundOne
{
    internal class ProgramC
    {
        static void MainC(string[] args)
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
                GetBuffChild(),
                GetCureChild(),
                GetDebuffChild()
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
                KnightHealth = KnightHealth - DragonAttack,
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
                       && KnightAttack.Equals(otherNode.KnightAttack);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return ((DragonHealth * 71 + DragonAttack) * 71 + KnightHealth) * 71 + KnightAttack;
        }
    }
}
