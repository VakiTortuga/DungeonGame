using DungeonGame.src.Game.Core.MapObject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.Cell.Interfaces
{
    internal interface ICellToMap
    {
        int X { get; }
        int Y { get; }
        bool IsOccupied { get; }
        Entity GetEntity();
        IMapToEntity GetMap();
        bool PlaceEntity(Entity entity);
        bool RemoveEntity();
    }
}
