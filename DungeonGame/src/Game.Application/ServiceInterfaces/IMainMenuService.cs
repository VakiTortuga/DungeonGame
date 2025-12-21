using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Application.DTOs;

namespace DungeonGame.src.Game.Application.ServiceInterfaces
{
    internal interface IMainMenuService
    {
        List<LevelInfoDTO> GetAvailableLevels();
        List<SaveInfoDTO> GetAvailibleSaves();
        AppState StartNewGame(string levelId);
        AppState ContinueGame(string saveId);
        AppState OpenEditor(string levelId);
        AppState ExitApp();
    }
}
