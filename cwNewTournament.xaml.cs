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
        /*Конструктор будет недоступен при неуказанном положении окна
         * public cwNewTournament()
        {
            InitializeComponent();
        }*/ 
        
        public cwNewTournament(double LeftMargin, double TopMargin)
        {
            InitializeComponent();
            this.Left = LeftMargin;
            this.Top = TopMargin;
        }

    }
}
