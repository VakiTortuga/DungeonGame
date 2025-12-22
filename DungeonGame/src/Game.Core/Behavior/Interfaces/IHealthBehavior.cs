using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorInterfaces
{
    internal interface IHealthBehavior
    {
        bool IsAlive { get; }
        int GetHealth { get; }
        int GetMaxHealth { get; }
        bool TakeDamage(int amount);
        bool Heal(int amount);
    }
}
