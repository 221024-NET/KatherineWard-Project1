using Project1.Logic;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

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
        
        public bool CheckLogin(string username, string password)
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

        public bool EmployeeRegister(string username, string password, string name)
        {
            if (CheckUsername(username))
            {
                return false;
            }
            else
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string cmdText = "INSERT INTO Project1.Users (Username, Password, isManager, Name) " +
                                    "VALUES (@username, @password, @isManager, @name);";

                using SqlCommand cmd = new(cmdText, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@isManager", 0);
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
        }

        public bool ManagerRegister(string username, string password, string name)
        {
            if (CheckUsername(username))
            {
                return false;
            }
            else
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string cmdText = "INSERT INTO Project1.Users (Username, Password, isManager, Name) " +
                                    "VALUES (@username, @password, @isManager, @name);";

                using SqlCommand cmd = new(cmdText, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@isManager", 1);
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
                                "WHERE ApprovedBy = 'Pending'";

            using SqlCommand cmd = new(cmdText, connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {                   //Ticket(int ticketNum, int amount, string description, string name, string approvedBy, int employeeId)
                tickets.Add(new Ticket(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetInt32(5)));
            }

            connection.Close();

            if (tickets != null) return tickets;
            else return null;
        }

        public List<Ticket> GetPreviousTickets(int employeeId)
        {
            List<Ticket> tickets = new List<Ticket>();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            //Ticket(int ticketNum, int amount, string description, string name, string approvedBy, int employeeId)
            string cmdText = "SELECT TicketNum, Amount, Description, Name, ApprovedBy, EmployeeId FROM Project1.Tickets " +
                                "JOIN Project1.Users ON EmployeeId = UserId " +
                                "WHERE Project1.Users.UserId = @employeeId";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {                   //Ticket(int ticketNum, int amount, string description, string name, string status, int employeeId)
                tickets.Add(new Ticket(reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetInt32(5)));
            }

            connection.Close();

            if (tickets != null) return tickets;
            else return null;
        }


        public Ticket NewTicket(Ticket newTicket, int employeeId)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "INSERT INTO Project1.Tickets (Amount, Description, EmployeeId) " +
                "VALUES " +
                "(@amount, @description, @employeeId); " +
                "SELECT @@IDENTITY;";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@amount", newTicket.Amount);
            cmd.Parameters.AddWithValue("@description", newTicket.Description);
            cmd.Parameters.AddWithValue("@employeeId", employeeId);

            using SqlDataReader reader = cmd.ExecuteReader();
            Ticket currentTicket = new Ticket();

            while (reader.Read())
            {
                currentTicket.TicketNum = reader.GetInt32(0);
            }
            cmd.Dispose();

            string cmdText2 = "SELECT Amount, Description, EmployeeId, TicketNum, Name, ApprovedBy FROM Project1.Tickets " +
                "JOIN Project1.Users ON EmployeeId = UserId " +
                "WHERE TicketNum = @ticketNum";

            using SqlCommand cmd2 = new(cmdText2, connection);
            cmd2.Parameters.AddWithValue("@ticketNum", currentTicket.TicketNum);

            using SqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                currentTicket.Amount = reader2.GetInt32(0);
                currentTicket.Description = reader2.GetString(1);
                currentTicket.EmployeeId = reader2.GetInt32(2);
                currentTicket.TicketNum = reader2.GetInt32(3);
                currentTicket.Name = reader2.GetString(4);
                currentTicket.Status = reader.GetString(5);
                return currentTicket;
            }

            return null;
        }
        public Ticket ManageTicket(int ticketId, int status) // 1 = "Approved", 2 = "Denied"
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string cmdText = "UPDATE Project1.Tickets SET ApprovedBy = @status " +
                "WHERE TicketNum = @ticketId;";

            using SqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@status", (status == 1 ? "Approved" : "Denied"));
            cmd.Parameters.AddWithValue("@ticketId", ticketId);

            cmd.ExecuteNonQuery();

            cmd.Dispose();

            string cmdText2 = "SELECT Amount, Description, EmployeeId, TicketNum, Name, ApprovedBy FROM Project1.Tickets " +
                "JOIN Project1.Users ON EmployeeId = UserId " +
                "WHERE TicketNum = @ticketNum";

            Ticket ticketStatus = new Ticket();

            using SqlCommand cmd2 = new(cmdText2, connection);
            cmd2.Parameters.AddWithValue("@ticketNum", ticketId);

            using SqlDataReader reader = cmd2.ExecuteReader();

            while (reader.Read())
            {
                ticketStatus.Amount = reader.GetInt32(0);
                ticketStatus.Description = reader.GetString(1);
                ticketStatus.EmployeeId = reader.GetInt32(2);
                ticketStatus.TicketNum = reader.GetInt32(3);
                ticketStatus.Name = reader.GetString(4);
                ticketStatus.Status = reader.GetString(5);
                return ticketStatus;
            }

            return null;
        }
    }
}