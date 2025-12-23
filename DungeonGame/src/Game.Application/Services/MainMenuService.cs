using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.Game.Infrastructure.Repository.Interface;
using DungeonGame.src.Game.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGame.src.Game.Application.Services
{
    public class MainMenuService : IMainMenuService
    {
        private readonly ILevelRepository levelRepository;
        private readonly ISaveGameRepository saveGameRepository;
        private GameSession currentGameSession;
        private GameEngine currentGameEngine;
        private string currentLevelId;

        public MainMenuService(
            ILevelRepository levelRepository,
            ISaveGameRepository saveGameRepository)
        {
            this.levelRepository = levelRepository ?? throw new ArgumentNullException(nameof(levelRepository));
            this.saveGameRepository = saveGameRepository ?? throw new ArgumentNullException(nameof(saveGameRepository));
        }

        public List<LevelInfoDTO> GetAvailableLevels()
        {
            try
            {
                var levels = levelRepository.GetAll();
                var result = new List<LevelInfoDTO>();

                int index = 1;
                foreach (var level in levels)
                {
                    result.Add(new LevelInfoDTO
                    {
                        LevelId = $"level_{index:000}",
                        DisplayName = $"Level {index}",
                        SourceType = LevelSourceType.BuiltIn
                    });
                    index++;
                }

                // Добавляем тестовый уровень, если нет сохраненных
                if (!result.Any())
                {
                    result.Add(new LevelInfoDTO
                    {
                        LevelId = "test_level",
                        DisplayName = "Test Level",
                        SourceType = LevelSourceType.BuiltIn
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error loading levels: {ex.Message}");
                return new List<LevelInfoDTO>();
            }
        }

        public List<SaveInfoDTO> GetAvailibleSaves()
        {
            var saves = new List<SaveInfoDTO>();

            try
            {
                if (saveGameRepository.HasSave())
                {
                    saves.Add(new SaveInfoDTO
                    {
                        SaveId = "latest_save",
                        LevelName = "Last Game",
                        SavedAt = DateTime.Now // TODO: хранить дату сохранения в репозитории
                    });
                }

                // Также можно добавлять автосохранения и т.д.
                return saves;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading saves: {ex.Message}");
                return saves;
            }
        }

        public AppState StartNewGame(string levelId)
        {
            try
            {
                GameSession gameSession;

                if (levelId == "test_level")
                {
                    // Создаем тестовый уровень
                    gameSession = CreateTestLevel();
                }
                else
                {
                    // Загружаем из репозитория
                    gameSession = levelRepository.Get(levelId);
                }

                currentLevelId = levelId;
                currentGameSession = gameSession;
                currentGameEngine = new GameEngine(gameSession);

                return new AppState
                {
                    CurrentState = ApplicationState.Playing,
                    GameState = MapGameStateToDTO(gameSession)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting game: {ex.Message}");
                return new AppState
                {
                    CurrentState = ApplicationState.MainMenu,
                    ErrorMessage = $"Failed to load level: {ex.Message}"
                };
            }
        }

        public AppState ContinueGame(string saveId)
        {
            try
            {
                if (!saveGameRepository.HasSave())
                {
                    return new AppState
                    {
                        CurrentState = ApplicationState.MainMenu,
                        ErrorMessage = "No save game found"
                    };
                }

                var gameSession = saveGameRepository.Load();
                currentGameSession = gameSession;
                currentGameEngine = new GameEngine(gameSession);
                currentLevelId = "loaded_save";

                return new AppState
                {
                    CurrentState = ApplicationState.Playing,
                    GameState = MapGameStateToDTO(gameSession)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error continuing game: {ex.Message}");
                return new AppState
                {
                    CurrentState = ApplicationState.MainMenu,
                    ErrorMessage = $"Failed to load save: {ex.Message}"
                };
            }
        }

        public AppState OpenEditor(string levelId)
        {
            try
            {
                GameSession gameSession;

                if (string.IsNullOrEmpty(levelId) || levelId == "new")
                {
                    // Создаем пустой уровень для редактирования
                    gameSession = CreateEmptyLevel();
                }
                else
                {
                    // Загружаем существующий уровень для редактирования
                    gameSession = levelRepository.Get(levelId);
                }

                currentGameSession = gameSession;
                currentLevelId = levelId;

                var editorState = new EditorStateDTO
                {
                    MapWidth = gameSession.Map.Width,
                    MapHeight = gameSession.Map.Height,
                    Tiles = ExtractTilesFromSession(gameSession),
                    SelectedEntityType = EntityVisualType.Player
                };

                return new AppState
                {
                    CurrentState = ApplicationState.Editing,
                    EditorState = editorState
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening editor: {ex.Message}");
                return new AppState
                {
                    CurrentState = ApplicationState.MainMenu,
                    ErrorMessage = $"Failed to open editor: {ex.Message}"
                };
            }
        }

        public AppState ExitApp()
        {
            // Сохраняем текущую игру если нужно
            if (currentGameSession != null && currentGameSession.Status == GameStatus.Playing)
            {
                saveGameRepository.Save(currentGameSession);
            }

            return new AppState
            {
                CurrentState = ApplicationState.Exiting
            };
        }

        // Вспомогательные методы
        private GameSession CreateTestLevel()
        {
            var map = new MapObject(10, 10);
            var entityFactory = new HardcodedEntityFactory();

            // Создаем игрока
            var playerCell = map.GetCell(1, 1);
            var player = entityFactory.Create(EntityType.Player, playerCell);

            var entities = new List<Entity> { player };

            // Создаем стены по краям
            for (int x = 0; x < 10; x++)
            {
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(x, 0)));
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(x, 9)));
            }
            for (int y = 1; y < 9; y++)
            {
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(0, y)));
                entities.Add(entityFactory.Create(EntityType.Wall, map.GetCell(9, y)));
            }

            // Добавляем кристаллы
            entities.Add(entityFactory.Create(EntityType.Crystal, map.GetCell(3, 3)));
            entities.Add(entityFactory.Create(EntityType.Crystal, map.GetCell(6, 6)));

            // Добавляем врага
            entities.Add(entityFactory.Create(EntityType.Enemy, map.GetCell(7, 2)));

            // Добавляем выход
            entities.Add(entityFactory.Create(EntityType.Exit, map.GetCell(8, 8)));

            return new GameSession(map, player, entities);
        }

        private GameSession CreateEmptyLevel()
        {
            var map = new MapObject(15, 15);

            // Создаем стены по краям
            var entityFactory = new HardcodedEntityFactory();
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

        private GameStateDTO MapGameStateToDTO(GameSession session)
        {
            return new GameStateDTO
            {
                MapWidth = session.Map.Width,
                MapHeight = session.Map.Height,
                Tiles = ExtractTilesFromSession(session),
                PlayerState = new PlayerStateDTO
                {
                    Health = session.Player.GetHealth,
                    MaxHealth = session.Player.GetMaxHealth,
                    FacingDirection = session.Player.FacingDirection
                },
                CollectedCrystals = session.CollectedCrystals,
                RequiredCrystals = CalculateRequiredCrystals(session),
                GameStatus = session.Status
            };
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

        private int CalculateRequiredCrystals(GameSession session)
        {
            // Считаем общее количество кристаллов на уровне
            return session.GetEntities().Count(e => e.EntityType == EntityType.Crystal)
                   + session.CollectedCrystals;
        }
    }
}