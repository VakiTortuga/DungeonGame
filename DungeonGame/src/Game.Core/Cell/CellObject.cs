using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Core.Cell.Interfaces;
using DungeonGame.src.Game.Core.MapObject.Interfaces;

namespace DungeonGame.src.Game.Core.Cell
{
    internal class CellObject : ICellToMap, ICellToEntity
    {
        private Entity entity;
        private readonly IMapToEntity map;

        public int X { get; }
        public int Y { get; }

        public bool IsOccupied => entity != null;

        public CellObject(int x, int y, IMapToEntity map)
        {
            X = x;
            Y = y;
            this.map = map;
        }

        public Entity GetEntity() => entity;

        public IMapToEntity GetMap() => map;

        public bool PlaceEntity(Entity entity)
        {
            if (IsOccupied)
                return false;

            this.entity = entity;
            return true;
        }

        public bool RemoveEntity()
        {
            if (!IsOccupied)
                return false;

            entity = null;
            return true;
        }
    }
}

