using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.Game.Infrastructure.Repository.Interface;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.EntityFactory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGame.src.Game.Application.Services
{
    public class LevelEditorService : ILevelEditorService
    {
        private readonly ILevelRepository levelRepository;
        private GameSession currentSession;
        private EntityVisualType selectedEntityType;
        private string currentLevelId;
        private readonly HardcodedEntityFactory entityFactory;

        public LevelEditorService(
            ILevelRepository levelRepository,
            GameSession initialSession = null)
        {
            this.levelRepository = levelRepository ?? throw new ArgumentNullException(nameof(levelRepository));
            this.entityFactory = new HardcodedEntityFactory();

            if (initialSession != null)
            {
                currentSession = initialSession;
            }
            else
            {
                // Создаем пустую сессию по умолчанию
                currentSession = CreateEmptySession();
            }

            selectedEntityType = EntityVisualType.Player;
            currentLevelId = null;
        }

        public EditorStateDTO CreateNewLevel()
        {
            currentSession = CreateEmptySession();
            currentLevelId = null;
            selectedEntityType = EntityVisualType.Player;

            return GetEditorState();
        }

        public EditorStateDTO LoadLevel(string levelId)
        {
            try
            {
                currentSession = levelRepository.Get(levelId);
                currentLevelId = levelId;
                selectedEntityType = EntityVisualType.Player;

                return GetEditorState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading level: {ex.Message}");
                // Возвращаем новый уровень при ошибке
                return CreateNewLevel();
            }
        }

        public List<AvailableEntityDTO> GetAvailableEntities()
        {
            return new List<AvailableEntityDTO>
            {
                new AvailableEntityDTO { EntityType = EntityVisualType.Player, DisplayName = "Player", IsUnique = true },
                new AvailableEntityDTO { EntityType = EntityVisualType.Enemy, DisplayName = "Enemy", IsUnique = false },
                new AvailableEntityDTO { EntityType = EntityVisualType.Wall, DisplayName = "Wall", IsUnique = false },
                new AvailableEntityDTO { EntityType = EntityVisualType.Trap, DisplayName = "Trap", IsUnique = false },
                new AvailableEntityDTO { EntityType = EntityVisualType.Crystal, DisplayName = "Crystal", IsUnique = false },
                new AvailableEntityDTO { EntityType = EntityVisualType.Exit, DisplayName = "Exit", IsUnique = true }
            };
        }

        public EditorStateDTO PlaceEntity(int x, int y, EntityVisualType entityType)
        {
            // Проверяем границы карты
            if (x < 0 || x >= currentSession.Map.Width || y < 0 || y >= currentSession.Map.Height)
                return GetEditorState();

            var cell = currentSession.Map.GetCell(x, y);
            if (cell == null)
                return GetEditorState();

            // Проверяем уникальность сущности
            if (IsUniqueEntity(entityType) && EntityOfTypeExists(entityType))
            {
                // Для уникальных сущностей удаляем старую и ставим новую
                RemoveExistingEntityOfType(entityType);
            }
            else if (cell.IsOccupied)
            {
                // Если клетка занята - удаляем существующую сущность
                RemoveEntityFromCell(cell);
            }

            // Создаем и размещаем новую сущность
            var coreEntityType = MapVisualTypeToEntityType(entityType);
            var entity = entityFactory.Create(coreEntityType, cell);

            // Добавляем в сессию
            currentSession.AddEntity(entity);

            return GetEditorState();
        }

        public EditorStateDTO RemoveEntity(int x, int y)
        {
            if (x < 0 || x >= currentSession.Map.Width || y < 0 || y >= currentSession.Map.Height)
                return GetEditorState();

            var cell = currentSession.Map.GetCell(x, y);
            if (cell == null || !cell.IsOccupied)
                return GetEditorState();

            RemoveEntityFromCell(cell);
            return GetEditorState();
        }

        public AppState SaveLevelAs(string name)
        {
            try
            {
                // Проверяем валидность уровня
                if (!IsLevelValid())
                {
                    return new AppState
                    {
                        CurrentState = ApplicationState.Editing,
                        ErrorMessage = "Level is not valid: must have exactly one player and one exit"
                    };
                }

                // Генерируем ID или используем существующий
                if (string.IsNullOrEmpty(currentLevelId))
                {
                    currentLevelId = GenerateLevelId(name);
                }

                // Сохраняем уровень
                levelRepository.Save(currentSession);

                return new AppState
                {
                    CurrentState = ApplicationState.MainMenu,
                    SuccessMessage = $"Level '{name}' saved successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving level: {ex.Message}");
                return new AppState
                {
                    CurrentState = ApplicationState.Editing,
                    ErrorMessage = $"Failed to save level: {ex.Message}"
                };
            }
        }

        public EditorStateDTO GetEditorState()
        {
            return new EditorStateDTO
            {
                MapWidth = currentSession.Map.Width,
                MapHeight = currentSession.Map.Height,
                Tiles = ExtractTilesFromSession(currentSession),
                SelectedEntityType = selectedEntityType
            };
        }

        // Вспомогательные методы
        private GameSession CreateEmptySession()
        {
            var map = new MapObject(15, 15);

            // Создаем стены по краям
            var entities = new List<Entity>();
            for (int x = 0; x < 15; x++)
            {
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(x, 0)));
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(x, 14)));
            }
            for (int y = 1; y < 14; y++)
            {
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(0, y)));
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(14, y)));
            }

            // Добавляем игрока по умолчанию
            var playerCell = map.GetCell(1, 1);
            var player = entityFactory.Create(EntityType.Player, playerCell);
            entities.Add(player);

            return new GameSession(map, player, entities);
        }

        private List<TileStateDTO> ExtractTilesFromSession(GameSession session)
        {
            var tiles = new List<TileStateDTO>();

            for (int x = 0; x < session.Map.Width; x++)
            {
                for (int y = 0; y < session.Map.Height; y++)
                {
                    var cell = session.Map.GetCell(x, y);
                    if (cell != null && cell.IsOccupied)
                    {
                        var entity = cell.GetEntity();
                        tiles.Add(new TileStateDTO
                        {
                            X = x,
                            Y = y,
                            EntityType = MapEntityTypeToVisualType(entity.EntityType),
                            FacingDirection = entity.FacingDirection
                        });
                    }
                }
            }

            return tiles;
        }

        private EntityVisualType MapEntityTypeToVisualType(EntityType entityType)
        {
            return entityType switch
            {
                EntityType.Player => EntityVisualType.Player,
                EntityType.Enemy => EntityVisualType.Enemy,
                EntityType.Wall => EntityVisualType.Wall,
                EntityType.Trap => EntityVisualType.Trap,
                EntityType.Crystal => EntityVisualType.Crystal,
                EntityType.Exit => EntityVisualType.Exit,
                _ => EntityVisualType.Empty
            };
        }

        private EntityType MapVisualTypeToEntityType(EntityVisualType visualType)
        {
            return visualType switch
            {
                EntityVisualType.Player => EntityType.Player,
                EntityVisualType.Enemy => EntityType.Enemy,
                EntityVisualType.Wall => EntityType.Wall,
                EntityVisualType.Trap => EntityType.Trap,
                EntityVisualType.Crystal => EntityType.Crystal,
                EntityVisualType.Exit => EntityType.Exit,
                _ => throw new ArgumentException($"Unknown visual type: {visualType}")
            };
        }

        private bool IsUniqueEntity(EntityVisualType visualType)
        {
            return visualType == EntityVisualType.Player || visualType == EntityVisualType.Exit;
        }

        private bool EntityOfTypeExists(EntityVisualType visualType)
        {
            var targetType = MapVisualTypeToEntityType(visualType);
            return currentSession.GetEntities().Any(e => e.EntityType == targetType);
        }

        private void RemoveExistingEntityOfType(EntityVisualType visualType)
        {
            var targetType = MapVisualTypeToEntityType(visualType);
            var entityToRemove = currentSession.GetEntities().FirstOrDefault(e => e.EntityType == targetType);

            if (entityToRemove != null)
            {
                // Удаляем из клетки
                var cell = entityToRemove.Location;
                cell.RemoveEntity();

                // Удаляем из сессии
                currentSession.RemoveEntity(entityToRemove);
            }
        }

        private void RemoveEntityFromCell(ICellToMap cell)
        {
            var entity = cell.GetEntity();
            if (entity != null)
            {
                cell.RemoveEntity();
                currentSession.RemoveEntity(entity);
            }
        }

        private bool IsLevelValid()
        {
            var entities = currentSession.GetEntities().ToList();

            // Должен быть ровно один игрок
            var playerCount = entities.Count(e => e.EntityType == EntityType.Player);
            if (playerCount != 1)
                return false;

            // Должен быть ровно один выход
            var exitCount = entities.Count(e => e.EntityType == EntityType.Exit);
            if (exitCount != 1)
                return false;

            return true;
        }

        private string GenerateLevelId(string name)
        {
            // Убираем недопустимые символы и добавляем timestamp
            var safeName = new string(name.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray());
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{safeName}_{timestamp}";
        }
    }
}