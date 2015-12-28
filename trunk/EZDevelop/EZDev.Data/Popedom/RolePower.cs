using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 角色权限对象
    /// </summary>
    public class RolePower: Entity<Guid>
    {
        /// <summary>
        /// 所属角色
        /// </summary>
        [NotNull]
        public virtual Role Role
        {
            get;
            set;
        }

        /// <summary>
        /// 所属权限
        /// </summary>
        [NotNull]
        public virtual Power Power
        {
            get;
            set;
        }

        /// <summary>
        /// 权限值
        /// </summary>
        [NotNullNotEmpty(Message = "值不能为空")]
        public virtual string Value
        {
            get;
            set;
        }
    }

    public class RolePowerMap:BaseEntityMap<RolePower, Guid>
    {
        public RolePowerMap()
        {
            Table("Sys_RolePower");
            References(x => x.Role).Column("RoleID").Not.Nullable().Not.LazyLoad();
            References(x => x.Power).Column("PowerID").Not.Nullable().Not.LazyLoad();
            Map(x => x.Value).Not.Nullable().CustomSqlType("nvarchar(max)");

        }
    }
}