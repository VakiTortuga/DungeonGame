using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Application.enumerations;
using System;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class EntityTests
    {
        private Entity.Behaviors CreateValidBehaviors()
        {
            return new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: true,
                isCollectable: false
            );
        }

        [TestMethod]
        public void Constructor_ValidParameters_CreatesEntity()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();

            // Act
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Assert
            Assert.AreEqual(1, entity.ID);
            Assert.AreEqual(EntityType.Player, entity.EntityType);
            Assert.AreEqual(FacingDirection.Right, entity.FacingDirection);
            Assert.AreEqual(cell, entity.Location);
            Assert.IsTrue(entity.IsAlive);
            Assert.IsTrue(entity.IsMovable);
            Assert.IsTrue(entity.CanAttack);
            Assert.IsTrue(entity.IsCollector);
            Assert.IsFalse(entity.IsCollectable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_InvalidID_ThrowsException()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();

            // Act & Assert
            var entity = new Entity(0, EntityType.Player, FacingDirection.Right, cell, behaviors);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullBehaviors_ThrowsException()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);

            // Act & Assert
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullLocation_ThrowsException()
        {
            // Arrange
            var behaviors = CreateValidBehaviors();

            // Act & Assert
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, null, behaviors);
        }

        [TestMethod]
        public void TryMove_AliveEntity_DelegatesToMovementBehavior()
        {
            // Arrange
            var map = new MapObject(5, 5);
            var startCell = map.GetCell(2, 2);
            var targetCell = map.GetCell(2, 1);

            var behaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, startCell, behaviors);
            startCell.PlaceEntity(entity);

            // Act
            bool result = entity.TryMove(FacingDirection.Up);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(targetCell, entity.Location);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TryMove_NoneDirection_ThrowsException()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Act & Assert
            entity.TryMove(FacingDirection.None);
        }

        [TestMethod]
        public void TryMove_DeadEntity_ReturnsFalse()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var health = new BasicHealth(100);
            health.TakeDamage(100);

            var behaviors = new Entity.Behaviors(
                health: health,
                movement: new WalkingMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Act
            bool result = entity.TryMove(FacingDirection.Right);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryAttack_AliveEntity_DelegatesToAttackBehavior()
        {
            // Arrange
            var map = new MapObject(5, 5);
            var attackerCell = map.GetCell(2, 2);
            var targetCell = map.GetCell(3, 2);

            var attackerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new WalkingMovement(),
                attack: new MeleeAttack(10),
                isCollector: false,
                isCollectable: false
            );

            var targetBehaviors = new Entity.Behaviors(
                health: new BasicHealth(50),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var attacker = new Entity(1, EntityType.Player, FacingDirection.Right, attackerCell, attackerBehaviors);
            var target = new Entity(2, EntityType.Enemy, FacingDirection.Left, targetCell, targetBehaviors);

            attackerCell.PlaceEntity(attacker);
            targetCell.PlaceEntity(target);

            // Act
            bool result = attacker.TryAttack();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(40, target.GetHealth);
        }

        [TestMethod]
        public void TakeDamage_ValidAmount_DelegatesToHealthBehavior()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Act
            bool result = entity.TakeDamage(30);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(70, entity.GetHealth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeDamage_InvalidAmount_ThrowsException()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Act & Assert
            entity.TakeDamage(-10);
        }

        [TestMethod]
        public void ChangeFacingDirection_ValidDirection_UpdatesDirection()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Act
            entity.ChangeFacingDirection(FacingDirection.Down);

            // Assert
            Assert.AreEqual(FacingDirection.Down, entity.FacingDirection);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ChangeFacingDirection_NoneDirection_ThrowsException()
        {
            // Arrange
            var map = new MapObject(10, 10);
            var cell = map.GetCell(0, 0);
            var behaviors = CreateValidBehaviors();
            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, behaviors);

            // Act & Assert
            entity.ChangeFacingDirection(FacingDirection.None);
        }
    }
}