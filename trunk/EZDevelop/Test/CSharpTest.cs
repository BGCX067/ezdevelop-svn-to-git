using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZDev.Data;
using EZDev.Data.Popedom;
using EZDev.Data.QueryModel;
using NHibernate;


namespace Test
{
    public class CSharpTest
    {
        [NUnit.Framework.Test]
        public void GenericTypeString()
        {
            string str = typeof (List<DateTime>).AssemblyQualifiedName;
            Type t = Type.GetType(str);
            Activator.CreateInstance(t);
            Console.WriteLine(t.Name);
        }

        [NUnit.Framework.Test]
        public void ConditionTest()
        {
            var  c = Conditions.ConditionGroup().Add(Conditions.ConditionGroup().Add(Conditions.Eq("Name", "Phoenix")).Add(Conditions.BetweenAnd("Age", 10, 30))).
                Add(Conditions.ConditionGroup(LogicalOperator.Or).Add(Conditions.Eq("Name", "Fox")).Add(Conditions.In(LogicalOperator.And, true, "Depart.ID", 1,2,3,4)));
            Console.WriteLine(c.ToHql());
            foreach(var o in c.Parameters)
            {
                Console.WriteLine(o);
            }
        }

        [NUnit.Framework.Test]
        public void NormalTest()
        {
            User user = new User();
            Type type = user.GetType();
            do
            {
                Console.WriteLine(type.FullName);
                Console.WriteLine(type.ToString());
                Console.WriteLine(type.Name);
                type = type.BaseType;
            } while (!type.Equals(typeof (Object)));
        }

    }
}
