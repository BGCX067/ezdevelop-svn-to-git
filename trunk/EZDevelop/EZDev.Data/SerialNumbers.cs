using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data
{
    /// <summary>
    /// 按日期分组类型
    /// </summary>
    public enum DateGroupType
    {
        /// <summary>
        /// 不分组
        /// </summary>
        None,
        /// <summary>
        /// 按年分组
        /// </summary>
        Year,

        /// <summary>
        /// 按月分组
        /// </summary>
        Month,
        /// <summary>
        /// 按天分组
        /// </summary>
        Day,
        /// <summary>
        /// 按小时分组
        /// </summary>
        Hour
    }

    /// <summary>
    /// 序号对象
    /// </summary>
    public class SerialNumber: Entity<Guid>
    {
        /// <summary>
        /// 序号名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public virtual string Prefix { get; set; }


        /// <summary>
        /// 日期分组类型
        /// </summary>
        public virtual DateGroupType GroupType { get; set; }

        /// <summary>
        /// 序号格式化长度
        /// </summary>
        public virtual int SerialLen { get; set; }

        /// <summary>
        /// 最小序号
        /// </summary>
        public virtual int MinIndex { get; set; }

        /// <summary>
        /// 格式化字符串
        /// 每个字母表示一种数据，如P表示前缀，D表示日期，S表示序号，格式字符串如PDS
        /// </summary>
        public virtual string FormatString { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 空号集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<EmptySerialNumber> EmptySerialNumbers { get; set; }
    }

    /// <summary>
    /// 序号项
    /// </summary>
    public class SerialNumberItem:Entity<Guid>
    {
        /// <summary>
        /// 所属序号
        /// </summary>
        public virtual SerialNumber SerialNumber { get; set; }

        /// <summary>
        /// 分组字符串
        /// </summary>
        public virtual string GroupString { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int Serial { get; set; }

    }

    /// <summary>
    /// 空号
    /// </summary>
    public class EmptySerialNumber:Entity<Guid>
    {
        /// <summary>
        /// 所属序号
        /// </summary>
        public virtual SerialNumber SerialNumber { get; set; }

        /// <summary>
        /// 分组字符串
        /// </summary>
        public virtual string GroupString { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public virtual int Serial { get; set; }

        /// <summary>
        /// 序号日期
        /// </summary>
        public virtual DateTime SerialNumberDate { get; set; }
    }

    public class SerialNumberMap : BaseEntityMap<SerialNumber, Guid>
    {
        public SerialNumberMap()
        {
            Table("Sys_SerialNumber");

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Prefix).Not.Nullable();
            Map(x => x.GroupType).Not.Nullable();
            Map(x => x.SerialLen).Not.Nullable();
            Map(x => x.MinIndex).Not.Nullable();
            Map(x => x.FormatString).Not.Nullable();
            Map(x => x.Remark);
            HasMany(x => x.EmptySerialNumbers).KeyColumn("SerialNumberID").ExtraLazyLoad().Cascade.AllDeleteOrphan();
        }
    }
    public class SerialNumberItemMap : BaseEntityMap<SerialNumberItem, Guid>
    {
        public SerialNumberItemMap()
        {
            Table("Sys_SerialNumberItem");

            References(x => x.SerialNumber).Column("SerialNumberID").ForeignKey("FK_SerialNumber_SerialNumberItem");
            Map(x => x.GroupString).Not.Nullable();
            Map(x => x.Serial);
            
        }
    }
    public class EmptySerialNumberMap : BaseEntityMap<EmptySerialNumber, Guid>
    {
        public EmptySerialNumberMap()
        {
            Table("Sys_EmptySerialNumber");

            References(x => x.SerialNumber).Column("SerialNumberID").ForeignKey("FK_SerialNumber_EmptySerialNumber");
            Map(x => x.GroupString).Not.Nullable();
            Map(x => x.Serial);
            Map(x => x.SerialNumberDate).Not.Nullable();

        }
    }
}
