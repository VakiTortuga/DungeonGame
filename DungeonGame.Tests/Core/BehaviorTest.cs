using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Application.enumerations;
using System;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class BasicHealthTests
    {
        [TestMethod]
        public void Constructor_ValidMaxHealth_SetsHealthCorrectly()
        {
            // Arrange & Act
            var health = new BasicHealth(100);

            // Assert
            Assert.AreEqual(100, health.GetMaxHealth);
            Assert.AreEqual(100, health.GetHealth);
            Assert.IsTrue(health.IsAlive);
        }

        [TestMethod]
        public void Constructor_ZeroMaxHealth_SetsHealthCorrectly()
        {
            // Arrange & Act
            var health = new BasicHealth(0);

            // Assert
            Assert.AreEqual(0, health.GetMaxHealth);
            Assert.AreEqual(0, health.GetHealth);
            Assert.IsFalse(health.IsAlive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NegativeMaxHealth_ThrowsException()
        {
            // Arrange & Act & Assert
            var health = new BasicHealth(-10);
        }

        [TestMethod]
        public void TakeDamage_AliveEntity_ReducesHealth()
        {
            // Arrange
            var health = new BasicHealth(100);

            // Act
            bool result = health.TakeDamage(30);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(70, health.GetHealth);
            Assert.IsTrue(health.IsAlive);
        }

        [TestMethod]
        public void TakeDamage_LethalDamage_KillsEntity()
        {
            // Arrange
            var health = new BasicHealth(50);

            // Act
            bool result = health.TakeDamage(100);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, health.GetHealth);
            Assert.IsFalse(health.IsAlive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TakeDamage_NegativeAmount_ThrowsException()
        {
            // Arrange
            var health = new BasicHealth(100);

            // Act & Assert
            health.TakeDamage(-10);
        }

        [TestMethod]
        public void TakeDamage_DeadEntity_ReturnsFalse()
        {
            // Arrange
            var health = new BasicHealth(0);

            // Act
            bool result = health.TakeDamage(10);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, health.GetHealth);
            Assert.IsFalse(health.IsAlive);
        }

        [TestMethod]
        public void Heal_AliveEntity_IncreasesHealth()
        {
            // Arrange
            var health = new BasicHealth(100);
            health.TakeDamage(40);

            // Act
            bool result = health.Heal(20);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(80, health.GetHealth);
        }

        [TestMethod]
        public void Heal_Overheal_ClampsToMaxHealth()
        {
            // Arrange
            var health = new BasicHealth(100);
            health.TakeDamage(30);

            // Act
            bool result = health.Heal(50);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(100, health.GetHealth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Heal_NegativeAmount_ThrowsException()
        {
            // Arrange
            var health = new BasicHealth(100);

            // Act & Assert
            health.Heal(-10);
        }

        [TestMethod]
        public void Heal_DeadEntity_ReturnsFalse()
        {
            // Arrange
            var health = new BasicHealth(0);

            // Act
            bool result = health.Heal(10);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, health.GetHealth);
        }
    }

    [TestClass]
    public class MeleeAttackTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_InvalidDamage_ThrowsException()
        {
            // Arrange & Act & Assert
            var attack = new MeleeAttack(0);
        }

        [TestMethod]
        public void Constructor_ValidDamage_CreatesAttack()
        {
            // Arrange & Act
            var attack = new MeleeAttack(10);

            // Assert
            Assert.IsTrue(attack.CanAttack);
        }

        [TestMethod]
        public void TryAttack_TargetInFront_AttacksSuccessfully()
        {
            // Arrange
            var attack = new MeleeAttack(15);
            var map = new MapObject(5, 5);

            var attackerCell = map.GetCell(2, 2);
            var targetCell = map.GetCell(3, 2);

            var attackerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: attack,
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
            bool result = attack.TryAttack(attacker);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(35, target.GetHealth);
        }

        [TestMethod]
        public void TryAttack_EmptyCell_ReturnsFalse()
        {
            // Arrange
            var attack = new MeleeAttack(10);
            var map = new MapObject(5, 5);

            var attackerCell = map.GetCell(2, 2);
            var attackerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var attacker = new Entity(1, EntityType.Player, FacingDirection.Right, attackerCell, attackerBehaviors);
            attackerCell.PlaceEntity(attacker);

            // Act
            bool result = attack.TryAttack(attacker);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryAttack_WallTarget_ReturnsFalse()
        {
            // Arrange
            var attack = new MeleeAttack(10);
            var map = new MapObject(5, 5);

            var attackerCell = map.GetCell(2, 2);
            var wallCell = map.GetCell(3, 2);

            var attackerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var wallBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var attacker = new Entity(1, EntityType.Player, FacingDirection.Right, attackerCell, attackerBehaviors);
            var wall = new Entity(2, EntityType.Wall, FacingDirection.None, wallCell, wallBehaviors);

            attackerCell.PlaceEntity(attacker);
            wallCell.PlaceEntity(wall);

            // Act
            bool result = attack.TryAttack(attacker);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryAttack_OutOfBounds_ReturnsFalse()
        {
            // Arrange
            var attack = new MeleeAttack(10);
            var map = new MapObject(5, 5);

            var attackerCell = map.GetCell(4, 2);
            var attackerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var attacker = new Entity(1, EntityType.Player, FacingDirection.Right, attackerCell, attackerBehaviors);
            attackerCell.PlaceEntity(attacker);

            // Act
            bool result = attack.TryAttack(attacker);

            // Assert
            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class NoAttackTests
    {
        [TestMethod]
        public void CanAttack_AlwaysReturnsFalse()
        {
            // Arrange
            var attack = new NoAttack();

            // Act & Assert
            Assert.IsFalse(attack.CanAttack);
        }

        [TestMethod]
        public void TryAttack_AlwaysReturnsFalse()
        {
            // Arrange
            var attack = new NoAttack();
            var map = new MapObject(5, 5);
            var cell = map.GetCell(2, 2);

            var entityBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, entityBehaviors);

            // Act
            bool result = attack.TryAttack(entity);

            // Assert
            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class NoHealthTests
    {
        [TestMethod]
        public void IsAlive_AlwaysReturnsTrue()
        {
            // Arrange
            var health = new NoHealth();

            // Act & Assert
            Assert.IsTrue(health.IsAlive);
        }

        [TestMethod]
        public void GetHealth_AlwaysReturnsOne()
        {
            // Arrange
            var health = new NoHealth();

            // Act & Assert
            Assert.AreEqual(1, health.GetHealth);
        }

        [TestMethod]
        public void GetMaxHealth_AlwaysReturnsOne()
        {
            // Arrange
            var health = new NoHealth();

            // Act & Assert
            Assert.AreEqual(1, health.GetMaxHealth);
        }

        [TestMethod]
        public void TakeDamage_AlwaysReturnsFalse()
        {
            // Arrange
            var health = new NoHealth();

            // Act
            bool result = health.TakeDamage(100);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, health.GetHealth);
        }

        [TestMethod]
        public void Heal_AlwaysReturnsFalse()
        {
            // Arrange
            var health = new NoHealth();

            // Act
            bool result = health.Heal(100);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(1, health.GetHealth);
        }
    }

    [TestClass]
    public class NoMovementTests
    {
        [TestMethod]
        public void IsMovable_AlwaysReturnsFalse()
        {
            // Arrange
            var movement = new NoMovement();

            // Act & Assert
            Assert.IsFalse(movement.IsMovable);
        }

        [TestMethod]
        public void TryMove_AlwaysReturnsFalse()
        {
            // Arrange
            var movement = new NoMovement();
            var map = new MapObject(5, 5);
            var cell = map.GetCell(2, 2);

            var entityBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: movement,
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, entityBehaviors);

            // Act
            bool result = movement.TryMove(entity, FacingDirection.Right);

            // Assert
            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class TrapAttackTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_InvalidDamage_ThrowsException()
        {
            // Arrange & Act & Assert
            var attack = new TrapAttack(0);
        }

        [TestMethod]
        public void Constructor_ValidDamage_CreatesAttack()
        {
            // Arrange & Act
            var attack = new TrapAttack(10);

            // Assert
            Assert.IsTrue(attack.CanAttack);
        }

        [TestMethod]
        public void TryAttack_TargetsInAllDirections_AttacksAll()
        {
            // Arrange
            var attack = new TrapAttack(15);
            var map = new MapObject(5, 5);

            var trapCell = map.GetCell(2, 2);

            var trapBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var playerBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var enemyBehaviors = new Entity.Behaviors(
                health: new BasicHealth(80),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var trap = new Entity(1, EntityType.Trap, FacingDirection.None, trapCell, trapBehaviors);
            var player = new Entity(2, EntityType.Player, FacingDirection.Down, map.GetCell(2, 1), playerBehaviors);
            var enemy = new Entity(3, EntityType.Enemy, FacingDirection.Up, map.GetCell(2, 3), enemyBehaviors);

            trapCell.PlaceEntity(trap);
            map.GetCell(2, 1).PlaceEntity(player);
            map.GetCell(2, 3).PlaceEntity(enemy);

            // Act
            bool result = attack.TryAttack(trap);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(85, player.GetHealth);
            Assert.AreEqual(65, enemy.GetHealth);
        }

        [TestMethod]
        public void TryAttack_NoTargets_ReturnsFalse()
        {
            // Arrange
            var attack = new TrapAttack(10);
            var map = new MapObject(5, 5);

            var trapCell = map.GetCell(2, 2);

            var trapBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var trap = new Entity(1, EntityType.Trap, FacingDirection.None, trapCell, trapBehaviors);
            trapCell.PlaceEntity(trap);

            // Act
            bool result = attack.TryAttack(trap);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryAttack_OnlyInvalidTargets_ReturnsFalse()
        {
            // Arrange
            var attack = new TrapAttack(10);
            var map = new MapObject(5, 5);

            var trapCell = map.GetCell(2, 2);
            var wallCell = map.GetCell(2, 1);

            var trapBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: attack,
                isCollector: false,
                isCollectable: false
            );

            var wallBehaviors = new Entity.Behaviors(
                health: new NoHealth(),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var trap = new Entity(1, EntityType.Trap, FacingDirection.None, trapCell, trapBehaviors);
            var wall = new Entity(2, EntityType.Wall, FacingDirection.None, wallCell, wallBehaviors);

            trapCell.PlaceEntity(trap);
            wallCell.PlaceEntity(wall);

            // Act
            bool result = attack.TryAttack(trap);

            // Assert
            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class WalkingMovementTests
    {
        [TestMethod]
        public void IsMovable_AlwaysReturnsTrue()
        {
            // Arrange
            var movement = new WalkingMovement();

            // Act & Assert
            Assert.IsTrue(movement.IsMovable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TryMove_NoneDirection_ThrowsException()
        {
            // Arrange
            var movement = new WalkingMovement();
            var map = new MapObject(5, 5);
            var cell = map.GetCell(2, 2);

            var entityBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: movement,
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, cell, entityBehaviors);

            // Act & Assert
            movement.TryMove(entity, FacingDirection.None);
        }

        [TestMethod]
        public void TryMove_ValidMove_MovesEntity()
        {
            // Arrange
            var movement = new WalkingMovement();
            var map = new MapObject(5, 5);

            var startCell = map.GetCell(2, 2);
            var targetCell = map.GetCell(3, 2);

            var entityBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: movement,
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, startCell, entityBehaviors);
            startCell.PlaceEntity(entity);

            // Act
            bool result = movement.TryMove(entity, FacingDirection.Right);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(targetCell, entity.Location);
            Assert.IsFalse(startCell.IsOccupied);
            Assert.IsTrue(targetCell.IsOccupied);
        }

        [TestMethod]
        public void TryMove_OccupiedCell_ReturnsFalse()
        {
            // Arrange
            var movement = new WalkingMovement();
            var map = new MapObject(5, 5);

            var startCell = map.GetCell(2, 2);
            var targetCell = map.GetCell(3, 2);

            var entity1Behaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: movement,
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity2Behaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: new NoMovement(),
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity1 = new Entity(1, EntityType.Player, FacingDirection.Right, startCell, entity1Behaviors);
            var entity2 = new Entity(2, EntityType.Enemy, FacingDirection.Left, targetCell, entity2Behaviors);

            startCell.PlaceEntity(entity1);
            targetCell.PlaceEntity(entity2);

            // Act
            bool result = movement.TryMove(entity1, FacingDirection.Right);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(startCell, entity1.Location);
        }

        [TestMethod]
        public void TryMove_OutOfBounds_ReturnsFalse()
        {
            // Arrange
            var movement = new WalkingMovement();
            var map = new MapObject(5, 5);

            var startCell = map.GetCell(4, 2);

            var entityBehaviors = new Entity.Behaviors(
                health: new BasicHealth(100),
                movement: movement,
                attack: new NoAttack(),
                isCollector: false,
                isCollectable: false
            );

            var entity = new Entity(1, EntityType.Player, FacingDirection.Right, startCell, entityBehaviors);
            startCell.PlaceEntity(entity);

            // Act
            bool result = movement.TryMove(entity, FacingDirection.Right);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(startCell, entity.Location);
        }
    }
}