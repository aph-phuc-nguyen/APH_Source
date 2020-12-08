using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AutocompleteMenuNS;

namespace Tester
{
    public partial class CustomListViewSample : Form
    {
        public CustomListViewSample()
        {
            InitializeComponent();

            //attach custom listview
            autocompleteMenu1.ListView = myListView1;

            //build menu
            for (int i = 0; i < 1000; i++)
                autocompleteMenu1.AddItem(new AutocompleteItem(string.Format("{0:000}", i), i % 4));
        }
    }

    /// <summary>
    /// Custom cotnrol, derived from ListView. 
    /// It is implements IAutocompleteListView interface.
    /// </summary>
    public class MyListView : ListView, IAutocompleteListView
    {
        #region IAutocompleteListView

        public int SelectedItemIndex
        {
            get { 
                var ind = this.SelectedIndices;
                if (ind.Count > 0)
                    return ind[0];
                else
                    return -1;
            }
            set
            {
                if (value >= 0 && value < Items.Count)
                {
                    Items[value].Selected = true;
                    EnsureVisible(value);
                    Invalidate();
                }
            }
        }

        private IList<AutocompleteItem> visibleItems;

        public IList<AutocompleteItem> VisibleItems
        {
            get { return visibleItems; }
            set
            {
                visibleItems = value;
                BeginUpdate();
                Items.Clear();
                if (visibleItems != null)
                foreach(var item in visibleItems)
                {
                    var listViewItem = new ListViewItem(item.ToString(), item.ImageIndex);
                    Items.Add(listViewItem);
                }
                EndUpdate();
            }
        }

        public event EventHandler ItemSelected;

        public ImageList ImageList
        {
            get { return this.LargeImageList; }
            set { this.LargeImageList = value; }
        }

        #endregion

        protected override void OnDoubleClick(EventArgs e)
        {
            OnItemSelected();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
                OnItemSelected();
            base.OnKeyPress(e);
        }

        private void OnItemSelected()
        {
            if (ItemSelected != null)
                ItemSelected(this, EventArgs.Empty);
        }
    }
}
