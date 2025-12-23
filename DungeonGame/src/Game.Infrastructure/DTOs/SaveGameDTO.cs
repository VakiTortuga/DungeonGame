using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Infrastructure.DTOs
{
    [Serializable]
    internal class SaveGameDTO
    {
        public int MapWidth;
        public int MapHeight;
        public List<EntitySaveDTO> Entities;
        public int PlayerId;
        public int CollectedCrystals;
        public int InitialCrystals;
    }

}
