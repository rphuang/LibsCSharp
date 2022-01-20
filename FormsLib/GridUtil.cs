using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using static Xamarin.Forms.Grid;

namespace FormsLib
{
    /// <summary>
    /// Provides helper utilities for for Xamarin Forms' Grid
    /// </summary>
    public static class GridUtil
    {
        /// <summary>
        /// Add a header label to the grid
        /// </summary>
        public static Label AddLabel(string text, IGridList<View> gridList, int row, int col, int colSpan = 1)
        {
            return AddLabel(text, gridList, row, col, colSpan, 1, Color.Black, false, LayoutOptions.Start);
        }

        /// <summary>
        /// Add a header label to the grid
        /// </summary>
        public static Label AddLabel(string text, IGridList<View> gridList, int row, int col, int colSpan, int maxLines, Color textColor, bool useRedForStartwithDash, LayoutOptions horizontalOptions)
        {
            Label label = new Label();
            label.Text = text;
            label.HorizontalOptions = horizontalOptions;
            if (useRedForStartwithDash && text.StartsWith("-")) label.TextColor = Color.Red;
            else label.TextColor = textColor;
            label.MaxLines = maxLines;
            gridList.Add(label, col, col + colSpan, row, row + 1);
            return label;
        }

        /// <summary>
        /// add a Label to the specified row and column
        /// </summary>
        public static Label AddLabel(IGridList<View> gridList, int row, int col, string text, Color defaultTextColor, bool useRedForStartwithDash, LayoutOptions horizontalOptions)
        {
            return AddLabel(text, gridList, row, col, 1, 1, defaultTextColor, useRedForStartwithDash, horizontalOptions);
        }

        /// <summary>
        /// add an Entry to the specified row and column
        /// </summary>
        public static Entry AddEntry(IGridList<View> gridList, int row, int col, string text, Color defaultTextColor, LayoutOptions horizontalOptions)
        {
            Entry entry = new Entry();
            entry.Text = text;
            entry.HorizontalOptions = horizontalOptions;
            entry.TextColor = defaultTextColor;
            gridList.Add(entry, col, row);
            return entry;
        }

        #region Grid Row utilities

        // TODO: how to customize custom layout of the Grid
        // Current implementation assumes:
        // 2 grid columns: column 0 for key and the column 1 for value

        /// <summary>
        /// add a description to the Grid that may span multiple rows
        /// </summary>
        public static void AddDescriptionToGrid(IGridList<View> gridList, ref int row, string value, string key = "Desc")
        {
            int rowSpan = 1;
            if (string.IsNullOrEmpty(value)) return;
            else
            {
                rowSpan = 1 + (value.Length / 50);
            }
            Label keyLabel = new Label { Text = key };
            //gridList.Add(keyLabel, 0, 1, row, row + rowSpan);
            gridList.Add(keyLabel, 0, row);
            Label valuelabel = new Label { Text = value };
            gridList.Add(valuelabel, 1, 2, row, row + rowSpan);
            row += rowSpan;
        }


        /// <summary>
        /// add a GridRow with key value pair. A linkLabel will be used when the Uri is not null.
        /// </summary>
        public static void AddGridRow(IGridList<View> gridList, int row, string key, string value, Uri uri)
        {
            AddGridRow(gridList, row, key, value, uri, false, FormsUtil.LabelDefaultFontSize);
        }

        /// <summary>
        /// add a GridRow with key value pair. A linkLabel will be used when the Uri is not null.
        /// </summary>
        public static void AddGridRow(IGridList<View> gridList, int row, string key, string value, Uri uri, bool boldFont, double fontSize, int maxLines = 1)
        {
            // add a label for the key in column 0
            Label label = new Label { Text = key, FontSize = fontSize, MaxLines = maxLines };
            if (boldFont) label.FontAttributes = FontAttributes.Bold;
            gridList.Add(label, 0, row);
            // add a label for the value in column 1
            label = FormsUtil.CreateLabel(value, uri);
            if (label != null)
            {
                label.FontSize = fontSize;
                label.MaxLines = maxLines;
                if (boldFont) label.FontAttributes = FontAttributes.Bold;
                gridList.Add(label, 1, row);
            }
        }

        /// <summary>
        /// add GridRows with key values. Each value has its own row. 
        /// LinkLabel will be used when the Uri is not null for the corresponding value.
        /// </summary>
        public static void AddGridRows(IGridList<View> gridList, string key, IList<object> values, IList<Uri> uris, ref int row)
        {
            // add a label for the key in column 0
            Label label = new Label { Text = key };
            gridList.Add(label, 0, row);
            if (values == null || values.Count == 0)
            {
                row++;
                return;
            }

            // for each value add one label per row at column 1
            for (int jj = 0; jj < values.Count; jj++)
            {
                string item = values[jj] as string;
                label = FormsUtil.CreateLabel(item, uris?[jj]);
                if (label != null)
                {
                    gridList.Add(label, 1, row++);
                }
            }
        }

        public static void AddImageGridRows(IGridList<View> gridList, string key, string imageFilePath, int height, ref int row, bool spanColumn = true)
        {
            // add a label for the key in column 0
            Label label = new Label { Text = key };
            gridList.Add(label, 0, row);
            if (!File.Exists(imageFilePath))
            {
                row++;
                return;
            }

            Image image = new ZoomImage() { Source = imageFilePath };
            if (spanColumn)
            {
                row++;
                gridList.Add(image, 0, 2, row, row + height);
            }
            else
            {
                gridList.Add(image, 1, 2, row, row + height);
            }
            row += height;

        }
        #endregion
    }
}
