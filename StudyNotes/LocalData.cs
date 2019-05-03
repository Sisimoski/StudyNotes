using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
        // or: Directory.GetCurrentDirectory() gives the same result

        // This will get the current PROJECT directory
        static string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

        static string xmlLocation = System.IO.Path.Combine(projectDirectory, "SubjectList.xml");

        static string executableLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string xmlLocationEXE = System.IO.Path.Combine(executableLocation, "SubjectList.xml");

        string startupPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "SubjectList.xml");

        //public List<string> AllSubjects;

        public ObservableCollection<SubjectsList> Subjects;

        //public ObservableCollection<SubjectsList> subjectsLists;

        public class SubjectsList
        {
            public string Subject { get; set; }
            public SubjectsList() { }
            public SubjectsList(string nameOfSubject)
            {
                this.Subject = nameOfSubject;
            }
        }

        public LocalData()
        {
            //AllSubjects = new List<string>();
            Subjects = new ObservableCollection<SubjectsList>();
            LoadXmlData();
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
                    Subjects.Add(new SubjectsList(item.InnerText));
                }
            }
        }

        public void SaveXmlData()
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<SubjectsList>));

            using (var stream = File.Open(startupPath, FileMode.Create))
            {
                serializer.Serialize(stream, Subjects);
            }
            
            //XmlSerializer serializer = new XmlSerializer(typeof());

            //using (FileStream stream = File.OpenWrite(myXmlFilePath))
            //{
            //    serializer.Serialize(stream, myListView.Items);
            //}
        }
    }
}
