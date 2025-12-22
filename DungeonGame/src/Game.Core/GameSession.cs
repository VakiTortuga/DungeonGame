using DungeonGame.src.Game.Application.enumerations;
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
        public GameStatus Status { get; private set; }

        private readonly List<Entity> entities;

        public GameSession(
            IMapToEntity map,
            Entity player,
            IEnumerable<Entity> entities)
        {
            Map = map;
            Player = player;
            this.entities = new List<Entity>(entities);
            Status = GameStatus.Playing;
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
                Status = GameStatus.Defeat;
        }

        public IEnumerable<Entity> GetEntities()
            => entities;
    }
}
