using System;
using System.Security;
using Microsoft.VisualBasic;
using character; 
using guns;
using Monsters;
using System.Diagnostics;
using System.Data;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Numerics;
using System.Runtime.Versioning;

namespace MyRPG
{
    class Program
    {
        static int currentLevel = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO THE DUNGEON!");
            Console.WriteLine("Enter Your Character Name: ");
            string username = Console.ReadLine();
    
            Player player = new Player(username);
    
            bool IsRunning = true;
            while (IsRunning)
            {
                Console.WriteLine("\n--- Main Menu ---");
                Console.WriteLine("1. Fight Monster");
                Console.WriteLine("2. Check Stats");
                Console.WriteLine("3. Exit Game");
                int choice;
                while (true)
                {
                    if(int.TryParse(Console.ReadLine(), out int temp)){
                        choice = temp;
                        break;
                    }
                    else {
                        Console.WriteLine("Please Enter A Number 1-3");
                    }
                }
                switch (choice)
                {
                    case 1:
                        StartBattle(player);
                        break;
                    case 2:
                        ShowStats(player);
                        break;
                    case 3:
                        IsRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
        static void ShowStats(Player p)
        {
            Console.WriteLine("\n--- PLAYER STATS ---");
            Console.WriteLine($"Name: {p.Name}");
            Console.WriteLine($"Health: {p.Health}/{p.MaxHealth}");
            Console.WriteLine($"Weapon Equipped: {p.EquipedWeapon.Name}");
            Console.WriteLine($"--- WEAPON STATS --- \nWeapon Min Damage: {p.EquipedWeapon.stats.MinDamage}");
            Console.WriteLine($"Weapon Max Damage: {p.EquipedWeapon.stats.MaxDamage}");
            Console.WriteLine($"Weapon Crit Damage: {p.EquipedWeapon.stats.MaxDamage * 1.5}");
            Console.WriteLine($"Weapon Value: {p.EquipedWeapon.stats.Value}");
        }
         static void StartBattle(Player p)
        {
            List<Monster> allMonsters = Monster.GetMonsterList();
            if (currentLevel >= allMonsters.Count)
            {
                Console.WriteLine("CONGRATULATIONS! You defeated the final boss!");
                return;
            }
            
            bool IsInFight = true;
            while (IsInFight)
            {   
                Monster preset = allMonsters[currentLevel];
                Console.WriteLine($"\n{p.Name} Encounters A Wild Monster!");
                Console.WriteLine($"Monsters Name Is {preset.Name}");
                int choice = 3;
                while (true && preset.Health > 0)
                {
                    Console.WriteLine("Choose Your Move: \n1.Attack. \n2.Evade \n3.Run The Fight ");
                    if(int.TryParse(Console.ReadLine(), out int temp) && temp >= 1 && temp <= 3)
                    {
                        choice = temp;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Enter Valid Number Between 1-3. ");
                    }
                }
                switch (choice)
                {
                    case 1:
                        Attack(p, preset);
                        break;
                    case 2:
                        Evade(p);
                        break;
                    case 3:
                        Console.WriteLine("You Ran Away Your Stats Are: ");
                        ShowStats(p);
                        IsInFight = false;
                        break;
                    default:
                        Console.WriteLine("Enter A Number Between The Range 1-3");
                        break;
                }
                if(preset.Health > 0 && choice != 3)
                {
                    MonsterAttack(p, preset);
                }
                else if(preset.Health <= 0)
                {
                    Console.WriteLine("Do You Want To: \n1. Continue To Fight Next Level Monster");
                    Console.WriteLine("2. Continue To Fight The Same Level Monster");
                    Console.WriteLine("3. Continue To Fight The Previous Level Monster ");
                    Console.WriteLine("4. Go Back To Lobby");
                    int User_Input = 3;
                    while (true)
                    {
                        if(int.TryParse(Console.ReadLine(), out int temp))
                        {
                            User_Input = temp;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Enter A Valid Number!");
                        }
                    }
                    switch (User_Input)
                    {
                        case 1:
                            currentLevel++;
                            break;
                        case 3:
                            if(currentLevel != 0)
                            {
                                currentLevel--;
                            }
                            else
                            {
                                Console.WriteLine("You Are At The Lowest Level Already.");
                                IsInFight = false;
                            }
                            break;
                        case 4:
                            IsInFight = false;
                            break;
                        default:
                            Console.WriteLine("Enter Number 1-4 !!!!!!!");
                            break;
                    }
                }
            }
        }
        static void Attack(Player p, Monster m)
        {   
            double min = p.EquipedWeapon.stats.MinDamage;
            double max = p.EquipedWeapon.stats.MaxDamage;
            double damage = min + (Random.Shared.NextDouble() * (max - min));
            Console.WriteLine($"{p.Name} Is Attacking And Hitting {damage} Points Of Damage.");
            m.Health -= damage;
            Console.WriteLine($"The Monster Have {m.Health} Left.");
            if(m.Health <= 0)
            {
                Console.WriteLine($"Congratulations You Won {m.Name}!");
            }
        }

        static void Evade(Player p)
        {
            Console.WriteLine("In near future");
        }
        static void MonsterAttack(Player p, Monster m)
        {   
            double min = m.Damage * 0.8;
            double max = m.Damage * 1.2;
            double damageChange = min + (Random.Shared.NextDouble() * (max - min));
            Console.WriteLine("Now The Monster Attacks!");
            Console.WriteLine($"Monster Dels A Damage! \n-{damageChange}");
            p.Health -= damageChange;
            Console.WriteLine($"Now You Have {p.Health} HP Left");
        }
    }
}