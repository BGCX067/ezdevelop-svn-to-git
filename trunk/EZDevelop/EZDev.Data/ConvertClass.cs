using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZDev.Data.Coding;

namespace EZDev.Data
{
    /// <summary>
    /// 单值转换器基类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class SingleValueConverter<TValue> : ISingleValueConvert<TValue>
    {
        public Type GetValueType()
        {
            return typeof (TValue);
        }

        public abstract string Name { get; }

        public abstract string ConverToString(TValue value);


        public abstract TValue ConvertToSingleValue(string stringValue);
    }

    /// <summary>
    /// 实现IConvertible接口的单值转换器基类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class NormalSingleValueConvert<TValue> : ISingleValueConvert<TValue> where TValue: IConvertible
    {
        public Type GetValueType()
        {
            return typeof(TValue);
        }

        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// 转换为了字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ConverToString(TValue value)
        {
            return Convert.ToString(value);
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public TValue ConvertToSingleValue(string stringValue)
        {
            var value = Convert.ChangeType(stringValue, typeof (TValue));
            return value is TValue ? (TValue) value : default(TValue);
        }
    }

    /// <summary>
    /// 实现IConvertible接口的区间值转换基类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class NormalDoubleValueConvert<TValue> : IDoubleValueConvert<TValue> where TValue : IConvertible
    {
        public abstract string Name
        {
            get;
        }

        public Type GetValueType()
        {
            return typeof (TValue);
        }

        /// <summary>
        /// 转换为字符串
        /// 元组的第一个值为起始值，第二值为结束值
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public Tuple<TValue, TValue> ConvertToValue(string stringValue)
        {
            TValue startValue = default(TValue);
            TValue endValue = default(TValue);
            string[] strs = stringValue.Split(StringValueManger.SplitChar);
            if (strs.Length >= 1)
            {
                startValue = (TValue)Convert.ChangeType(strs[0], typeof (TValue));
            }
            if (strs.Length >= 2)
            {
                endValue = (TValue)Convert.ChangeType(strs[2], typeof (TValue));
            }
            return new Tuple<TValue, TValue>(startValue, endValue);
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        public string ConvertToString(TValue startValue, TValue endValue)
        {
#warning 未完成
            string start;
            string end;
            if (startValue == null)
            {
            }
            return "";
        }
    }

    /// <summary>
    /// 双值转换基类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class DoubleValueConverter<TValue> : IDoubleValueConvert<TValue>
    {
        public string Name
        {
            get;
            protected set;
        }

        public Type GetValueType()
        {
            return typeof(TValue);
        }

        public abstract Tuple<TValue, TValue> ConvertToValue(string stringValue);

        public abstract string ConvertToString(TValue startValue, TValue endValue);
    }

    /// <summary>
    /// 多值转换基类
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class MultiValueConverter<TValue> : IMultiValueConvert<TValue>
    {
        public string Name
        {
            get;
            protected set;
        }

        public Type GetValueType()
        {
            return typeof (TValue);
        }

        public abstract TValue[] ConvertToMultiValue(string stringValue);
    
        public abstract string ConvertToString(params TValue[] values);
    }

    #region 单值
    /// <summary>
    /// 字符转换
    /// 为了兼容。实现字符转换到字符的转换
    /// </summary>
    public class StringConvert : NormalSingleValueConvert<string>
    {
        public override string Name
        {
            get
            {
                return "字符型";
            }
        }
    }

    /// <summary>
    /// 整型转换
    /// </summary>
    public class IntConvert : NormalSingleValueConvert<int>
    {
        public override string Name
        {
            get
            {
                return "整型值";
            }
        }
    }

    /// <summary>
    /// 布尔型转换
    /// </summary>
    public class BoolConvert : NormalSingleValueConvert<bool>
    {
        public override string Name
        {
            get
            {
                return "布尔型";
            }
        }
    }

    /// <summary>
    /// 浮点型转换
    /// </summary>
    public class DecimalConver : NormalSingleValueConvert<decimal>
    {
        public override string Name
        {
            get
            {
                return "数值型";
            }
        }
    }

    /// <summary>
    /// 时间型转换
    /// </summary>
    public class DatetimeConvert : NormalSingleValueConvert<DateTime>
    {
        public override string Name
        {
            get
            {
                return "日期时间型";
            }
        }
    }

    /// <summary>
    /// 代码转换
    /// </summary>
    public class CodeConvert : SingleValueConverter<Code>
    {
        public override string Name
        {
            get
            {
                return "代码";
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="value"/>
        /// <returns/>
        public override string ConverToString(Code value)
        {
            return value.ID.ToString();
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="stringValue"/>
        /// <returns/>
        public override Code ConvertToSingleValue(string stringValue)
        {
            return NHHelper.Instance.GetCurrentSession().Get<Code>(Convert.ToInt32(stringValue));
        }
    }

    /// <summary>
    /// 对象类型转换
    /// </summary>
    public class TypeConvert : SingleValueConverter<Type>
    {
        public override string Name
        {
            get
            {
                return "对象类型";
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="value"/>
        /// <returns/>
        public override string ConverToString(Type value)
        {
            return value.FullName;
        }

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="stringValue"/>
        /// <returns/>
        public override Type ConvertToSingleValue(string stringValue)
        {
            return Type.GetType(stringValue);
        }
    }

    #endregion


    #region 区间值
     
    #endregion

    #region 多值
    #endregion
}
