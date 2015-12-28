using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using EZDev.Data.QueryModel;

namespace EZDev.Data
{
    public interface IRepository<TEntity, in TIdentifier>: IDisposable where TEntity : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        /// <summary>
        /// 插入一个对象
        /// </summary>
        /// <param name="entity">指定的对象</param>
        void Insert(TEntity entity);

        /// <summary>
        /// 更新一个对象
        /// </summary>
        /// <param name="entity">指定的对象</param>
        void Update(TEntity entity);

        /// <summary>
        /// 得到指定ID的对象
        /// </summary>
        /// <param name="id">指定的ID</param>
        /// <returns>返回的对象，如果没有找到则返回null</returns>
        TEntity Get(TIdentifier id);

        /// <summary>
        /// 删除指定ID的对象
        /// </summary>
        /// <param name="id">指定的ID</param>
        void Delete(TIdentifier id);

        /// <summary>
        /// 删除指定的对象
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 得到记录数量
        /// </summary>
        /// <returns>记录总数</returns>
        int Count();

        /// <summary>
        /// 得到指定条件记录的数量
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 得到指定条件记录的数量
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        int Count(ICondition condition);

        /// <summary>
        /// 得到指定条件的查询结果
        /// 如果条件可返回多条记录，则只返回第一条
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回的对象</returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 得到指定条件的查询结果
        /// 如果条件可返回多条记录，则只返回第一条
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回的对象</returns>
        TEntity Get(ICondition condition);

        /// <summary>
        /// 返回所有的数据
        /// </summary>
        /// <returns>返回的数据集合</returns>
        IList<TEntity> Query();

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>返回的结果集</returns>
        IList<TEntity> Query(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns>返回的结果集</returns>
        IList<TEntity> Query(ICondition condition);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码，从 1 开始</param>
        /// <param name="orderbyStrings"></param>
        /// <returns>返回结果集</returns>
        IList<TEntity> QueryPage(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNum, params string[] orderbyStrings);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码</param>
        /// <returns></returns>
        IList<TEntity> QueryPage(ICondition condition, OrderByGroup orderBy, int pageSize, int pageNum);

        /// <summary>
        /// 开始一个事务
        /// </summary>
        /// <returns></returns>
        ITransaction BeginTransaction();
    }

    /// <summary>
    /// 支持逻辑删除的仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TIdentifier"></typeparam>
    public interface ILogicDeleteRepository<TEntity, in TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        /// <summary>
        /// 物理删除数据库中已经标记删除的数据
        /// </summary>
        void PhysicalDelete();

        /// <summary>
        /// 物理删除数据库中已经标记删除的数据
        /// </summary>
        /// <param name="predicate">指定的过滤条件</param>
        void PhysicalDelete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 物理删除数据库中已经标记删除的数据
        /// </summary>
        /// <param name="condition"></param>
        void PhysicalDelete(ICondition condition);

        /// <summary>
        /// 返回所有的数据，包含已经标记为删除的数据
        /// </summary>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <returns>返回的数据集合</returns>
        IList<TEntity> Query(bool includeLogicDelete);

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <param name="orderbyStrings"></param>
        /// <returns>返回的结果集</returns>
        IList<TEntity> Query(Expression<Func<TEntity, bool>> predicate, bool includeLogicDelete, params string[] orderbyStrings);

        /// <summary>
        /// 按条件查询结果集
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除的数据</param>
        /// <returns>返回的结果集</returns>
        IList<TEntity> Query(ICondition condition, OrderByGroup orderBy, bool includeLogicDelete);

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
        IList<TEntity> QueryPage(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNum, bool includeLogicDelete, params string[] orderbyStrings);

        /// <summary>
        /// 分布查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="includeLogicDelete">是否包含逻辑删除数据</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageNum">页码</param>
        /// <returns></returns>
        IList<TEntity> QueryPage(ICondition condition, OrderByGroup orderBy, bool includeLogicDelete, int pageSize,
                                 int pageNum);

    }
}