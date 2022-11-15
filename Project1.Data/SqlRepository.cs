using Project1.Logic;
using System.Data.SqlClient;

namespace Project1.Data
{
    public class SqlRepository : IRepository
    {
        public string connectionString { get; set; }

        //public SqlRepository(string connectionString)
        //{
        //    this.connectionString = connectionString;
        //}
        public bool CheckUsername(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "SELECT * FROM Project1.Users WHERE Username = @username";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                return true; //Username exists, try another
            }

            connection.Close();
            return false; //Username is free
        }
        
        public bool GetLogin(string username, string password)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "SELECT Username, Password FROM Project1.Users WHERE Username = @username";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = cmd.ExecuteReader();

            string userAuth, passAuth = "";

            while (reader.Read())
            {
                userAuth = reader.GetString(0);
                passAuth = reader.GetString(1);
                if (username.Equals(userAuth) && password.Equals(passAuth)) return true;
            }

            connection.Close();
            return false; // Could not log in
        }

        public bool Register(string username, string password, bool isManager, string name)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "INSERT INTO Project1.Users (Username, Password, isManager, Name) " +
                                "VALUES (@username, @password, @isManager, @name);";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@isManager", (isManager ? 1 : 0));
            cmd.Parameters.AddWithValue("@name", name);

            cmd.ExecuteNonQuery();

            string cmdText2 = "SELECT Username, Password FROM Project1.Users";

            using SqlCommand cmd2 = new(cmdText2, connection);

            using SqlDataReader reader = cmd2.ExecuteReader();

            string userAuth, passAuth = "";

            while (reader.Read())
            { 
                userAuth = reader.GetString(0); 
                passAuth = reader.GetString(1);
                if (username.Equals(userAuth) && password.Equals(passAuth)) return true;
            }

            connection.Close();
            return false;
        }

        public User GetUser(string username)
        {
            User user = new User();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "SELECT UserId, IsManager, Name, Username, Password FROM Project1.Users " +
                                "WHERE Username = @username;";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // User(int employeeId, bool isManager, string name)
                //user = new User(reader.GetInt32(0), reader.GetBoolean(1), reader.GetString(2));
                user.EmployeeId = reader.GetInt32(0);
                user.IsManager = reader.GetBoolean(1);
                user.Name = reader.GetString(2);
                user.Username = reader.GetString(3);
                user.Password = reader.GetString(4);
            }
            connection.Close();
            return user;
            //if (user != null) 
            //else return null;
        }

        public List<Ticket> GetOpenTickets()
        {
            List<Ticket> tickets = new List<Ticket>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            //Ticket(int ticketNum, int amount, string description, string name, string approvedBy, int employeeId)
            string cmdText = "SELECT TicketNum, Amount, Description, Name, ApprovedBy, EmployeeId FROM Project1.Tickets " +
                                "JOIN Project1.Users ON EmployeeId = UserId " +
                                "WHERE ApprovedBy IS NULL";

            using SqlCommand cmd = new(cmdText, connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {                   //Ticket(int ticketNum, int amount, string description, string name, string approvedBy, int employeeId)
                tickets.Add(new Ticket(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    "Pending",
                    reader.GetInt32(5)));
            }

            connection.Close();

            if (tickets != null) return tickets;
            else return null;
        }

        public List<Ticket> GetPreviousTickets(string username)
        {
            List<Ticket> tickets = new List<Ticket>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            //Ticket(int ticketNum, int amount, string description, string name, string approvedBy, int employeeId)
            string cmdText = "SELECT TicketNum, Amount, Description, Name, ApprovedBy, EmployeeId FROM Project1.Tickets " +
                                "JOIN Project1.Users ON EmployeeId = UserId " +
                                "WHERE Project1.Users.Username = @username";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@username", username);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {                   //Ticket(int ticketNum, int amount, string description, string name, string approvedBy, int employeeId)
                tickets.Add(new Ticket(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    (reader.GetString(4) != null ? reader.GetString(4) : "Pending" ),
                    reader.GetInt32(5)));
            }

            connection.Close();

            if (tickets != null) return tickets;
            else return null;
        }


    }
}