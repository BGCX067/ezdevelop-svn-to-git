using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data.QueryModel
{
    /// <summary>
    /// 投影接口
    /// </summary>
    public interface IProjection
    {
        /// <summary>
        /// 转换为HQL
        /// </summary>
        /// <returns></returns>
        string ToHql();
    }

    /// <summary>
    /// 属性投影接口
    /// </summary>
    public interface IProperty : IProjection
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// 别名
        /// </summary>
        string Alias
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 查询条件接口
    /// </summary>
    public interface ICondition : IProjection
    {
        /// <summary>
        /// 条件是否求反
        /// 尽量少使用Not条件，对性能影响较大
        /// </summary>
        bool IsNot
        {
            get;
        }

        /// <summary>
        /// 与上一条件关系
        /// </summary>
        LogicalOperator LogicalOperator
        {
            get;
        }

        /// <summary>
        /// 参数列表
        /// 多值条件使用
        /// </summary>
        object[] Parameters
        {
            get;
        }
    }

    /// <summary>
    /// 查询条件接口
    /// </summary>
    public interface IPropertyCondition : ICondition
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        string PropertyName
        {
            get;
        }

        /// <summary>
        /// 比较运算符
        /// </summary>
        Operator Operator
        {
            get;
        }
    }

    /// <summary>
    /// 单值条件
    /// </summary>
    public interface ISignleValueCondition : IPropertyCondition
    {
        /// <summary>
        /// 参数
        /// </summary>
        object Parameter
        {
            get;
        }
    }

    /// <summary>
    /// 双值条件
    /// </summary>
    public interface IDoubleValueCondition : IPropertyCondition
    {
        /// <summary>
        /// 开始值参数
        /// </summary>
        object StartParameter
        {
            get;
        }

        /// <summary>
        /// 结束值参数
        /// </summary>
        object EndParameter
        {
            get;
        }
    }

    /// <summary>
    /// 多值 条件
    /// </summary>
    public interface IMultiValueCondition : IPropertyCondition
    {
    }

    /// <summary>
    /// 条件组接口
    /// </summary>
    public interface IConditionGroup : ICondition
    {
        IConditionGroup Add(ICondition condition);
        IConditionGroup Add(IConditionGroup conditionGroup);
    }
}
