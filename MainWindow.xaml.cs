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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTournament
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private cwNewTournament wNewTournament;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnLabelMouseEnter(object sender, EventArgs e)
        {
            (sender as TextBlock).TextDecorations = TextDecorations.Underline;
        }
        public void OnLabelMouseLeave(object sender, EventArgs e)
        {
            (sender as TextBlock).TextDecorations = null;
        }

        public void OnNewTournamentClick(object sender, EventArgs e)
        {
            this.Hide();
            wNewTournament = new cwNewTournament(this.Left,this.Top);
            wNewTournament.Show();
        }

        public void Show(double LeftMargin, double TopMargin)
        {
            this.
        }
    }
}
