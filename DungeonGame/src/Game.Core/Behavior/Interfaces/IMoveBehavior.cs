using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorInterfaces
{
    public interface IMoveBehavior
    {
        bool IsMovable { get; }
        bool TryMove(Entity entity, FacingDirection facingDirection);
    }
}
