using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Threading;

namespace Game
{
    public class Encounter
    {
        Shop shop = new Shop();        
        //Encounters
        public void FirstEncounter()
        {
            Console.WriteLine("You throw open the door and grab a rusty metal sword, while charging toward your captor");
            Console.WriteLine("He turns...");
            Console.ReadKey();
            Combat(false, "Human Rogue", 2, 4,false);
        }
        public void BasicFightEncounter()
        {
            Console.Clear();
            Console.WriteLine("You turn the corner and there you see a hulking beast...");
            Console.ReadKey();
            Combat(true, "", 0, 0,false);
        }
        public void WizardEncounter(Player p)
        {
            Console.Clear();
            Console.WriteLine("The door slowly creaks open as you peer into the dark room you see a tall man with a");
            Console.WriteLine("long beard looking at a large tome.");
            Console.ReadKey();
            Combat(false, "Dark Wizard", Program.r.Next(2+(p.Level*10)/50, 3+(p.Level*10)/20), 3+p.Level,false);
        }
        //Encounter Tools
        public void RandomEncounter()
        {           
             switch (Program.r.Next(0, 2))
             {
                 case 0:
                     BasicFightEncounter();
                     break;
                 case 1:
                     WizardEncounter(Program.currentPlayer);
                     break;
             }           
        }
        public void Boss20Encounter()
        {
            Console.Clear();
            Console.WriteLine("As you reach lvl 20, you encountered a boss name 'Dreadstep', it says");
            Console.WriteLine("'i always learn so much... from a live dissection...'");
            Console.ReadKey();
            Combat(false, "Dreadstep (LVL-20)", Program.r.Next(15,20), 135, true);        
        }
        public void Combat(bool random, string name, int power, int health, bool boss)
        {
            string n = "";
            int p = 0;
            int h = 0;
            if (random && !boss)
            {
                n = GetEnemyName();
                p = Program.currentPlayer.GetPower();
                h = Program.currentPlayer.GetHealth();
            }
            else if (!random && !boss)
            {
                n = name;
                p = power;
                h = health;
            }
            else if (!random && boss)
            {
                n = name; 
                p = power; 
                h = health;
            }
            while (h > 0 && Program.currentPlayer.Health > 0)
            {
                
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("BOSS = "+boss);
                Console.WriteLine("Enemy : "+n);
                Console.WriteLine("Enemy Power: "+p + " / Enemy HP: " + h);
                Console.ResetColor();
                PlayerStat(Program.currentPlayer);
                GuiAttack();
                Console.Write("= ");

                string input = Console.ReadLine().ToLower();
                if (input == "a")
                {//attack
                    Console.WriteLine("With haste you surge fourth, your sword flying in your hands! As you pass, the *" + n + "* strikes you as you pass");
                    int damage = p - Program.currentPlayer.ArmorValue - (Program.currentPlayer.Level/10);
                    if (damage < 0)
                        damage = 0;
                    int attack = 0;
                    if (Program.currentPlayer.Level == 1)
                        attack = Program.currentPlayer.WeaponValue + 1;
                    else if (Program.currentPlayer.Level == 2)
                        attack = Program.currentPlayer.WeaponValue + Program.r.Next(1, 3);
                    else if (Program.currentPlayer.Level == 3)
                        attack = Program.currentPlayer.WeaponValue + Program.r.Next(1, 4);
                    else if (Program.currentPlayer.Level >= 4)
                        attack = Program.currentPlayer.WeaponValue + Program.r.Next((Program.currentPlayer.Level/4), Program.currentPlayer.Level);

                    Console.WriteLine("You loss " + damage + " health and deal " + attack + " damage");
                    Program.currentPlayer.Health -= damage;
                    h -= attack;
                }// damage dealt = weponvalue + r.Next(0,Mod) // damage received = power - armorvalue
                else if (input == "d")
                {//defend
                    Console.WriteLine("As the " + n + " prepares to strike, you ready your sword in a defensive stance.");
                    int damage = (p / 4) - Program.currentPlayer.ArmorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = Program.r.Next(0, Program.currentPlayer.WeaponValue) / 2;
                    Console.WriteLine("You loss " + damage + " health and deal " + attack + " damage");
                    Program.currentPlayer.Health -= damage;
                    h -= attack;
                }
                else if (input == "r")
                {//run
                    if (Program.r.Next(0, 2) == 0)
                    {// fail escape
                        int damage = p - Program.currentPlayer.ArmorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("As you sprint aways from the " + n + ", its strike catches you in the back, sending you sprawling onto the ground.");
                        Console.WriteLine("You lose " + damage + " health and are unable to escape.");
                        Program.currentPlayer.Health -= damage;
                    }
                    else
                    {// success escape
                        Console.WriteLine("You use your crazy ninja moves to evade the " + n + " and you successfully escape!");
                        break;
                    }
                }
                else if (input == "h")
                {//heal
                    if (Program.currentPlayer.Potion == 0)
                    {// potion = 0
                        int damage = p - Program.currentPlayer.ArmorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("As you desperatly grab for a potion in your bag, all that you feel are empty glass flasks");
                        Console.WriteLine("The " + n + " strikes you with a mighty blow and you lose " + damage + " health!");
                        Program.currentPlayer.Health -= damage;
                    }
                    else
                    {// potion > 0
                        int potionValue = 5 + Program.currentPlayer.Level;
                        Console.WriteLine("You reach into your bag and pull out a glowing purple flask. You take a long drink");
                        Console.WriteLine("You gain " + potionValue + " health");
                        Program.currentPlayer.Health += potionValue;
                        Program.currentPlayer.Potion -= 1;
                        Console.WriteLine("As you were occupied, the " + n + " advanced and struck.");
                        int damage = (p / 2) - Program.currentPlayer.ArmorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine("You lose " + damage + " health.");
                        Program.currentPlayer.Health -= damage;
                    }
                }
                else if (input == "l")
                {
                    if (Program.currentPlayer.Skill <= 0)
                        Console.WriteLine("Skill is not available");
                    else
                    {
                        int damage = (p / 2) - Program.currentPlayer.ArmorValue;
                        if (damage < 0)
                            damage = 0;
                        int attack = Program.r.Next(Program.currentPlayer.Level * 2, (Program.currentPlayer.WeaponValue * 2) + (Program.currentPlayer.Level * 2));
                        Console.WriteLine($"Ultimate! {n} lose {attack} health");
                        Console.WriteLine($"You suffer {damage} damage!");
                        Program.currentPlayer.Health -= damage;
                        Program.currentPlayer.Skill -= 1;
                        h -= attack;
                    }
                }
                else if (input == "s")  // shop
                    shop.LoadShop(Program.currentPlayer);
                else if (input == "cheat") // cheat
                    Cheat(Program.currentPlayer);
                else
                    Console.WriteLine("Invalid input!");
                // die
                if (Program.currentPlayer.Health <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("As the " + n + " stands tall comes down to strike. You have then slayn by the mighty " + n);
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            if (!boss) // if not boss 
            {
                SystemSounds.Exclamation.Play();
                int coins = Program.currentPlayer.GetCoins(0);
                int exp = Program.currentPlayer.GetExperience(0);
                Console.WriteLine("==============================================================================================");
                Console.WriteLine($"As you stand victorious over the {n}, its body dissolves into {coins} gold coins!");
                Console.WriteLine($"You get {exp} Experience!");
                Program.currentPlayer.Coins += coins;
                Program.currentPlayer.Experience += exp;
                // level up after mob die
                if (Program.currentPlayer.Experience >= 100)
                {
                    Program.currentPlayer.Level += 1;
                    Program.currentPlayer.Experience -= 100;
                    Console.WriteLine("===============");
                    Console.WriteLine($"Level up {Program.currentPlayer.Level} !  |");
                    Console.WriteLine("===============");
                }
                Console.ReadKey();
            }
            else if (boss)  // if boss slayed
            {               
                SystemSounds.Exclamation.Play();
                int coins = Program.currentPlayer.GetCoins(500);
                int exp = Program.currentPlayer.GetExperience(200);
                Console.Clear();
                Program.PrintAnimation($"{n} BOSS DEFEATED!...");
                Thread.Sleep(2000);
                Console.WriteLine("==============================================================================================");
                Console.WriteLine($"As you stand victorious over the {n}, its body dissolves into {coins} gold coins!");
                Console.WriteLine($"You get {exp} Experience!");
                Program.currentPlayer.Coins += coins;
                Program.currentPlayer.Experience += exp;
                RandomDrop(Program.currentPlayer);
                // level up after encounter
                while (Program.currentPlayer.Experience >= 100)
                {                    
                    Program.currentPlayer.Level += 1;
                    Program.currentPlayer.Experience -= 100;                                            
                }
                Console.WriteLine("===============");
                Console.WriteLine($"Level up {Program.currentPlayer.Level} !  |");
                Console.WriteLine("===============");
                Console.ReadKey();
            }
        }
        void RandomDrop(Player p)
        {
            switch (Program.r.Next(0, 3))
            {
                case 0:
                    p.WeaponValue += 1;
                    Console.WriteLine("You gained +1 WeaponValue!");
                    break;
                case 1:
                    p.ArmorValue += 1;
                    Console.WriteLine("You gained +1 ArmorValue!");
                    break;
                case 2:
                    p.Potion += 5;
                    Console.WriteLine("You gained +5 Potion!");
                    break;
            }
        }
        void GuiAttack()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("==============================");
            Console.WriteLine("|  (A)ttack    (D)efend      |");
            Console.WriteLine("|  (R)un       (H)eal        |");
            Console.WriteLine("|  (S)hop      Skil(L)       |");
            Console.WriteLine("==============================");
            Console.ResetColor();
        }
        void PlayerStat(Player p)
        {
            Program a = new Program();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==============================");
            Console.WriteLine($"{p.Name}'s Stats //// {p.currentClass}");
            Console.WriteLine($"HP : " + p.Health + " / LVL : "+p.Level + " / EXP : "+p.Experience);
            Console.ResetColor();

            Console.WriteLine();
            a.ProgressBar(p.Experience);
            Console.Write("    XP BAR");
            Console.WriteLine();
            Console.WriteLine("______________________________");
            Console.WriteLine("Coins : " + p.Coins);
            Console.WriteLine("Potion : " + p.Potion);
            Console.WriteLine("Weapon_Strength : " + p.WeaponValue);
            Console.WriteLine("Armor_Strength : " + p.ArmorValue);
            Console.WriteLine("Skill : " + p.Skill);
            Console.WriteLine("______________________________");
            Console.WriteLine($"Boss Slained : {p.BossEncounter}");
        }      
        public string GetEnemyName()
        {
            switch (Program.r.Next(0, 4))
            {
                case 0:
                    return "Skeleton";
                case 1:
                    return "Zombie";
                case 2:
                    return "Human Cultist";
                case 3:
                    return "Grave Robber";
            }
            return "Human Rogue";
                
        }
        void Cheat(Player p)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("_name : "+p.Name);
                Console.WriteLine("_changeclass : "+p.ChangeClass);
                Console.WriteLine("_bossencounter : "+p.BossEncounter);
                Console.WriteLine("_coins : "+p.Coins);
                Console.WriteLine("_health : "+p.Health);
                Console.WriteLine("_damage : "+p.Damage);
                Console.WriteLine("_armorValue : "+p.ArmorValue);
                Console.WriteLine("_potion : "+p.Potion);
                Console.WriteLine("_weaponValue : "+p.WeaponValue);
                Console.WriteLine("_experience : "+p.Experience);
                Console.WriteLine("_level : "+p.Level);
                Console.WriteLine("_skill : "+p.Skill);
                string exit = Console.ReadLine();
                if (exit == "e")
                    break;
                else if (exit == "level")
                {
                    int level = (Int32.Parse(Console.ReadLine()));
                    if (level < 1)
                        level = 1;
                    p.Level = level;
                }
                else if (exit == "coins")
                {
                    int coins = (Int32.Parse(Console.ReadLine()));
                    p.Coins = coins;
                }
                else if (exit == "skill")
                    p.Skill += 5;
                else if (exit == "name")
                {
                    string name = Console.ReadLine();
                    p.Name = name;
                }

            }     
        }
    } 
}
