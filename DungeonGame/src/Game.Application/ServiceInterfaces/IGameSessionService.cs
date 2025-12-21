using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Application.DTOs;

namespace DungeonGame.src.Game.Application.ServiceInterfaces
{
    internal interface IGameSessionService
    {
        GameStateDTO MovePlayer(FacingDirection direction);
        GameStateDTO PlayerAttack();
        GameStateDTO PauseGame();
        GameStateDTO ResumeGame();
        AppState SaveAndExit();
        GameStateDTO GetGameState();
    }
}
