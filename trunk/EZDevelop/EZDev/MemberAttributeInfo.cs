#region  修订历史
/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：存放类型的成员和其对应的属性的信息
 */
#endregion
namespace EZDev
{
    /// <summary>
    /// 存放类型的成员和其对应的属性的信息
    /// </summary>
    public class MemberAttributeInfo
    {
        /// <summary>
        /// 存放成员的属性列表
        /// </summary>
        private System.Attribute[] attributes;
        /// <summary>
        /// 成员 (操作，字段，属性等)。
        /// </summary>
        private System.Reflection.MemberInfo memberInfo;

        /// <summary>
        /// 构建一个成员与属性的对应信息
        /// </summary>
        /// <param name="memberInfo">成员</param>
        /// <param name="attributes">成员的属性数组</param>
        public MemberAttributeInfo(System.Reflection.MemberInfo memberInfo, System.Attribute[] attributes)
        {
            this.memberInfo = memberInfo;
            this.attributes = attributes;
        }

        /// <summary>
        /// 获取成员的第一个属性(或唯一的一个属性)
        /// </summary>
        public System.Attribute Attribute
        {
            get
            {
                return this[0];
            }
        }

        /// <summary>
        /// 获取成员的属性列表
        /// </summary>
        public System.Attribute[] Attributes
        {
            get
            {
                return this.attributes;
            }
        }

        /// <summary>
        /// 获取成员的属性个数
        /// </summary>
        public int Count
        {
            get
            {
                return this.attributes.GetLength(0);
            }
        }

        /// <summary>
        /// 获取指定序号的成员属性
        /// </summary>
        public System.Attribute this[int index]
        {
            get
            {
                return this.attributes[index];
            }
        }

        /// <summary>
        /// 获取成员 (操作，字段，属性等)。
        /// </summary>
        public System.Reflection.MemberInfo MemberInfo
        {
            get
            {
                return this.memberInfo;
            }
        }
    }
}