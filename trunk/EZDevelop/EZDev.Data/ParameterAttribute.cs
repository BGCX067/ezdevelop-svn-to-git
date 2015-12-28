#region  修订历史
/*
 * 创建时间：2010-4-18
 * 创建人：董永刚
 * 描述：参数特性，用于描述字段或属性是系统参数
 */
#endregion

using System;

namespace EZDev.Data
{
    /// <summary>
    /// 参数特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Field| AttributeTargets.Property, AllowMultiple = true, Inherited=true)]
    public sealed class ParameterAttribute: Attribute
    {
        /// <summary>
        /// 通过ID构造对象
        /// </summary>
        /// <param name="id">ID</param>
        public ParameterAttribute(int id)
        {
            this.ID = id;
        }

        /// <summary>
        /// 通过ID，空值构造对象
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="defaultValue">当对象为空时的值</param>
        public ParameterAttribute(int id, object defaultValue)
        {
            ID=id;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// 通过ID，空值构造对象
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="defaultValue">当对象为空时的值</param>
        /// <param name="explanation">当前参数的说明</param>
        public ParameterAttribute(int id, object defaultValue, string explanation)
        {
            ID = id;
            DefaultValue = defaultValue;
            Explanation = explanation;
        }

        /// <summary>
        /// ID
        /// </summary>
        public int ID
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
        /// 说明
        /// </summary>
        public string Explanation
        {
            get;
            set;
        }
    }
}
