using System;
using System.Collections.Generic;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Popedom
{
    /// <summary>
    /// 系统模块对象
    /// </summary>
    public class Module:Entity<Guid>
    {

        /// <summary>
        /// 模块名称
        /// </summary>
        [NotNullNotEmpty(Message="模块名称不能为空！")]
        [Length(Max = 30, Message = "备注超长！")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 拼音名
        /// 名称拼音首字母 
        /// </summary>
        [NotNullNotEmpty(Message="拼音名不能为空！")]
        [Length(Max = 30, Message = "拼音名超长！")]
        public virtual string SpellName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Length(Max=255, Message="备注信息超长！")]
        public virtual string Remark
        {
            get;
            set;
        }

    }

    public class ModuleMap : BaseEntityMap<Module, Guid>
    {
        public ModuleMap()
        {
            Table("Sys_Module");
            
            Map(x => x.Name).Not.Nullable().Length(30);
            Map(x => x.SpellName).Not.Nullable().Length(30);
            Map(x => x.Remark).Length(255);
        }
    }
}