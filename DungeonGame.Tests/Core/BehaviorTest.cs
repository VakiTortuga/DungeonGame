using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.BehaviorInterfaces;
using DungeonGame.src.Game.Core.Cell.Interfaces;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class BehaviorTests
    {
        [TestMethod]
        public void BasicHealth_TakeDamage_ShouldReduceHealth()
        {
            var health = new BasicHealth(100);
            var result = health.TakeDamage(30);
            Assert.IsTrue(result);
            Assert.AreEqual(70, health.GetHealth);
            Assert.IsTrue(health.IsAlive);
        }

        [TestMethod]
        public void BasicHealth_TakeDamage_MoreThanHealth_ShouldDie()
        {
            var health = new BasicHealth(50);
            var result = health.TakeDamage(100);
            Assert.IsTrue(result);
            Assert.AreEqual(0, health.GetHealth);
            Assert.IsFalse(health.IsAlive);
        }

        [TestMethod]
        public void BasicHealth_Heal_ShouldIncreaseHealth()
        {
            var health = new BasicHealth(100);
            health.TakeDamage(40); 
            var result = health.Heal(20);
            Assert.IsTrue(result);
            Assert.AreEqual(80, health.GetHealth);
        }

        [TestMethod]
        public void BasicHealth_Heal_MoreThanMax_ShouldSetToMax()
        {
            var health = new BasicHealth(100);
            health.TakeDamage(30); 

            var result = health.Heal(50); 
            Assert.IsTrue(result);
            Assert.AreEqual(100, health.GetHealth);
        }

        [TestMethod]
        public void WalkingMovement_IsMovable_ShouldReturnTrue()
        {
            var movement = new WalkingMovement();

            Assert.IsTrue(movement.IsMovable);
        }

        [TestMethod]
        public void MeleeAttack_CanAttack_ShouldReturnTrue()
        {
            var attack = new MeleeAttack(10);
            Assert.IsTrue(attack.CanAttack);
        }

        [TestMethod]
        public void NoAttack_CanAttack_ShouldReturnFalse()
        {
            var attack = new NoAttack();
            Assert.IsFalse(attack.CanAttack);
        }

        [TestMethod]
        public void NoMovement_IsMovable_ShouldReturnFalse()
        {
            var movement = new NoMovement();
            Assert.IsFalse(movement.IsMovable);
        }

        [TestMethod]
        public void NoHealth_IsAlive_ShouldReturnFalse()
        {
            var health = new NoHealth();
            Assert.IsFalse(health.IsAlive);
        }
    }
}