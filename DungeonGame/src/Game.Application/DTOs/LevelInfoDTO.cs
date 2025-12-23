using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Application.enumerations;

namespace DungeonGame.src.Game.Application.DTOs
{
    public class LevelInfoDTO
    {
        public string LevelId { get; set; }
        public string DisplayName { get; set; }
        public LevelSourceType SourceType { get; set; }
    }
}
