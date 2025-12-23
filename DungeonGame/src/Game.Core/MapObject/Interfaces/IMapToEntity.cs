using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.Cell.Interfaces;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.MapObject.Interfaces
{
    public interface IMapToEntity
    {
        int Width { get; }
        int Height { get; }
        bool TryMoveEntity(ICellToMap from, ICellToMap to);
        ICellToMap GetCellByDirection(ICellToMap cell, FacingDirection direction);
        ICellToMap GetCell(int x, int y);
        (int width, int height) GetDimensions();
    }
}
