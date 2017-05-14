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
        //общедоступные формы

        //public static cwMainWindow wMainWindow = null;
        public cwMainWindow()
        {
            InitializeComponent();
            GlobalForms.wPlayerInfoEditor = new cwPlayerInfoEditor();
            GlobalForms.wPlayerInfoEditor.Close();
            GlobalForms.wNewTournament = new cwNewTournament();
            GlobalForms.wNewTournament.Visibility = Visibility.Hidden;

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
            GlobalFunctions.ShowWindowAtLoc(GlobalForms.wNewTournament, this.Left, this.Top, this.Width, this.Height, this.WindowState);
            this.Hide();
        }

        private void btnOpenFile_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
