using DungeonGame.src.Game.Application.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Application.DTOs
{
    internal class AvailableEntityDTO
    {
        public EntityVisualType EntityType { get; set; }
        public string DisplayName { get; set; }
        public bool IsUnique { get; set; }
    }
}
