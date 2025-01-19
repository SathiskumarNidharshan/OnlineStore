using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Entities
{
    public class Order : EntityBase<int>
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public System.DateTime OrderDate { get; set; }

        public virtual Book Book { get; set; }
        public object Users { get; internal set; }
    }
}