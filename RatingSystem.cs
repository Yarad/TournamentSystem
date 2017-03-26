using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTournament
{
    public delegate int SortByRatingDel(string Raiting1, string Raiting2);

    class Game
    {
        public string Name;
        public List<Player> PlayersOfGame;
        private SortByRatingDel RatingCompareFunction;

        public Game(string Name,SortByRatingDel RatingCompareFunction)
        {
            this.RatingCompareFunction = RatingCompareFunction;
            this.Name = Name;

            PlayersOfGame = new List<Player>();
        }

        public void SortByName()
        {
            PlayersOfGame.Sort(
                delegate(Player p1, Player p2)
                {
                    return String.Compare(p1.Surname, p2.Surname);
                }
            );
        }
        public void SortByAge()
        {
            PlayersOfGame.Sort(delegate(Player p1, Player p2)
            {
                if (p1.Age > p2.Age)
                    return 1;
                else if (p1.Age < p2.Age)
                    return -1;
                else
                    return 0;
            });
        }
        public void SortByLevel()
        {
            PlayersOfGame.Sort(delegate(Player p1, Player p2) { return this.RatingCompareFunction(p1.Rating, p2.Rating); });
        }

        public void UpdateByAnotherList(List<Player> ListToAdd)
        {
            bool WasFound;
            for (int i = 0; i < ListToAdd.Count; i++)
            {
                WasFound = false;
                for (int j = 0; j < PlayersOfGame.Count; j++)
                {
                    if (PlayersOfGame[i].ID == ListToAdd[i].ID)
                    {
                        PlayersOfGame[i] = ListToAdd[j].Clone();
                        WasFound = true;                           
                        break;
                    }
                }
                if (!WasFound)
                {
                    PlayersOfGame.Add(ListToAdd[i]);
                }
            }

        }
    }

}
