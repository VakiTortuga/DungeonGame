using DungeonGame.src.Game.Application.enumerations;
using System;

namespace DungeonGame.src.Game.Application.DTOs
{
    public class AppState
    {
        public ApplicationState CurrentState { get; set; }
        public GameStateDTO GameState { get; set; }
        public EditorStateDTO EditorState { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
    }
}