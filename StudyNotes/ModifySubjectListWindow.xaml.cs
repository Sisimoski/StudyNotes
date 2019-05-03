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
        string selectedSubjectItem;

        public ModifySubjectListWindow()
        {
            InitializeComponent();
            SubjectList.ItemsSource = LocalData.CurrentLocalData.Subjects;
            this.DataContext = LocalData.CurrentLocalData.Subjects;
        }

        private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            //XmlDocument document = new XmlDocument();
            //document.Load(xmlLocation);
            //XmlNode subjectNode = document.CreateNode(XmlNodeType.Element, "Subject", "");
            //subjectNode.InnerText = InputNewSubject.Text;
            //document.DocumentElement.AppendChild(subjectNode);
            //document.Save(xmlLocation);

            //XmlDataProvider dataProvider = SubjectList.FindResource("SubjectData") as XmlDataProvider;
            //if (dataProvider != null && dataProvider.Document != null)
            //{
            //    dataProvider.Document = document;
            //    dataProvider.Refresh();
            //}
            LocalData.CurrentLocalData.Subjects.Add(new LocalData.SubjectsList(InputNewSubject.Text));
            LocalData.CurrentLocalData.SaveXmlData();
        }

        private void DeleteSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            //XmlDocument document = new XmlDocument();
            //document.Load(xmlLocation);
            //foreach(XmlNode node in document.SelectNodes("/Subjects/Subject"))
            //{
            //    if (node.InnerText == selectedSubjectItem)
            //    {
            //        node.ParentNode.RemoveChild(node);
            //    }
            //    document.Save(xmlLocation);
            //}
            selectedSubjectItem = SubjectList.SelectedItem.ToString();
            LocalData.CurrentLocalData.Subjects.Remove(new LocalData.SubjectsList(selectedSubjectItem));
            LocalData.CurrentLocalData.SaveXmlData();
        }
    }
}
