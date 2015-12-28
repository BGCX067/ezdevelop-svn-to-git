using System;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Coding
{
    /// <summary>
    /// 代码类型
    /// </summary>
    public class CodeKind : LogicDeleteEntity<int>
    {

        /// <summary>
        /// 代码类型名称
        /// </summary>
        [NotNullNotEmpty(Message = "名称不能为空！")]
        [Length(Max = 30, Message = "名称超长！")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是树型代码
        /// </summary>
        public virtual bool IsTree
        {
            get;
            set;
        }

        /// <summary>
        /// 是否系统代码，系统代码不可删除
        /// </summary>
        public virtual bool IsSystemCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否内部代码
        /// 非超级管理员在管理界面看不到内部代码
        /// </summary>
        public virtual bool IsInnerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Length(Max = 255, Message = "备注超长！")]
        public virtual string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 代码类型包含的代码对象集合
        /// 如果是树型代码，只包含顶级代码
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Code> Codes
        {
            get;
            set;
        }
    }

    public class CodeKindMap : LogicDeleteEntityMap<CodeKind, int>
    {
        public CodeKindMap()
        {
            Table("Sys_CodeKind");

            Map(x => x.Name).Not.Nullable();
            Map(x => x.IsTree);
            Map(x => x.IsSystemCode);
            Map(x => x.IsInnerCode);
            HasMany(x => x.Codes).KeyColumn("CodeKindID");
        }
    }
}