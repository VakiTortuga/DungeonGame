using DungeonGame.src.Game.Application.DTOs;
using DungeonGame.src.Game.Application.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DungeonGame.src.Game.Application.ServiceInterfaces
{
    internal interface ILevelEditorService
    {
        EditorStateDTO CreateNewLevel();
        EditorStateDTO LoadLevel(string levelId);
        List<AvailableEntityDTO> GetAvailableEntities();
        EditorStateDTO PlaceEntity(int x, int y, EntityVisualType entityType);
        EditorStateDTO RemoveEntity(int x, int y);
        AppState SaveLevelAs(string name);
        EditorStateDTO GetEditorState();
    }
}
