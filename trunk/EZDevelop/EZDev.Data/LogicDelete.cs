using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZDev.Data.Popedom;
using FluentNHibernate.Mapping;


namespace EZDev.Data
{
    /// <summary>
    /// 逻辑删除
    /// </summary>
    public class LogicDelete
    {
        /// <summary>
        /// 是否已经逻辑删除
        /// </summary>
        public virtual bool IsDelete
        {
            get;
            set;
        }

        /// <summary>
        /// 删除时间
        /// </summary>
        public virtual DateTime DeleteTime
        {
            get;
            set;
        }

        /// <summary>
        /// 删除人
        /// </summary>
        public virtual User DeleteBy
        {
            get;
            set;
        }
    }

    public class LogicDeleteMap : ComponentMap<LogicDelete>
    {
        public LogicDeleteMap()
        {
            References(l => l.DeleteBy).Column("DeleteBy");
            Map(l => l.DeleteTime).Column("DeleteTime");
            Map(l => l.IsDelete).Column("IsDelete");
        }
    }
}