using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Core;

namespace DungeonGame.src.Game.Infrastructure.Repository.Interface
{
    public interface ILevelRepository
    {
        IEnumerable<GameSession> GetAll();
        GameSession Get(string id);
        void Save(GameSession level);
    }
}
