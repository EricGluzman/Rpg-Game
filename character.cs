using System;

namespace character
{
    using guns;
    public class Player{
        public string Name;
        public double Health;
        public int MaxHealth;
        public double Exp;
        public Weapon EquipedWeapon;

        public Player(string name)
        {
            Name = name;
            Health = 100;
            MaxHealth = 100;
            Exp = 0;

            EquipedWeapon = new Weapon("Rusty Sword", 3, 1, 5, 0);
        }

    }
}