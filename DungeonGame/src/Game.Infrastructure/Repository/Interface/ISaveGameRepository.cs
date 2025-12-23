using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Core;

namespace DungeonGame.src.Game.Infrastructure.Repository.Interface
{
    public interface ISaveGameRepository
    {
        void Save(GameSession session);
        GameSession Load();
        bool HasSave();
        void Clear();
    }
}
