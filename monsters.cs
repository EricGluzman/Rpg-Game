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

        public Monster(string name, double hp, int MaxHealth, double dmg)
        {
            Health = hp;
            Damage = dmg;
            Name = name;
        }
        public static List<Monster> GetMonsterList()
        {
            return new List<Monster>
            {
                new Monster("Slime", 20, 20, 5),
                new Monster("Goblin", 50, 50, 12),
                new Monster("Skeleton", 40, 40, 15),
                new Monster("Orc", 100, 100, 20)
            };
        }
    }
}