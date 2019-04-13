using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes
{
    class Unused_Code
    {
        private void SaveDocument()
        {
            // Configure save file dialog
            var dlg = new SaveFileDialog
            {
                FileName = "Document",
                DefaultExt = ".rtf",
                Filter = "Rich Text Format (.rtf)|*.rtf"
            };
            // Default file name
            // Default file extension
            // Filter files by extension

            // Show save file dialog
            var result = dlg.ShowDialog();

            // Process save file dialog results
            if (result == true)
            {
                // Save document
                var filename = dlg.FileName;
            }

        }
    }
}
