namespace EZDev
{
    /// <summary>
    /// 参数类型
    /// </summary>
    [Explanation("参数类型")]
    public enum ParameterType
    {
        /// <summary>
        /// 无类型
        /// </summary>
        [Explanation("无类型")]
        None,
        /// <summary>
        /// 字符串
        /// </summary>
        [Explanation("字符串")]
        String = 1,
        /// <summary>
        /// 整数
        /// </summary>
        [Explanation("整数")]
        Integer,
        /// <summary>
        /// 浮点数
        /// </summary>
        [Explanation("浮点数")]
        Numeric,
        /// <summary>
        /// 时间
        /// </summary>
        [Explanation("时间")]
        DateTime,
        /// <summary>
        /// 布尔
        /// </summary>
        [Explanation("布尔")]
        Boolean,
        /// <summary>
        /// 时间区间
        /// </summary>
        [Explanation("时间区间")]
        DateTimeQuantum,
        /// <summary>
        /// 整数区间
        /// </summary>
        [Explanation("整数区间")]
        IntergerQuantum,
        /// <summary>
        /// 浮点区间
        /// </summary>
        [Explanation("浮点区间")]
        NumricQuantum
    }
}
