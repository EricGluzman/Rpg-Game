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
                new Monster("Wolf", 25, 25, 9, 30, 55),
                new Monster("Goblin", 50, 50, 12, 50, 70),
                new Monster("Skeleton", 40, 40, 15, 70, 100),
                new Monster("Cave Bat", 30, 30, 18, 100, 120),
                new Monster("Dire Wolf", 70, 70, 20, 150, 200),
                new Monster("Orc", 100, 100, 20, 250, 330),
                new Monster("Giant Spider", 170, 170, 25, 350, 530),
                new Monster("Orc Warrior", 250, 250, 35, 700, 1000),
                new Monster("Stone Golem", 360, 360, 20, 700, 1000),
                new Monster("Minotaur", 500, 500, 45, 2000, 5000)
            };
        }
    }
}