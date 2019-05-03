using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace StudyNotes
{
    public class LocalData
    {
        public static LocalData CurrentLocalData = new LocalData();

        // This will get the current WORKING directory (i.e. \bin\Debug)
        static string workingDirectory = Environment.CurrentDirectory;

        // This will get the current PROJECT directory
        static string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

        static string xmlLocation = System.IO.Path.Combine(projectDirectory, "SubjectList.xml");

        string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "SubjectList.xml");

        public List<string> AllSubjects;

        public ObservableCollection<string> Subjects;

        public LocalData()
        {
            AllSubjects = new List<string>();
            LoadXmlData();
            Subjects = new ObservableCollection<string>(AllSubjects);
        }

        private void LoadXmlData()
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNodeList xmlNodeList;
            using (FileStream fs = new FileStream(startupPath, FileMode.Open, FileAccess.Read))
            {
                xmlDocument.Load(fs);
                xmlNodeList = xmlDocument.GetElementsByTagName("Subject");
                foreach (XmlNode item in xmlNodeList)
                {
                    AllSubjects.Add(item.InnerText);
                }
            }
        }

        public void SaveXmlData()
        {
            var serializer = new XmlSerializer(typeof(List<string>));

            using (var stream = File.Open(startupPath, FileMode.Create))
            {
                serializer.Serialize(stream, AllSubjects);
            }

            //XmlSerializer serializer = new XmlSerializer(typeof());

            //using (FileStream stream = File.OpenWrite(myXmlFilePath))
            //{
            //    serializer.Serialize(stream, myListView.Items);
            //}
        }
    }
}
