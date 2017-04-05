using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GamesFunctions;

namespace WpfTournament
{
    static class GlobalConstansts
    {
        public static string FOLDER_WITH_GAMES_NAME = "ExistedGames";
        public static string FOLDER_WITH_IMAGES_IN_GAMEINFO = "images";
        //public static string MAIN_GAME_IMAGE_NAME = "main";
        public static string GAME_MAIN_PAGE_NAME = "index.html";
        public static string PAGE_DEFAULT_NAME = "default.html";
        public static string PLAYERS_LIST_FILE_NAME = "players.txt";
    }

    static class GlobalInfoMessages
    {
        public static string CHOOSE_GAME = "Выберите игру";
    }

    static class GlobalFunctions
    {
        public static int IsFirstGreater(int a, int b)
        {
            if (a > b)
                return 1;
            else
                if
                (a < b)
                    return -1;
                else
                    return 0;
        }

        //для каждого из методов нужно обеспечить правильный формат строки
        //проверку делать очень неудобно
        public static int ChessRatingCompare(string Raiting1, string Raiting2)
        {
            Raiting1.Trim();
            Raiting2.Trim();
            return IsFirstGreater(Int32.Parse(Raiting1), Int32.Parse(Raiting2));

        }
        public static int GoRatingCompare(string Raiting1, string Raiting2)
        {
            Raiting1.Trim();
            Raiting2.Trim();

            var Priority = new char[] { 'k', 'd', 'p' };
            int GameClass1 = -1, GameClass2 = -1;

            for (int i = 0; i < Priority.Length - 1; i++) //выясняем приоритет второй части
            {
                if (Raiting1.Last() == Priority[i])
                    GameClass1 = i;
                if (Raiting1.Last() == Priority[i])
                    GameClass2 = i;
            }


            int TempPriority = IsFirstGreater(GameClass1, GameClass2);

            switch (TempPriority)
            {
                case 1: return 1; break;
                case -1: return -1; break;
                case 0:
                    string Substr1 = Raiting1.Substring(0, Raiting1.Length - 1);
                    string Substr2 = Raiting2.Substring(0, Raiting2.Length - 1);

                    if (GameClass1 == 1 || GameClass1 == 2) //dan or pro
                    {
                        return IsFirstGreater(Int32.Parse(Substr1), Int32.Parse(Substr2));
                    }
                    else
                        return IsFirstGreater(Int32.Parse(Substr2), Int32.Parse(Substr1)); //kyu
                    break;
                default:
                    return -1;
                    break;
            }

        }

        public static void ShowWindowAtLoc(object sender, double LeftMargin, double TopMargin, double Width, double Height, WindowState PrevWindowState)
        {
            (sender as Window).Left = LeftMargin;
            (sender as Window).Top = TopMargin;
            (sender as Window).Width = Width;
            (sender as Window).Height = Height;
            (sender as Window).WindowState = PrevWindowState;
            (sender as Window).Show();
        }
    }
}
