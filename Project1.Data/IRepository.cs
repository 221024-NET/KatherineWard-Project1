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
        bool GetLogin(string username, string password);
        bool Register(string username, string password, bool isManager, string name);
        User GetUser(string username);

        List<Ticket> GetOpenTickets();
        List<Ticket> GetPreviousTickets(string username);

        //bool NewTicket(Ticket newTicket);

    }
}
