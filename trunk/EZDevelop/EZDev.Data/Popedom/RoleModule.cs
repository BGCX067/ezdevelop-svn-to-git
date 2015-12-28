using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 角色模块对象
    /// </summary>
    public class RoleModule: Entity<Guid>
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
        /// 所属模块
        /// </summary>
        [NotNull]
        public virtual Module Module
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


    public class RoleModuleMap: BaseEntityMap<RoleModule, Guid>
    {
        public RoleModuleMap()
        {
            Table("Sys_RoleModule");
            References(x => x.Role).Column("RoleID").Not.Nullable().Not.LazyLoad();
            References(x => x.Module).Column("ModuleID").Not.Nullable().Not.LazyLoad();
            Map(x => x.Value).Not.Nullable();
        }
    }
}