using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfTournament
{
    /// <summary>
    /// Логика взаимодействия для NewTournament.xaml
    /// </summary>
    public partial class cwNewTournament : Window
    {
        private cGamesInfoLoader GamesInfoLoader;
        public cGame CurrGame;

        public cwNewTournament()
        {
            InitializeComponent();
            GamesInfoLoader = new cGamesInfoLoader();
            FillGamesPreInfo();
            CurrGame = new cGame();
        }
        public cwNewTournament(double LeftMargin, double TopMargin)
        {
            //обязательно потом продублировать код из обычного конструктора
            InitializeComponent();
            this.Left = LeftMargin;
            this.Top = TopMargin;
        }



        private void ComboBoxGamesList_Selected_1(object sender, RoutedEventArgs e)
        {
            UpdateInfoInWebBrowserByComboBox((sender as ComboBox), GamesInfoLoader.DefaultPageHTML);
        }
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlobalFunctions.ShowWindowAtLoc(/*cwMainWindow.wMainWindow*/App.Current.MainWindow, this.Left, this.Top, this.Width, this.Height,this.WindowState);
        }

        private void FillGameObjByGameShowInfoObj(cGameShowInfo GameShowInfiObj)
        {
            CurrGame.Name = GameShowInfiObj.ShowingName;
        }
        private void FillGamesPreInfo()
        {
            ResetAllControls();
            GamesInfoLoader.UpdateGamesInfo_local();

            for (int i = 0; i < GamesInfoLoader.ExistedGames.Count; i++)
                ComboBoxGamesList.Items.Add(GamesInfoLoader.ExistedGames[i]);
            ComboBoxGamesList_Selected_1(ComboBoxGamesList, new RoutedEventArgs());
        }
        private void ResetAllControls()
        {
            ComboBoxGamesList.Items.Clear();
            ComboBoxGamesList.SelectedIndex = -1; //проверить, нужно ли это
            ComboBoxGamesList.Text = GlobalInfoMessages.CHOOSE_GAME;
        }
        private void UpdateInfoInWebBrowserByComboBox(ComboBox sender, string Default)
        {
            //архитектурно это не очень хорошо, но более практично
            if ((sender as ComboBox).SelectedIndex != -1)
                WebBrowserGameInfo.NavigateToString(((sender as ComboBox).SelectedItem as cGameShowInfo).PageHTMLString);
            else
                WebBrowserGameInfo.NavigateToString(Default);
        }
    }
}
