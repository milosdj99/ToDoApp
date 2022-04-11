using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCore
{
    public class ToDoShareList
    {
        public Guid Id { get; set; }

        public Guid ListId { get; set; }

        public DateTime ExpirationTime { get; set; }
        
    }
}
