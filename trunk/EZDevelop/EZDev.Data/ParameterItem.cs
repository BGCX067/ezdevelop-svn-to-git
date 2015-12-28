using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data
{
    /// <summary>
    /// 参数项
    /// </summary>
    public class ParameterItem: Entity<int>
    {
        /// <summary>
        /// 参数值
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// 说明、备注
        /// </summary>
        public virtual string Remark { get; set; }
    }

    public class ParameterItemMap : BaseEntityMap<ParameterItem, int>
    {
        public ParameterItemMap()
        {
            Table("Sys_ParameterItem");

            Map(x => x.Value).Not.Nullable();
            Map(x => x.Remark);
        }
    }
}
