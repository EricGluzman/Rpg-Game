using System;

namespace inventory
{
    using guns;
    using potion;
    public class Inventory
    {
        public List<Weapon> Weapons = new List<Weapon>();
        public List<Potion> Potions = new List<Potion>();
    }
}