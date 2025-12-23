using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Infrastructure.DTOs
{
    [Serializable]
    internal class EntitySaveDTO
    {
        public int Id;
        public EntityType Type;
        public int X;
        public int Y;
        public int Health;
        public FacingDirection Facing;
    }
}
