using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EZDev.Data.QueryModel;
using NHibernate;
using NHibernate.Criterion;

namespace EZDev.Data
{
    /// <summary>
    /// 基础的数据访问对象
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TIdentifier">ID</typeparam>
    public class Repository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        #region 内部变量

        /// <summary>
        /// 实体的类型
        /// </summary>
        protected readonly Type entityType = typeof (TEntity);

        /// <summary>
        /// 数据库操作的Session
        /// </summary>
        protected ISession session;

        #endregion

        /// <summary>
        /// 构造函数
        /// 内部Session通过 GetCurrentSession获得
        /// </summary>
        public Repository() : this(NHHelper.Instance.GetCurrentSession())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="session">使用的Session</param>
        public Repository(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.session = session;
        }

        #region 基本操作

        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="entity">指定的对象</param>
        public void Insert(TEntity entity)
        {
            session.Save(entity);
        }

        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="entity">指定的对象</param>
        public void Update(TEntity entity)
        {
            session.Update(entity);
        }

        /// <summary>
        /// 得到指定ID的对象
        /// </summary>
        /// <param name="id">指定的ID</param>
        /// <returns>返回的对象，如果没有找到则返回null</returns>
        public TEntity Get(TIdentifier id)
        {
            return session.Get<TEntity>(id);
        }

        /// <summary>
        /// 删除指定ID的对象
        /// </summary>
        /// <param name="id">指定的ID</param>
        public void Delete(TIdentifier id)
        {
            session.CreateQuery(string.Format("DELETE FROM {0} entity WHERE entity.ID = :id", entityType.Name)).
                SetParameter(0, id).ExecuteUpdate();
        }

        /// <summary>
        /// 删除指定的对象
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            session.Delete(entityType);
        }

        /// <summary>
        /// 得到记录数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return session.QueryOver<TEntity>().RowCount();
        }

        /// <summary>
        /// 得到指定条件记录的数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return session.QueryOver<TEntity>().Where(predicate).RowCount();
        }

        /// <summary>
        /// 得到指定条件记录的数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public int Count(ICondition condition)
        {
            IConditionGroup cg = RepositoryHelper.ToConditionGroup(condition);
            return
                session.CreateQuery(string.Format("Select COUNT(ID) From {0} WHERE {1}", entityType.Name, cg.ToHql())).
                    SetParameters(cg.Parameters).UniqueResult<int>();
        }

        /// <summary>
        /// 得到指定条件的查询结果
        /// 如果条件可返回多条记录，则只返回第一条
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回的对象</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return session.QueryOver<TEntity>().Where(predicate).SingleOrDefault();
        }

        /// <summary>
        /// 得到指定条件的查询结果
        /// 如果条件可返回多条记录，则只返回第一条
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回的对象</returns>
        public TEntity Get(ICondition condition)
        {
            IQuery query = session.CreateQuery(string.Format("FROM {0} WHERE {1}", entityType.Name, condition.ToHql()));
            query.SetParameters(condition.Parameters);
            return query.SetMaxResults(1).UniqueResult<TEntity>();
        }

        /// <summary>
        /// 查询所有的数据
        /// </summary>
        /// <returns></returns>
        public IList<TEntity> Query()
        {
            return session.QueryOver<TEntity>().List();
        }

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回的结果集</returns>
        public IList<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return session.QueryOver<TEntity>().Where(predicate).List();
        }

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回的结果集</returns>
        public IList<TEntity> Query(ICondition condition)
        {
            return
                session.CreateQuery(string.Format("FROM {0} WHERE {1}", entityType.Name, condition.ToHql())).
                    SetParameters(condition.Parameters).List<TEntity>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从 1 开始</param>
        /// <param name="orderbyStrings"></param>
        /// <returns>返回结果集</returns>
        public IList<TEntity> QueryPage(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNum,
                                        params string[] orderbyStrings)
        {
            IQueryOver<TEntity, TEntity> query = session.QueryOver<TEntity>().Where(predicate);
            query = orderbyStrings.Select(RepositoryHelper.ParseOrderbyString).
                Aggregate(query,
                          (current, tup) =>
                          tup.Item2
                              ? current.OrderBy(Projections.Property(tup.Item1)).Desc
                              : current.OrderBy(Projections.Property(tup.Item1)).Asc);
            return query.Take(pageSize*pageNum).Skip(pageSize*(pageNum - 1)).List();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码</param>
        /// <returns></returns>
        public IList<TEntity> QueryPage(ICondition condition, OrderByGroup orderBy, int pageSize, int pageNum)
        {
            return
                session.CreateQuery(string.Format("FROM {0} WHERE {1} ORDER BY {2}", entityType.Name, condition.ToHql(),
                                                  orderBy.ToHql())).SetMaxResults(pageSize).SetFirstResult(
                                                      (pageNum - 1)*pageSize).List<TEntity>();
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public ITransaction BeginTransaction()
        {
            return new NHTransaction(session);
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (session != null)
            {
                session.Flush();
                session.Close();
                session.Dispose();
                session = null;
            }
        }
    }

    /// <summary>
    /// 支持逻辑删除的存储管理器
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TIdentifier">ID数据类型</typeparam>
    public class LogicDeleteRepository<TEntity, TIdentifier> : ILogicDeleteRepository<TEntity, TIdentifier>
        where TEntity : LogicDeleteEntity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        #region 内部变量

        /// <summary>
        /// 逻辑删除条件
        /// </summary>
        protected  Expression<Func<TEntity, bool>> isLogicDeleteExpression =
            entity => entity.LogicDelete.IsDelete == false;

        /// <summary>
        /// 逻辑删除表达式
        /// </summary>
        protected  ICondition isLogicDeleteCondition =
            Conditions.ConditionGroup().Add(Conditions.Eq("IsDelete", false));

        /// <summary>
        /// 实体的类型
        /// </summary>
        protected readonly Type entityType = typeof (TEntity);

        /// <summary>
        /// Session
        /// </summary>
        protected ISession session;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public LogicDeleteRepository() : this(NHHelper.Instance.GetCurrentSession())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="session">内部使用的Session</param>
        public LogicDeleteRepository(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.session = session;
        }

        #region ILogicDeleteRepository<TEntity,TIdentifier> Members

        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="entity">指定的对象</param>
        public void Insert(TEntity entity)
        {
            session.SaveOrUpdate(entity);
        }

        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="entity">指定的对象</param>
        public void Update(TEntity entity)
        {
            session.Update(entity);
        }

        /// <summary>
        /// 得到指定ID的对象
        /// </summary>
        /// <param name="id">指定的ID</param>
        /// <returns>返回的对象，如果没有找到则返回null</returns>
        public TEntity Get(TIdentifier id)
        {
            return session.Get<TEntity>(id);
        }

        /// <summary>
        /// 删除指定ID的对象
        /// </summary>
        /// <param name="id">指定的ID</param>
        public void Delete(TIdentifier id)
        {
            session.CreateQuery(string.Format("DELETE FROM {0} entity WHERE entity.ID = :id", entityType.Name)).
                SetParameter(0, id).ExecuteUpdate();
        }

        /// <summary>
        /// 删除指定的对象
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TEntity entity)
        {
            session.Delete(entity);
        }


        /// <summary>
        /// 得到记录数量
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return session.QueryOver<TEntity>().Where(isLogicDeleteExpression).RowCount();
        }

        /// <summary>
        /// 得到指定条件记录的数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            predicate = predicate.ContactExpressions<TEntity, bool>(isLogicDeleteExpression);
            return session.QueryOver<TEntity>().Where(predicate).RowCount();
        }

        /// <summary>
        /// 得到指定条件记录的数量
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int Count(ICondition condition)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition).Add(isLogicDeleteCondition);
            return
                session.CreateQuery(string.Format("SELECT COUNT(ID) FROM {0} where {1}", entityType.Name, group.ToHql()))
                    .SetParameters(group.Parameters).UniqueResult<int>();
        }

        /// <summary>
        /// 得到指定条件的查询结果
        /// 如果条件可返回多条记录，则只返回第一条
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回的对象</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return
                session.QueryOver<TEntity>().Where(
                    predicate.ContactExpressions<TEntity, bool>(isLogicDeleteExpression)).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// 得到指定条件的查询结果
        /// 如果条件可返回多条记录，则只返回第一条
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回的对象</returns>
        public TEntity Get(ICondition condition)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition).Add(isLogicDeleteCondition);
            return
                session.CreateQuery(string.Format("FROM {0} where {1}", entityType.Name, group.ToHql())).SetParameters(
                    group.Parameters).SetMaxResults(1).UniqueResult<TEntity>();
        }

        /// <summary>
        /// 返回所有的数据
        /// 不包含标记删除的数据
        /// </summary>
        /// <returns>返回的数据集合</returns>
        public IList<TEntity> Query()
        {
            return session.QueryOver<TEntity>().Where(isLogicDeleteExpression).List();
        }

        /// <summary>
        /// 按条件查询结果集
        /// 不包含逻辑删除数据
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回的结果集</returns>
        public IList<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return
                session.QueryOver<TEntity>().Where(
                    predicate.ContactExpressions<TEntity, bool>(isLogicDeleteExpression)).List();
        }

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回的结果集</returns>
        public IList<TEntity> Query(ICondition condition)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition).Add(isLogicDeleteCondition);
            return
                session.CreateQuery(string.Format("FROM {0} WHERE {1}", entityType.Name, group.ToHql())).SetParameters(
                    group.Parameters).List<TEntity>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从 1 开始</param>
        /// <param name="orderbyStrings"></param>
        /// <returns>返回结果集</returns>
        public IList<TEntity> QueryPage(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNum,
                                        params string[] orderbyStrings)
        {
            return
                session.QueryOver<TEntity>().Where(
                    predicate.ContactExpressions<TEntity, bool>(isLogicDeleteExpression)).Take(pageSize*pageNum).
                    Skip(
                        pageSize*(pageNum - 1)).List();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码</param>
        /// <returns></returns>
        public IList<TEntity> QueryPage(ICondition condition, OrderByGroup orderBy, int pageSize, int pageNum)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition).Add(isLogicDeleteCondition);
            return
                session.CreateQuery(string.Format("FROM  {0} where {1} Order By {2}", entityType.Name, group.ToHql(),
                                                  orderBy.ToHql())).SetParameters(group.Parameters).SetFirstResult(
                                                      (pageNum - 1)*pageSize).SetMaxResults(pageNum).List<TEntity>();
        }

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        public ITransaction BeginTransaction()
        {
            return new NHTransaction(session);
        }

        /// <summary>
        /// 物理删除数据库中已经标记删除的数据
        /// 此方法直接操作数据库不可恢复
        /// </summary>
        public void PhysicalDelete()
        {
            session.CreateQuery(string.Format("DELETE FROM {0} entity WHERE entity.IsDelete = true", entityType.Name))
                .ExecuteUpdate();
        }

        /// <summary>
        /// 物理删除数据库中已经标记删除的数据
        /// </summary>
        /// <param name="predicate">指定的过滤条件</param>
        public void PhysicalDelete(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TIdentifier> idList =
                session.QueryOver<TEntity>().Select(x => x.ID).Where(predicate).List<TIdentifier>();
            session.CreateQuery(
                string.Format("DELETE FROM {0} entity WHERE entity.IsDelete = true AND entity.ID in (:idList)",
                              entityType.Name)).SetParameterList("idList", idList).ExecuteUpdate();
        }

        /// <summary>
        /// 物理删除数据库中已经标记删除的数据
        /// </summary>
        /// <param name="condition"></param>
        public void PhysicalDelete(ICondition condition)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition).Add(isLogicDeleteCondition);
            session.CreateQuery(string.Format("DELETE FROM {0} WHERE {1}", entityType.Name, group.ToHql())).
                SetParameters(group.Parameters).ExecuteUpdate();
        }

        /// <summary>
        /// 返回所有的数据，包含已经标记为删除的数据
        /// </summary>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <returns>返回的数据集合</returns>
        public IList<TEntity> Query(bool includeLogicDelete)
        {
            return includeLogicDelete ? session.QueryOver<TEntity>().List() : Query();
        }

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <param name="orderbyStrings">OrderBy条件，支持以下格式：
        /// Name 表示按名称升序排列
        /// Name ASC 表示按名称升序排列
        /// Name DESC 表示按名称降序排列
        /// 其中排序字段名为属性名，大小写必须一致
        /// </param>
        /// <returns>返回的结果集</returns>
        public IList<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool includeLogicDelete,
                                    params string[] orderbyStrings)
        {
            IQueryOver<TEntity, TEntity> query = includeLogicDelete
                                                     ? session.QueryOver<TEntity>().Where(predicate)
                                                     : session.QueryOver<TEntity>().Where(predicate).Where(
                                                         isLogicDeleteExpression);
            query = orderbyStrings.Select(RepositoryHelper.ParseOrderbyString).
                Aggregate(query,
                          (current, tup) =>
                          tup.Item2
                              ? current.OrderBy(Projections.Property(tup.Item1)).Desc
                              : current.OrderBy(Projections.Property(tup.Item1)).Asc);
            return query.List();
        }

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <returns>返回的结果集</returns>
        public IList<TEntity> Query(ICondition condition, OrderByGroup orderBy, bool includeLogicDelete)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition);
            if (!includeLogicDelete)
            {
                group = group.Add(isLogicDeleteCondition);
            }
            return
                session.CreateQuery(string.Format("FROM {0} WHERE {1} ORDER BY {2}", entityType.Name, group.ToHql(),
                                                  orderBy.ToHql())).List<TEntity>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从 1 开始</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <param name="orderbyStrings">OrderBy条件，支持以下格式：
        /// Name 表示按名称升序排列
        /// Name ASC 表示按名称升序排列
        /// Name DESC 表示按名称降序排列
        /// 其中排序字段名为属性名，大小写必须一致
        /// </param>        /// <returns>返回结果集</returns>
        public IList<TEntity> QueryPage(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNum,
                                        bool includeLogicDelete, params string[] orderbyStrings)
        {
            IQueryOver<TEntity, TEntity> query = includeLogicDelete
                                                     ? session.QueryOver<TEntity>().Where(predicate)
                                                     : session.QueryOver<TEntity>().Where(predicate).Where(
                                                         isLogicDeleteExpression);
            query = orderbyStrings.Select(RepositoryHelper.ParseOrderbyString).
                Aggregate(query,
                          (current, tup) =>
                          tup.Item2
                              ? current.OrderBy(Projections.Property(tup.Item1)).Desc
                              : current.OrderBy(Projections.Property(tup.Item1)).Asc);
            return query.Take(pageSize*pageNum).Skip(pageSize*(pageNum - 1)).List();
        }

        /// <summary>
        /// 分布查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除数据</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码</param>
        /// <returns></returns>
        public IList<TEntity> QueryPage(ICondition condition, OrderByGroup orderBy, bool includeLogicDelete,
                                        int pageSize, int pageNum)
        {
            IConditionGroup group = RepositoryHelper.ToConditionGroup(condition);
            if (!includeLogicDelete)
            {
                group = group.Add(isLogicDeleteCondition);
            }
            return
                session.CreateQuery(string.Format("FROM {0} WHERE {1} ORDER BY {2}", entityType.Name, group.ToHql(),
                                                  orderBy.ToHql())).SetFirstResult((pageNum - 1)*pageSize).SetMaxResults
                    (pageSize).List<TEntity>();
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (session != null)
            {
                session.Flush();
                session.Close();
                session.Dispose();
                session = null;
            }
        }
    }

    /// <summary>
    /// 数据仓库的辅助对象
    /// </summary>
    internal static class RepositoryHelper
    {
        public static Tuple<string, bool> ParseOrderbyString(string orderbyString)
        {
            string realStr = orderbyString.Trim();
            if (realStr == "")
            {
                throw new ArgumentNullException("orderbyString");
            }
            int idx = realStr.IndexOf(' ');
            string prop = idx == -1 ? realStr : realStr.Substring(0, idx);
            bool isDesc = idx != -1 && realStr.Substring(idx).Trim().Equals("desc", StringComparison.OrdinalIgnoreCase);
            return new Tuple<string, bool>(prop, isDesc);
        }

        public static IConditionGroup ToConditionGroup(ICondition condition)
        {
            if (condition == null)
            {
                return null;
            }
            if (condition is IConditionGroup)
            {
                return condition as IConditionGroup;
            }
            return Conditions.ConditionGroup().Add(condition);
        }
    }
}