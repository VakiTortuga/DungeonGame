using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.EntityFactory;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Application.enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DungeonGame.Tests.Infrastructure
{
    [TestClass]
    public class FileSaveGameRepositoryTests
    {
        [TestMethod]
        public void Repository_Entity_Creation_ShouldSetProperties()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new WalkingMovement(),
                new MeleeAttack(10),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            Assert.AreEqual(1, entity.ID);
            Assert.AreEqual(EntityType.Player, entity.EntityType);
            Assert.IsTrue(entity.IsMovable);
            Assert.IsTrue(entity.CanAttack);
            Assert.IsTrue(entity.IsAlive);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Repository_Entity_InvalidId_ShouldThrowException()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var entity = new Entity(0, EntityType.Player, FacingDirection.Up, null, behaviors);
        }

        [TestMethod]
        public void Repository_Entity_TryMove_ShouldCallBehavior()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new WalkingMovement(),
                new NoAttack(),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            entity.TryMove(FacingDirection.Down);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Repository_Entity_TryMove_NoneDirection_ShouldThrowException()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new WalkingMovement(),
                new NoAttack(),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            entity.TryMove(FacingDirection.None);
        }

        [TestMethod]
        public void Repository_Entity_TryAttack_ShouldCallBehavior()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new MeleeAttack(10),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            entity.TryAttack();
        }

        [TestMethod]
        public void Repository_Entity_ChangeFacingDirection_ShouldUpdate()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            entity.ChangeFacingDirection(FacingDirection.Right);

            Assert.AreEqual(FacingDirection.Right, entity.FacingDirection);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Repository_Entity_ChangeFacingDirection_None_ShouldThrowException()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            entity.ChangeFacingDirection(FacingDirection.None);
        }

        [TestMethod]
        public void Repository_GameSession_CollectCrystal_ShouldIncreaseCount()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var session = new GameSession(map, player, new List<Entity>());

            session.CollectCrystal();
            session.CollectCrystal();

            Assert.AreEqual(2, session.CollectedCrystals);
        }

        [TestMethod]
        public void Repository_Factory_CreateEnemy_ShouldHaveAttackBehavior()
        {
            var factory = new HardcodedEntityFactory();
            var enemy = factory.Create(EntityType.Enemy, null);

            Assert.AreEqual(EntityType.Enemy, enemy.EntityType);
            Assert.IsTrue(enemy.CanAttack);
        }
    }
}