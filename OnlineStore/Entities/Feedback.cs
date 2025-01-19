using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Entities
{
    public class Feedback : EntityBase<int>
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public string Comments { get; set; }

        public virtual Book Book { get; set; }
    }
}