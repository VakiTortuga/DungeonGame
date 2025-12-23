using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.EntityFactory;
using DungeonGame.src.Game.Core.EntityFactory.Interface;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Infrastructure
{
    public static class SaveGameMapper
    {
        private static IEntityFactory entityFactory;

        public static IEntityFactory EntityFactory { get; set; }
        = new HardcodedEntityFactory();

        public static SaveGameDTO ToDTO(GameSession session)
        {
            return new SaveGameDTO
            {
                MapWidth = session.Map.GetDimensions().width,
                MapHeight = session.Map.GetDimensions().height,
                PlayerId = session.Player.ID,
                CollectedCrystals = session.CollectedCrystals,
                Entities = session.GetEntities().Select(e => new EntitySaveDTO
                {
                    Id = e.ID,
                    Type = e.EntityType,
                    X = e.Location.X,
                    Y = e.Location.Y,
                    Health = e.GetHealth,
                    Facing = e.FacingDirection
                }).ToList()
            };
        }

        public static GameSession FromDTO(SaveGameDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // 1. Создаем карту с заданными размерами
            var map = new MapObject(dto.MapWidth, dto.MapHeight);

            // 2. Создаем временные структуры для хранения данных сущностей
            var entities = new List<Entity>();
            Entity player = null;

            // Сначала создаем все сущности
            foreach (var entityDto in dto.Entities)
            {
                // Получаем клетку для размещения
                var cell = map.GetCell(entityDto.X, entityDto.Y);
                if (cell == null)
                    throw new InvalidOperationException(
                        $"Неверные координаты сущности: X={entityDto.X}, Y={entityDto.Y}. " +
                        $"Допустимый диапазон: 0-{dto.MapWidth - 1}, 0-{dto.MapHeight - 1}");

                // Создаем сущность через фабрику
                var entity = EntityFactory.Create(entityDto.Type, cell);

                // Восстанавливаем направление взгляда
                if (entityDto.Facing != FacingDirection.None)
                {
                    entity.ChangeFacingDirection(entityDto.Facing);
                }

                // Восстанавливаем здоровье
                RestoreEntityHealth(entity, entityDto.Health);

                entities.Add(entity);

                // Сохраняем ссылку на игрока
                if (entityDto.Id == dto.PlayerId)
                {
                    player = entity;
                }
            }

            // Проверяем, что игрок был создан
            if (player == null)
            {
                // Если не нашли по ID, ищем первого игрока по типу
                player = entities.FirstOrDefault(e => e.EntityType == EntityType.Player);
                if (player == null)
                    throw new InvalidOperationException("Игрок не найден в сохранении");
            }

            // Проверяем кол-во собранных кристаллов
            if (dto.CollectedCrystals < 0) 
                throw new ArgumentOutOfRangeException("Кол-во собранных кристаллов не может быть отрицательным.");

            // 3. Создаем сессию
            var session = new GameSession(map, player, entities, dto.CollectedCrystals);

            return session;
        }

        private static void RestoreEntityHealth(Entity entity, int targetHealth)
        {
            // Самый простой способ: вычисляем разницу и применяем урон
            int currentHealth = entity.GetHealth;
            int difference = currentHealth - targetHealth;

            if (difference >= 0)
            {
                // Текущее здоровье больше целевого - наносим урон
                entity.TakeDamage(difference);
            }
            else if (difference < 0)
            {
                throw new NotImplementedException("SaveGameMapper. Can't heal entity.");
            }
        }
    }

}
