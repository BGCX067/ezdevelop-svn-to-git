using System;
using FluentNHibernate.Mapping;

namespace EZDev.Data
{
    /// <summary>
    /// ֧���߼�ɾ����ʵ�����
    /// </summary>
    /// <typeparam name="TIdentifier">��ʶ������</typeparam>
    [Serializable]
    public abstract class LogicDeleteEntity<TIdentifier> : Entity<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
    {
        /// <summary>
        /// �߼�ɾ��
        /// </summary>
        public virtual LogicDelete LogicDelete
        {
            get;
            set;
        }
    }

    /// <summary>
    /// ʵ��������
    /// </summary>
    public abstract class BaseEntity
    {
    }

    /// <summary>
    /// ʵ�����
    /// </summary>
    /// <typeparam name="TIdentifier">��ʶ������</typeparam>
    public class Entity<TIdentifier>: BaseEntity where TIdentifier: IEquatable<TIdentifier>
    {
        /// <summary>
        /// ���
        /// </summary>
        public virtual TIdentifier ID
        {
            get;
            set;
        }

        /// <summary>
        /// �汾����
        /// </summary>
        public virtual DateTime Version
        {
            get;
            set;
        }

        /// <summary>
        /// �õ�ID������
        /// </summary>
        /// <returns></returns>
        public static Type GetIDType()
        {
            return typeof (TIdentifier);
        }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override bool Equals(object entity)
        {
            return Equals(entity as Entity<TIdentifier>);
        }

        /// <summary>
        /// �Ƿ�Ϊ˲̬,δ�־û�
        /// </summary>
        /// <param name="entity">ʵ�����</param>
        /// <returns>�����Ƿ��Ѿ��־û�</returns>
        private static bool IsTransient(Entity<TIdentifier> entity)
        {
            return entity != null &&
                   Equals(entity.ID, default(TIdentifier));
        }

        /// <summary>
        /// �õ�δ�������
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <summary>
        /// ȷ���Ƿ����
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
        /// �õ�Hashcode
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