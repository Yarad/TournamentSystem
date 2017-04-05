using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace WpfTournament
{
    public class cPlayer
    {
        public ulong ID;
        public string Name;
        public string Surname;
        public int Age;
        public string Rating;
        public string OtherInfo;
        public List<string> GamesIDs = new List<string>(); 

        public cPlayer Clone()
        {
            return (cPlayer)this.MemberwiseClone();
        }
    }

    class cGamesInfoLoader
    {
        public string DefaultPageHTML;
        public List<cGameShowInfo> ExistedGames;

        public cGamesInfoLoader()
        {
            ExistedGames = new List<cGameShowInfo>();
        }

        //выясняет, какие на диске есть игры
        public void UpdateGamesInfo_local() //готово
        {
            var DirInfo = new DirectoryInfo(GlobalConstansts.FOLDER_WITH_GAMES_NAME);
            var GamesFolders = DirInfo.GetDirectories();

            ExistedGames.Clear();
            for (int i = 0; i < GamesFolders.Count(); i++)
            {
                ExistedGames.Add(new cGameShowInfo(
                                GamesFolders[i].Name,
                                GamesFolders[i].FullName,
                                File.ReadAllText(GamesFolders[i].FullName + '/' + GlobalConstansts.GAME_MAIN_PAGE_NAME, Encoding.UTF8)
                                ));
                //полностью заполняет инфу об игре для вывода в список
            }
            DefaultPageHTML = File.ReadAllText(GlobalConstansts.FOLDER_WITH_GAMES_NAME + '/' + GlobalConstansts.PAGE_DEFAULT_NAME, Encoding.UTF8);
        }

        //возвращает список игроков по имени игры
        public List<cPlayer> GetListOfPlayersByGameName(string GameName)
        {
            string temp = File.ReadAllText(GlobalConstansts.FOLDER_WITH_GAMES_NAME + '/' + GlobalConstansts.PLAYERS_LIST_FILE_NAME);
            return (List<cPlayer>)JsonConvert.DeserializeObject(temp);
        }

        public void SaveListOfPlayersByGameName(string GameName, List<cPlayer> PlayersList)
        {
            using (FileStream fs = new FileStream(GlobalConstansts.FOLDER_WITH_GAMES_NAME + '/' + GlobalConstansts.PLAYERS_LIST_FILE_NAME, FileMode.OpenOrCreate))
            {
                byte[] CurrPlayersListInJSON = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PlayersList));
                fs.Write(CurrPlayersListInJSON, 0, CurrPlayersListInJSON.Length);
            }
        }
    }

}
