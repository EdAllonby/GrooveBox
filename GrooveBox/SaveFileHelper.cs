using Microsoft.Win32;

namespace GrooveBox
{
    public static class SaveFileHelper
    {
        public static string SaveFile(string filter, string extension)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = extension,
                AddExtension = false
            };

            bool? showDialog = saveFileDialog.ShowDialog();

            if (showDialog != null && showDialog.Value)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }
    }
}