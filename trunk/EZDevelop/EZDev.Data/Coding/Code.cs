using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Constraints;


namespace EZDev.Data.Coding
{
    /// <summary>
    /// 代码
    /// </summary>
    public class Code : LogicDeleteEntity<int>
    {
        /// <summary>
        /// 代码
        /// </summary>
        [NotNullNotEmpty(Message="代码不能为空！")]
        [Length(Max=30, Message="代码超长！")]
        public virtual string Coding
        {
            get;
            set;
        }

        /// <summary>
        /// 代码名称
        /// </summary>
        [NotNullNotEmpty(Message= "名称不能为空！")]
        [Length(Max=100, Message= "名称超长！")]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 拼音名，拼音首字母
        /// </summary>
        [NotNullNotEmpty(Message="拼音名不能为空！")]
        [Length(Max=100, Message="拼音名超长！")]
        public virtual string SpellName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否末级节点
        /// </summary>
         public virtual bool IsLastNode
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
        /// 所属代码类型
        /// </summary>
        [NotNull(Message="所属代码类型不能为空！")]
        public virtual CodeKind Kind
        {
            get;
            set;
        }

        /// <summary>
        /// 上级代码，只对树型代码有效，否则为null
        /// </summary>
        public virtual Code Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 下级节点
        /// </summary>
        public virtual ISet<Code> Codes
        {
            get;
            set;
        }
    }

    public class CodeMap : LogicDeleteEntityMap<Code, int>
    {
        public CodeMap()
        {
            Table("Sys_Code");

            Map(x => x.Name).Length(100).Not.Nullable();
            Map(x => x.Coding).Length(30).Not.Nullable();
            Map(x => x.SpellName).Length(100).Not.Nullable();
            Map(x => x.IsLastNode).Not.Nullable();
            Map(x => x.Remark).Length(255).Nullable();
            References(x => x.Kind).Cascade.All().Not.Nullable().Column("CodeKindID").LazyLoad();
            References(x => x.Parent).Cascade.All().Column("ParentID").LazyLoad();
            HasMany(x => x.Codes).KeyColumn("ParentID").LazyLoad();
        }
    }


}