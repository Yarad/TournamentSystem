
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfTournament
{
    class cListOfPlayers : ListView
    {
        private GridView TemplateGridView;
        
        public cListOfPlayers()
        {
            TemplateGridView = new GridView();
            this.View = TemplateGridView;
            TemplateGridView.Columns.Add(new GridViewColumn
            {
                Header = "ID",
                DisplayMemberBinding = new Binding("ID")
            });
            TemplateGridView.Columns.Add(new GridViewColumn
            {
                Header = "Фамилия",
                DisplayMemberBinding = new Binding("Surname")
            });
            TemplateGridView.Columns.Add(new GridViewColumn
            {
                Header = "Имя",
                DisplayMemberBinding = new Binding("Name")
            });
            TemplateGridView.Columns.Add(new GridViewColumn
            {
                Header = "Возраст",
                DisplayMemberBinding = new Binding("Age")
            });
            TemplateGridView.Columns.Add(new GridViewColumn
            {
                Header = "Рейтинг",
                DisplayMemberBinding = new Binding("Rating")
            });
            TemplateGridView.Columns.Add(new GridViewColumn
            {
                Header = "Кол-во турнирных партий",
                DisplayMemberBinding = new Binding("AmountOfTournamentGames")
            });
            
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Margin = new Thickness(22,22,22,0);
            this.Height = 200;
            this.Items.SortDescriptions.Add( new System.ComponentModel.SortDescription("Name",System.ComponentModel.ListSortDirection.Ascending));
            
        }

        public void AddPlayer(cPlayer CurrPlayer)
        {
            this.Items.Add(CurrPlayer);
        }
    }
}
