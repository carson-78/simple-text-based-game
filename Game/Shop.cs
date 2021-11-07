using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class Shop
    {
        public void LoadShop(Player p)
        {
            //armorMod = p.ArmorValue;
            //weaponMod = p.WeaponValue;
            //difMod = p.Mod;
            RunShop(p);
        }
        public void RunShop(Player p)
        {
            Program program = new Program();
            int potionPrice;
            int armorPrice;
            int weaponPrice;
            int skillPrice;
            while (true)
            {                
                potionPrice = 20 + 10 * p.Level;
                armorPrice = 100 * (p.ArmorValue+1);
                weaponPrice = 100 * p.WeaponValue;
                skillPrice = 500 + (p.Level * 20);

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("==========================");
                Console.WriteLine("========== SHOP ==========");
                Console.WriteLine("=========================="); 
                Console.WriteLine("| (P)otion  $" + potionPrice); 
                Console.WriteLine("| (W)eapon  $" + weaponPrice); 
                Console.WriteLine("| (A)rmor   $" + armorPrice);
                Console.WriteLine("| (S)kill   $" + skillPrice);
                Console.WriteLine("==========================");
                Console.WriteLine("| (E)xit ");
                Console.WriteLine($"| (Q)uit and save as {p.Name}");
                Console.WriteLine("==========================");
                //
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;                
                Console.WriteLine($"{p.Name}'s Stats");
                Console.WriteLine("__________________________");
                Console.WriteLine("Coins : " + p.Coins);
                Console.WriteLine("Potion : " + p.Potion) ;
                Console.WriteLine("WeaponStrength : "+p.WeaponValue);
                Console.WriteLine("ArmorStrength : "+p.ArmorValue);
                Console.WriteLine("Skill : " + p.Skill);
                Console.WriteLine("==========================");
                Console.Write(">>>>> : ");
                Console.ResetColor();

                string input = Console.ReadLine().ToLower();
                if (input == "p")// potion
                    Buy(input, potionPrice, p);
                else if (input == "w")// weapon value
                    Buy(input, weaponPrice, p);
                else if (input == "a")// armor value
                    Buy(input, armorPrice, p);
                else if (input == "s")// armor value
                    Buy(input, skillPrice, p);
                else if (input == "q")
                {
                    bool quit = true;
                    while (quit)   // quit loop
                    {
                        Console.WriteLine($"Are you sure want to save as {p.Name}");
                        Console.Write("Y/N > ");
                        string input_quit = Console.ReadLine().ToLower();
                        if (input_quit == "y")
                        {
                            Console.WriteLine("\n(M)ain menu or (Q)uit ?");
                            Console.Write("M/Q > ");
                            string menu = Console.ReadLine();
                            if (menu == "m")
                            {
                                quit = false;
                                program.Save();
                                Console.Clear();
                                Program.RunMainCode();
                            }
                            else if (menu == "q")
                                program.Quit();
                            else
                                Console.WriteLine("Not valid input!\n");
                        }
                        else if (input_quit == "n")
                            quit = false; // quit loop                                                                             
                        else
                            Console.WriteLine("Not valid input!\n");                                                                                                  
                    }
                }
                else if (input == "e")
                    break;
            }
        }
        
        void Buy(string item, int cost,Player p)
        {
            if (p.Coins >= cost)
            {
                if (item == "p")
                {
                    p.Potion++;
                    Console.WriteLine("Purchase successful!");
                    Console.ReadKey();
                }
                else if (item == "w")
                {
                    p.WeaponValue++;
                    Console.WriteLine("Purchase successful!");
                    Console.ReadKey();
                }
                else if (item == "a")
                {
                    p.ArmorValue++;
                    Console.WriteLine("Purchase successful!");
                    Console.ReadKey();
                }
                else if (item == "s")
                {
                    p.ArmorValue++;
                    Console.WriteLine("Purchase successful!");
                    Console.ReadKey();
                }
                p.Coins -= cost;
            }
            else
            {
                Console.WriteLine("Not enough gold");
                Console.ReadKey();
            }
        }


    }
}
