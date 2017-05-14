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
        public List<cPlayer> ListOfPlayers; //подгрузится отдельно
        public RatingCompareDelegate RatingCompareFunction;
        public event EventDelegateWithPlayer PlayerWasAdded;

        public cGame()
        {
            ListOfPlayers = new List<cPlayer>();
            //используем как стандартное значение
            RatingCompareFunction = GlobalFunctions.ChessRatingCompare;
        }

        public cGame(string Name)
        {
            this.Name = Name;
            ListOfPlayers = new List<cPlayer>();
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
                    delegate(cPlayer p1, cPlayer p2) { return String.Compare(p1.Surname, p2.Surname); }
                );
            else
                ListOfPlayers.Sort(
                delegate(cPlayer p1, cPlayer p2) { return String.Compare(p2.Surname, p1.Surname); }
            );
        }
        public void SortPlayersByAge(int Direction)
        {
            ListOfPlayers.Sort(delegate(cPlayer p1, cPlayer p2)
            {
                if (p1.Age * Direction > p2.Age * Direction)
                    return 1;
                else if (p1.Age * Direction < p2.Age * Direction)
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
                if(Direction==ListSortDirection.Ascending)
                    return this.RatingCompareFunction(p1.Rating, p2.Rating);
                else
                    return this.RatingCompareFunction(p2.Rating, p1.Rating);
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
                    if (ListOfPlayers[i].ID == ListToAdd[i].ID)
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

        //временно. нужно перенести в админку
        public void SaveToFile(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                byte[] CurrGameInJSON = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
                fs.Write(CurrGameInJSON, 0, CurrGameInJSON.Length);
            }
        }

        public ListSortDirection SortDirection;
        public int Compare(object x, object y)
        {
            if (SortDirection == ListSortDirection.Ascending)
                return RatingCompareFunction(((cPlayer)x).Rating, ((cPlayer)y).Rating);
            else
                return RatingCompareFunction(((cPlayer)y).Rating, ((cPlayer)x).Rating);
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
