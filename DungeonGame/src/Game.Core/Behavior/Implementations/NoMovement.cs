using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.BehaviorInterfaces;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorImplementations
{
    public class NoMovement : IMoveBehavior
    {
        public bool IsMovable => false;

        public bool TryMove(Entity entity, FacingDirection direction)
        {
            return false;
        }
    }
}
