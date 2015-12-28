using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 权限对象
    /// </summary>
    public class Power:Entity<Guid>
    {

        /// <summary>
        /// 名称
        /// </summary>
        [NotNullNotEmpty(Message = "角色名称不能为空！")]
        [Length(Max = 30, Message = "角色名称超长！")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 权限类型
        /// </summary>
        [NotNullNotEmpty(Message = "权限类型！")]
        public virtual string Type
        {
            get;
            set;
        }


        /// <summary>
        /// 备注
        /// </summary>
        [Length(Max = 30, Message = "备注超长！")]
        public virtual string Remark
        {
            get;
            set;
        }

    }

    public class PowerMap : BaseEntityMap<Power, Guid>
    {
        public PowerMap()
        {
            Table("Sys_Power");
            Map(x => x.Name).Not.Nullable().Length(30);
            Map(x => x.Type).Not.Nullable().Length(255);
            Map(x => x.Remark).Length(255);
        }
    }
}