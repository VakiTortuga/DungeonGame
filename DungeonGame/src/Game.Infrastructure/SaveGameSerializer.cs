using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using DungeonGame.src.Game.Infrastructure.DTOs;

namespace DungeonGame.src.Game.Infrastructure
{
    internal static class SaveGameSerializer
    {
        public static string Serialize(SaveGameDTO dto)
        {
            return JsonSerializer.Serialize(dto);
        }

        public static SaveGameDTO Deserialize(string json)
        {
            return JsonSerializer.Deserialize<SaveGameDTO>(json);
        }
    }
}
