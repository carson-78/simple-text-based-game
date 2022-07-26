using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Media;
namespace Game
{
    public class Program
    {
        // test12345
        public static Random r = new Random();
        public static Player currentPlayer = new Player();
        public static Encounter encounter = new Encounter();
        static DateTime time = DateTime.Now;
        //public static bool start = true;
        static void Main(string[] args)
        {
            RunMainCode();           
        }
        public static void RunMainCode()
        {            
            Program program = new Program();            
            WelcomeScreen();           
            if (!Directory.Exists("saves"))
                Directory.CreateDirectory("saves");
            currentPlayer = Load(out bool newP);
            ChangeClass(currentPlayer);
            WelcomeBack();
            if (newP)
                encounter.FirstEncounter();
            //
            while (currentPlayer.Level <= 20 || currentPlayer.BossEncounter < 1)
            {
                if (currentPlayer.Level < 20)
                    encounter.RandomEncounter();
                else if (currentPlayer.Level >= 20 && currentPlayer.BossEncounter < 1)
                {
                    encounter.Boss20Encounter();
                    currentPlayer.BossEncounter++;
                }
            }
            while (currentPlayer.Level > 20)
            {
                if (currentPlayer.Level > 20)
                    encounter.RandomEncounter();
            }           
            Console.ReadKey();
        }
        static void WelcomeScreen()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            if(time.Hour > 6 && time.Hour < 12)
                PrintAnimation("========== GOOD MORNING =========", 10);
            PrintAnimation("=========== WELCOME TO ==========",10);
            PrintAnimation("============ Dungeon ============",10);
            PrintAnimation("[ "+DateTime.Now.ToString("HH:mm:ss(fff) tt , dd/MMMM/yyyy")+" ]",10);            
            Console.WriteLine();
            Console.Write("Press any key to start the game");
            Console.ReadKey();
            Console.WriteLine("\nLoading...");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Thread.Sleep(1500);
        }
        static void WelcomeBack()
        {//13
            Console.Clear();
            int name = currentPlayer.Name.Length;
            int space = 11;
            Console.WriteLine("\\      /\\      / ");
            Console.WriteLine(" \\    /  \\    /  ____     ___  ___          ___ ");
            Console.WriteLine("  \\  /    \\  /   |__  |   |    |  | |\\  /|  |__ ");
            Console.WriteLine("   \\/      \\/    |___ |__ |__  |__| | \\/ |  |__ ");
            Console.WriteLine("=============================");
            if (space > name)
            {
                int space_left = space - name;
                if (space_left < 0)
                    space_left = 0;
                string s = new string(' ', space_left);
                Console.WriteLine($"| WELCOME BACK *{currentPlayer.Name}*{s}|");
            }
            Console.WriteLine("=============================");
            Thread.Sleep(1500);
            //Console.WriteLine("| WELCOME BACK " + currentPlayer.Name+"|");           
        }
        static Player NewStart(int i)
        {
            Player p = new Player();
            Console.WriteLine("\n=========WELCOME TO========");
            Console.WriteLine("======Ender's Dungeon======");
            Console.Write("Enter your name: ");
            p.Name = Console.ReadLine();
            
            bool p_class = false;
            while (p_class == false)
            {
                PrintAnimation("Class: Mage, Archer, Warrior");
                p_class = true;
                string input = Console.ReadLine().ToLower();
                if (input == "mage")
                    p.currentClass = Player.PlayerClass.Mage;
                else if (input == "archer")
                    p.currentClass = Player.PlayerClass.Archer;
                else if (input == "warrior")
                    p.currentClass = Player.PlayerClass.Warrior;
                else
                {
                    Console.WriteLine("Please choose a existing class!");
                    p_class = false;
                    Console.Clear();
                }
            }
            p.id = i;
            if (p.Name == "")
            {
                Console.WriteLine("You can't even remember your own name!...");
                p = NewStart(i);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You know your name is " + p.Name);
                Console.WriteLine("You awake in a cold, stone, dark room. You feel dazed, and are having trouble remembering ");
                Console.WriteLine("anything about your past.");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("You grope around in the darkness until you find a door handle. You feel some resistance as");
                Console.WriteLine("you turn the handle, but the rusty lock breaks with little effort. You see your captor");
                Console.WriteLine("standing with his back to you outside the door.");
                Console.ReadKey();
                Console.Clear();
            }
            return p;
        }
        public void Quit()
        {
            Save();
            Exit();           
        }
        public void Save()
        {
            BinaryFormatter binary = new BinaryFormatter();
            string path = "saves/" + currentPlayer.id.ToString();
            FileStream file = File.Open(path, FileMode.Create);
            binary.Serialize(file, currentPlayer);
            file.Close();
        }
        public static Player Load(out bool newP)
        {
            newP = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");     // get files in /saves

            List<Player> players = new List<Player>();        // create a players list

            BinaryFormatter binary = new BinaryFormatter();
            foreach (string path in paths)                    // foreach file in /saves
            {
                FileStream file = File.Open(path, FileMode.Open);     //file = open file
                Player p = (Player)binary.Deserialize(file);          //player = file
                file.Close();
                players.Add(p);                               // add player into *players list*

            }

            int idCount = players.Count;
            int idnum = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose your player > ");
                Console.WriteLine("ID : Name : Level");
                foreach (Player p in players)      // foreach player in *players list*
                {
                    Console.WriteLine(p.id + "  :  " + p.Name + "  :  " + p.Level+ "  :  "+p.currentClass);   // write id + name
                }
                Console.WriteLine("\n============================");
                Console.WriteLine("(ID # / (C)reate / (D)elete)");
                Console.Write("Input > ");
                string idoutput = Console.ReadLine();
                //string[] data = Console.ReadLine().Split(':');  // split the string with ':' id : 4                                                                                    // ======[0]:[1]
                try
                {
                    if (int.TryParse(idoutput, out int id)) // *idoutput* is string // if input number then convert successful
                    {                                       // convert STRING *idoutput* to (INT variable *id*)
                        foreach (Player p in players)       // for each player in *players list*
                        {
                            if (id == p.id)                 // if player.id == input id                            
                                return p;
                        }
                        Console.WriteLine("There is no player with that id");
                        Thread.Sleep(1000);
                    }
                    else if (idoutput == "c".ToLower())
                    {
                        // idnum = 0;
                        foreach (Player p in players)  // if idnum exits then idnum + 1
                        {
                            if (idnum == p.id)
                                idnum++;
                        }
                        Player newPlayer = NewStart(idnum);
                        newP = true;
                        return newPlayer;
                    }
                    else if (idoutput == "d".ToLower())
                        Delete();
                    else
                    {
                        Console.WriteLine("Your id needs to be a number! Press any key to continue!");
                        Console.ReadKey();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Your id needs to be a number! Press any key to continue!");
                    Console.ReadKey();
                }
            }
        }
        public static void Delete()
        {// file delete
            Console.WriteLine("\n============================");
            Console.WriteLine("Enter ID you want to delete");
            Console.WriteLine("Or type 'b' return to main_menu");
            Console.Write("Input > ");
            string delete = Console.ReadLine();
            if (delete == "b")   // if "b" then back
            {
                Console.Clear();
                RunMainCode();
            }
            else if (delete != "b")
            {
                if (File.Exists("saves/" + delete))
                    File.Delete("saves/" + delete);
                else
                {
                    Console.WriteLine("File not exits!");
                    Delete();
                }

            }
            Console.WriteLine("\nDelete successful!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
            RunMainCode();

        }  // file delete
        public static void PrintAnimation(string text, int time = 30)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(time);
            }
            Console.WriteLine();
        }
        static void ChangeClass(Player p)
        {
            bool flag = false;
            while (flag == false)
            if (currentPlayer.ChangeClass < 1)
            {
                flag = true;
                Console.WriteLine("==================================");
                Console.WriteLine("Do you  want to change class?");
                Console.Write("Y/N ?");
                string input = Console.ReadLine();
                if (input == "y")
                {
                    bool flag_1 = false;
                    while (flag_1 == false)
                    {
                        flag_1 = true;
                        Console.WriteLine("==================================");
                        Console.WriteLine("(M)age, (A)rcher, (W)arrior ? ");
                        Console.Write("> ");
                        
                        string class_input = Console.ReadLine();
                        if (class_input == "m")
                        { p.currentClass = Player.PlayerClass.Mage; p.ChangeClass++; }
                        else if (class_input == "a")
                        { p.currentClass = Player.PlayerClass.Archer; p.ChangeClass++; }
                        else if (class_input == "w")
                        { p.currentClass = Player.PlayerClass.Warrior; p.ChangeClass++; }
                        else
                        {
                            Console.WriteLine("Enter a correct class!");
                            Console.Clear();
                            flag_1 = false;
                        }
                    }

                }
                else if (input == "n")
                    continue;
                else
                    flag = false;
            }
            else
                break;            
        }
        public void ProgressBar(int value) // max 100
        {
            //30
            string lines = "--------------------"; //20
            string line = "-";
            string bar = "#";
            int diff = 0 + value; // if 50 or half
            int left = diff / 5;  // left = 50/5 = 10*
            int space = 20 - left;  // space = 20 - 10 = 10*
            if (value <= 4)
                Console.Write(lines);
            else
            {
                for (int x = 0; x < left; x++)
                {
                    Console.Write(bar);
                }
                for (int x = 0; x < space; x++)
                {
                    Console.Write(line);
                }
            }
            
        }
        static void Exit()
        {
            Console.Clear();           
            if (time.Hour > 20)
            {
                Console.WriteLine("===== Good Night! =====");
                Console.WriteLine("Press any key to exit...");
            }
            else
                Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }        
        void Countdount(int input_m, int input_s)
        {
            Console.Write("enter minute : ");
            input_m = Int32.Parse(Console.ReadLine());
            Console.Write("enter seconds : ");
            input_s = Int32.Parse(Console.ReadLine());
            bool first = true;
            for (int minute = input_m; minute >= 0; minute--)
            {
                if (first)
                {
                    for (int first_second = input_s; first_second >= 0; first_second--)
                    {
                        Console.WriteLine($"{minute}m : {first_second}s"); Thread.Sleep(1000);
                    }
                    first = false;
                }
                else
                {
                    for (int second = 59; second >= 0; second--)
                    {
                        Console.WriteLine($"{minute}m : {second}s"); Thread.Sleep(1000);
                    }
                }               
            }            
        }
        
    }    
}
