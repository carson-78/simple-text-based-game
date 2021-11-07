using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    [Serializable]
    public class Player
    {
        //Random r = new Random();
        public int id;
        private string _name;
        private int _coins = 10; //shop
        private int _health = 10;
        private int _damage = 1;
        private int _armorValue = 1;  //shop
        private int _potion = 5;  //shop
        private int _weaponValue = 1; //shop
        private int _experience = 1;
        private int _level = 1;
        private int _skill = 1;
        private int _changeclass = 0;
        private int _bossencounter = 0;
        public enum PlayerClass { Mage,Archer,Warrior};
        public PlayerClass currentClass = PlayerClass.Warrior;
        public string Name
        { get { return _name; } set { _name = value; } }
        public int ChangeClass
        { get { return _changeclass; } set { _changeclass = value; } }
        public int BossEncounter
        { get { return _bossencounter; } set { _bossencounter = value; } }
        public int Coins
        { get { return _coins; } set { _coins = value; } }
        public int Health
        { get { return _health; }set { _health = value; } }
        public int Damage
        { get { return _damage; }set { _damage = value; } }
        public int ArmorValue
        { get { return _armorValue; }set { _armorValue = value; } }
        public int Potion
        { get { return _potion; }set { _potion = value; } }
        public int WeaponValue
        { get { return _weaponValue; }set { _weaponValue = value; } }       
        public int Experience
        { get { return _experience; } set { _experience = value; } }
        public int Level
        { get { return _level; } set { _level = value; } }
        public int Skill
        { get { return _skill; }set { _skill = value; } }
        public int GetHealth()
        {
            int upper = (2 * Level + 5);
            int lower = (Level + 2);
            return Program.r.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = 0;
            int lower = 0;
            if (Level <= 5)
            {
                upper = (Level + 1);
                lower = (Level + WeaponValue) / 2;
                return Program.r.Next(lower,upper);
            }
            else if (Level > 5)
            {
                upper = Level;
                lower = Level / 2;
                return Program.r.Next(lower,upper);
            }
            return 5;

        }
        public int GetCoins(int coins)
        {
            int upper = (15* Level + 50);
            int lower = (10* Level + 10);
            return Program.r.Next(lower, upper)+coins;
        }
        public int GetExperience(int exp)
        {
            return Program.r.Next(25, 55)+exp;   
        }
    }
}
