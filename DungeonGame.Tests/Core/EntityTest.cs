using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.BehaviorInterfaces;
using DungeonGame.src.Game.Core.enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class EntityTests
    {
        [TestMethod]
        public void Entity_Initialize_WithValidParameters_ShouldSetProperties()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);

            var entity = new Entity(
                id: 1,
                entityType: EntityType.Player,
                facingDirection: FacingDirection.Up,
                location: null,
                behaviors: behaviors);

            Assert.AreEqual(1, entity.ID);
            Assert.AreEqual(EntityType.Player, entity.EntityType);
            Assert.AreEqual(FacingDirection.Up, entity.FacingDirection);
        }

        [TestMethod]
        public void Entity_TakeDamage_WithHealthBehavior_ShouldReduceHealth()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);

            var entity = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Up,
                null,
                behaviors);

            entity.TakeDamage(30);

            Assert.AreEqual(70, entity.GetHealth);
        }

        [TestMethod]
        public void Entity_TakeDamage_MoreThanHealth_ShouldDie()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(50),
                new NoMovement(),
                new NoAttack(),
                false,
                false);

            var entity = new Entity(
                1,
                EntityType.Player,
                FacingDirection.Up,
                null,
                behaviors);

            entity.TakeDamage(100);

            Assert.AreEqual(0, entity.GetHealth);
            Assert.IsFalse(entity.IsAlive);
        }
    }
}
