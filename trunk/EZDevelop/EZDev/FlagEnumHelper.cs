using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev
{
    /// <summary>
    /// 位计算枚举助手
    /// </summary>
    public static class FlagEnumHelper
    {
        /// <summary>
        /// 是否是一个按位枚举的类型
        /// </summary>
        /// <param name="source">源值</param>
        /// <param name="value">值</param>
        /// <returns>是否是通过校验</returns>
        private static bool ValidValue(object source, object value)
        {
            // 得到源值类型
            var enumType = source.GetType();

            // 源值类型是否是枚举
            if (!enumType.IsEnum)
            {
                return false;
            }
            // 源值和目标值是否类型相同
            if (!enumType.Equals(value.GetType()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 按位运算枚举值是否包含指定值。此方法只适用于按位运算枚举
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="self">源值</param>
        /// <param name="target">值</param>
        /// <returns>包含返回true,否则false</returns>
        public static bool IsInclude<TEnum>(this TEnum self, TEnum target) where TEnum: struct
        {
            var enumType = typeof (TEnum);
            if (!typeof(TEnum).IsEnum)
            {
                throw new Exception("不是一个有效的枚举类型！");
            }

            if (!enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                throw new Exception("不是一个按位运算的枚举类型！");
            }

            var sourceValue = ((IConvertible) self).ToInt32(null);
            var targetValue = ((IConvertible)target).ToInt32(null);

            return (sourceValue & targetValue) == targetValue;
        }

        /// <summary>
        /// 从按位运算枚举值中移除一个枚举值。此方法只适用于按位运算枚举
        /// </summary>
        /// <typeparam name="TEnum">按位运算枚举类型</typeparam>
        /// <param name="self">源值</param>
        /// <param name="target">要移除的值</param>
        /// <returns>移除指定值后的新枚举值</returns>
        public static TEnum RemoveValue<TEnum>(this TEnum self, TEnum target) where TEnum: struct
        {
            var enumType = typeof(TEnum);
            if (!typeof(TEnum).IsEnum)
            {
                throw new Exception("不是一个有效的枚举类型！");
            }

            if (!enumType.IsDefined(typeof(FlagsAttribute), false))
            {
                throw new Exception("不是一个按位运算的枚举类型！");
            }

            int sourceValue = ((IConvertible)self).ToInt32(null);
            int targetValue = ((IConvertible)target).ToInt32(null);

            //求反后再按位与，得到移除后的值
            int result =  sourceValue & (~targetValue);

            TEnum res;
            if (Enum.TryParse<TEnum>(result.ToString(), out res))
            {
                return res;
            }
            
            return default(TEnum);
        }

        /// <summary>
        /// 得到指定枚举值的说明信息.此方法只适用于枚举值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static EnumValueExplanation GetExplanation<TEnum>(this TEnum enumValue) where TEnum:struct
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new Exception("不是一个有效的枚举类型！");
            }
            return Explanation.GetEnumValueExplanation(enumValue as Enum);
        }
    }
}
