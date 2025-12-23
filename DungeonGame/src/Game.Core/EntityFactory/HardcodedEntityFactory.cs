using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.BehaviorImplementations;
using DungeonGame.src.Game.Core.Cell.Interfaces;
using DungeonGame.src.Game.Core.EntityFactory.Interface;
using DungeonGame.src.Game.Core.enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.EntityFactory
{
    internal class HardcodedEntityFactory : IEntityFactory
    {
        private int nextId = 1;

        public Entity Create(EntityType type, ICellToMap startCell)
        {
            Entity.Behaviors behaviors;

            switch (type)
            {
                case EntityType.Player:
                    behaviors = new Entity.Behaviors(
                        health: new BasicHealth(10),
                        movement: new WalkingMovement(),
                        attack: new MeleeAttack(2),
                        isCollector: true,
                        isCollectable: false
                    );
                    break;

                case EntityType.Enemy:
                    behaviors = new Entity.Behaviors(
                        health: new BasicHealth(5),
                        movement: new WalkingMovement(),
                        attack: new MeleeAttack(1),
                        isCollector: false,
                        isCollectable: false
                    );
                    break;

                case EntityType.Wall:
                    behaviors = new Entity.Behaviors(
                        health: new NoHealth(),
                        movement: new NoMovement(),
                        attack: new NoAttack(),
                        isCollector: false,
                        isCollectable: false
                    );
                    break;

                case EntityType.Trap:
                    behaviors = new Entity.Behaviors(
                        health: new NoHealth(),
                        movement: new NoMovement(),
                        attack: new TrapAttack(2),
                        isCollector: false,
                        isCollectable: false
                    );
                    break;

                case EntityType.Crystal:
                    behaviors = new Entity.Behaviors(
                        health: new NoHealth(),
                        movement: new NoMovement(),
                        attack: new NoAttack(),
                        isCollector: false,
                        isCollectable: true
                    );
                    break;

                case EntityType.Exit:
                    behaviors = new Entity.Behaviors(
                        health: new NoHealth(),
                        movement: new NoMovement(),
                        attack: new NoAttack(),
                        isCollector: false,
                        isCollectable: false
                    );
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }

            var entity = new Entity(
                id : ++nextId,
                entityType: type,
                facingDirection: FacingDirection.None,
                location: startCell,
                behaviors: behaviors
            );

            startCell.PlaceEntity(entity);
            return entity;
        }
    }
}
