using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using NHibernate;

namespace EZDev.Data
{
    public class NHHelper
    {
        /// <summary>
        /// 内部的SessionFactory
        /// </summary>
        private static ISessionFactory factory = null;
        
        /// <summary>
        /// 无参构造函数
        /// 设计为私有表示不让用户自己构造此对象，如果使用对象，需要直接调用Instance
        /// </summary>
        private NHHelper()
        {
        }

        /// <summary>
        /// 得到SessionFactory
        /// </summary>
        public ISessionFactory SessionFactory
        {
            get
            {
                return factory;
            }
        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static NHHelper()
        {
            var config = new NHibernate.Cfg.Configuration();
            config.Configure();
            factory = config.BuildSessionFactory();
        }

        private static NHHelper helper;
        private static object locker = new object();
        public static NHHelper Instance
        {
            get
            {
                if (helper == null)
                {
                    lock(locker)
                    {
                        helper = new NHHelper();
                    }
                }
                return helper;
            }
        }

        /// <summary>
        /// 得到当前Session，与ISessionFactory的用法一致
        /// </summary>
        /// <returns></returns>
        public ISession GetCurrentSession()
        {
            var session = factory.GetCurrentSession();
            return session;
        }

        /// <summary>
        /// 打开一个新的Session，用户与ISessionFactory的用法一致
        /// </summary>
        /// <returns></returns>
        public ISession OpenSession()
        {
            return factory.OpenSession();
        }

        /// <summary>
        /// 打开一个新的Session，用户与ISessionFactory的用法一致
        /// </summary>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        public ISession OpenSession(IInterceptor interceptor)
        {
            return factory.OpenSession(interceptor);
        }
    }
}
