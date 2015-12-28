using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace TestData
{
    public class Code
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Coding
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }
    }

    public class Employee
    {
        public Employee()
        {
            //EMs = new HashedSet<EmpMsg>();
            Messages = new HashedSet<Message>();
        }

        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Name
        {
            get;
            set;
        }
        public virtual Iesi.Collections.Generic.ISet<EmpMsg> EMs
        {
            get;
            set;
        }

        public virtual Iesi.Collections.Generic.ISet<Message> Messages
        {
            get;
            set;
        }
    }

    public class Message
    {
        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual string Content
        {
            get;
            set;
        }

        //public virtual Iesi.Collections.Generic.ISet<Employee> Employees
        //{
        //    get;
        //    set;
        //}

    }

    public class EmpMsg
    {

        public virtual Guid ID
        {
            get;
            set;
        }
        public virtual Employee Employee
        {
            get;
            set;
        }

        public virtual Message Message
        {
            get;
            set;
        }

        public virtual bool IsRead
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            EmpMsg em = obj as EmpMsg;
            if (em == null)
                return false;
            return em.Employee.ID == Employee.ID && em.Message.ID == Message.ID;
        }

        public override int GetHashCode()
        {
            return (Employee.ID.ToString() + Message.ID.ToString()).GetHashCode();
        }

    }

}
