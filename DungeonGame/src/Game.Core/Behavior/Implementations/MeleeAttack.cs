using DungeonGame.src.Game.Core.BehaviorInterfaces;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorImplementations
{
    internal class MeleeAttack : IAttackBehavior
    {
        public bool CanAttack => true;

        public bool TryAttack(Entity attacker)
        {
            var currentCell = attacker.Location;
            var map = currentCell.GetMap();
            var direction = attacker.FacingDirection;
            var targetCell = map.GetCellByDirection(currentCell, direction);

            if (targetCell == null || !targetCell.IsOccupied)
                return false;

            var targetEntity = targetCell.GetEntity();

            // Проверка, можно ли атаковать эту сущность
            if (targetEntity.EntityType == EntityType.Enemy ||
                targetEntity.EntityType == EntityType.Player)
            {
                // Нанести урон
                targetEntity.TakeDamage(10); // Примерный урон
                return true;
            }

            return false;
        }
    }
}
