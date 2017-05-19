
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
using System.Windows.Media;

namespace WpfTournament
{
    class ListOfPlayers : DataGrid
    {
        private cGame RefToGameInfo; //просто как компаратор и всё!!!
        public List<int> ChoosedIndexes = new List<int>();
        public int ListForm;

        public ListOfPlayers(ref cGame CurrGame, int ListForm)
        {
            this.ListForm = ListForm;
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
                Header = "Доп. инфо",
                Binding = new Binding("OtherInfo")
            });

            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;

            this.Margin = new Thickness(22, 22, 22, 22);
            this.IsReadOnly = true;

            this.CanSelectMultipleItems = false;
            this.CanUserDeleteRows = false;

            //    <ListBox.Resources>
            //    <SolidColorBrush Color="Red" x:Key ="{x:Static SystemColors.HighlightBrushKey}">
            //    </SolidColorBrush>                
            //</ListBox.Resources>

            this.Resources.Add("SolidColorBrush", SystemColors.ControlBrushKey);
            this.Resources.Add("Color", "Red");
            //this.Resources = new ResourceDictionary();
            //this.Style = "ListBoxItemStyle";
            this.Sorting += new DataGridSortingEventHandler(SortHandler);
            this.LoadingRow += ListOfPlayers_LoadingRow;

            this.RefToGameInfo.PlayerWasAdded += ElementWasAddedHandler;
            this.SelectedCellsChanged += SelectionHandler;
            this.KeyUp += ListOfPlayers_KeyUp;
        }

        void ListOfPlayers_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            PaintChoosedItems();
        }

        void ListOfPlayers_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (ListForm == GlobalConstansts.LST_FORM)
                {
                    ChoosedIndexes.Sort(GlobalFunctions.IsFirstGreater);
                    for (int i = ChoosedIndexes.Count() - 1; i >= 0; i--)
                        RefToGameInfo.ListOfPlayers.RemoveAt(ChoosedIndexes[i]);
                    ChoosedIndexes.Clear();
                }

                if (ListForm == GlobalConstansts.LST_CHOOSE)
                {


                }
                this.Items.Refresh();
            }

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
            (sender as ListOfPlayers).CurrentColumn = e.Column;
            if ((column == null) || ((string)column.Header != "Рейтинг")) return;

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

        void SelectionHandler(object sender, EventArgs e)
        {
            if ((sender as ListOfPlayers).SelectedIndex == -1) return;
            int realIndex = 0;

            for (int i = 0; i < (sender as ListOfPlayers).RefToGameInfo.ListOfPlayers.Count(); i++)
                if (((sender as ListOfPlayers).SelectedItem as cPlayer).ID == (sender as ListOfPlayers).RefToGameInfo.ListOfPlayers[i].ID)
                {
                    realIndex = i;
                    break;
                }

            if (!ChoosedIndexes.Contains(realIndex))
            {
                ChoosedIndexes.Add(realIndex);
                PaintItemById(this.SelectedIndex, 1);
            }
            else
            {
                ChoosedIndexes.Remove(realIndex);
                PaintItemById(this.SelectedIndex, 0);
            }
            this.UnselectAll();
        }

        void SetItemsSourceWithComparer(ref cGame CurrComparer, ref List<cPlayer> ForItemsSource)
        {
            this.ItemsSource = ForItemsSource;
            this.RefToGameInfo = CurrComparer;
        }

        void PaintChoosedItems()
        {
            for (int i = 0; i < ChoosedIndexes.Count(); i++)
                for (int j = 0; j < this.Items.Count; j++)
                {
                    if ((this.Items[j] as cPlayer).ID == RefToGameInfo.ListOfPlayers[ChoosedIndexes[i]].ID)
                    {
                        PaintItemById(j, 1);
                        continue;
                    }
                }
        }

        void PaintItemById(int index, int Mode) //1-выделен. 2-не выделен
        {
            DataGridRow lbi = (DataGridRow)this.ItemContainerGenerator.ContainerFromIndex(index);
            if (lbi != null)
                if (Mode == 1)
                {
                    lbi.Background = Brushes.Brown;
                    lbi.Foreground = Brushes.White;
                }
                else
                {
                    lbi.Background = Brushes.White;
                    lbi.Foreground = Brushes.Black;
                }
        }
    }
}
