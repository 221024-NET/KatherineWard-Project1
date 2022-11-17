using Project1.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Data
{
    public interface IRepository
    {
        string connectionString { get; set; }

        bool CheckUsername(string username);
        bool CheckLogin(string username, string password);
        bool EmployeeRegister(string username, string password, string name);
        bool ManagerRegister(string username, string password, string name);
        User GetUser(string username);

        List<Ticket> GetOpenTickets();
        List<Ticket> GetPreviousTickets(int employeeId);

        Ticket NewTicket(Ticket newTicket, int employeeId);
        Ticket ManageTicket(int ticketId, int status);

    }
}
