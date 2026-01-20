using System;

namespace Monsters
{
    using System.Reflection.Metadata.Ecma335;
    public class Monster
    {
        public double Health;
        public double Damage;
        public string Name;

        public Monster(string name, double hp, double dmg)
        {
            Health = hp;
            Damage = dmg;
            Name = name;
        }
        public static List<Monster> GetMonsterList()
        {
            return new List<Monster>
            {
                new Monster("Slime", 20, 5),
                new Monster("Goblin", 50, 12),
                new Monster("Skeleton", 40, 15),
                new Monster("Orc", 100, 20)
            };
        }
    }
}