using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DungeonGame.src.Game.Infrastructure.Repository.Interface;
using DungeonGame.src.Game.Core;

namespace DungeonGame.src.Game.Infrastructure.Repository.Implementation
{
    internal class FileSaveGameRepository : ISaveGameRepository
    {
        private const string Path = "savegame.json";

        public bool HasSave() => File.Exists(Path);

        public void Save(GameSession session)
        {
            var dto = SaveGameMapper.ToDTO(session);
            var json = SaveGameSerializer.Serialize(dto);
            File.WriteAllText(Path, json);
        }

        public GameSession Load()
        {
            if (!HasSave())
                throw new InvalidOperationException("No save");

            var json = File.ReadAllText(Path);
            var dto = SaveGameSerializer.Deserialize(json);
            return SaveGameMapper.FromDTO(dto);
        }

        public void Clear()
        {
            if (HasSave())
                File.Delete(Path);
        }
    }
}
