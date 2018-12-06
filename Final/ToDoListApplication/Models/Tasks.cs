using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoListApplication.Models
{
    public class Tasks
    {
        public virtual int Id { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual string Title {get; set;}
        public virtual DateTime DueDate { get; set; }
    }
}