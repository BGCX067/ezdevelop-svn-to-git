using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 用户权限对象
    /// </summary>
    public class UserPower : Entity<Guid>
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        [NotNull]
        public virtual User User
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
    
    public class UserPowerMap: BaseEntityMap<UserPower, Guid>
    {
        public UserPowerMap()
        {
            Table("Sys_UserPower");
            References(x => x.User).Column("UserID").Not.Nullable().Not.LazyLoad();
            References(x => x.Power).Column("PowerID").Not.Nullable().Not.LazyLoad();
            Map(x => x.Value).Not.Nullable().CustomSqlType("nvarchar(max)");
        }
    }
}