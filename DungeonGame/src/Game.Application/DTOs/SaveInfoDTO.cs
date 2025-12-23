using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Application.DTOs
{
    public class SaveInfoDTO
    {
        public string SaveId { get; set; }
        public string LevelName { get; set; }
        public DateTime SavedAt { get; set; }
    }
}
