using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.EntityFactory;
using DungeonGame.src.Game.Core.enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DungeonGame.Tests.Core
{
    [TestClass]
    public class HardcodedEntityFactoryTests
    {
        private HardcodedEntityFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new HardcodedEntityFactory();
        }

        [TestMethod]
        public void Create_PlayerType_ShouldReturnPlayerEntity()
        {
            var entity = _factory.Create(EntityType.Player, null);

            Assert.AreEqual(EntityType.Player, entity.EntityType);
            Assert.IsInstanceOfType(entity, typeof(Entity));
        }

        [TestMethod]
        public void Create_WallType_ShouldReturnWallEntity()
        {
            var entity = _factory.Create(EntityType.Wall, null);

            Assert.AreEqual(EntityType.Wall, entity.EntityType);
            Assert.IsInstanceOfType(entity, typeof(Entity));
        }

        [TestMethod]
        public void Create_TrapType_ShouldReturnTrapEntity()
        {
            var entity = _factory.Create(EntityType.Trap, null);

            Assert.AreEqual(EntityType.Trap, entity.EntityType);
            Assert.IsInstanceOfType(entity, typeof(Entity));
        }
    }
}
