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
    public partial class cwMainWindow : Window
    {

        private cwNewTournament wNewTournament;

        public static cwMainWindow wMainWindow = null;
        public cwMainWindow()
        {
            InitializeComponent();
            wMainWindow = this;
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
            
            wNewTournament = new cwNewTournament();
            GlobalFunctions.ShowWindowAtLoc(wNewTournament, this.Left, this.Top, this.Width,this.Height);
            this.Hide();
        }
    }
}
