using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorInterfaces
{
    internal interface IAttackBehavior
    {
        bool CanAttack { get; }
        bool TryAttack(Entity attacker);
    }
}
