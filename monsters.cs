using System;

namespace Monsters
{
    using System.Reflection.Metadata.Ecma335;
    public class Monster
    {
        public double Health;
        public int MaxHealth;
        public double Damage;
        public string Name;
        public double ExpGivenMin;
        public double ExpGivenMax;

        public Monster(string name, double hp, int maxhp, double dmg, double xpmin , double xpmax)
        {
            Health = Math.Round(hp, 2);
            Damage = Math.Round(dmg, 2);
            Name = name;
            MaxHealth = maxhp;
            ExpGivenMin = xpmin;
            ExpGivenMax = xpmax;

        }
        public static List<Monster> GetMonsterList()
        {
            return new List<Monster>
            {
                new Monster("Slime", 20, 20, 5, 10, 20),
                new Monster("Goblin", 50, 50, 12, 50, 70),
                new Monster("Skeleton", 40, 40, 15, 70, 100),
                new Monster("Orc", 100, 100, 20, 250, 330)
            };
        }
    }
}