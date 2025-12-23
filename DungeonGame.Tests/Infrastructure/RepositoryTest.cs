using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.EntityFactory;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Application.enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonGame.Tests.Infrastructure
{
    [TestClass]
    public class FileLevelRepositoryTests
    {
        [TestMethod]
        public void Repository_CreateEntity_ShouldCreateEntity()
        {
            var factory = new HardcodedEntityFactory();
            var entity = factory.Create(EntityType.Player, null);

            Assert.IsNotNull(entity);
            Assert.AreEqual(EntityType.Player, entity.EntityType);
        }

        [TestMethod]
        public void Repository_CreateMap_ShouldCreateMap()
        {
            var map = new MapObject(10, 10);

            Assert.IsNotNull(map);
            Assert.AreEqual(10, map.Width);
            Assert.AreEqual(10, map.Height);
        }

        [TestMethod]
        public void Repository_CreateGameSession_ShouldCreateSession()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var entities = new List<Entity>();

            var session = new GameSession(map, player, entities);

            Assert.IsNotNull(session);
            Assert.AreEqual(map, session.Map);
            Assert.AreEqual(player, session.Player);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repository_GameSession_NullMap_ShouldThrowException()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            var session = new GameSession(null, player, new List<Entity>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Repository_GameSession_NullPlayer_ShouldThrowException()
        {
            var map = new MapObject(10, 10);

            var session = new GameSession(map, null, new List<Entity>());
        }

        [TestMethod]
        public void Repository_Entity_TakeDamage_ShouldWork()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var entity = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            entity.TakeDamage(30);

            Assert.AreEqual(70, entity.GetHealth);
        }

        [TestMethod]
        public void Repository_Factory_CreateMultipleEntities_ShouldWork()
        {
            var factory = new HardcodedEntityFactory();

            var player = factory.Create(EntityType.Player, null);
            var wall = factory.Create(EntityType.Wall, null);
            var trap = factory.Create(EntityType.Trap, null);

            Assert.AreEqual(EntityType.Player, player.EntityType);
            Assert.AreEqual(EntityType.Wall, wall.EntityType);
            Assert.AreEqual(EntityType.Trap, trap.EntityType);
        }
    }
}
