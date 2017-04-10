using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
//using System.Text;


namespace WpfTournament
{
    public delegate int RatingCompareDelegate(string Raiting1, string Raiting2);
    [JsonObject]
    public class cGame:IComparer<string> //управляет абсолютно всей информацией, необходимой для дальнейшего проведения турнира
    {
        [JsonIgnore]
        public string Name;
        private List<cPlayer> PlayersOfGame; //подгрузится отдельно
        public RatingCompareDelegate RatingCompareFunction;
        int IComparer<string>.Compare(string x, string y)
        {
            return RatingCompareFunction(x,y);
        }

        public cGame()
        {
            PlayersOfGame = new List<cPlayer>();
            //используем как стандартное значение
            RatingCompareFunction = GlobalFunctions.ChessRatingCompare;
        }

        public cGame(string Name)
        {
            this.Name = Name;
            PlayersOfGame = new List<cPlayer>();
        }

        public void SortPlayersByName(int Direction)
        {
            if (Direction == GlobalConstansts.DirectionUp)
                PlayersOfGame.Sort(
                    delegate(cPlayer p1, cPlayer p2){ return String.Compare(p1.Surname, p2.Surname);}
                );
            else
                PlayersOfGame.Sort(
                delegate(cPlayer p1, cPlayer p2){return String.Compare(p2.Surname, p1.Surname);}
            );
        }
        public void SortPlayersByAge(int Direction)
        {
            PlayersOfGame.Sort(delegate(cPlayer p1, cPlayer p2)
            {
                if (p1.Age*Direction > p2.Age*Direction)
                    return 1;
                else if (p1.Age*Direction < p2.Age*Direction)
                    return -1;
                else
                    return 0;
            });
        }
        public void SortPlayersByLevel(int Direction)
        {
            PlayersOfGame.Sort(delegate(cPlayer p1, cPlayer p2) {
                if(Direction==GlobalConstansts.DirectionUp)
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

        //временно. нужно перенести в админку
        public void SaveToFile(string FileName)
        {
            using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                byte[] CurrGameInJSON = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this));
                fs.Write(CurrGameInJSON, 0, CurrGameInJSON.Length);
            }
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
