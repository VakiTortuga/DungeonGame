using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core
{
    internal interface ICellToEntity
    {
        int X { get; }
        int Y { get; }
        bool IsOccupied { get; }
        Entity GetEntity();
        IMapToEntity getMap();
    }
}
