using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;


namespace EZDev.Data 
{
    /// <summary>
    /// 基础的数据访问对象
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TIdentifier">ID</typeparam>
    public class BaseAccess<TEntity, TIdentifier>
    {
        #region 内部变量

        /// <summary>
        /// 该对象使用的数据访问Session
        /// </summary>
        private ISession session = NHHelper.Instance.GetCurrentSession();

        /// <summary>
        /// 实体对象名称
        /// </summary>
        private string entityName = typeof (TEntity).Name;
        #endregion

        #region 基本操作
        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(TEntity entity)
        {
            session.Save(entity);
            session.Flush();
        }

        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TEntity entity)
        {
            session.Update(entity);
            session.Flush();
        }

        /// <summary>
        /// 插入或更新一个对象
        /// </summary>
        /// <param name="entity"></param>
        public void InsertOrUpdate(TEntity entity)
        {
            session.SaveOrUpdate(entity);
            session.Flush();
        }

        /// <summary>
        /// 删除指定ID的对象
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(TIdentifier id)
        {
            var result = session.CreateQuery(string.Format("delete {0} entity where entity.ID=?", entityName)).
                SetParameter(0, id).ExecuteUpdate();
            return result == 1;
        }

        /// <summary>
        /// 删除指定的对象
        /// </summary>
        /// <param name="entity"></param>
        public int Delete(TEntity entity)
        {
            session.Delete(entity);
            return 1;
        }

        /// <summary>
        /// 通过HQL语句删除对象
        /// </summary>
        /// <param name="hql"></param>
        public int Delete(string hql)
        {
            return session.Delete(hql);
        }

        /// <summary>
        /// 查找指定ID的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Find(TIdentifier id)
        {
            return session.Get<TEntity>(id);
        }

        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="hql">HQL语句</param>
        /// <returns></returns>
        public TEntity Find(string hql)
        {
            return session.CreateQuery(hql).SetMaxResults(1).UniqueResult<TEntity>();
        }
        
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="hql">HQL语句</param>
        /// <param name="parameters">语句中使用的参数</param>
        /// <returns></returns>
        public TEntity Find(string hql, params object[] parameters)
        {
            var query = session.CreateQuery(hql);
            for(int i = 0; i < parameters.Length; i ++)
            {
                query.SetParameter(i, parameters[i]);
            }
            return query.SetMaxResults(1).UniqueResult<TEntity>();
        }

        #endregion

        #region 查询
        /// <summary>
        /// 查找多个对象
        /// </summary>
        /// <param name="hql">指定的HQL语句</param>
        /// <returns>返回的对象集合</returns>
        public IList<TEntity> Query(string hql)
        {
            return session.CreateQuery(hql).List<TEntity>();
        }

        /// <summary>
        /// 查询多个对象
        /// </summary>
        /// <param name="hql">指定的HQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns>返回的对象集合</returns>
        public IList<TEntity> Query(string hql, params object[] parameters)
        {
            var query = session.CreateQuery(hql);
            for(int i = 0; i < parameters.Length; i ++)
            {
                query.SetParameter(i, parameters[i]);
            }
            return query.List<TEntity>();
        }

        /// <summary>
        /// 得到所有记录条数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return session.CreateCriteria(typeof(TEntity)).SetProjection(Projections.RowCount()).UniqueResult<int>();
        }

        /// <summary>
        /// 得到指定查询的结果条数
        /// 例：from Employee where Employee.Age > 15
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        public int GetCount(string hql)
        {
            return GetCount(hql, new object[0]);
        }

        /// <summary>
        /// 得到指定查询的结果条数
        /// 例：from Employee where Employee.Age > ?
        /// </summary>
        /// <param name="hql">HQL语句</param>
        /// <param name="parameters">参数数组</param>
        /// <returns></returns>
        public int GetCount(string hql, params object[] parameters)
        {
            var query = session.CreateQuery("select Count(*) " + hql);
            for(int i = 0; i < parameters.Length; i ++)
            {
                query.SetParameter(i, parameters[i]);
            }
            return query.UniqueResult<int>();
        }

        /// <summary>
        /// 得到指定页的记录
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNumber">页码,从 1 开始计数</param>
        /// <param name="orders">排序条件</param>
        /// <returns></returns>
        public IList<TEntity> Query(int pageSize, int pageNumber, params Order[] orders)
        {
            var query =session.CreateCriteria(typeof (TEntity));
            if (orders != null)
            {
                for(int i = 0; i < orders.Length; i ++)
                {
                    query.AddOrder(orders[i]);
                }
            }
            return query.SetFirstResult(pageSize*pageNumber).SetMaxResults(pageSize).
                    List<TEntity>();
        }

        /// <summary>
        /// 查询指定页的数据
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNumber">页码，从1开始计数</param>
        /// <param name="hql">指定的HQL语句</param>
        /// <param name="parameters">HQL语句使用的参数</param>
        /// <returns>查询结果，如果没有查询到相应数据，则返回空集合</returns>
        public IList<TEntity> Query(int pageSize, int pageNumber, string hql, params object[] parameters)
        {
            var query = session.CreateQuery(hql);
            for (int i = 0; i< parameters.Length;i ++)
            {
                query.SetParameter(i, parameters[i]);
            }
            return query.SetFirstResult(pageSize*pageNumber).SetMaxResults(pageSize).List<TEntity>();
        }

        #endregion
    }
}
