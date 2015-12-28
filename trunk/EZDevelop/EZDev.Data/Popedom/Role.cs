using System;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 系统角色对象
    /// </summary>
    public class Role:Entity<Guid>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [NotNullNotEmpty(Message="角色名称不能为空！")]
        [Length(Max=30, Message="角色名称超长！")]
        public virtual string Name
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

        /// <summary>
        /// 得到 用户所属的角色模块对象集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<RoleModule> RoleModules
        {
            get;
            protected set;
        }

        /// <summary>
        /// 得到 角色拥有的模块列表
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Module> Modules
        {
            get;
            set;
        }
    }


    public class RoleMap : BaseEntityMap<Role, Guid>
    {
        public RoleMap()
        {
            Table("Sys_Role");
            Map(x => x.Name).Not.Nullable().Length(30);
            Map(x => x.Remark).Length(255);
            HasMany(x => x.RoleModules).KeyColumn("RoleID").Cascade.AllDeleteOrphan();
            HasManyToMany(x => x.Modules).Table("Sys_RoleModule").ParentKeyColumn("RoleID").ChildKeyColumn("ModuleID").LazyLoad();
        }
    }
}