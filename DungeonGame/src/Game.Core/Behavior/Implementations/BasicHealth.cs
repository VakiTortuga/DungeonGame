using DungeonGame.src.Game.Core.BehaviorInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame.src.Game.Core.BehaviorImplementations
{
    public class BasicHealth : IHealthBehavior
    {
        private int maxHealth;
        private int currentHealth;
        public bool IsAlive => currentHealth > 0;

        public int GetHealth => currentHealth;
        public int GetMaxHealth => maxHealth;
        public BasicHealth(int maxHealth)
        {
            if (maxHealth < 0)
            { 
                throw new ArgumentOutOfRangeException(
                    "BasicHealth. MaxHealth can not be negative."
                    );
            }
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
        }

        public bool Heal(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "BasicHealth. Heal amount can not be less or equal to zero."
                    );
            }
            if (IsAlive)
            {
                currentHealth = Math.Min(currentHealth + amount, maxHealth);
                return true;
            }
            return false;
        }

        public bool TakeDamage(int amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    "BasicHealth. Damage amount can not be less or equal to zero."
                    );
            }
            if (IsAlive)
            {
                currentHealth = Math.Max(currentHealth - amount, 0);
                return true;
            }
            return false;
        }
    }
}
