using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tournament_System
{
    static class GlobalConstansts
    {
        public static List<string> ExistedGames = new List<string>() {"Шахматы", "Го", "Другое"};
        public static Dictionary<string, SortByRatingDel> GamesRatingCompareFunctions = new Dictionary<string, SortByRatingDel>
        {
            {"Chess",GlobalFunctions.ChessRatingCompare},
            {"Go",GlobalFunctions.GoRatingCompare}
        };
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

        public static void ModifyFormAsAll(Form CurrForm)
        {
            CurrForm.Height = 570;
            CurrForm.Width = 960;
            double Ratio = 960 / 570;

            if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width < CurrForm.Width)
            {
                CurrForm.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
                CurrForm.Height = (int)Math.Round(CurrForm.Width / Ratio);
            }

            if (System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height < CurrForm.Height)
            {
                CurrForm.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
                CurrForm.Width = (int)Math.Round(CurrForm.Height * Ratio);
            }

            CurrForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            CurrForm.StartPosition = FormStartPosition.CenterScreen;
        }
    }
}
