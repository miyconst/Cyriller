using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace Cyriller.Desktop.Views
{
    public static class ViewExtensions
    {
        public static Window GetParentWindow(this IControl control)
        {
            while (control != null)
            {
                if (control is Window window)
                {
                    return window;
                }

                control = control.Parent;
            }

            throw new InvalidOperationException("Current Control is not attached to any Window object.");
        }

        public static async Task<string> SaveFileDialog(this IControl control, string title, string fileExtension, string fileTypeDescription)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Title = title;
            sfd.Filters.Clear();
            sfd.Filters.Add(new FileDialogFilter() { Extensions = new List<string> { fileExtension }, Name = fileTypeDescription });

            Window window = control.GetParentWindow();
            string file = await sfd.ShowAsync(window);

            return file;
        }
    }
}
