using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Core.MapObject.Interfaces;

namespace DungeonGame.src.Game.Core.Cell.Interfaces
{
    public interface ICellToEntity
    {
        int X { get; }
        int Y { get; }
        bool IsOccupied { get; }
        Entity GetEntity();
        IMapToEntity GetMap();
    }
}


