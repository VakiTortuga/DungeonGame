using Microsoft.VisualStudio.TestTools.UnitTesting;
using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.MapObject;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Application.enumerations;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class GameEngineTests
    {
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void GameEngine_Constructor_NullSession_ShouldThrowException()
        {
            var engine = new GameEngine(null);
        }

        [TestMethod]
        public void GameEngine_Constructor_ValidSession_ShouldCreateEngine()
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

            var engine = new GameEngine(session);

            Assert.IsNotNull(engine);
        }

        [TestMethod]
        public void GameEngine_PlayerMove_StatusPlaying_ShouldCallTryMove()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new WalkingMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity>());

            var engine = new GameEngine(session);
            engine.PlayerMove(FacingDirection.Up);
        }

        [TestMethod]
        public void GameEngine_PlayerMove_StatusNotPlaying_ShouldNotCallTryMove()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(0),
                new WalkingMovement(),
                new NoAttack(),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity>());

            var engine = new GameEngine(session);
            engine.PlayerMove(FacingDirection.Up);
        }

        [TestMethod]
        public void GameEngine_PlayerAttack_StatusPlaying_ShouldCallTryAttack()
        {
            var map = new MapObject(10, 10);
            var behaviors = new Entity.Behaviors(
                new BasicHealth(100),
                new NoMovement(),
                new MeleeAttack(10),
                false,
                false);
            var player = new Entity(1, EntityType.Player, FacingDirection.Up, null, behaviors);
            var session = new GameSession(map, player, new System.Collections.Generic.List<Entity>());

            var engine = new GameEngine(session);
            engine.PlayerAttack();
        }

        [TestMethod]
        public void GameEngine_GetStatus_ShouldReturnSessionStatus()
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

            var engine = new GameEngine(session);
            var status = engine.GetStatus();

            Assert.AreEqual(GameStatus.Playing, status);
        }
    }
}