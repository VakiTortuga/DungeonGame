using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core
{
    internal class GameEngine
    {
        private readonly GameSession session;

        public GameEngine(GameSession session)
        {
            this.session = session;
        }

        public void PlayerMove(FacingDirection direction)
        {
            if (session.Status != GameStatus.Playing)
                return;

            session.Player.TryMove(direction);
            UpdateAI();
            session.UpdateStatus();
        }

        public void PlayerAttack()
        {
            if (session.Status != GameStatus.Playing)
                return;

            session.Player.TryAttack();
            UpdateAI();
            session.UpdateStatus();
        }

        private void UpdateAI()
        {
            foreach (var entity in session.GetEntities())
            {
                if (entity.EntityType == EntityType.Enemy && entity.IsAlive)
                {
                    // примитивный AI
                    entity.TryMove(FacingDirection.Left);
                }
            }
        }

        public GameStatus GetStatus()
            => session.Status;
    }
}
