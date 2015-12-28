#region  修订历史
/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：说明信息特性，用于描述字段、属性等
 */
#endregion
using System;

namespace EZDev
{
    /// <summary>
    /// 说明信息
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class ExplanationAttribute : Attribute
    {
        /// <summary>
        /// 描述
        /// </summary>
        private string description;
        /// <summary>
        /// 名称
        /// </summary>
        private string name;
        /// <summary>
        /// 附加信息
        /// </summary>
        private object tag;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        public ExplanationAttribute(string name)
            : this(name, name)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        public ExplanationAttribute(string name, string description)
            : this(name, description, null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <param name="tag">附加信息</param>
        public ExplanationAttribute(string name, string description, object tag)
        {
            this.name = "";
            this.description = "";
            this.tag = null;
            this.name = name;
            this.description = description;
            this.tag = tag;
        }

        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// 获取或设置附加信息
        /// </summary>
        public object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }
    }
}