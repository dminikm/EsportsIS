using System;
using System.Collections.Generic;
using System.Windows.Forms;

class Common
{
    public static void ResizeColumns(ListView view)
    {
        int rowHeight = view.Items.Count > 0 ? view.GetItemRect(0).Height : 0;
        int fullHeight = rowHeight * view.Items.Count;

        int scrollbarWidth = 0;
        if (fullHeight >= view.Height)
            scrollbarWidth = SystemInformation.VerticalScrollBarWidth;

        int availLength = view.Width - 4 - scrollbarWidth;
        var lengths = new List<int>();
        int totalLength = 0;

        for (int i = 0; i < view.Columns.Count; i++)
        {
            var column = view.Columns[i];

            totalLength += column.Width;
            lengths.Add(column.Width);
        }

        for (int i = 0; i < view.Columns.Count; i++)
        {
            var column = view.Columns[i];
            var len = lengths[i];

            float percent = (float)len / (float)totalLength;
            int newLen = (int)((float)availLength * percent);

            column.Width = newLen;
        }
    }
}