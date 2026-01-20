using System;

namespace guns
{
    public struct WeaponStats
    {
        public double MaxDamage;
        public double MinDamage;
        public int CritHitChance;
        public int Value;
    }

    public class Weapon
    {
        public string Name;
        public WeaponStats stats;

        public Weapon(string name, int maxdmg,int mindmg, int crit, int val)
        {
            Name = name;
            stats = new WeaponStats
            {
                MaxDamage = maxdmg,
                MinDamage = mindmg,
                CritHitChance = crit,
                Value = val,
            };
        }
    }
}