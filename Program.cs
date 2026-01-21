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
using System.Threading;


namespace MyRPG
{
    class Program
    {
        static int currentLevel = 0;
        static async Task Main(string[] args)
        {
            await TypeWrite("WELCOME TO THE DUNGEON!");
            await TypeWrite("Enter Your Character Name: ");
            string username = Console.ReadLine();
    
            Player player = new Player(username);
    
            bool IsRunning = true;
            while (IsRunning)
            {
                await TypeWrite("\n--- Main Menu ---");
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
                        ErrorMessage("Please Enter A Number 1-3");
                    }
                }
                switch (choice)
                {
                    case 1:
                        await StartBattle(player);
                        break;
                    case 2:
                        await ShowStats(player);
                        break;
                    case 3:
                        IsRunning = false;
                        break;
                    case 1591:
                        player.EquipedWeapon = new Weapon("Admin Sword", 3000000, 1000000, 100, 100000);
                        break;
                    default:
                        ErrorMessage("Invalid Choice!");
                        break;
                }
            }
        }
        static async Task ShowStats(Player p)
        {
            Console.Clear();
            await TypeWrite("\n--- PLAYER STATS ---");
            await TypeWrite($"Name: {p.Name}", 10);
            await TypeWrite($"Health: {p.Health}/{p.MaxHealth}", 10);
            await TypeWrite($"Weapon Equipped: {p.EquipedWeapon.Name}", 10);
            await TypeWrite($"--- WEAPON STATS --- \nWeapon Min Damage: {p.EquipedWeapon.stats.MinDamage}", 10);
            await TypeWrite($"Weapon Max Damage: {p.EquipedWeapon.stats.MaxDamage}", 10);
            await TypeWrite($"Weapon Crit Damage: {p.EquipedWeapon.stats.MaxDamage * 1.5}", 10);
            await TypeWrite($"Weapon Value: {p.EquipedWeapon.stats.Value} \n", 10);
            Console.Write("Press Enter To Continue. ");
            Console.ReadLine();
        }
         static async Task StartBattle(Player p)
        {
            Console.Clear();
            List<Monster> allMonsters = Monster.GetMonsterList();
            Monster preset = allMonsters[currentLevel];
            Console.WriteLine($"\n{p.Name} Encounters A Wild Monster!");
            Console.WriteLine($"Monsters Name Is {preset.Name}");
            Thread.Sleep(1700);

            bool IsInFight = true;
            bool showTheMessage = false;
            while (IsInFight)
            {   
                if (currentLevel >= allMonsters.Count)
                {
                    Console.WriteLine("CONGRATULATIONS! You defeated the final boss!");
                    return;
                }
                else
                {
                    preset = allMonsters[currentLevel];
                }

                if(currentLevel != 0 && showTheMessage)
                {
                    Console.Clear();
                    await TypeWrite($"You Now Facing A {preset.Name}");
                    showTheMessage = false;
                    Thread.Sleep(500);
                }
                else if(showTheMessage == false)
                {
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine($"The {preset.Name} Is Still Alive");
                }

                int choice;
                while (true)
                {   
                    Console.WriteLine("Choose Your Move: \n1.Attack. \n2.Evade \n3.Run The Fight ");
                    if(int.TryParse(Console.ReadLine(), out int temp) && temp >= 1 && temp <= 3)
                    {
                        choice = temp;
                        break;
                    }
                    else
                    {
                       ErrorMessage("Please Enter A Number 1-3");
                    }
                }
                switch (choice)
                {
                    case 1:
                        await Attack(p, preset);
                        break;
                    case 2:
                        Evade(p);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("You Ran Away.");
                        IsInFight = false;
                        break;
                    default:
                        ErrorMessage("Please Enter A Number 1-3");
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
                    int User_Input;
                    while (true)
                    {
                        if(int.TryParse(Console.ReadLine(), out int temp))
                        {
                            User_Input = temp;
                            break;
                        }
                        else
                        {
                            ErrorMessage("Please Enter A Valid Number!");
                        }
                    }
                    switch (User_Input)
                    {
                        case 1:
                            currentLevel++;
                            preset = allMonsters[currentLevel]; 
                            showTheMessage = true;
                            continue; 
                        case 2:
                            preset.Health = preset.MaxHealth; 
                            continue;
                        case 3:
                            if(currentLevel != 0) {
                                currentLevel--;
                                preset = allMonsters[currentLevel];
                            }
                            else
                            {
                                Console.WriteLine("You Are At The Lowest Level Already.");
                                IsInFight = false;
                            }
                            continue;
                        case 4:
                            IsInFight = false;
                            break;
                        default:
                            ErrorMessage("Please Enter A Number 1-4");
                            break;
                    }
                    
                }
            }
        }
        static async Task Attack(Player p, Monster m)
        {   
            int CritCalculation = Random.Shared.Next(1,101);
            Console.Clear();
            double min = p.EquipedWeapon.stats.MinDamage;
            double max = p.EquipedWeapon.stats.MaxDamage;
            double damage = Math.Round(min + (Random.Shared.NextDouble() * (max - min)), 2);
            if(CritCalculation <= p.EquipedWeapon.stats.CritHitChance)
            {
              damage *= 1.5;
              Console.ForegroundColor = ConsoleColor.DarkCyan;
              await TypeWrite("Crit Damage!!", 100);
              Console.ResetColor();
            }  
            await TypeWrite($"{p.Name} Is Attacking And Hitting {damage:F2} Points Of Damage." , 12);
            m.Health -= damage;
            m.Health = Math.Max(0, m.Health);
            Console.WriteLine($"The Monster Have {m.Health:F2} Left. \n");
            if(m.Health <= 0)
            {
                Thread.Sleep(1700);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Congratulations You Won {m.Name}!");
                Console.ResetColor();
            }
            Thread.Sleep(500);
        }

        static void Evade(Player p)
        {
            Console.WriteLine("In near future");
        }
        static async Task MonsterAttack(Player p, Monster m)
        {   
            int CritHitChance = Random.Shared.Next(1,101);
            double min = m.Damage * 0.8;
            double max = m.Damage * 1.2;
            double damageChange = Math.Round(min + (Random.Shared.NextDouble() * (max - min)), 2);
            if(CritHitChance <= 100)
            {
                Thread.Sleep(150);
                damageChange *= 1.5;
                Console.ForegroundColor = ConsoleColor.Red;
                await TypeWrite("THE MONSTER GIVES A CRIT!", 80);
                Console.ResetColor();
            }
            else Console.WriteLine("Now The Monster Attacks!"); 
            Console.WriteLine($"Monster Deals A Damage! \n-{damageChange}\n");
            p.Health -= damageChange;
            Console.Write("Players Health: ");
            DrawHealthBar(p);
            Console.WriteLine();
            Thread.Sleep(500);
        }
        static async Task TypeWrite(string message, int speed = 30)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                await Task.Delay(speed); 
            }
            Console.WriteLine();
        }      
        static void ErrorMessage(string s)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        } 
        static void DrawHealthBar(Player p)
        {
            RenderBar(p.Health / p.MaxHealth, p.Health, p.MaxHealth);
        }
        static void DrawHealthBar(Monster m)
        {
            RenderBar(m.Health / m.MaxHealth, m.Health, m.MaxHealth);
        }
        static void RenderBar(double pr, double hp, int mxhp)
        {
            if (pr > 0.6) Console.ForegroundColor = ConsoleColor.Green;
            else if (pr > 0.3) Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.Red;
            int sections = 10;
            for (int i = 0; i < sections; i++)
            {
                if (i < pr*10) Console.Write("#");
                else Console.Write("-");
            }
            Console.WriteLine($" {hp:F2}/{mxhp} HP");
            Console.ResetColor();
        }
    }
}