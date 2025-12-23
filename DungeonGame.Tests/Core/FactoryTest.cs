using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGame.src.Game.Core.EntityFactory;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Core.enumerations;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class FactoryTest
    {
        [TestMethod]
        public void HardcodedEntityFactory_CreatePlayer_CreatesValidPlayer()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act
            var player = factory.Create(EntityType.Player, startCell);

            // Assert
            Assert.IsNotNull(player);
            Assert.AreEqual(EntityType.Player, player.EntityType);
            Assert.IsTrue(player.IsAlive);
            Assert.IsTrue(player.IsMovable);
            Assert.IsTrue(player.CanAttack);
            Assert.IsTrue(player.IsCollector);
            Assert.IsFalse(player.IsCollectable);
            Assert.AreEqual(10, player.GetMaxHealth);
            Assert.AreEqual(startCell, player.Location);
        }

        [TestMethod]
        public void HardcodedEntityFactory_CreateEnemy_CreatesValidEnemy()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act
            var enemy = factory.Create(EntityType.Enemy, startCell);

            // Assert
            Assert.IsNotNull(enemy);
            Assert.AreEqual(EntityType.Enemy, enemy.EntityType);
            Assert.IsTrue(enemy.IsAlive);
            Assert.IsTrue(enemy.IsMovable);
            Assert.IsTrue(enemy.CanAttack);
            Assert.IsFalse(enemy.IsCollector);
            Assert.IsFalse(enemy.IsCollectable);
            Assert.AreEqual(5, enemy.GetMaxHealth);
            Assert.AreEqual(startCell, enemy.Location);
        }

        [TestMethod]
        public void HardcodedEntityFactory_CreateWall_CreatesValidWall()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act
            var wall = factory.Create(EntityType.Wall, startCell);

            // Assert
            Assert.IsNotNull(wall);
            Assert.AreEqual(EntityType.Wall, wall.EntityType);
            Assert.IsTrue(wall.IsAlive); // NoHealth always returns true
            Assert.IsFalse(wall.IsMovable);
            Assert.IsFalse(wall.CanAttack);
            Assert.IsFalse(wall.IsCollector);
            Assert.IsFalse(wall.IsCollectable);
            Assert.AreEqual(startCell, wall.Location);
        }

        [TestMethod]
        public void HardcodedEntityFactory_CreateTrap_CreatesValidTrap()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act
            var trap = factory.Create(EntityType.Trap, startCell);

            // Assert
            Assert.IsNotNull(trap);
            Assert.AreEqual(EntityType.Trap, trap.EntityType);
            Assert.IsTrue(trap.IsAlive);
            Assert.IsFalse(trap.IsMovable);
            Assert.IsTrue(trap.CanAttack);
            Assert.IsFalse(trap.IsCollector);
            Assert.IsFalse(trap.IsCollectable);
            Assert.AreEqual(startCell, trap.Location);
        }

        [TestMethod]
        public void HardcodedEntityFactory_CreateCrystal_CreatesValidCrystal()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act
            var crystal = factory.Create(EntityType.Crystal, startCell);

            // Assert
            Assert.IsNotNull(crystal);
            Assert.AreEqual(EntityType.Crystal, crystal.EntityType);
            Assert.IsTrue(crystal.IsAlive);
            Assert.IsFalse(crystal.IsMovable);
            Assert.IsFalse(crystal.CanAttack);
            Assert.IsFalse(crystal.IsCollector);
            Assert.IsTrue(crystal.IsCollectable);
            Assert.AreEqual(startCell, crystal.Location);
        }

        [TestMethod]
        public void HardcodedEntityFactory_CreateExit_CreatesValidExit()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act
            var exit = factory.Create(EntityType.Exit, startCell);

            // Assert
            Assert.IsNotNull(exit);
            Assert.AreEqual(EntityType.Exit, exit.EntityType);
            Assert.IsTrue(exit.IsAlive);
            Assert.IsFalse(exit.IsMovable);
            Assert.IsFalse(exit.CanAttack);
            Assert.IsFalse(exit.IsCollector);
            Assert.IsFalse(exit.IsCollectable);
            Assert.AreEqual(startCell, exit.Location);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void HardcodedEntityFactory_CreateInvalidType_ThrowsException()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell = map.GetCell(5, 5);

            // Act & Assert
            factory.Create((EntityType)999, startCell);
        }

        [TestMethod]
        public void HardcodedEntityFactory_IdsAreIncremented()
        {
            // Arrange
            var factory = new HardcodedEntityFactory();
            var map = new MapObject(10, 10);
            var startCell1 = map.GetCell(1, 1);
            var startCell2 = map.GetCell(2, 2);

            // Act
            var entity1 = factory.Create(EntityType.Player, startCell1);
            var entity2 = factory.Create(EntityType.Enemy, startCell2);

            // Assert
            Assert.AreEqual(2, entity1.ID);
            Assert.AreEqual(3, entity2.ID);
        }
    }
}