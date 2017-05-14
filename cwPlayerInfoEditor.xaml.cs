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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public delegate void DelegateWithPlayerInReturn(cPlayer ReturnPlayer);

    public partial class cwPlayerInfoEditor : Window
    {

        public event DelegateWithPlayerInReturn PlayerInfoWasFormed;

        public cPlayer ResPlayerInfo;
        public cwPlayerInfoEditor()
        {
            InitializeComponent();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResPlayerInfo = new cPlayer(TextBoxName.Text, TextBoxSurname.Text, Int32.Parse(TextBoxAge.Text), TextBoxRating.Text, TextBoxOther.Text);
                if (PlayerInfoWasFormed != null) PlayerInfoWasFormed(ResPlayerInfo);
                this.Visibility = Visibility.Hidden;
            }
            catch
            {
                MessageBox.Show("Ошибка ввода");
            }
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (sender as Window).Visibility = Visibility.Hidden;
            if (PlayerInfoWasFormed != null) PlayerInfoWasFormed(null);
            e.Cancel = true;
        }

        private void ResetAllControls()
        {
            TextBoxName.Clear();
            TextBoxSurname.Clear();
            TextBoxAge.Clear();
            TextBoxRating.Clear();
            TextBoxOther.Clear();
        }

        private void Window_IsVisibleChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as Window).Visibility == Visibility.Visible)
                ResetAllControls();
        }
    }
}
