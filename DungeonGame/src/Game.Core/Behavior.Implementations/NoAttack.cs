using DungeonGame.src.Game.Core.BehaviorInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorImplementations
{
    internal class NoAttack : IAttackBehavior
    {
        public bool CanAttack => false;

        public bool TryAttack(Entity attacker)
        {
            return false;
        }
    }
}
