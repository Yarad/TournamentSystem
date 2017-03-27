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
        public cwNewTournament()
        {
            InitializeComponent();
            cGame GoGame = new cGame("Go", GlobalFunctions.GoRatingCompare);
            GoGame.SaveToFile("temp.txt");
        }
        public cwNewTournament(double LeftMargin, double TopMargin)
        {
            InitializeComponent();
            this.Left = LeftMargin;
            this.Top = TopMargin;
        }

        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlobalFunctions.ShowWindowAtLoc(cwMainWindow.wMainWindow, this.Left,this.Top, this.Width,this.Height);
        }

    }
}
