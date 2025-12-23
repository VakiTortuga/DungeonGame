using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Application.enumerations;
using System.Collections.Generic;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class GameEngineTests
    {
        private GameSession CreateTestSession()
        {
            var map = new MapObject(10, 10);

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );

            var player = new Entity(1, EntityType.Player, FacingDirection.Right, map.GetCell(5, 5), playerBehaviors);
            map.GetCell(5, 5).PlaceEntity(player);

            var entities = new List<Entity> { player };

            return new GameSession(map, player, entities);
        }

        [TestMethod]
        public void Constructor_ValidSession_CreatesEngine()
        {
            // Arrange
            var session = CreateTestSession();

            // Act
            var engine = new GameEngine(session);

            // Assert
            Assert.IsNotNull(engine);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Constructor_NullSession_ThrowsException()
        {
            // Arrange & Act & Assert
            var engine = new GameEngine(null);
        }

        [TestMethod]
        public void PlayerMove_GamePlaying_MovesPlayer()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );

            var player = new Entity(1, EntityType.Player, FacingDirection.Right, map.GetCell(5, 5), playerBehaviors);
            map.GetCell(5, 5).PlaceEntity(player);

            var session = new GameSession(map, player, new List<Entity> { player });
            var engine = new GameEngine(session);

            // Act
            engine.PlayerMove(FacingDirection.Right);

            // Assert
            Assert.AreEqual(map.GetCell(6, 5), player.Location);
        }

        [TestMethod]
        public void PlayerMove_GameNotPlaying_DoesNothing()
        {
            // Arrange
            var session = CreateTestSession();
            var engine = new GameEngine(session);

            // Симулируем поражение
            session.Player.TakeDamage(100);
            session.UpdateStatus();

            var originalLocation = session.Player.Location;

            // Act
            engine.PlayerMove(FacingDirection.Right);

            // Assert
            Assert.AreEqual(originalLocation, session.Player.Location);
        }

        [TestMethod]
        public void PlayerAttack_GamePlaying_AttacksEnemy()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );

            var enemyBehaviors = new Entity.Behaviors(
                health: new BasicHealth(50),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var player = new Entity(1, EntityType.Player, FacingDirection.Right, map.GetCell(5, 5), playerBehaviors);
            var enemy = new Entity(2, EntityType.Enemy, FacingDirection.Left, map.GetCell(6, 5), enemyBehaviors);

            map.GetCell(5, 5).PlaceEntity(player);
            map.GetCell(6, 5).PlaceEntity(enemy);

            var session = new GameSession(map, player, new List<Entity> { player, enemy });
            var engine = new GameEngine(session);

            // Act
            engine.PlayerAttack();

            // Assert
            Assert.AreEqual(40, enemy.GetHealth);
        }

        [TestMethod]
        public void GetStatus_ReturnsCurrentGameStatus()
        {
            // Arrange
            var session = CreateTestSession();
            var engine = new GameEngine(session);

            // Act
            var status = engine.GetStatus();

            // Assert
            Assert.AreEqual(GameStatus.Playing, status);
        }

        [TestMethod]
        public void AI_MovesEnemyTowardsPlayer()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );

            var enemyBehaviors = new Entity.Behaviors(
                health: new BasicHealth(50),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(5),
                isCollector: false,
                isCollectable: false
            );

            var player = new Entity(1, EntityType.Player, FacingDirection.Right, map.GetCell(7, 5), playerBehaviors);
            var enemy = new Entity(2, EntityType.Enemy, FacingDirection.Left, map.GetCell(5, 5), enemyBehaviors);

            map.GetCell(7, 5).PlaceEntity(player);
            map.GetCell(5, 5).PlaceEntity(enemy);

            var session = new GameSession(map, player, new List<Entity> { player, enemy });
            var engine = new GameEngine(session);

            // Act - Вызываем движение игрока, чтобы активировать AI
            engine.PlayerMove(FacingDirection.Right);

            // Assert - Враг должен был сдвинуться к игроку
            Assert.AreEqual(map.GetCell(6, 5), enemy.Location);
        }

        [TestMethod]
        public void AI_EnemyAttacksWhenAdjacent()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );

            var enemyBehaviors = new Entity.Behaviors(
                health: new BasicHealth(50),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(20),
                isCollector: false,
                isCollectable: false
            );

            var player = new Entity(1, EntityType.Player, FacingDirection.Right, map.GetCell(6, 5), playerBehaviors);
            var enemy = new Entity(2, EntityType.Enemy, FacingDirection.Right, map.GetCell(5, 5), enemyBehaviors);

            map.GetCell(6, 5).PlaceEntity(player);
            map.GetCell(5, 5).PlaceEntity(enemy);

            var session = new GameSession(map, player, new List<Entity> { player, enemy });
            var engine = new GameEngine(session);

            // Act - Враг рядом с игроком и должен атаковать
            engine.PlayerMove(FacingDirection.Right);

            // Assert - Игрок должен получить урон
            Assert.AreEqual(80, player.GetHealth);
        }
    }
}