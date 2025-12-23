using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.Cell.Interfaces;
using DungeonGame.src.Game.Core.Cell;
using DungeonGame.src.Game.Core.MapObject.Interfaces;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.MapObject
{
    internal class MapObject : IMapToEntity
    {
        private readonly CellObject[,] cells;

        public int Width { get; }
        public int Height { get; }

        public MapObject(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException("Map dimensions must be positive");

            Width = width;
            Height = height;

            cells = new CellObject[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    cells[x, y] = new CellObject(x, y, this);
        }

        public (int width, int height) GetDimensions()
            => (Width, Height);

        public ICellToMap GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
                return null;

            return cells[x, y];
        }

        public ICellToMap GetCellByDirection(
            ICellToMap cell,
            FacingDirection direction)
        {
            int x = cell.X;
            int y = cell.Y;

            ICellToMap result;
            switch (direction)
            {
                case FacingDirection.Up:
                    result = GetCell(x, y - 1);
                    break;
                case FacingDirection.Down:
                    result = GetCell(x, y + 1);
                    break;
                case FacingDirection.Left:
                    result = GetCell(x - 1, y);
                    break;
                case FacingDirection.Right:
                    result = GetCell(x + 1, y);
                    break;
                default:
                    result = null;
                    break;
            }
            return result;
        }

        public bool CanEnterCell(ICellToMap cell)
        {
            if (cell == null || cell.IsOccupied)
                return false;

            // Проверка, нет ли стены или другого непроходимого объекта
            var entity = cell.GetEntity();
            if (entity != null)
            {
                switch (entity.EntityType)
                {
                    case EntityType.Wall:
                        return false;
                    case EntityType.Exit:
                    case EntityType.Crystal:
                    case EntityType.Trap:
                        return true; // Эти объекты проходимы
                    default:
                        return !entity.IsMovable; // Если сущность не подвижна, нельзя войти
                }
            }

            return true;
        }

        // Обновить TryMoveEntity
        public bool TryMoveEntity(ICellToMap from, ICellToMap to)
        {
            if (from == null || to == null)
                return false;

            if (!from.IsOccupied || !CanEnterCell(to))
                return false;

            var entity = from.GetEntity();

            from.RemoveEntity();
            to.PlaceEntity(entity);

            entity.SetLocation(to);

            return true;
        }
    }
}
