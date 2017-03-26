using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTournament
{
    public class Player
    {
        public ulong ID;
        public string Name;
        public string Surname;
        public int Age;
        public string Rating;
        public string OtherInfo;
        public List<string> GamesIDs = new List<string>(); 

        public Player Clone()
        {
            return (Player)this.MemberwiseClone();
        }
    }

}
