namespace EZDev.Data.QueryModel
{
    /// <summary>
    /// 逻辑运算
    /// </summary>
    [Explanation("逻辑运算")]
    public enum Operator
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Explanation("等于")]
        Equals,

        /// <summary>
        /// Like
        /// </summary>
        [Explanation("Like")]
        Like,

        /// <summary>
        /// 小于
        /// </summary>
        [Explanation("小于")]
        LessThan,

        /// <summary>
        /// 小于等于
        /// </summary>
        [Explanation("小于等于")]
        LessThanOrEquals,

        /// <summary>
        /// 大于
        /// </summary>
        [Explanation("大于")]
        GreaterThan,

        /// <summary>
        /// 大于等于
        /// </summary>
        [Explanation("大于等于")]
        GreaterThanOrEquals,

        /// <summary>
        /// 介于值之间
        /// </summary>
        [Explanation("介于值之间")]
        BetweenAnd,

        /// <summary>
        /// 包含
        /// </summary>
        [Explanation("包含")]
        In
} 

    /// <summary>
    /// 条件间的逻辑关系
    /// </summary>
    [Explanation("条件间的逻辑关系")]
    public enum LogicalOperator
    {
        /// <summary>
        /// 并且
        /// </summary>
        [Explanation("并且")]
        And,
        /// <summary>
        /// 或
        /// </summary>
        [Explanation("或")]
        Or
    }

    /// <summary>
    /// 数据排序方式
    /// </summary>
    [Explanation("数据排序方式")]
    public enum OrderByMode
    {
        /// <summary>
        /// 升序
        /// </summary>
        [Explanation("升序")]
        ASC,
        /// <summary>
        /// 降序
        /// </summary>
        [Explanation("降序")]
        DESC
    }

    /// <summary>
    /// 条件值类型
    /// </summary>
    [Explanation("条件值类型")]
    public enum WhereType
    {
        /// <summary>
        /// 单值 
        /// </summary>
        [Explanation("单值")]
        SingleValue,
        /// <summary>
        /// 双值 
        /// </summary>
        [Explanation("双值")]
        DoubleValue,
        /// <summary>
        /// 多值
        /// </summary>
        [Explanation("多值")]
        MultiValue
    }
}
