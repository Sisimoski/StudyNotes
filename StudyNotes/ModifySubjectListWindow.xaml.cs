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
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace StudyNotes
{
    /// <summary>
    /// Logika interakcji dla klasy ModifySubjectListWindow.xaml
    /// </summary>
    public partial class ModifySubjectListWindow : Window
    {
        // This will get the current WORKING directory (i.e. \bin\Debug)
        static string workingDirectory = Environment.CurrentDirectory;
        // or: Directory.GetCurrentDirectory() gives the same result

        // This will get the current PROJECT directory
        static string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

        static string xmlLocation = System.IO.Path.Combine(projectDirectory, "SubjectList.xml");

        //string executableLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //string xmlLocation = System.IO.Path.Combine(executableLocation, "SubjectList.xml");

        string selectedSubjectItem;

        public ModifySubjectListWindow()
        {
            InitializeComponent();
        }

        private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlLocation);
            XmlNode subjectNode = document.CreateNode(XmlNodeType.Element, "Subject", "");
            subjectNode.InnerText = InputNewSubject.Text;
            document.DocumentElement.AppendChild(subjectNode);
            document.Save(xmlLocation);

            XmlDataProvider dataProvider = SubjectList.FindResource("SubjectData") as XmlDataProvider;
            if (dataProvider != null && dataProvider.Document != null)
            {
                dataProvider.Document = document;
                dataProvider.Refresh();
            }
        }

        private void DeleteSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument document = new XmlDocument();
            document.Load(xmlLocation);
            foreach(XmlNode node in document.SelectNodes("/Subjects/Subject"))
            {
                if (node.InnerText == selectedSubjectItem)
                {
                    node.ParentNode.RemoveChild(node);
                }
                document.Save(xmlLocation);
            }
        }

        private void SubjectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSubjectItem = SubjectList.SelectedItem.ToString();
        }
    }
}
