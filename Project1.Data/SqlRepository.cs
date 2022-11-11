using Project1.Logic;
using System.Data.SqlClient;

namespace Project1.Data
{
    public class SqlRepository : IRepository
    {
        private string connectionString;
        public SqlRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public bool CheckUsername(string username)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "SELECT * FROM Users WHERE Username = @username";

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

            string cmdText = "SELECT Username, Password FROM Users WHERE Username = @username";

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

        public bool Register(string username, string password, bool isManager)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "INSERT INTO Users (Username, Password, isManager) " +
                "VALUES (@username, @password, @isManager);";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@isManager", (isManager ? 1 : 0));

            cmd.ExecuteNonQuery();

            string cmdText2 = "SELECT Username, Password, isManager FROM Users";

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
/////////
        public string GetManager()
        {
            return "Not Implemented";
        }
/////////

        public List<Ticket> GetOpenTickets()
        {
            List<Ticket> tickets = new List<Ticket>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            // int ticketNum, string amount, string description, bool isPending, int employeeId, string approvedBy
            string cmdText = "SELECT TicketNum, Amount, Description, isPending, Name, ApprovedBy, EmployeeId FROM Project1.Tickets " +
                "JOIN Project1.Users ON EmployeeId = UserId " +
                "WHERE ApprovedBy IS NULL";

            using SqlCommand cmd = new(cmdText, connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {                   //              TicketNum             Amount               Description           isPending                                   Name
                tickets.Add(new(Convert.ToInt32(reader.GetString(0))), reader.GetString(1), reader.GetString(2), (reader.GetString(3) == "0" ? false : true), reader.GetString(4), "", (Convert.ToInt32(reader.GetString(6))));
            }

            connection.Close();

            if (tickets != null) return tickets;
            else return null;
        }
    }
}