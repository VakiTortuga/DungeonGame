using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.UI.Commands;
using System.Windows.Input;

namespace DungeonGame.src.UI.ViewModels
{
    internal class GameWindowVM : BaseViewModel
    {
        private readonly IGameSessionService _gameService;

        private GameStateDTO _state;
        public GameStateDTO State
        {
            get { return _state; }
            private set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public ICommand MoveCommand { get; }
        public ICommand AttackCommand { get; }
        public ICommand PauseCommand { get; }

        public GameWindowVM(IGameSessionService gameService)
        {
            _gameService = gameService;
            State = _gameService.GetGameState();

            MoveCommand = new RelayCommand(p =>
            {
                var direction = (FacingDirection)p;
                State = _gameService.MovePlayer(direction);
            });

            AttackCommand = new RelayCommand(_ =>
            {
                State = _gameService.PlayerAttack();
            });

            PauseCommand = new RelayCommand(_ =>
            {
                State = _gameService.PauseGame();
            });
        }
    }
}
