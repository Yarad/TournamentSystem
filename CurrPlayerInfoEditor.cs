using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace WpfTournament
{
    class cCurrPlayerInfoEditor : StackPanel
    {
        private cGame RefToGameInfo;
        public TextBox tbName = new TextBox();
        public TextBox tbSurname = new TextBox();
        public TextBox tbRating = new TextBox();
        public Button btnAdd = new Button();
        
        public cCurrPlayerInfoEditor(ref cGame CurrGame)
        {
            RefToGameInfo = CurrGame;

            this.IsVisibleChanged += cCurrPlayerInfoEditor_IsVisibleChanged;

            this.Children.Add(tbSurname);
            this.Children.Add(tbName);
            this.Children.Add(tbRating);
            this.Children.Add(btnAdd);
            this.btnAdd.Height = 50;

            btnAdd.Click += btnAdd_MouseDown;

            this.VerticalAlignment = VerticalAlignment.Bottom;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.Margin = new Thickness(30,200,30,0);
            this.Visibility = Visibility.Hidden;
        }

        void cCurrPlayerInfoEditor_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ResetAllControls();
        }

        void btnAdd_MouseDown(object sender, RoutedEventArgs e)
        {
            RefToGameInfo.AddPlayer(new cPlayer(0,tbName.Text,tbSurname.Text,0,"vk.com",tbRating.Text));
            this.Visibility = Visibility.Hidden;
        }

        public void ResetAllControls()
        {
            tbRating.Clear();
            tbName.Clear();
            tbSurname.Clear();
        }

    }
}