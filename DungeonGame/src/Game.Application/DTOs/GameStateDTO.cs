using DungeonGame.src.Game.Application.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Application.DTOs
{
    internal class GameStateDTO
    {
        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public List<TileStateDTO> Tiles { get; set; }
        public PlayerStateDTO PlayerState {  get; set; }
        public int CollectedCrystals { get; set; }
        public int RequiredCrystals { get; set; }
        public GameStatus GameStatus { get; set; }
    }
}
