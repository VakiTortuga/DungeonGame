using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class SessionTest
    {
        [TestMethod]
        public void Constructor_ValidParameters_CreatesSession()
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
            var entities = new List<Entity> { player };

            // Act
            var session = new GameSession(map, player, entities);

            // Assert
            Assert.AreEqual(map, session.Map);
            Assert.AreEqual(player, session.Player);
            Assert.AreEqual(GameStatus.Playing, session.Status);
            Assert.AreEqual(0, session.CollectedCrystals);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Constructor_NullMap_ThrowsException()
        {
            // Arrange
            var player = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Right,
                new MapObject(10, 10).GetCell(5, 5),
                new Entity.Behaviors(
                    new BasicHealth(100),
                    new WalkingMovement(),
                    new MeleeAttack(10),
                    true,
                    false
                )
            );

            var entities = new List<Entity> { player };

            // Act & Assert
            var session = new GameSession(null, player, entities);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Constructor_NullPlayer_ThrowsException()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var entities = new List<Entity>();

            // Act & Assert
            var session = new GameSession(map, null, entities);
        }

        [TestMethod]
        public void AddEntity_AddsEntityToList()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var player = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Right,
                map.GetCell(5, 5),
                new Entity.Behaviors(
                    new BasicHealth(100),
                    new WalkingMovement(),
                    new MeleeAttack(10),
                    true,
                    false
                )
            );

            var enemy = new Entity(
                2,
                EntityType.Enemy,
                FacingDirection.Left,
                map.GetCell(7, 5),
                new Entity.Behaviors(
                    new BasicHealth(50),
                    new WalkingMovement(),
                    new MeleeAttack(5),
                    false,
                    false
                )
            );

            var session = new GameSession(map, player, new List<Entity> { player });

            // Act
            session.AddEntity(enemy);

            // Assert
            Assert.AreEqual(2, session.GetEntities().Count());
        }

        [TestMethod]
        public void RemoveEntity_RemovesEntityFromList()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var player = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Right,
                map.GetCell(5, 5),
                new Entity.Behaviors(
                    new BasicHealth(100),
                    new WalkingMovement(),
                    new MeleeAttack(10),
                    true,
                    false
                )
            );

            var enemy = new Entity(
                2,
                EntityType.Enemy,
                FacingDirection.Left,
                map.GetCell(7, 5),
                new Entity.Behaviors(
                    new BasicHealth(50),
                    new WalkingMovement(),
                    new MeleeAttack(5),
                    false,
                    false
                )
            );

            var session = new GameSession(map, player, new List<Entity> { player, enemy });

            // Act
            session.RemoveEntity(enemy);

            // Assert
            Assert.AreEqual(1, session.GetEntities().Count());
            Assert.AreEqual(player, session.GetEntities().First());
        }

        [TestMethod]
        public void CollectCrystal_IncreasesCollectedCrystals()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var player = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Right,
                map.GetCell(5, 5),
                new Entity.Behaviors(
                    new BasicHealth(100),
                    new WalkingMovement(),
                    new MeleeAttack(10),
                    true,
                    false
                )
            );

            var session = new GameSession(map, player, new List<Entity> { player });

            // Act
            session.CollectCrystal();
            session.CollectCrystal();

            // Assert
            Assert.AreEqual(2, session.CollectedCrystals);
        }

        [TestMethod]
        public void UpdateStatus_PlayerDead_SetsDefeat()
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
            var session = new GameSession(map, player, new List<Entity> { player });

            // Act
            player.TakeDamage(100);
            session.UpdateStatus();

            // Assert
            Assert.AreEqual(GameStatus.Defeat, session.Status);
        }

        [TestMethod]
        public void UpdateStatus_PlayerOnExit_SetsVictory()
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

            var exitBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var exitCell = map.GetCell(5, 5);
            var player = new Entity(1, EntityType.Player, FacingDirection.Right, exitCell, playerBehaviors);
            var exit = new Entity(2, EntityType.Exit, FacingDirection.None, exitCell, exitBehaviors);

            exitCell.PlaceEntity(exit);

            var session = new GameSession(map, player, new List<Entity> { player, exit });

            // Act
            session.UpdateStatus();

            // Assert
            Assert.AreEqual(GameStatus.Victory, session.Status);
        }

        [TestMethod]
        public void GetEntities_ReturnsCopyOfEntities()
        {
            // Arrange
            var map = new MapObject(10, 10);

            var player = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Right,
                map.GetCell(5, 5),
                new Entity.Behaviors(
                    new BasicHealth(100),
                    new WalkingMovement(),
                    new MeleeAttack(10),
                    true,
                    false
                )
            );

            var enemy = new Entity(
                2,
                EntityType.Enemy,
                FacingDirection.Left,
                map.GetCell(7, 5),
                new Entity.Behaviors(
                    new BasicHealth(50),
                    new WalkingMovement(),
                    new MeleeAttack(5),
                    false,
                    false
                )
            );

            var session = new GameSession(map, player, new List<Entity> { player, enemy });

            // Act
            var entities1 = session.GetEntities();
            var entities2 = session.GetEntities();

            // Assert
            Assert.AreNotSame(entities1, entities2); // Должны быть разные экземпляры
            Assert.AreEqual(2, entities1.Count());
            Assert.AreEqual(2, entities2.Count());
        }

        [TestMethod]
        public void UpdateStatus_CollectsCrystalAndRemovesIt()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var crystalCell = map.GetCell(5, 5);

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );

            var crystalBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: true
            );

            var player = new Entity(1, EntityType.Player, FacingDirection.Right, crystalCell, playerBehaviors);
            var crystal = new Entity(2, EntityType.Crystal, FacingDirection.None, crystalCell, crystalBehaviors);

            crystalCell.PlaceEntity(crystal);

            var session = new GameSession(map, player, new List<Entity> { player, crystal });

            // Act
            session.UpdateStatus();

            // Assert
            Assert.AreEqual(1, session.CollectedCrystals);
            Assert.AreEqual(1, session.GetEntities().Count()); // Кристалл должен быть удалён
            Assert.AreEqual(player, session.GetEntities().First());
        }

        [TestMethod]
        public void UpdateStatus_AllCrystalsCollected_SetsVictory()
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

            // Сессия без кристаллов, но с собранными кристаллами
            var session = new GameSession(map, player, new List<Entity> { player }, collectedCrystals: 3);

            // Act
            session.UpdateStatus();

            // Assert
            Assert.AreEqual(GameStatus.Victory, session.Status);
        }
    }
}