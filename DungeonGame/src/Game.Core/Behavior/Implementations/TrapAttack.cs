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
    internal class TrapAttack : IAttackBehavior
    {
        public bool CanAttack => true;

        public bool TryAttack(Entity attacker)
        {
            // Ловушка атакует все сущности на соседних клетках
            var currentCell = attacker.Location;
            var map = currentCell.GetMap();

            bool attacked = false;

            // Проверяем все 4 направления
            var directions = new[] {
        FacingDirection.Up,
        FacingDirection.Down,
        FacingDirection.Left,
        FacingDirection.Right
    };

            foreach (var direction in directions)
            {
                var targetCell = map.GetCellByDirection(currentCell, direction);

                if (targetCell != null && targetCell.IsOccupied)
                {
                    var targetEntity = targetCell.GetEntity();

                    // Ловушка может атаковать игрока и врагов
                    if (targetEntity.EntityType == EntityType.Player ||
                        targetEntity.EntityType == EntityType.Enemy)
                    {
                        targetEntity.TakeDamage(5); // Урон от ловушки
                        attacked = true;
                    }
                }
            }

            return attacked;
        }
    }
}
