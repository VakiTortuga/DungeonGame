using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.UI.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DungeonGame.src.UI.ViewModels
{
    internal class InitialWindowVM : BaseViewModel
    {
        private readonly IMainMenuService _menuService;

        public ObservableCollection<LevelInfoDTO> Levels { get; }
        public ObservableCollection<SaveInfoDTO> Saves { get; }

        public ICommand StartNewGameCommand { get; }
        public ICommand ContinueGameCommand { get; }
        public ICommand OpenEditorCommand { get; }
        public ICommand ExitCommand { get; }

        public InitialWindowVM(IMainMenuService menuService)
        {
            _menuService = menuService;

            Levels = new ObservableCollection<LevelInfoDTO>(_menuService.GetAvailableLevels());
            Saves = new ObservableCollection<SaveInfoDTO>(_menuService.GetAvailibleSaves());

            StartNewGameCommand = new RelayCommand(p =>
            {
                var level = (LevelInfoDTO)p;
                _menuService.StartNewGame(level.LevelId);
            });

            ContinueGameCommand = new RelayCommand(p =>
            {
                var save = (SaveInfoDTO)p;
                _menuService.ContinueGame(save.SaveId);
            });

            OpenEditorCommand = new RelayCommand(p =>
            {
                var level = (LevelInfoDTO)p;
                _menuService.OpenEditor(level.LevelId);
            });

            ExitCommand = new RelayCommand(_ => _menuService.ExitApp());
        }
    }
}
