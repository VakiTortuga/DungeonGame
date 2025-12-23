using DungeonGame.src.Game.Core;
using DungeonGame.src.Game.Infrastructure.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Infrastructure.Repository.Implementation
{
    public class FileLevelRepository : ILevelRepository
    {
        private const string LevelsDirectory = "Levels";
        private const string FileExtension = ".json";

        public FileLevelRepository()
        {
            // Создаем директорию для уровней, если ее нет
            if (!Directory.Exists(LevelsDirectory))
            {
                Directory.CreateDirectory(LevelsDirectory);
            }
        }

        public IEnumerable<GameSession> GetAll()
        {
            if (!Directory.Exists(LevelsDirectory))
                return Enumerable.Empty<GameSession>();

            var levelFiles = Directory.GetFiles(LevelsDirectory, $"*{FileExtension}");
            var sessions = new List<GameSession>();

            foreach (var filePath in levelFiles)
            {
                try
                {
                    var levelId = Path.GetFileNameWithoutExtension(filePath);
                    sessions.Add(Get(levelId));
                }
                catch (Exception ex)
                {
                    // Логируем ошибку, но продолжаем загрузку других уровней
                }
            }

            return sessions;
        }

        public GameSession Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID уровня не может быть пустым", nameof(id));

            var filePath = GetFilePath(id);

            if (!File.Exists(filePath))
                throw new InvalidOperationException($"Уровень с ID '{id}' не найден");

            try
            {
                var json = File.ReadAllText(filePath);
                var dto = SaveGameSerializer.Deserialize(json);
                return SaveGameMapper.FromDTO(dto);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка загрузки уровня '{id}': {ex.Message}", ex);
            }
        }

        public void Save(GameSession level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            // Генерируем ID на основе времени или используем существующий ID сессии
            var levelId = GenerateLevelId();
            var dto = SaveGameMapper.ToDTO(level);
            var json = SaveGameSerializer.Serialize(dto);

            var filePath = GetFilePath(levelId);
            File.WriteAllText(filePath, json);
        }

        private string GetFilePath(string levelId)
        {
            // Убираем недопустимые символы из имени файла
            var safeFileName = RemoveInvalidFileNameChars(levelId);
            return Path.Combine(LevelsDirectory, safeFileName + FileExtension);
        }

        private string GenerateLevelId()
        {
            int count = Directory.GetFiles(LevelsDirectory, $"*{FileExtension}").Length;
            return $"level_{count + 1:000}";
        }

        private string RemoveInvalidFileNameChars(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return new string(fileName
                .Where(ch => !invalidChars.Contains(ch))
                .ToArray());
        }

        // Дополнительные полезные методы:
        public bool LevelExists(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var filePath = GetFilePath(id);
            return File.Exists(filePath);
        }

        public void Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID уровня не может быть пустым", nameof(id));

            var filePath = GetFilePath(id);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public int GetLevelCount()
        {
            if (!Directory.Exists(LevelsDirectory))
                return 0;

            return Directory.GetFiles(LevelsDirectory, $"*{FileExtension}").Length;
        }
    }
}
