using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 用户模块对象
    /// </summary>
    public class UserModule : Entity<Guid>
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        [NotNullNotEmpty]
        public virtual User User
        {
            get;
            set;
        }

        /// <summary>
        /// 所属模块
        /// </summary>
        [NotNullNotEmpty]
        public virtual Module Module
        {
            get;
            set;
        }

        /// <summary>
        /// 模块权限值
        /// </summary>
        [NotNullNotEmpty(Message = "值不能为空")]
        public virtual string Value
        {
            get;
            set;
        }
    }

    public class UserModuleMap: BaseEntityMap<UserModule, Guid>
    {
        public UserModuleMap()
        {
            Table("Sys_UserModule");
            References(x => x.User).Column("UserID").Not.Nullable().Not.LazyLoad();
            References(x => x.Module).Column("ModuleID").Not.Nullable().Not.LazyLoad();
            Map(x => x.Value).Not.Nullable().CustomSqlType("nvarchar(max)");
        }
    }
}