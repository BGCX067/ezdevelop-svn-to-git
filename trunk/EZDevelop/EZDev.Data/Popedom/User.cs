using System;
using EZDev.Data.Coding;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 系统用户对象
    /// </summary>
    public class User : LogicDeleteEntity<Guid>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [NotNullNotEmpty(Message="姓名不能为空！")]
        [Length(Max=30, Message="姓名超长！")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [NotNullNotEmpty(Message = "登录名不能为空！")]
        [Length(Max = 30, Message = "登录名超长！")]
        public virtual string LoginName { get; set; }

        /// <summary>
        /// 性别代码
        /// </summary>
        public virtual Code Sex
        {
            get;
            set;
        }

        /// <summary>
        /// 出生年月，可为空
        /// </summary>
        public virtual DateTime? Birthday
        {
            get;
            set;
        }

        /// <summary>
        /// 密码，经过MD5加密的字符串
        /// </summary>
        public virtual string Password
        { get; set; }

        /// <summary>
        /// 用户是否被锁定
        /// 锁定的用户无法进行操作
        /// </summary>
        public virtual bool IsLocked
        {
            get;set;
        }

        /// <summary>
        /// 备注信息
        /// </summary>
        [Length(Max = 255, Message = "备注超长！")]
        public virtual string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 用户所有的用户模块对象集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<UserModule> UserModules
        {
            get;
            protected set;
        }

        /// <summary>
        /// 用户所拥有的角色集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Role> Roles
        {
            get;
            set;
        }

        /// <summary>
        /// 用户所有的模块信息集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Module> Modules
        {
            get;
            set;
        }

        /// <summary>
        /// 用户所有的权限信息集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Power> Powers
        {
            get;
            set;
        }

        /// <summary>
        /// 用户所有的用户权限集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<UserPower> UserPowers { get; protected set; }
    }

    public class UserMap: BaseEntityMap<User, Guid>
    {
        public UserMap()
        {
            Table("Sys_user");
            Map(x => x.Name).Not.Nullable().Length(30);
            Map(x => x.LoginName).Not.Nullable().Length(30);
            References(x => x.Sex).Column("SexCodeID");
            Map(x => x.Birthday);
            Map(x => x.Password).Not.Nullable().Length(50);
            Map(x => x.IsLocked).Default("0");
            Map(x => x.Remark).Length(255);
            HasMany(x => x.UserModules).Table("Sys_UserModule").KeyColumn("UserID").Cascade.All();
            HasManyToMany(x => x.Roles).Table("Sys_UserRole").ParentKeyColumn("UserID").ChildKeyColumn("RoleID").Cascade.All();
            HasManyToMany(x => x.Modules).Table("Sys_UserModule").ParentKeyColumn("UserID").ChildKeyColumn("ModuleID").Cascade.All();
            HasManyToMany(x => x.Powers).Table("Sys_UserPower").ParentKeyColumn("UserID").ChildKeyColumn("PowerID").Cascade.All();
            HasMany(x => x.UserPowers).Table("Sys_UserPower").KeyColumn("UserID").Cascade.All();
        }
    }
}