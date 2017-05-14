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
using System.IO;

namespace WpfTournament
{
    /// <summary>
    /// Логика взаимодействия для NewTournament.xaml
    /// </summary>
    public partial class cwNewTournament : Window
    {
        private cGamesInfoLoader GamesInfoLoader;
        public cGame ChoosedGame;
        private cCurrPlayerInfoEditor PlayerInfoEditor;
        private cListOfPlayers FinalPlayersList;

        public cwNewTournament()
        {
            InitializeComponent();
            GamesInfoLoader = new cGamesInfoLoader();
            ChoosedGame = new cGame();

            PlayerInfoEditor = new cCurrPlayerInfoEditor(ref ChoosedGame);
            FinalPlayersList = new cListOfPlayers(ref ChoosedGame);

            grdFinalListOfPlayers.Children.Add(FinalPlayersList);
            grdFinalListOfPlayers.Children.Add(PlayerInfoEditor);

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
            UpdateInfoInWebBrowserByComboBox((sender as ComboBox));
            ComboBoxGamesList.IsEditable = false;
            btnFormList.IsEnabled = true;
        }
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlobalFunctions.ShowWindowAtLoc(/*cwMainWindow.wMainWindow*/App.Current.MainWindow, this.Left, this.Top, this.Width, this.Height, this.WindowState);
            this.Visibility = Visibility.Hidden;
            
            foreach (Window w in this.OwnedWindows)
                w.Close();
            
            e.Cancel = true;
        }
        private void btnFormList_Click(object sender, RoutedEventArgs e)
        {
            TabItemPlayers2.Visibility = Visibility.Visible;
            TabItemPlayers2.IsSelected = true;
            GamesInfoLoader.FillGameObjByGameShowInfoObj(ChoosedGame, (cGameShowInfo)ComboBoxGamesList.SelectedValue);
        }
        private void btnAddPlayersFromLocalDB_Click(object sender, RoutedEventArgs e)
        {
            //не написано
        }
        private void btnAddPlayersFromInput_Click(object sender, RoutedEventArgs e)
        {
            GlobalForms.wPlayerInfoEditor.Owner = this;
            GlobalFunctions.ShowWindowAtLoc(GlobalForms.wPlayerInfoEditor, this.Left + (this.Width - GlobalForms.wPlayerInfoEditor.Width) / 2, this.Top + (this.Height - GlobalForms.wPlayerInfoEditor.Height) / 2, GlobalForms.wPlayerInfoEditor.Width, GlobalForms.wPlayerInfoEditor.Height);
            //GlobalForms.wPlayerInfoEditor.Visibility = Visibility.Visible;
        }

        private void FillGamesPreInfo()
        {
            ResetAllControls();
            GamesInfoLoader.UpdateGamesInfo_local();

            for (int i = 0; i < GamesInfoLoader.ExistedGames.Count; i++)
                ComboBoxGamesList.Items.Add(GamesInfoLoader.ExistedGames[i]);
            //ComboBoxGamesList_Selected_1(ComboBoxGamesList, new RoutedEventArgs());
            //не помню, зачем это было нужно
        }
        private void ResetAllControls()
        {
            ComboBoxGamesList.Items.Clear();
            ComboBoxGamesList.SelectedIndex = -1; //проверить, нужно ли это
            ComboBoxGamesList.Text = GlobalInfoMessages.CHOOSE_GAME;
            ComboBoxGamesList.IsEditable = true;

            UpdateInfoInWebBrowserByComboBox(ComboBoxGamesList);

            TabItemGame1.IsSelected = true;
            TabItemPlayers2.Visibility = Visibility.Hidden;
            btnFormList.IsEnabled = false;
        }
        private void UpdateInfoInWebBrowserByComboBox(ComboBox sender, string Default = "")
        {
            //архитектурно это не очень хорошо, но более практично
            if (Default.Length == 0)
                Default = File.ReadAllText(GlobalConstansts.FOLDER_WITH_GAMES_NAME + '/' + GlobalConstansts.PAGE_DEFAULT_NAME, Encoding.UTF8);
            if ((sender as ComboBox).SelectedIndex != -1)
                WebBrowserGameInfo.NavigateToString(((sender as ComboBox).SelectedItem as cGameShowInfo).PageHTMLString);
            else
                WebBrowserGameInfo.NavigateToString(Default);
        }

        private void Window_IsVisibleChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {
            FillGamesPreInfo();
        }

    }
}
