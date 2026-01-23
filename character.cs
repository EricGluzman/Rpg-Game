using System;

namespace character
{
    using guns;
    using potion;
    using inventory;
    public class Player{
        public string Name;
        public double Health;
        public double MaxHealth;
        public double Exp;
        public double ExpCap;
        public int Level;
        public int UpgradePoints;
        public Weapon EquipedWeapon;
        public int EvadeChance;
        public Inventory user_Inventory;

        public Player(string name)
        {
            Name = name;
            Health = 100;
            MaxHealth = 100;
            Exp = 0;
            ExpCap = 200;
            Level = 1;
            UpgradePoints = 1;
            EvadeChance = 15;

            user_Inventory = new Inventory();
            user_Inventory.Potions.Add(new Potion ("Health Potion", 30,0,0,-1));
            user_Inventory.Weapons.Add(new Weapon ("Rusty Sword", 3, 1, 5, 0));
            user_Inventory.Weapons.Add(new Weapon ("Rusty Nigga", 3, 1, 5, 0));

            EquipedWeapon = user_Inventory.Weapons[0];
        }

    }
}