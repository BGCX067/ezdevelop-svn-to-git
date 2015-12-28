using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev
{
    /// <summary>
    /// 参数转换
    /// </summary>
    public class Parameter 
    {
        /// <summary>
        /// 代表无限制的数字
        /// </summary>
        public const string InfinitudeFlag = "~~~";

        /// <summary>
        /// 字符串分割字符
        /// </summary>
        public const char SplitChar = '|';

        /// <summary>
        /// 
        /// </summary>
        public static bool GetBoolean(string value)
        {
            return DataConvert.ToBoolean(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static int GetInteger(string value)
        {
            return DataConvert.ToInt32(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static decimal GetDecimal(string value)
        {
            return DataConvert.ToDecimal(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DateTime GetDateTime(string value)
        {
            return DataConvert.ToDateTime(value, 14);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        public static void GetIntegerQuantum(string value, out int? beginValue, out int? endValue)
        {
            beginValue = null;
            endValue = null;
            string[] strs = value.Split(SplitChar);

            if (strs.Length > 0)
            {
                beginValue = DataConvert.ToInt32(strs[0]);
            }
            if (strs.Length > 1)
            {
                endValue = DataConvert.ToInt32(strs[1]);
            }
        }


        /// <summary>
/// 
/// </summary>
/// <param name="value"></param>
/// <param name="beginValue"></param>
/// <param name="endValue"></param>
        public static void GetDateTimeQuantum(string value, out DateTime? beginValue, out DateTime? endValue)
        {
            beginValue = null;
            endValue = null;
            string[] strs = value.Split(SplitChar);

            if (strs.Length > 0)
            {
                DateTime dt = DataConvert.ToDateTime(strs[0], 14);
                if (dt != EZDev.SysUtils.EmptyDateTime)
                {
                    beginValue = dt;
                }
            }
            if (strs.Length > 1)
            {
                DateTime dt = DataConvert.ToDateTime(strs[1], 14);
                if (dt != EZDev.SysUtils.EmptyDateTime)
                {
                    endValue = dt;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="beginValue"></param>
        /// <param name="endValue"></param>
        public static void GetNumricQuantum(string value, out decimal? beginValue, out decimal? endValue)
        {
            beginValue = null;
            endValue = null;
            string[] strs = value.Split(Parameter.SplitChar);
            
            if (strs.Length > 0)
            {
                if (strs[0].Trim() != "")
                {
                    beginValue = DataConvert.ToInt32(strs[0]);
                }
            }
            if (strs.Length > 1)
            {
                if (strs[1].Trim() != "")
                {
                    endValue = DataConvert.ToInt32(strs[1]);
                }
            }
        }
    }
}
