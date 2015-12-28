using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev
{
    #region 基类
    /// <summary>
    /// 单个值的参数
    /// </summary>
    public abstract class SingleStringValue<TValueType> : IStringValue
    {
        /// <summary>
        /// 内部值
        /// </summary>
        private TValueType innerValue;

        /// <summary>
        /// 从字符串加载
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool LoadFromString(string value)
        {
            try
            {
                innerValue = (TValueType)Convert.ChangeType(value, typeof(TValueType));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ConvertToString()
        {
            return innerValue.ToString();
        }

        /// <summary>
        /// 校验值
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// 得到相应的值
        /// </summary>
        public virtual TValueType Value
        {
            get
            {
                return innerValue;
            }
            set
            {
                innerValue = value;
            }
        }
    }


    /// <summary>
    /// 数据范围参数
    /// </summary>
    public abstract class SpanStringValue<TValueType> : IStringValue
    {
        /// <summary>
        /// 起始值
        /// </summary>
        private TValueType startValue;

        /// <summary>
        /// 结束值
        /// </summary>
        private TValueType endValue;

        /// <summary>
        /// 从字符串加载
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public virtual bool LoadFromString(string value)
        {
            TValueType oldStart = startValue;

            TValueType oldEnd = endValue;
            try
            {
                startValue = default(TValueType);
                endValue = default(TValueType);

                if (!string.IsNullOrEmpty(value))
                {
                    var strs = value.Split(StringValueManager.SplitChar);

                    if (strs.Length > 0 && !string.IsNullOrEmpty(strs[0]))
                    {
                        startValue = (TValueType)Convert.ChangeType(strs[0], typeof(TValueType));
                    }
                    if (strs.Length > 1 && !string.IsNullOrEmpty(strs[1]))
                    {
                        endValue = (TValueType)Convert.ChangeType(strs[1], typeof(TValueType));
                    }
                }
                return true;
            }
            catch
            {
                startValue = oldStart;
                endValue = oldEnd;
                return false;
            }
        }


        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ConvertToString()
        {
            return string.Format("{0}{1}{2}",
                                 startValue.Equals(default(TValueType)) ? "" : startValue.ToString(),
                                 StringValueManager.SplitChar,
                                 endValue.Equals(default(TValueType)) ? "" : endValue.ToString());
        }

        /// <summary>
        /// 校验值
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// 得到或设置 起始值
        /// </summary>
        public virtual TValueType StartValue
        {
            get
            {
                return startValue;
            }
            set
            {
                startValue = value;
            }
        }

        /// <summary>
        /// 得到或设置 结束值
        /// </summary>
        public virtual TValueType EndValue
        {
            get
            {
                return endValue;
            }
            set
            {
                endValue = value;
            }

        }
    }


    /// <summary>
    /// 多个值的参数
    /// </summary>
    public abstract class MultiStringValue<TValueType> : IStringValue
    {
        /// <summary>
        /// 内部值集合
        /// </summary>
        private List<TValueType> valueList;

        /// <summary>
        /// 从字符串加载
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool LoadFromString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                valueList = new List<TValueType>();
                return true;
            }
            var oldList = valueList;
            try
            {
                valueList.Clear();
                var strs = value.Split(StringValueManager.SplitChar);
                foreach (string str in strs)
                {
                    valueList.Add((TValueType)Convert.ChangeType(str, typeof(TValueType)));
                }
                return true;
            }
            catch
            {
                valueList = oldList;
                return false;
            }
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ConvertToString()
        {
            if (valueList == null || valueList.Count == 0)
            {
                return "";
            }
            else
            {
                string result = "";
                valueList.ForEach(t => result += result == "" ? t.ToString() : StringValueManager.SplitChar + t.ToString());
                return result;
            }
        }

        /// <summary>
        /// 校验值
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
        {
            return true;
        }

        /// <summary>
        /// 得到 值集合
        /// </summary>
        public virtual List<TValueType> Values
        {
            get
            {
                return valueList;
            }
        }
    }

    #endregion

#region  实现类

#region 单值实现
    public class Int16SingleStringValue : SingleStringValue<Int16>
    {
    }


    public class Int32SingleStringValue : SingleStringValue<Int32>
    {
    }


    public class StringSingleStringValue : SingleStringValue<string>
    {
    }


    public class SingleSingleStringValue : SingleStringValue<System.Single>
    {
    }


    public class DoubleSingleStringValue : SingleStringValue<System.Double>
    {
    }


    public class DateTimeSingleStringValue : SingleStringValue<DateTime>
    {
    }

    public class BooleanSingleStringValue: SingleStringValue<bool>
    {
        
    }


    public class CharSingleStringValue : SingleStringValue<char>
    {
    }


    public class Int64SingleStringValue : SingleStringValue<Int64>
    {
    }

#endregion

#region 区间实现

    public class Int16SpanStringValue : SpanStringValue<Int16>
    {
    }
    public class Int32SpanStringValue : SpanStringValue<Int32>
    {
    }
    public class Int64SpanStringValue : SpanStringValue<Int64>
    {
    }


    public class SingleSpanStringValue : SpanStringValue<Single>
    {
    }


    public class DoubleSpanStringValue : SpanStringValue<Double>
    {
    }


    public class DecimalSpanStringValue : SpanStringValue<Decimal>
    {
    }


    public class DatetimSpanStringValue : SpanStringValue<DateTime>
    {
    }

    #endregion

#region 多值

    public class StringMultiStringValue : MultiStringValue<String>
    {
    }


    public class GuigMultiStringValue : MultiStringValue<Guid>
    {
    }

    #endregion
    #endregion
}
