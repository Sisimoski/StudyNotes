using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace StudyNotes
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _needsToBeSaved;
        string[] filePaths;

        public MainWindow()
        {
            InitializeComponent();
            InitializeFontOnStartup();
            DefaultWelcomeDocument();
            SelectSubject.SelectedIndex = 0;
        }

        void addNotes()
        {
            NotesList.Items.Clear();

            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, @SelectSubject.SelectedValue + @"\"));
            if (System.IO.Directory.Exists(path))
            {
                filePaths = Directory.GetFiles(path, "*.rtf", SearchOption.TopDirectoryOnly);
                for (int i = 0; i < filePaths.Length; i++) NotesList.Items.Add(System.IO.Path.GetFileNameWithoutExtension(filePaths[i]));
            }

        }

        private void MainTextEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            _needsToBeSaved = true;
        }

        private void InitializeFontOnStartup()
        {
            fontFamilySelector.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            fontSizeSelector.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }
        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFamilySelector.SelectedItem != null)
                MainTextEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilySelector.SelectedItem);
        }
        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fontSizeSelector.SelectedItem != null)
            MainTextEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeSelector.Text);
        }
        private void MainTextEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = MainTextEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = MainTextEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = MainTextEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = MainTextEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontFamilySelector.SelectedItem = temp;
            temp = MainTextEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            fontSizeSelector.Text = temp.ToString();
        }

        private void ModifySubjectListButton_Click(object sender, RoutedEventArgs e)
        {
            ModifySubjectListWindow subjectListWindow = new ModifySubjectListWindow();
            subjectListWindow.ShowDialog();
        }

        private void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_needsToBeSaved)
            {
                var messageBoxText =
                    "Twój dokument nie został zapisany, chcesz kontynuować bez zapisywania?";
                var caption = "Study Notes";
                var button = MessageBoxButton.OKCancel;
                var icon = MessageBoxImage.Warning;

                // Display message box
                var messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (messageBoxResult)
                {
                    case MessageBoxResult.OK: // Save document and exit
                        NewEmptyDocument();
                        break;
                    case MessageBoxResult.Cancel: // Exit without saving
                        break;
                }
            }
            else NewEmptyDocument();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveDocument();
        }


        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument();
        }

        private void NewEmptyDocument()
        {
            FlowDocument document = new FlowDocument();
            MainTextEditor.Document = document;
            _needsToBeSaved = false;
        }

        private void DefaultWelcomeDocument()
        {
            FlowDocument defaultDocument = new FlowDocument();
            defaultDocument.Language = XmlLanguage.GetLanguage("pl-pl");
            Paragraph paragraph = new Paragraph(new Run("Witaj w Study Notes!"));
            paragraph.FontSize = 36;
            defaultDocument.Blocks.Add(paragraph);

            paragraph = new Paragraph(new Run("Twój nowy ulubiony studencki notatnik."));
            paragraph.FontSize = 14;
            paragraph.FontStyle = FontStyles.Italic;
            paragraph.FontFamily = new FontFamily("Cambria");
            paragraph.TextAlignment = TextAlignment.Left;
            paragraph.Foreground = Brushes.Gray;
            defaultDocument.Blocks.Add(paragraph);

            MainTextEditor.Document = defaultDocument;
            _needsToBeSaved = false;
        }

        private void SaveDocument()
        {
            string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "Notatka";
                saveFileDialog.DefaultExt = ".rtf";
                saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.CheckPathExists = true;
                saveFileDialog.InitialDirectory = documentsDirectory;
                if (saveFileDialog.ShowDialog() == true)
                {
                    FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                    TextRange range = new TextRange(MainTextEditor.Document.ContentStart, MainTextEditor.Document.ContentEnd);
                    range.Save(fileStream, DataFormats.Rtf);
                    fileStream.Close();
                    _needsToBeSaved = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrintDocument()
        {
            // Configure printer dialog
            var dlg = new PrintDialog
            {
                PageRangeSelection = PageRangeSelection.AllPages,
                UserPageRangeEnabled = true
            };

            // Show save file dialog
            var result = dlg.ShowDialog();

            // Process save file dialog results
            if (result == true)
            {
                // Print document
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // If the document needs to be saved
            if (_needsToBeSaved)
            {
                // Configure the message box
                var messageBoxText =
                    "Twój dokument nie został zapisany. Kliknij Tak aby zapisać, Nie aby wyjść bez zapisywania, lub Anuluj aby nie wychodzić.";
                var caption = "Study Notes";
                var button = MessageBoxButton.YesNoCancel;
                var icon = MessageBoxImage.Warning;

                // Display message box
                var messageBoxResult = MessageBox.Show(messageBoxText, caption, button, icon);

                // Process message box results
                switch (messageBoxResult)
                {
                    case MessageBoxResult.Yes: // Save document and exit
                        SaveDocument();
                        break;
                    case MessageBoxResult.No: // Exit without saving
                        break;
                    case MessageBoxResult.Cancel: // Don't exit
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void NotesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FileStream fileStream = File.Open(filePaths[NotesList.SelectedIndex], FileMode.Open);
                MainTextEditor.Document.Blocks.Clear();
                MainTextEditor.Selection.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void SelectSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addNotes();
        }
    }
}
