using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.Game.Infrastructure.Repository.Interface;
using DungeonGame.src.Game.Core;
using System;

namespace DungeonGame.src.Game.Application.Services
{
    public class GameSessionService : IGameSessionService
    {
        private readonly ISaveGameRepository saveGameRepository;
        private GameEngine gameEngine;
        private GameSession gameSession;
        private bool isPaused;

        public GameSessionService(
            GameSession initialSession,
            ISaveGameRepository saveGameRepository)
        {
            this.gameSession = initialSession ?? throw new ArgumentNullException(nameof(initialSession));
            this.saveGameRepository = saveGameRepository ?? throw new ArgumentNullException(nameof(saveGameRepository));
            this.gameEngine = new GameEngine(gameSession);
            this.isPaused = false;
        }

        public GameStateDTO MovePlayer(FacingDirection direction)
        {
            if (isPaused || gameSession.Status != GameStatus.Playing)
                return GetGameState();

            try
            {
                gameEngine.PlayerMove(direction);
                return GetGameState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving player: {ex.Message}");
                return GetGameState();
            }
        }

        public GameStateDTO PlayerAttack()
        {
            if (isPaused || gameSession.Status != GameStatus.Playing)
                return GetGameState();

            try
            {
                gameEngine.PlayerAttack();
                return GetGameState();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error performing attack: {ex.Message}");
                return GetGameState();
            }
        }

        public GameStateDTO PauseGame()
        {
            isPaused = true;
            return GetGameState();
        }

        public GameStateDTO ResumeGame()
        {
            isPaused = false;
            return GetGameState();
        }

        public AppState SaveAndExit()
        {
            try
            {
                if (gameSession.Status == GameStatus.Playing)
                {
                    saveGameRepository.Save(gameSession);
                }

                return new AppState
                {
                    CurrentState = ApplicationState.MainMenu
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
                return new AppState
                {
                    CurrentState = ApplicationState.Playing,
                    ErrorMessage = $"Failed to save: {ex.Message}"
                };
            }
        }

        public GameStateDTO GetGameState()
        {
            var tiles = ExtractTilesFromSession(gameSession);

            return new GameStateDTO
            {
                MapWidth = gameSession.Map.Width,
                MapHeight = gameSession.Map.Height,
                Tiles = tiles,
                PlayerState = new PlayerStateDTO
                {
                    Health = gameSession.Player.GetHealth,
                    MaxHealth = gameSession.Player.GetMaxHealth,
                    FacingDirection = gameSession.Player.FacingDirection
                },
                CollectedCrystals = gameSession.CollectedCrystals,
                RequiredCrystals = CalculateRequiredCrystals(gameSession),
                GameStatus = gameSession.Status
            };
        }

        // Вспомогательные методы
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
            // Общее количество кристаллов = собранные + оставшиеся на карте
            var remainingCrystals = 0;
            foreach (var entity in session.GetEntities())
            {
                if (entity.EntityType == EntityType.Crystal)
                {
                    remainingCrystals++;
                }
            }
            return session.CollectedCrystals + remainingCrystals;
        }
    }
}