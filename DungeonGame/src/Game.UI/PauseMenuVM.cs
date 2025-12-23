using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.UI.Commands;
using System.Windows.Input;

namespace DungeonGame.src.UI.ViewModels
{
    internal class PauseMenuVM : BaseViewModel
    {
        private readonly IGameSessionService _gameService;

        public ICommand ResumeCommand { get; }
        public ICommand SaveAndExitCommand { get; }

        public PauseMenuVM(IGameSessionService gameService)
        {
            _gameService = gameService;

            ResumeCommand = new RelayCommand(_ =>
            {
                _gameService.ResumeGame();
            });

            SaveAndExitCommand = new RelayCommand(_ =>
            {
                _gameService.SaveAndExit();
            });
        }
    }
}
