using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZDev.Data.Coding;
using EZDev.Data.QueryModel;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Impl;
using NHibernate.SqlCommand;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using NUnit.Framework;


namespace Test
{
    [NUnit.Framework.TestFixture]
    public class DataTest
    {
        private class TestInterceptor : NHibernate.IInterceptor
        {
            public bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
            {
                return true;
            }

            public bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
            {
                return true;
            }

            public bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
            {
                return true;
            }

            public void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
            {
            }

            public void OnCollectionRecreate(object collection, object key)
            {
            }

            public void OnCollectionRemove(object collection, object key)
            {
            }

            public void OnCollectionUpdate(object collection, object key)
            {
            }

            public void PreFlush(ICollection entities)
            {
            }

            public void PostFlush(ICollection entities)
            {
            }

            public bool? IsTransient(object entity)
            {
                return null;
            }

            public int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
            {
                return new int[0];
            }

            public object Instantiate(string entityName, EntityMode entityMode, object id)
            {
                return null;
            }

            public string GetEntityName(object entity)
            {
                return null;
            }

            public object GetEntity(string entityName, object id)
            {
                return null;
            }

            public void AfterTransactionBegin(ITransaction tx)
            {
            }

            public void BeforeTransactionCompletion(ITransaction tx)
            {
            }

            public void AfterTransactionCompletion(ITransaction tx)
            {
            }

            public SqlString OnPrepareStatement(SqlString sql)
            {
                return sql;
            }

            public void SetSession(ISession session)
            {
            }
        }
        TestInterceptor interceptor = null;
        private NHibernate.ISessionFactory sessionFactory;
        private NHibernate.ISession session = null;
        private NHibernate.Cfg.Configuration config = null;
        [SetUp]
        public void Setup()
        {

            config = new NHibernate.Cfg.Configuration();
            config.Configure("NHibernate.cfg.xml");

            

            // 附加IInterceptor 给ISession对象
            //interceptor = new TestInterceptor();
            //config.Interceptor = interceptor;
            var cfg  = Fluently.Configure(config);
            config = cfg.Mappings(m => m.FluentMappings.AddFromAssembly(typeof(EZDev.Data.ParameterItem).Assembly)).BuildConfiguration();
            sessionFactory = config.BuildSessionFactory();
            //var session2 = sessionFactory.OpenSession();
            //var session1 = sessionFactory.GetCurrentSession();
        }

        [Test]
        public void CreateTable()
        {
            //config.AddAssembly(typeof (User).Assembly);
            var exp = new SchemaExport(config);
            exp.Create(true, true);
        }

        [Test]
        public void TestParameters()
        {
            var query = sessionFactory.OpenSession().CreateQuery("FROM Code WHERE Name = :a or Name = :b or Name = :c");
            var list = query.NamedParameters;
            foreach(var param in list)
            {
                Console.WriteLine(param);
            }
            Console.WriteLine(DateTime.Now.Ticks);
        }

        [NUnit.Framework.Test]
        public void TestCondition()
        {

            var c = Conditions.ConditionGroup().Add(
                Conditions.ConditionGroup().Add(Conditions.Eq("Name", "Phoenix")).
                Add(Conditions.BetweenAnd("Coding", 10,30))).
                Add(Conditions.ConditionGroup(LogicalOperator.Or).Add(Conditions.Eq("Name", "Fox")).Add(
                        Conditions.In(LogicalOperator.And, true, "Name", "2", "3")).Add(Conditions.Like("Name", "PH")));
            var hql = c.ToHql();
            var query = sessionFactory.OpenSession().CreateQuery("From Code where " + c.ToHql());
            Console.WriteLine(query.QueryString);
            var ps = c.Parameters;
            var ns = query.NamedParameters;
            foreach (var p in ps)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("===================================");

            foreach(var n in ns)
            {
                Console.WriteLine(n);
            }
            //for (int i = 0; i < ps.Length; i ++ )
            //{
            //    var p = ps[i];
            //    if (p is IList)
            //    {
            //        query.SetParameterList(query.NamedParameters[i], p as IList);
            //    }
            //    else
            //    {
            //        query.SetParameter(query.NamedParameters[i], p);
            //    }
            //}

            //Console.WriteLine(query.List<Code>().Count);
        }
    }
}
