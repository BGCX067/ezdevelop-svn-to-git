using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data.MetaDatas
{
    /// <summary>
    /// 属性元数据
    /// </summary>
    public class PropertyMetaInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType"></param>
        internal PropertyMetaInfo(Type entityType)
        {
            EntityType = entityType;
        }

        /// <summary>
        /// 所属实体
        /// </summary>
        public Type EntityType { get; private set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 标签名
        /// </summary>
        public string DisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库数据类型
        /// </summary>
        public string DBType
        {
            get;
            set;
        }

        /// <summary>
        /// 数据精度，仅对数值类型有效
        /// </summary>
        public string Precision { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public object MaxValue
        {
            get;
            set;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public object MinValue
        {
            get;
            set;
        }


        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 最大长度，仅对字符串有效
        /// </summary>
        public int MaxLen
        {
            get;
            set;
        }

        /// <summary>
        /// 最小长度，仅对字符串有效
        /// </summary>
        public int MinLen
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许为空
        /// </summary>
        public bool AllowNull
        {
            get;
            set;
        }

        /// <summary>
        /// 索引序列
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public DataView DataView
        {
            get;
            set;
        }

        
    }

    /// <summary>
    /// 实体元数据
    /// </summary>
    public class EntityMetaInfo
    {
        /// <summary>
        /// 所属实体
        /// </summary>
        public Type EntityType
        {
            get;
            set;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IList<PropertyMetaInfo> Properties
        {
            get;
            internal set;
        }
    }
}