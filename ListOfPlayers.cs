
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;

namespace WpfTournament
{
    class cListOfPlayers : DataGrid
    {
        private cGame RefToGameInfo;
        public cListOfPlayers(ref cGame CurrGame)
        {
            this.RefToGameInfo = CurrGame;
            this.ItemsSource = CurrGame.ListOfPlayers;

            this.Columns.Clear();
            this.Columns.Add(new DataGridTextColumn()
            {
                Header = "Фамилия",
                Binding = new Binding("Surname")
            });
            this.Columns.Add(new DataGridTextColumn()
            {
                Header = "Имя",
                Binding = new Binding("Name")
            });
            this.Columns.Add(new DataGridTextColumn()
            {
                Header = "Возраст",
                Binding = new Binding("Age")
            });
            this.Columns.Add(new DataGridTextColumn()
            {
                Header = "Рейтинг",
                Binding = new Binding("Rating")
            });
            this.Columns.Add(new DataGridTextColumn()
            {
                Header = "Кол-во турнирных партий",
                Binding = new Binding("AmountOfTournamentGames")
            });
            //this.CurrentColumn = this.Columns[0];
            
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Margin = new Thickness(22,22,22,0);
            this.Height = 200;
            this.IsReadOnly = true;
            
            this.Sorting += new DataGridSortingEventHandler(SortHandler);
            RefToGameInfo.PlayerWasAdded += ElementWasAddedHandler;
           
            //this.Sor += cListOfPlayers_Click;
            //this.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Rating", System.ComponentModel.ListSortDirection.Ascending));
            //this.Items.Comparer = this.RefToGameInfo;
            //var view = (ListCollectionView)CollectionViewSource.GetDefaultView(this.ItemsSource);
        }

        void ElementWasAddedHandler(cPlayer Temp)
        {
            if (this.CurrentColumn != null)
            {
                this.OnSorting(new DataGridSortingEventArgs(this.CurrentColumn));
                this.OnSorting(new DataGridSortingEventArgs(this.CurrentColumn));
            }
            this.Items.Refresh();
            //if (this.CurrentColumn==null)
            //    this.OnSorting(new DataGridSortingEventArgs(this.Columns[0]));
            //else
            //    this.OnSorting(new DataGridSortingEventArgs(this.CurrentColumn));
        }

        void SortHandler(object sender, DataGridSortingEventArgs e)
        {
            DataGridColumn column = e.Column;
            (sender as cListOfPlayers).CurrentColumn = e.Column;
            if ((column==null)||((string)column.Header != "Рейтинг")) return;
            
            System.Collections.IComparer comparer = null;
            //i do some custom checking based on column to get the right comparer
            //i have different comparers for different columns. I also handle the sort direction
            //in my comparer

            // prevent the built-in sort from sorting
            e.Handled = true;

            ListSortDirection direction = (column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            //set the sort order on the column
            column.SortDirection = direction;
            this.RefToGameInfo.SortDirection = direction;
            //use a ListCollectionView to do the sort.
            ICollectionView temp = CollectionViewSource.GetDefaultView(this.ItemsSource);
            ListCollectionView lcv = (ListCollectionView)temp;

            //this is my custom sorter it just derives from IComparer and has a few properties
            //you could just apply the comparer but i needed to do a few extra bits and pieces
            comparer = this.RefToGameInfo;

            //apply the sort
            lcv.CustomSort = comparer;
        }

    }
}
