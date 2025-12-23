using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core
{
    internal class GameSession
    {
        public IMapToEntity Map { get; }
        public Entity Player { get; }
        public int CollectedCrystals { get; private set; }
        public int InitialCrystals { get; private set; }
        public GameStatus Status { get; private set; }

        private readonly List<Entity> entities;

        public GameSession(
            IMapToEntity map,
            Entity player,
            IEnumerable<Entity> entities,
            int collectedCrystals = 0
            )
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            if (collectedCrystals < 0)
                throw new ArgumentOutOfRangeException(nameof(collectedCrystals));


            Map = map;
            Player = player;
            this.entities = new List<Entity>(entities);
            Status = GameStatus.Playing;
            CollectedCrystals = collectedCrystals;
        }

        public void AddEntity(Entity entity)
            => entities.Add(entity);

        public void RemoveEntity(Entity entity)
            => entities.Remove(entity);

        public void CollectCrystal()
            => CollectedCrystals++;

        public void UpdateStatus()
        {
            if (!Player.IsAlive)
            {
                Status = GameStatus.Defeat;
                return;
            }

            // Проверка победы: достиг ли игрок выхода
            var playerCell = Player.Location;
            var entitiesOnCell = GetEntitiesOnCell(playerCell.X, playerCell.Y);

            foreach (var entity in entitiesOnCell)
            {
                if (entity.EntityType == EntityType.Exit)
                {
                    Status = GameStatus.Victory;
                    return;
                }
                else if (entity.EntityType == EntityType.Crystal && entity.IsCollectable)
                {
                    CollectCrystal();
                    RemoveEntity(entity); // Кристалл исчезает после сбора
                }
            }

            // Альтернативная логика победы: собрать все кристаллы
            int totalCrystals = entities.Count(e => e.EntityType == EntityType.Crystal);
            if (totalCrystals == 0 && CollectedCrystals > 0)
            {
                Status = GameStatus.Victory;
            }
        }

        private IEnumerable<Entity> GetEntitiesOnCell(int x, int y)
        {
            return entities.Where(e =>
                e.Location != null &&
                e.Location.X == x &&
                e.Location.Y == y);
        }

        public IEnumerable<Entity> GetEntities()
            => entities.ToList(); // Создаем копию
    }
}
