using DungeonGame.src.Game.Core.Cell.Interfaces;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.EntityFactory.Interface
{
    internal interface IEntityFactory
    {
        Entity Create(EntityType type, ICellToMap startCell);
    }
}
