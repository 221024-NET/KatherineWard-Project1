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
        public string Amount { get; set; }
        public string Description { get; set; }
        public bool IsPending { get; set; }
        public string Name { get; set; }
        public string ApprovedBy { get; set; }
        public int EmployeeId { get; set; }

        public Ticket() { }

        public Ticket(int ticketNum, string amount, string description, bool isPending, string name, string approvedBy, int employeeId)
        {
            TicketNum = ticketNum;
            Amount = amount;
            Description = description;
            IsPending = isPending;
            Name = name;
            ApprovedBy = approvedBy;
            EmployeeId = employeeId;
        }
    }
}
