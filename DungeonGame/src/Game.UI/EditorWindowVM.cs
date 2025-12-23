using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Application.ServiceInterfaces;
using DungeonGame.src.UI.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DungeonGame.src.UI.ViewModels
{
    internal class EditorWindowVM : BaseViewModel
    {
        private readonly ILevelEditorService _editorService;

        private EditorStateDTO _state;
        public EditorStateDTO State
        {
            get { return _state; }
            private set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AvailableEntityDTO> AvailableEntities { get; }

        public ICommand PlaceEntityCommand { get; }
        public ICommand RemoveEntityCommand { get; }
        public ICommand SaveLevelCommand { get; }

        public EditorWindowVM(ILevelEditorService editorService)
        {
            _editorService = editorService;

            State = _editorService.GetEditorState();
            AvailableEntities = new ObservableCollection<AvailableEntityDTO>(
                _editorService.GetAvailableEntities());

            PlaceEntityCommand = new RelayCommand(p =>
            {
                var data = (Tuple<int, int>)p;
                State = _editorService.PlaceEntity(
                    data.Item1,
                    data.Item2,
                    State.SelectedEntityType);
            });

            RemoveEntityCommand = new RelayCommand(p =>
            {
                var data = (Tuple<int, int>)p;
                State = _editorService.RemoveEntity(
                    data.Item1,
                    data.Item2);
            });

            SaveLevelCommand = new RelayCommand(p =>
            {
                _editorService.SaveLevelAs((string)p);
            });
        }
    }
}
