

namespace Project1.Logic
{
    public class User
    {
        public int EmployeeId { get; set; }
        public bool IsManager { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(int employeeId, bool isManager, string name, string username, string password)
        {
            EmployeeId = employeeId;
            IsManager = isManager;
            Name = name;
            Username = username;
            Password = password;
        }
    }
}