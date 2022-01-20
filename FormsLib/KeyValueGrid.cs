using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FormsLib
{
    /// <summary>
    /// KeyValueGrid is a grid with 2 columns that can be used for display key-value pairs
    /// </summary>
    public class KeyValueGrid : Grid
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="keyHeader">the header text for the key column</param>
        /// <param name="valueHeader">the header text for the value column</param>
        /// <param name="gridWidthRequest">the total requested width for the grid</param>
        /// <param name="rowSpacing">row spacing for the grid</param>
        /// <param name="keyColumWidth">the key column width as 1*</param>
        /// <param name="valueColumnWidth">the key column width as 1*</param>
        public KeyValueGrid(string keyHeader, string valueHeader, int gridWidthRequest, int rowSpacing = 2, int keyColumWidth = 1, int valueColumnWidth = 1)
        {
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(keyColumWidth, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(valueColumnWidth, GridUnitType.Star) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            RowSpacing = rowSpacing;
            WidthRequest = gridWidthRequest;
            _keyHeader = keyHeader;
            _valueHeader = valueHeader;
            Initialize();
        }

        /// <summary>
        /// get row count
        /// </summary>
        public int RowCount { get { return _rowIndex; } }

        /// <summary>
        /// add a row with key value pair. A linkLabel will be used when the Uri is not null.
        /// </summary>
        public void AddRow(string key, string value, Uri uri = null)
        {
            AddRow(key, value, uri, false, FormsUtil.LabelDefaultFontSize);
        }

        /// <summary>
        /// add a row with key value pair. A linkLabel will be used when the Uri is not null.
        /// </summary>
        public void AddRow(string key, string value, Uri uri, bool boldFont, double fontSize, int maxLines = 1)
        {
            IGridList<View> gridList = Children;
            // add a label for the key in column 0
            Label label = new Label { Text = key, FontSize = fontSize, MaxLines = maxLines };
            if (boldFont) label.FontAttributes = FontAttributes.Bold;
            gridList.Add(label, 0, _rowIndex);
            // add a label for the value in column 1
            label = FormsUtil.CreateLabel(value, uri);
            if (label != null)
            {
                label.FontSize = fontSize;
                label.MaxLines = maxLines;
                if (boldFont) label.FontAttributes = FontAttributes.Bold;
                gridList.Add(label, 1, _rowIndex);
            }
            _rowIndex++;
        }

        /// <summary>
        /// clear all the rows except the header orw
        /// </summary>
        public void Clear()
        {
            Children.Clear();
            Initialize();
        }

        protected void Initialize()
        {
            _rowIndex = 0;
            AddRow(_keyHeader, _valueHeader, null, true, FormsUtil.LabelDefaultFontSize);
        }

        private int _rowIndex;
        private string _keyHeader;
        private string _valueHeader;
    }
}
