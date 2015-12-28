using System;
using FluentNHibernate.Mapping;

namespace EZDev.Data
{
    /// <summary>
    /// 支持逻辑删除的实体对象
    /// </summary>
    /// <typeparam name="TIdentifier">标识符类型</typeparam>
    [Serializable]
    public abstract class LogicDeleteEntity<TIdentifier> : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        /// <summary>
        /// 逻辑删除
        /// </summary>
        public virtual LogicDelete LogicDelete
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 实体对象基类
    /// </summary>
    public abstract class BaseEntity
    {
    }

    /// <summary>
    /// 实体对象
    /// </summary>
    /// <typeparam name="TIdentifier">标识符类型</typeparam>
    public class Entity<TIdentifier>: BaseEntity where TIdentifier: IEquatable<TIdentifier>
    {
        /// <summary>
        /// 编号
        /// </summary>
        public virtual TIdentifier ID
        {
            get;
            set;
        }

        /// <summary>
        /// 版本控制
        /// </summary>
        public virtual DateTime Version
        {
            get;
            set;
        }

        /// <summary>
        /// 得到ID的类型
        /// </summary>
        /// <returns></returns>
        public static Type GetIDType()
        {
            return typeof (TIdentifier);
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            return Equals(entity as Entity<TIdentifier>);
        }

        /// <summary>
        /// 是否为瞬态,未持久化
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>对象是否已经持久化</returns>
        private static bool IsTransient(Entity<TIdentifier> entity)
        {
            return entity != null &&
                   Equals(entity.ID, default(TIdentifier));
        }

        /// <summary>
        /// 得到未代理对象
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <summary>
        /// 确定是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(Entity<TIdentifier> other)
        {
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(ID, other.ID))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }
            return false;
        }

        /// <summary>
        /// 得到Hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Equals(ID, default(TIdentifier)))
                return base.GetHashCode();
            return ID.GetHashCode();
        }
    }

    public abstract class BaseEntityMap<T, TID> : ClassMap<T> where T : Entity<TID> where TID : IEquatable<TID>
    {
        protected BaseEntityMap()
        {
            Id(x => x.ID);
            Map(x => x.Version).OptimisticLock();
        }
    }

    public abstract class LogicDeleteEntityMap<T, TID> : ClassMap<T> where T : LogicDeleteEntity<TID> where TID : IEquatable<TID>
    {
        public LogicDeleteEntityMap()
        {
            Id(x => x.ID);
            Map(x => x.Version).OptimisticLock();

            Component(x => x.LogicDelete);
        }
    }

}