using DungeonGame.src.Game.Application.enumerations;
using DungeonGame.src.Game.Core.BehaviorInterfaces;
using DungeonGame.src.Game.Core.enumerations;
using DungeonGame.src.Game.Core.Cell.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core
{
    public class Entity
    {
        public class Behaviors
        {
            IHealthBehavior healthBehavior;
            IMoveBehavior moveBehavior;
            IAttackBehavior attackBehavior;
            bool? isCollector;
            bool? isCollectable;

            public IHealthBehavior GetHealthBehavior() { return healthBehavior; }
            public IMoveBehavior GetMoveBehavior() { return moveBehavior; }
            public IAttackBehavior GetAttackBehavior() { return attackBehavior; }
            public bool IsCollector { get { return isCollector??false;  } }
            public bool IsCollectable { get { return isCollectable??false;  } }

            public Behaviors(
                IHealthBehavior health,
                IMoveBehavior movement,
                IAttackBehavior attack,
                bool? isCollector,
                bool? isCollectable
                )
            {
                healthBehavior = health;
                moveBehavior = movement;
                attackBehavior = attack;
                this.isCollector = isCollector;
                this.isCollectable = isCollectable;
            }
            public bool IsValid()
            {
                if (healthBehavior != null && 
                    moveBehavior != null &&
                    attackBehavior != null &&
                    isCollector != null &&
                    isCollectable != null)
                {
                    return true;
                }
                return false;
            }
        }

        private int id;
        private ICellToMap location;
        private IHealthBehavior healthBehavior;
        private IMoveBehavior moveBehavior;
        private IAttackBehavior attackBehavior;

        public int ID
        {
            get { return id; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Entity. ID is out of range.");
                id = value;
            }
        }
        public EntityType EntityType { get;  private set; }
        public FacingDirection FacingDirection { get; private set; }
        public ICellToMap Location { 
            get { return location; }
            private set 
            {
                if (value == null) 
                    throw new ArgumentNullException("Entity. Location can not be null.");
                location = value;
            } 
        }
        public bool IsCollector { get; private set; }
        public bool IsCollectable { get; private set; }
        public bool IsMovable => moveBehavior.IsMovable;
        public bool CanAttack => attackBehavior.CanAttack;
        public bool IsAlive => healthBehavior.IsAlive;
        public int GetHealth => healthBehavior.GetHealth;
        public int GetMaxHealth => healthBehavior.GetMaxHealth;


        public Entity(
            int id,
            EntityType entityType,
            FacingDirection facingDirection,
            ICellToMap location,
            Behaviors behaviors
            )
        {
            ID = id;
            EntityType = entityType;
            FacingDirection = facingDirection;
            Location = location;
            if (!behaviors.IsValid())
                throw new ArgumentNullException("Entity constructor. Behaviors are not valid.");
            healthBehavior = behaviors.GetHealthBehavior();
            moveBehavior = behaviors.GetMoveBehavior();
            attackBehavior = behaviors.GetAttackBehavior();
            IsCollectable = behaviors.IsCollectable;
            IsCollector = behaviors.IsCollector;
        }

        public bool TryMove(FacingDirection direction)
        {
            if (direction == FacingDirection.None)
                throw new ArgumentOutOfRangeException("Entity. Entity can not move if direction is none.");

            return healthBehavior.IsAlive && moveBehavior.TryMove(this, direction);
        }

        public bool TryAttack()
        {
            return healthBehavior.IsAlive && CanAttack && attackBehavior.TryAttack(this);
        }

        public bool TakeDamage(int amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException("Entity. Damage amount can not be less or equal to zero.");

            return healthBehavior.IsAlive && healthBehavior.TakeDamage(amount);
        }
        public void ChangeFacingDirection(FacingDirection newDirection)
        {
            if (newDirection == FacingDirection.None)
                throw new ArgumentOutOfRangeException("Entity. Direction can not be none.");

            FacingDirection = newDirection;
        }
        internal void SetLocation(ICellToMap newLocation)
        {
            if (newLocation == null)
                throw new ArgumentNullException(nameof(newLocation));

            Location = newLocation;
        }
    }
}
