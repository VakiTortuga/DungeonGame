using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Core.MapObject.Interfaces;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Application.enumerations;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class GameSessionTests
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GameSession_Constructor_NullMap_ShouldThrowException()
        {
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            var session = new GameSession(null, player, new System.Collections.Generic.List<Entity>());
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GameSession_Constructor_NullPlayer_ShouldThrowException()
        {
            var map = new MapObject(10, 10);

            var session = new GameSession(map, null, new System.Collections.Generic.List<Entity>());
        }

        [TestMethod]
        public void GameSession_Constructor_ValidParameters_ShouldCreateSession()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity>());

            Assert.IsNotNull(session);
            Assert.AreEqual(map, session.Map);
            Assert.AreEqual(player, session.Player);
            Assert.AreEqual(GameStatus.Playing, session.Status);
        }

        [TestMethod]
        public void GameSession_AddEntity_ShouldAddEntityToList()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var entity = new Entity(2, EntityType.Wall, FacingDirection.Up, null, behaviors);

            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity>());
            session.AddEntity(entity);
        }

        [TestMethod]
        public void GameSession_RemoveEntity_ShouldRemoveEntityFromList()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var entity = new Entity(2, EntityType.Wall, FacingDirection.Up, null, behaviors);

            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity> { entity });
            session.RemoveEntity(entity);
        }

        [TestMethod]
        public void GameSession_CollectCrystal_ShouldIncreaseCount()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);

            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity>());
            session.CollectCrystal();

            Assert.AreEqual(1, session.CollectedCrystals);
        }
    }
}
