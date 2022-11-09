using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Project1
{
    public class User
    {
        // set the fields as xml attributes
        [XmlAttribute]
        public string email { get; set; }
        public string password { get; set; }
        public bool isManager { get; set; }

        // make a serializer object
        public XmlSerializer Serializer { get; } = new XmlSerializer(typeof(List<User>));

        public User() {}

        public User(string email, string password)
        {
            // set user info
            this.email = email;
            this.password = password;
            this.isManager = false;
        }

        public User(string email, string password, bool isManager)
        {
            // set user info (with manager flag)
            this.email = email;
            this.password = password;
            this.isManager = isManager;
        }

        public void SerializeUser(List<User> users)
        {
            // create new StringWriter
            var newStringWriter = new StringWriter();
            // Serialize the users and writes it to the StringWriter
            Serializer.Serialize(newStringWriter, users);
            // write the serialized users string to the xml file
            File.WriteAllText("./.xml", newStringWriter.ToString());
            // close the StringWriter
            newStringWriter.Close();
        }

        public List<User> GetUsers()
        {
            // create a new StreamReader reading from the xml file
            StreamReader reader = new StreamReader("./.xml");
            // Deserialize the information read from the xml and store it in users (if it isn't null)
            var users = (List<User>?)Serializer.Deserialize(reader);
            // close the reader
            reader.Close();
            // return the Deserialized list of Users
            return users;
        }
    }
}