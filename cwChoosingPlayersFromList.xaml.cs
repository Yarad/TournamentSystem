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
    /// Логика взаимодействия для cwChoosingPlayersFromList.xaml
    /// </summary>
    public delegate void dListIsFormed(List<cPlayer> FormedList);
    public partial class cwChoosingPlayersFromList : Window
    {
        private cGame GameWithWholeList = new cGame();
        private ListOfPlayers ShowingListOfPlayers;
        private Button ButtonAddChoosedPlayers;
        public event dListIsFormed eListIsFormed;
        

        public cwChoosingPlayersFromList()
        {
            InitializeComponent();

            ShowingListOfPlayers = new ListOfPlayers(ref GameWithWholeList, GlobalConstansts.LST_CHOOSE);
            ShowingListOfPlayers.MinHeight = 330;
            //ShowingListOfPlayers.

            ButtonAddChoosedPlayers = new Button();
            ButtonAddChoosedPlayers.Click += ButtonOKHandler;
            ButtonAddChoosedPlayers.Height = 30;
            ButtonAddChoosedPlayers.Content = "Добавить";
            
            StackPanelMain.Children.Add(ShowingListOfPlayers);
            StackPanelMain.Children.Add(ButtonAddChoosedPlayers);
        }

        public void SetComparerAndPlayers(ref cGame GameComparer, ref List<cPlayer> ListOfAllPlayers)
        {
            ShowingListOfPlayers.ChoosedIndexes.Clear();
            GameComparer.CloneTo(ref GameWithWholeList);
            GameWithWholeList.ListOfPlayers = ListOfAllPlayers;
            ShowingListOfPlayers.ItemsSource = GameWithWholeList.ListOfPlayers;
            ShowingListOfPlayers.Items.Refresh();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (sender as Window).Visibility = Visibility.Hidden;
            if (eListIsFormed != null) eListIsFormed(new List<cPlayer>());
            e.Cancel = true;
        }

        private void ButtonOKHandler(object sender, EventArgs e)
        {
            List<cPlayer> FormedListOfPlayers = new List<cPlayer>();
            for (int i = 0; i < ShowingListOfPlayers.ChoosedIndexes.Count; i++)
                FormedListOfPlayers.Add(GameWithWholeList.ListOfPlayers[ShowingListOfPlayers.ChoosedIndexes[i]]);
            eListIsFormed(FormedListOfPlayers);
            this.Close();
        }
    }
}
