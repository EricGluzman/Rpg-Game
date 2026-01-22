using System;

namespace character
{
    using guns;
    public class Player{
        public string Name;
        public double Health;
        public double MaxHealth;
        public double Exp;
        public int ExpCap;
        public int Level;
        public int UpgradePoints;
        public Weapon EquipedWeapon;

        public Player(string name)
        {
            Name = name;
            Health = 100;
            MaxHealth = 100;
            Exp = 0;
            ExpCap = 150;
            Level = 1;
            UpgradePoints = 1;

            EquipedWeapon = new Weapon("Rusty Sword", 3, 1, 5, 0);
        }

    }
}