using DungeonGame.src.Game.Application.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Application.DTOs
{
    internal class EditorStateDTO
    {
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public List<TileStateDTO> Tiles { get; set; }
        public EntityVisualType SelectedEntityType { get; set; }
    }
}
