#region  修订历史
/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：枚举说明类，用于描述枚举对象
 */
#endregion
using System;

namespace EZDev
{
    /// <summary>
    /// 枚举数说明信息
    /// </summary>
    [Serializable]
    public class EnumValueExplanation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enumValue">枚举数</param>
        /// <param name="name">枚举数名称</param>
        /// <param name="description">枚举数描述</param>
        /// <param name="tag">附加信息</param>
        internal EnumValueExplanation(Enum enumValue, string name, string description, object tag)
        {
            EnumValue = enumValue;
            Name = name;
            Description = description;
            Tag = tag;
        }

        /// <summary>
        /// 获取枚举数的描述
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取该枚举的等同的整数值，-1表示无效
        /// </summary>
        public int EnumInt32Value
        {
            get
            {
                try
                {
                    return ((IConvertible)this.EnumValue).ToInt32(null);
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 获取枚举数
        /// </summary>
        public Enum EnumValue
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取枚举数的名称
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取附加信息
        /// </summary>
        public object Tag
        {
            get;
            private set;
        }

        /// <summary>
        /// 得到字符串描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}