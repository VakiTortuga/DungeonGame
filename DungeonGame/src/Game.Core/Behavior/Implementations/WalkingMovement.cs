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
    public class WalkingMovement : IMoveBehavior
    {
        public bool IsMovable => true;

        public bool TryMove(Entity entity, FacingDirection direction)
        {
            if (direction == FacingDirection.None)
                throw new ArgumentOutOfRangeException("WalkingMovement. Entity can not move if direction is none.");

            // Реализация:
            var currentCell = entity.Location;
            var map = currentCell.GetMap();
            var targetCell = map.GetCellByDirection(currentCell, direction);

            if (targetCell == null || targetCell.IsOccupied)
                return false;

            // Проверка, можно ли войти в клетку (например, не в стену)
            // Логика движения...

            return map.TryMoveEntity(currentCell, targetCell);
        }
    }
}
