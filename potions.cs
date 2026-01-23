using System;

namespace potion
{
    public struct potions
    {
        public int Health;
        public int Damage;
        public int CritChance;
        public int Time;
    }
    public class Potion
    {
        public string Name;
        public potions stats;

        public Potion(string name, int hp, int damage,int crit, int time)
        {
            Name = name;
            stats = new potions
            {
                Health = hp,
                Damage = damage,
                CritChance = crit,
                Time = time
            };
        }

    }
}