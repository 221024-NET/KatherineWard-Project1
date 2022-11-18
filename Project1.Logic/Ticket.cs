using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Logic
{
    public class Ticket
    {
        public int TicketNum { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int EmployeeId { get; set; }

        public Ticket() { }

        public Ticket(int ticketNum, decimal amount, string description, string name, string status, int employeeId)
        {
            TicketNum = ticketNum;
            Amount = amount;
            Description = description;
            Name = name;
            Status = status;
            EmployeeId = employeeId;
        }
    }
}
