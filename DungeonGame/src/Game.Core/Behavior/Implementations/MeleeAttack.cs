using DungeonGame.src.Game.Core.BehaviorInterfaces;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorImplementations
{
    public class MeleeAttack : IAttackBehavior
    {
        private readonly int damage;

        public bool CanAttack => true;

        public MeleeAttack(int damage)
        {
            if (damage < 1)
            {
                throw new ArgumentOutOfRangeException(
                    "MeleeAttack. Damage must be greater than 0."
                    );
            }
            this.damage = damage;
        }

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
                targetEntity.TakeDamage(damage);
                return true;
            }

            return false;
        }
    }
}
