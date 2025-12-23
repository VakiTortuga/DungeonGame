using DungeonGame.src.Game.Application.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Application.DTOs
{
    public class PlayerStateDTO
    {
        public int Health {  get; set; }
        public int MaxHealth { get; set; }
        public FacingDirection FacingDirection { get; set; }
    }
}
