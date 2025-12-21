using DungeonGame.src.Game.Application.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Application.DTOs
{
    internal class TileStateDTO
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public EntityVisualType EntityType { get; set; }
        public FacingDirection FacingDirection { get; set; }
    }
}
