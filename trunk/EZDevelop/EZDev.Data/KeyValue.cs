using System;

namespace EZDev.Data
{

    #region 键值对象
    /// <summary>
    /// 键值接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IKeyValue<TKey, TValue>
    {
        /// <summary>
        /// 主键,ID
        /// </summary>
        Guid ID
        {
            get;
            set;
        }

        /// <summary>
        /// 值分类
        /// </summary>
        string Category
        {
            get;
            set;
        }

        /// <summary>
        /// 外部键
        /// </summary>
        TKey Key
        {
            get;
            set;
        }

        /// <summary>
        /// 值
        /// </summary>
        TValue Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 联合键值接口
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IDoubleKeyValue<TKey1, TKey2, TValue> : IKeyValue<TKey1, TValue>
    {
        /// <summary>
        /// ID2
        /// </summary>
        TKey2 Key2
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 键值对象
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class KeyValueObject<TKey, TValue> : Entity<Guid>, IKeyValue<TKey, TValue>
    {
        public KeyValueObject(TKey key, string category, TValue value)
        {
            Key = key;
            Category = category;
            Value = value;
        }

        /// <summary>
        /// 值分类
        /// </summary>
        public virtual string Category
        {
            get;
            set;
        }

        /// <summary>
        /// 外部键
        /// </summary>
        public virtual TKey Key
        {
            get;
            set;
        }

        /// <summary>
        /// 值
        /// </summary>
        public virtual TValue Value
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 键值对象
    /// </summary>
    /// <typeparam name="TKey1"></typeparam>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class DoubleKeyValueObject<TKey1, TKey2, TValue> : IDoubleKeyValue<TKey1, TKey2, TValue>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key1">第一个键</param>
        /// <param name="key2">第二个键</param>
        /// <param name="category">分类</param>
        /// <param name="value">值</param>
        protected DoubleKeyValueObject(TKey1 key1, TKey2 key2, string category, TValue value)
        {
            Key = key1;
            Key2 = key2;
            Value = value;
            Category = category;
        }

        #region IDoubleKeyValueObject<TKey1,TKey2,TValueType> Members

        /// <summary>
        /// 主键,ID
        /// </summary>
        public virtual Guid ID
        {
            get;
            set;
        }

        /// <summary>
        /// 值分类
        /// </summary>
        public string Category
        {
            get;
            set;
        }

        /// <summary>
        /// Key
        /// </summary>
        public virtual TKey1 Key
        {
            get;
            set;
        }

        /// <summary>
        /// Key2
        /// </summary>
        public virtual TKey2 Key2
        {
            get;
            set;
        }

        /// <summary>
        /// 值
        /// </summary>
        public virtual TValue Value
        {
            get;
            set;
        }

        #endregion
    }

    #endregion




    #region  值转换
    /// <summary>
    /// 值转换接口
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface IValueConvert<TValue>
    {
        string Name { get; }

        Type GetValueType();
    }

    /// <summary>
    /// 单值转换
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface ISingleValueConvert<TValue> : IValueConvert<TValue>
    {
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string ConverToString(TValue value);

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        TValue ConvertToSingleValue(string stringValue);
    }

    /// <summary>
    /// 双值转换
    /// 一般针对数据区间，如时间段、数据段等
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface IDoubleValueConvert<TValue> : IValueConvert<TValue>
    {
        /// <summary>
        /// 转换为字符串
        /// 元组的第一个值为起始值，第二值为结束值
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        Tuple<TValue, TValue> ConvertToValue(string stringValue);

        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <returns></returns>
        string ConvertToString(TValue startValue, TValue endValue);
    }

    /// <summary>
    /// 多值转换
    /// </summary>
    /// <typeparam name="TValue">值类型</typeparam>
    public interface IMultiValueConvert<TValue> : IValueConvert<TValue>
    {
        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        TValue[] ConvertToMultiValue(string stringValue);
        
        /// <summary>
        /// 转换为对象
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        string ConvertToString(params TValue[] values);
    }

    #endregion
}