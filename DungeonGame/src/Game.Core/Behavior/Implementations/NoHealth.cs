using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Core.BehaviorInterfaces;

namespace DungeonGame.src.Game.Core.BehaviorImplementations
{
    public class NoHealth : IHealthBehavior
    {
        public bool IsAlive => true;

        public int GetHealth => 1;
        public int GetMaxHealth => 1;

        public bool Heal(int amount)
        {
            return false;
        }

        public bool TakeDamage(int amount)
        {
            return false;
        }
    }
}
