using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core
{
    public class GameEngine
    {
        private readonly GameSession session;

        public GameEngine(GameSession session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            this.session = session;
        }

        public void PlayerMove(FacingDirection direction)
        {
            if (session.Status != GameStatus.Playing)
                return;

            session.Player.TryMove(direction);
            UpdateAI();
            session.UpdateStatus();
        }

        public void PlayerAttack()
        {
            if (session.Status != GameStatus.Playing)
                return;

            session.Player.TryAttack();
            UpdateAI();
            session.UpdateStatus();
        }

        private void UpdateAI()
        {
            foreach (var entity in session.GetEntities())
            {
                if (entity.EntityType == EntityType.Enemy && entity.IsAlive)
                {
                    // Простой AI: двигаться к игроку
                    var enemyCell = entity.Location;
                    var playerCell = session.Player.Location;

                    // Определяем направление к игроку
                    int deltaX = playerCell.X - enemyCell.X;
                    int deltaY = playerCell.Y - enemyCell.Y;

                    FacingDirection preferredDirection;

                    // Предпочитаем движение по горизонтали, если возможно
                    if (Math.Abs(deltaX) > Math.Abs(deltaY) && deltaX != 0)
                    {
                        preferredDirection = deltaX > 0 ? FacingDirection.Right : FacingDirection.Left;
                    }
                    else if (deltaY != 0)
                    {
                        preferredDirection = deltaY > 0 ? FacingDirection.Down : FacingDirection.Up;
                    }
                    else
                    {
                        continue; // Игрок на той же клетке
                    }

                    // Пытаемся двигаться
                    entity.TryMove(preferredDirection);

                    // Если можем атаковать - атакуем
                    if (entity.CanAttack)
                    {
                        // Проверяем, рядом ли игрок
                        var adjacentCell = entity.Location.GetMap()
                            .GetCellByDirection(entity.Location, entity.FacingDirection);

                        if (adjacentCell != null && adjacentCell.IsOccupied &&
                            adjacentCell.GetEntity() == session.Player)
                        {
                            entity.TryAttack();
                        }
                    }
                }
            }
        }

        public GameStatus GetStatus()
            => session.Status;
    }
}
