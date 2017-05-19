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
using Microsoft.Win32;

namespace WpfTournament
{
    /// <summary>
    /// Логика взаимодействия для NewTournament.xaml
    /// </summary>
    public partial class cwNewTournament : Window
    {
        private cGamesInfoLoader GamesInfoLoader;
        public cGame ChoosedGame;
        private ListOfPlayers FinalPlayersList; //это и есть элемент со списком

        public cwNewTournament()
        {
            InitializeComponent();
            GamesInfoLoader = new cGamesInfoLoader();

            FillGamesPreInfo();
            ChoosedGame = new cGame();
            FinalPlayersList = new ListOfPlayers(ref ChoosedGame, GlobalConstansts.LST_FORM);

            grdFinalListOfPlayers.Children.Add(FinalPlayersList);
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
        private void btnAddPlayersFromDB_Click(object sender, RoutedEventArgs e)
        {
            //не написано
        }
        private void btnAddPlayersFromLocalDB_Click(object sender, RoutedEventArgs e)
        {
            var TempPlayersList = new List<cPlayer>();
            var opndlg = new OpenFileDialog();
            opndlg.Filter = GlobalConstansts.SAVED_FILES_FILTER;
            opndlg.Multiselect = true;
            opndlg.Title = "Выберите файлы для загрузки";
            opndlg.ShowDialog();
            string[] FileNames = opndlg.FileNames;

            for (int i = 0; i < FileNames.Count(); i++)
                TempPlayersList.AddRange(GamesInfoLoader.GetListOfPlayersFromFile_CSV(FileNames[i]));

            if (TempPlayersList.Count == 0)
                MessageBox.Show("В этих файлах нет игроков");
            else
            {
                GlobalForms.wChoosingPlayersFromList.SetComparerAndPlayers(ref ChoosedGame, ref TempPlayersList);
                GlobalForms.wChoosingPlayersFromList.Owner = this;
                GlobalFunctions.ShowWindowAtLoc(GlobalForms.wChoosingPlayersFromList, this.Left + (this.Width - GlobalForms.wPlayerInfoEditor.Width) / 2, this.Top + (this.Height - GlobalForms.wPlayerInfoEditor.Height) / 2, GlobalForms.wPlayerInfoEditor.Width, GlobalForms.wPlayerInfoEditor.Height);
                GlobalForms.wChoosingPlayersFromList.eListIsFormed += this.ListIsFormedHandler;
                this.IsEnabled = false;
            }
        }
        private void btnAddPlayersFromInput_Click(object sender, RoutedEventArgs e)
        {
            GlobalForms.wPlayerInfoEditor.Owner = this;
            GlobalFunctions.ShowWindowAtLoc(GlobalForms.wPlayerInfoEditor, this.Left + (this.Width - GlobalForms.wPlayerInfoEditor.Width) / 2, this.Top + (this.Height - GlobalForms.wPlayerInfoEditor.Height) / 2, GlobalForms.wPlayerInfoEditor.Width, GlobalForms.wPlayerInfoEditor.Height);
            GlobalForms.wPlayerInfoEditor.PlayerInfoWasFormed += this.PlayerWasFormedHandler;
            this.IsEnabled = false;
        }
        private void btnSaveCurrListInFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            string FileName;
            bool Result;

            SaveFileDialog.Title = "Сохранить список игроков";
            SaveFileDialog.Filter = GlobalConstansts.SAVED_FILES_FILTER;
            SaveFileDialog.ShowDialog();
            FileName = SaveFileDialog.FileName;
            if ((FileName != null) && (FileName != ""))
                Result = GamesInfoLoader.SaveListOfPlayersInFile_CSV(FileName, ChoosedGame.ListOfPlayers);
            else
                Result = false;

            if (Result)
                MessageBox.Show("Успешно сохранено");
            else
                MessageBox.Show("Ошибка сохранения");

        }
        private void PlayerWasFormedHandler(cPlayer FormedPlayer)
        {
            if (FormedPlayer != null)
                this.ChoosedGame.AddPlayer(FormedPlayer);
            GlobalForms.wPlayerInfoEditor.PlayerInfoWasFormed -= this.PlayerWasFormedHandler;
            this.IsEnabled = true;
        }
        private void ListIsFormedHandler(List<cPlayer> FormedList)
        {
            bool res;
            if (FormedList != null)
                for (int i = 0; i < FormedList.Count; i++)
                {
                    res = true;
                    for (int j = 0; j < ChoosedGame.ListOfPlayers.Count(); j++)
                        if ((FormedList[i].ID != -1) && (ChoosedGame.ListOfPlayers[j].ID == FormedList[i].ID))
                            res = false;
                    if (res)
                        this.ChoosedGame.AddPlayer(FormedList[i]);
                }
            GlobalForms.wChoosingPlayersFromList.eListIsFormed -= this.ListIsFormedHandler;
            this.IsEnabled = true;
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
            this.IsEnabled = true;
            ComboBoxGamesList.Items.Clear();
            ComboBoxGamesList.SelectedIndex = -1; //проверить, нужно ли это
            ComboBoxGamesList.Text = GlobalInfoMessages.CHOOSE_GAME;
            ComboBoxGamesList.IsEditable = true;
            if (FinalPlayersList != null)
                FinalPlayersList.Items.Refresh();
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
            if ((sender as Window).Visibility == Visibility.Visible)
            {

                ChoosedGame.Reset();
                FillGamesPreInfo();
            }
        }

    }
}
