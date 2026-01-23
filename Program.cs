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
                Console.Clear();
                await newLevelCheck(player);
                await TypeWrite("\n--- Main Menu ---");
                Console.WriteLine("1. Fight Monster");
                Console.WriteLine("2. Check Stats");
                Console.WriteLine("3. Point Investment");
                Console.WriteLine("4. Inventory");
                Console.WriteLine("5. Exit Game");
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
                        LevelUpSystem(player);
                        break;
                    case 4:
                        await Inventory(player, false);
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("See Ya Soon.");
                        IsRunning = false;
                        break;
                    case 1591:
                        player.user_Inventory.Weapons.Add(new Weapon("Admin Sword", 3000000, 1000000, 100, 100000));
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
            await TypeWrite($"Health: {p.Health:F2}/{p.MaxHealth:F2}", 10);
            await TypeWrite($"Exp: {p.Exp:F2}/{p.ExpCap:F2}");
            await TypeWrite($"Level: {p.Level}");
            await TypeWrite($"Weapon Equipped: {p.EquipedWeapon.Name}", 10);
            await TypeWrite($"--- WEAPON STATS --- \nWeapon Min Damage: {p.EquipedWeapon.stats.MinDamage:F2}", 10);
            await TypeWrite($"Weapon Max Damage: {p.EquipedWeapon.stats.MaxDamage:F2}", 10);
            double temp = p.EquipedWeapon.stats.MaxDamage * 1.5;
            await TypeWrite($"Weapon Crit Damage: {temp:F2}", 10);
            await TypeWrite($"Weapon Crit Chance: {p.EquipedWeapon.stats.CritHitChance:F0}%", 10);
            await TypeWrite($"Weapon Value: {p.EquipedWeapon.stats.Value:F2} \n", 10);
            Console.Write("Press Enter To Continue. ");
            Console.ReadLine();
        }
         static async Task StartBattle(Player p)
        {
            bool evaded = false;
            Console.Clear();
            List<Monster> allMonsters = Monster.GetMonsterList();
            Monster preset = allMonsters[currentLevel];
            Console.WriteLine($"\n{p.Name} Encounters A Wild Monster!");
            Console.WriteLine($"Monsters Name Is {preset.Name}");
            Thread.Sleep(1300);

            bool IsInFight = true;
            bool showTheMessage = false;
            while (IsInFight)
            {   
                if(p.Health <= 0)
                {
                    ErrorMessage("You Have lost.");
                    Thread.Sleep(1500);
                    p.Exp -= p.Exp * 0.5;
                    p.Health = p.MaxHealth;
                    IsInFight = false;
                    return;
                }
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
                        evaded = await Evade(p,evaded);
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
                if(preset.Health > 0 && choice != 3 || preset.Health > 0 && evaded)
                {
                    await MonsterAttack(p, preset);
                }
                else if(preset.Health <= 0)
                {
                    Exp(p,preset);
                    Console.WriteLine("Press Enter To Continue: ");
                    Console.ReadLine();
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
                            if(currentLevel != allMonsters.Count()-1)
                            {
                                currentLevel++;
                                preset = allMonsters[currentLevel]; 
                                showTheMessage = true;
                            }
                            else Console.WriteLine("No Monster`s Left To Fight."); preset.Health = preset.MaxHealth; Thread.Sleep(1000);
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

        static async Task<bool> Evade(Player p , bool evaded)
        {
            int Evaded = Random.Shared.Next(1,101);
            if(Evaded <= p.EvadeChance) evaded = true;
            if (evaded) await TypeWrite("You Evaded Successfully!");
            else await TypeWrite("You Evaded Unsuccessfully!");
            Thread.Sleep(1200);
            await Inventory(p,true);
            Console.Clear();
            return evaded;
        }
        static async Task MonsterAttack(Player p, Monster m)
        {   
            int CritHitChance = Random.Shared.Next(1,101);
            double min = m.Damage * 0.8;
            double max = m.Damage * 1.2;
            double damageChange = Math.Round(min + (Random.Shared.NextDouble() * (max - min)), 2);
            if(CritHitChance <= 5)
            {
                Thread.Sleep(150);
                damageChange *= 1.5;
                Console.ForegroundColor = ConsoleColor.Red;
                await TypeWrite("THE MONSTER GIVES A CRIT!", 80);
                Console.ResetColor();
            }
            else Console.WriteLine("Now The Monster Attacks!"); 
            Console.WriteLine($"Monster Deals A Damage! \n-{damageChange:F2}\n");
            p.Health -= damageChange;
            Console.Write("Players Health: ");
            DrawHealthBar(p);
            Console.WriteLine();
            Console.Write("Press Enter To Continue: ");
            Console.ReadLine();
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
        static void RenderBar(double pr, double hp, double mxhp)
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

        static void Exp(Player p, Monster m)
        {
            double theGivenExp = Math.Round(m.ExpGivenMin + (Random.Shared.NextDouble() * (m.ExpGivenMax - m. ExpGivenMin)),2);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"You Recived {theGivenExp} Exp From The Monster. ");
            Console.ResetColor();
            p.Exp += theGivenExp;
        }

        static void LevelUpSystem(Player p){
            bool Running = true;
           while (Running)
           {
             int players_chooice;
             Console.Clear();
             Console.WriteLine($"You Have {p.UpgradePoints} Upgrade Points Left To Use.");
             Console.WriteLine("Choose Where To Invest: ");
             Console.WriteLine("1. Damage: Multiply By 1.1 The Given Damage.");
             Console.WriteLine("2. Health: Multiply By 1.1 The Max Health.");
             Console.WriteLine("3. Crit Chance: +1 To The WEAPON Crit Chance.");
             Console.WriteLine("4. Back To Main Menu.");
             while (true)
             {
                 if(int.TryParse(Console.ReadLine(), out int temp))
                 {
                     players_chooice = temp;
                     break;
                 }
                 else
                 {
                     ErrorMessage("Enter A Valid Input!");
                 }
             }
             if(p.UpgradePoints <= 0 && players_chooice != 4)
                {
                    ErrorMessage("No Points Left To Invest. ");
                    Thread.Sleep(1230);
                    break;
                }
             switch (players_chooice)
             {
                 case 1:
                     p.EquipedWeapon.stats.MaxDamage *= 1.1;
                     p.EquipedWeapon.stats.MinDamage *= 1.1;
                     Console.WriteLine($"The Weapon Now Deals {p.EquipedWeapon.stats.MaxDamage:F2} Of Max Damage.");
                     p.UpgradePoints -= 1;
                     Thread.Sleep(1250);
                     break;
                 case 2:
                     p.MaxHealth *= 1.1;
                     Console.WriteLine($"Your Max Health Is Now {p.MaxHealth:F0}");
                     p.UpgradePoints -= 1;
                     Thread.Sleep(1250);
                     break;
                 case 3:
                     p.EquipedWeapon.stats.CritHitChance += 1;
                     Console.WriteLine($"Your Weapon Crit Chance Now Is {p.EquipedWeapon.stats.CritHitChance:F0}");
                     p.UpgradePoints -= 1;
                     Thread.Sleep(1250);
                     break;
                 case 4:
                     Running = false;
                     break;
                 default:
                     ErrorMessage("Enter A Number 1-3");
                     Thread.Sleep(1700);
                     break;
             }
           }
        }
        static async Task Inventory(Player p, bool b)
        {
            bool isRunning = true;
            Console.Clear();
            await TypeWrite(" --- INVENTORY --- \n");
            Console.WriteLine("  --- Weapons --- ");
            for(int i = 0; i < p.user_Inventory.Weapons.Count(); i++)
            {
                Console.Write($"{i+1}. {p.user_Inventory.Weapons[i].Name} ");
                if(i % 5 == 0 && i != 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n  --- Potions --- ");
            for(int i = 0; i < p.user_Inventory.Potions.Count(); i++)
            {
                Console.Write($"{i+1}. {p.user_Inventory.Potions[i].Name} ");
                if(i % 5 == 0 && i != 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
             Thread.Sleep(200);
            while (isRunning)
            {
                await TypeWrite("You Want To Choose 1. Weapon 2. Potion 3. Back");
                int players_chooice;
               while (true)
               {
                 if(int.TryParse(Console.ReadLine(), out int temp))
                 {
                     players_chooice = temp;
                     break;
                 }
                 else ErrorMessage("Enter A Valid Input");
               }
                switch (players_chooice)
                {
                    case 1:
                        int chooice;
                          while (true)
                            {
                                await TypeWrite($"Chooice A Weapon 1-{p.user_Inventory.Weapons.Count()}");
                                if(int.TryParse(Console.ReadLine(), out int temp) && temp >= 1 && temp <= p.user_Inventory.Weapons.Count()) 
                                {
                                    chooice = temp;
                                    break;
                                }
                                else ErrorMessage("Enter A Valid Input");
                            }
                        p.EquipedWeapon = p.user_Inventory.Weapons[chooice-1];
                        await TypeWrite($"Weapon Equiped: {p.EquipedWeapon.Name}");
                        Console.Write("Press Enter To Continue: ");
                        Console.ReadLine();
                        break;
                    case 2:
                        int cchooice;
                          while (true)
                            {
                                await TypeWrite($"Chooice A Potion 1-{p.user_Inventory.Potions.Count()}");
                                if(int.TryParse(Console.ReadLine(), out int temp)) 
                                {
                                    cchooice = temp;
                                    break;
                                }
                                else ErrorMessage("Enter A Valid Input");
                            }
                       if (p.user_Inventory.Potions.Count() != 0)
                       {
                         var user_potion = p.user_Inventory.Potions[cchooice-1];
                         if (user_potion.Name.Contains("Health"))
                         {
                             p.Health += user_potion.stats.Health; 
                             p.user_Inventory.Potions.RemoveAt(cchooice-1);
                         } 
                         else if(user_potion.Name.Contains("Damage")) {
                             p.EquipedWeapon.stats.MaxDamage *= user_potion.stats.Damage;
                             p.EquipedWeapon.stats.MinDamage *= user_potion.stats.Damage;
                             p.user_Inventory.Potions.RemoveAt(cchooice-1);
                         }
                         else if(user_potion.Name.Contains("Crit")) {
                             p.EquipedWeapon.stats.CritHitChance *= user_potion.stats.CritChance;
                             p.user_Inventory.Potions.RemoveAt(cchooice-1);
                         }
                        
                         await TypeWrite($"Potion Used: {user_potion.Name}");
                         Console.Write("Press Enter To Continue: ");
                         Console.ReadLine();
                       }
                        else ErrorMessage("No Potions Left To Use.");
                        break;
                    case 3:
                        isRunning = false;
                        break;
                    default:
                        ErrorMessage("Enter A Valid Input.");
                        break;
                }
            }
        }
        static async Task newLevelCheck(Player p)
        {
           while (p.Exp >= p.ExpCap)
           {
                p.Exp -= p.ExpCap;
                p.ExpCap *= 1.5;
                p.Level++;
                p.UpgradePoints++;
                await TypeWrite($"Congradulation You`ve Reached New Level {p.Level}");
                await TypeWrite($"You Now Have {p.UpgradePoints} Upgrade Points.");
                Thread.Sleep(800);
           }
        }
    }
}