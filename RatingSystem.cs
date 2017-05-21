using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Threading;

namespace WpfTournament
{
    public delegate int RatingCompareDelegate(string Raiting1, string Raiting2);
    public delegate void EventDelegateWithPlayer(cPlayer CurrPlayer);

    [JsonObject]
    public class cGame : System.Collections.IComparer //управляет абсолютно всей информацией, необходимой для дальнейшего проведения турнира
    {
        [JsonIgnore]
        public string Name;
        public long MinID;
        public List<cPlayer> ListOfPlayers = new List<cPlayer>(); //подгрузится отдельно
        public RatingCompareDelegate RatingCompareFunction;
        public event EventDelegateWithPlayer PlayerWasAdded;

        public cGame()
        {
            //используем как стандартное значение
            RatingCompareFunction = GlobalFunctions.ChessRatingCompare;
        }

        public cGame(string Name)
        {
            this.Name = Name;
            RatingCompareFunction = GlobalFunctions.ChessRatingCompare;
        }

        public void AddPlayer(cPlayer PlayerObj)
        {
            ListOfPlayers.Add(PlayerObj);
            PlayerWasAdded(PlayerObj);
        }

        public void SortPlayersByName(int Direction)
        {
            if (Direction == GlobalConstansts.DirectionUp)
                ListOfPlayers.Sort(
                    delegate(cPlayer p1, cPlayer p2) { return String.Compare(p1.surname, p2.surname); }
                );
            else
                ListOfPlayers.Sort(
                delegate(cPlayer p1, cPlayer p2) { return String.Compare(p2.surname, p1.surname); }
            );
        }
        public void SortPlayersByAge(int Direction)
        {
            ListOfPlayers.Sort(delegate(cPlayer p1, cPlayer p2)
            {
                if (p1.age * Direction > p2.age * Direction)
                    return 1;
                else if (p1.age * Direction < p2.age * Direction)
                    return -1;
                else
                    return 0;
            });
        }
        public void SortPlayersByLevel(ListSortDirection Direction = ListSortDirection.Descending)
        {
            /*ListOfPlayers.Sort(delegate(cPlayer p1, cPlayer p2)
            {
                if(Direction==GlobalConstansts.DirectionUp)
                    return this.RatingCompareFunction(p1.Rating, p2.Rating);
                else
                    return this.RatingCompareFunction(p2.Rating, p1.Rating);
            });*/
            ListOfPlayers.Sort(delegate(cPlayer p1, cPlayer p2)
            {
                if (Direction == ListSortDirection.Ascending)
                    return this.RatingCompareFunction(p1.rating, p2.rating);
                else
                    return this.RatingCompareFunction(p2.rating, p1.rating);
            });
        }

        public void UpdatePlayersListByAnotherList(List<cPlayer> ListToAdd)
        {
            bool WasFound;
            for (int i = 0; i < ListToAdd.Count; i++)
            {
                WasFound = false;
                for (int j = 0; j < ListOfPlayers.Count; j++)
                {
                    if (ListOfPlayers[i].id == ListToAdd[i].id)
                    {
                        ListOfPlayers[i] = ListToAdd[j].Clone();
                        WasFound = true;
                        break;
                    }
                }
                if (!WasFound)
                {
                    ListOfPlayers.Add(ListToAdd[i]);
                }
            }

        }
        public void Reset()
        {
            ListOfPlayers.Clear();
        }

        public void CloneTo(ref cGame GameToCloneTo)
        {
            if (GameToCloneTo == null)
                GameToCloneTo = new cGame();

            GameToCloneTo.Name = this.Name;
            GameToCloneTo.RatingCompareFunction = this.RatingCompareFunction;

        }

        public long GetNextLocalID()
        {
            MinID--;
            return MinID;
        }
        //временно. нужно перенести в админку
        /*public void SaveToFile(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                byte[] CurrGameInJSON = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
                fs.Write(CurrGameInJSON, 0, CurrGameInJSON.Length);
            }
        }
        */
        public ListSortDirection SortDirection;
        public int Compare(object x, object y)
        {
            if (SortDirection == ListSortDirection.Ascending)
                return RatingCompareFunction(((cPlayer)x).rating, ((cPlayer)y).rating);
            else
                return RatingCompareFunction(((cPlayer)y).rating, ((cPlayer)x).rating);
        }
    }

    public class cGameShowInfo
    {
        public string ShowingName;
        public string FolderPath;
        public string DLLPath;
        public string PageHTMLString;

        public cGameShowInfo(string ShowName, string FolderPath, string DLLPath, string HTMLString)
        {
            this.ShowingName = ShowName;
            this.FolderPath = FolderPath;
            this.DLLPath = DLLPath;
            this.PageHTMLString = HTMLString;
        }
        public override string ToString()
        {
            return this.ShowingName;
        }
    }
}
