﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace WpfTournament
{
    public class cPlayer
    {
        public ulong ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Rating { get; set; }
        public int AmountOfTournamentGames { get; set; }
        public string URL { get; set; }
        public string OtherInfo { get; set; }
        public List<string> GamesIDs = new List<string>();

        public cPlayer() { }
        public cPlayer(ulong ID, string Name, string Surname, int Age, string URL, string Rating = "0", int AmountOfTournaments = 0)
        {
            this.ID = ID;
            this.Name = Name;
            this.Surname = Surname;
            this.Age = Age;
            this.URL = URL;
            this.Rating = Rating;
            this.AmountOfTournamentGames = AmountOfTournaments;
        }

        public cPlayer(string Name, string Surname, int Age, string Rating, string OtherInfo)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Age = Age;
            this.Rating = Rating;
            this.OtherInfo = OtherInfo;
        }
        public cPlayer Clone()
        {
            return (cPlayer)this.MemberwiseClone();
        }
        /*public override string ToString()
        {
            return this.Surname + this.Rating.ToString();
        }*/
    }

    class cGamesInfoLoader
    {
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
                                GamesFolders[i].FullName + '/' + GlobalConstansts.DLL_FILE_OF_GAME,
                                File.ReadAllText(GamesFolders[i].FullName + '/' + GlobalConstansts.GAME_MAIN_PAGE_NAME, Encoding.UTF8)
                                ));
                //полностью заполняет инфу об игре для вывода в список
            }
        }

        //возвращает список игроков по имени игры
        public List<cPlayer> GetListOfPlayersByGameName(string GameName)
        {
            string temp = File.ReadAllText(GlobalConstansts.FOLDER_WITH_GAMES_NAME + '/' + GlobalConstansts.PLAYERS_LIST_FILE_NAME);
            return (List<cPlayer>)JsonConvert.DeserializeObject(temp);
        }

        //сохраняет список игроков по имени игры
        public void SaveListOfPlayersByGameName(string GameName, List<cPlayer> PlayersList)
        {
            using (FileStream fs = new FileStream(GlobalConstansts.FOLDER_WITH_GAMES_NAME + '/' + GlobalConstansts.PLAYERS_LIST_FILE_NAME, FileMode.OpenOrCreate))
            {
                byte[] CurrPlayersListInJSON = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PlayersList));
                fs.Write(CurrPlayersListInJSON, 0, CurrPlayersListInJSON.Length);
            }
        }

        //функция для заполнения основного объекта по текущей информации
        public void FillGameObjByGameShowInfoObj(cGame CurrGame, cGameShowInfo GameShowInfo)
        {
            CurrGame.Name = GameShowInfo.ShowingName;

            var DLLLoader = Assembly.LoadFile(GameShowInfo.DLLPath); //создаем загрузчик
            var ClassMethods = DLLLoader.GetType(GlobalConstansts.DLL_MAIN_CLASS_NAME); //получить информацию о методах в dll-ке
            var o = DLLLoader.CreateInstance(GlobalConstansts.DLL_MAIN_CLASS_NAME); //создаем объект этого типа
            var RatingCompareDelegate = ClassMethods.GetMethod(GlobalConstansts.DLL_RATING_COMPARE_FUNC);

            CurrGame.RatingCompareFunction = (string Raiting1, string Raiting2) =>
            {
                var DllMethodParams = new object[2] { Raiting1, Raiting2 };
                return (int)RatingCompareDelegate.Invoke(o, DllMethodParams);
            };
        }


    }

}
