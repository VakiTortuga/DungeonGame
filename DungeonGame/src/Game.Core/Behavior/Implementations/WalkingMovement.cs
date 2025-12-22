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
    internal class WalkingMovement : IMoveBehavior
    {
        public bool IsMovable => true;

        public bool TryMove(Entity entity, FacingDirection direction)
        {
            if (direction == FacingDirection.None)
            {
                throw new ArgumentOutOfRangeException("WalkingMovement. Entity can not move if direction is none.");
            }

            throw new NotImplementedException();
        }
    }
}
