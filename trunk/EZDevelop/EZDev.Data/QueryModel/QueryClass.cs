using System;
using System.Collections.Generic;
using EZDev.Data.QueryModel;

namespace EZDev.Data.QueryModel
{

    #region 条件

    /// <summary>
    /// 条件组
    /// </summary>
    public class ConditionGroup : IConditionGroup
    {
        #region 私有变量

        /// <summary>
        /// 条件列表
        /// </summary>
        private readonly List<ICondition> conditions = new List<ICondition>();

        /// <summary>
        /// 参数列表
        /// </summary>
        private readonly List<object> paramList = new List<object>();

        /// <summary>
        /// 生成的HQL语句
        /// </summary>
        private string hql = "";

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数，新建一个条件组对象
        /// </summary>
        /// <param name="isNot">是否针对条件使用Not</param>
        /// <param name="logicalOperator">与上一条的逻辑关系</param>
        internal ConditionGroup(LogicalOperator logicalOperator, bool isNot)
        {
            IsNot = isNot;
            LogicalOperator = logicalOperator;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 条件是否求反
        /// 尽量少使用Not条件，对性能影响较大
        /// </summary>
        public bool IsNot
        {
            get;
            private set;
        }

        /// <summary>
        /// 与上一条件关系
        /// </summary>
        public LogicalOperator LogicalOperator
        {
            get;
            private set;
        }

        /// <summary>
        /// 得到参数数组
        /// </summary>
        public object[] Parameters
        {
            get
            {
                BuildHql();
                return paramList.ToArray();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 转换为HQL语句
        /// </summary>
        /// <returns></returns>
        public string ToHql()
        {
            hql = "";
            BuildHql();
            return hql;
        }

        /// <summary>
        /// 增加一个条件组
        /// </summary>
        /// <param name="conditionGroup"></param>
        /// <returns></returns>
        public IConditionGroup Add(IConditionGroup conditionGroup)
        {
            conditions.Add(conditionGroup);
            return this;
        }

        /// <summary>
        /// 增加一个条件
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IConditionGroup Add(ICondition condition)
        {
            conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// 构建HQL语句
        /// </summary>
        private void BuildHql()
        {
            paramList.Clear();
            hql = "";
            string str = "";
            foreach (ICondition condition in conditions)
            {
                string logic = QueryHelper.LogicalOperatorToString(condition.LogicalOperator);
                if (condition is IMultiValueCondition)
                {
                    paramList.Add(new List<object> {condition.Parameters});
                }
                else
                {
                    paramList.AddRange(condition.Parameters);
                }
                str = condition.ToHql();
                hql += hql == ""
                           ? string.Format("{0} {1}", QueryHelper.IsNotToString(IsNot), str)
                           : string.Format(" {0} {1} {2}", logic, QueryHelper.IsNotToString(IsNot), str);
            }
            hql = string.Format("({0})", hql);
        }

        #endregion
    }


    /// <summary>
    /// 单值查询条件
    /// </summary>
    public class SingleValueCondition : ISignleValueCondition
    {
        #region 私有变量

        private static Random random = new Random();

        /// <summary>
        /// 参数列表
        /// </summary>
        private readonly List<object> innerValues = new List<object>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 单值条件构造函数
        /// </summary>
        /// <param name="logicalOperator">与上一条件的逻辑关系</param>
        /// <param name="isNot">是否使用Not</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="_operator">当前条件的比较运算符</param>
        /// <param name="parameter">参数</param>
        internal SingleValueCondition(LogicalOperator logicalOperator, bool isNot, string propertyName,
                                      Operator _operator, object parameter)
        {
            if (new List<Operator> {Operator.In, Operator.BetweenAnd}.Contains(_operator))
            {
                throw new TypeInitializationException(typeof (Operator).GetType().Name,
                                                      new ArgumentException("不是一个单值操作类型"));
            }
            LogicalOperator = logicalOperator;
            PropertyName = propertyName;
            Operator = _operator;
            Parameter = parameter;
            innerValues.Add(parameter);
            IsNot = isNot;
        }

        #endregion  构造函数

        #region 属性

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// 条件是否求反
        /// 尽量少使用Not条件，对性能影响较大
        /// </summary>
        public bool IsNot
        {
            get;
            private set;
        }

        /// <summary>
        /// 关系运算符，标识与上个条件的关系
        /// </summary>
        public LogicalOperator LogicalOperator
        {
            get;
            private set;
        }

        /// <summary>
        /// 比较运算符
        /// </summary>
        public Operator Operator
        {
            get;
            private set;
        }

        /// <summary>
        /// 得到参数列表，按添加顺序
        /// </summary>
        public object[] Parameters
        {
            get
            {
                return innerValues.ToArray();
            }
        }


        /// <summary>
        /// 参数
        /// </summary>
        public object Parameter
        {
            get;
            private set;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 转换为HQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToHql()
        {
            var paramName = string.Format("{0}{1}", PropertyName.Trim().Replace('.', '_'), random.Next(1000));
            switch (Operator)
            {
                case Operator.Equals:
                    return string.Format("{0} = :{1}", PropertyName, paramName);
                case Operator.Like:
                    return string.Format("{0} LIKE '%'||:{1}||'%'", PropertyName, paramName);
                case Operator.LessThan:
                    return string.Format("{0} < :{1}", PropertyName, paramName);
                case Operator.LessThanOrEquals:
                    return string.Format("{0} <= :{1}", PropertyName, paramName);
                case Operator.GreaterThan:
                    return string.Format("{0} > :{1}", PropertyName, paramName);
                case Operator.GreaterThanOrEquals:
                    return string.Format("{0} >= :{1}", PropertyName, paramName);
                default:
                    throw new ArgumentOutOfRangeException(string.Format("不支持的操作符 {0}", Operator));
            }
        }

        /// <summary>
        /// 得到字符串描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToHql();
        }

        #endregion
    }


    /// <summary>
    /// 双值查询条件
    /// </summary>
    public class DoubleValueCondition : IDoubleValueCondition
    {

        private static Random random = new Random();

        #region 构造函数

        /// <summary>
        /// 双值 条件构造函数
        /// </summary>
        /// <param name="logicOperator">与上一条件的逻辑关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="_operator">当前条件的比较运算符</param>
        /// <param name="startValue">起始值 参数</param>
        /// <param name="endValue">结束值参数</param>
        internal DoubleValueCondition(LogicalOperator logicOperator, bool isNot, string propertyName, Operator _operator,
                                      object startValue, object endValue)
        {
            if (_operator != Operator.BetweenAnd)
            {
                throw new TypeInitializationException(typeof (Operator).GetType().Name,
                                                      new ArgumentException("不是一个双值操作类型"));
            }

            LogicalOperator = logicOperator;
            PropertyName = propertyName;
            Operator = _operator;
            innerValues.Add(startValue);
            innerValues.Add(endValue);
            IsNot = isNot;
        }

        #endregion  构造函数

        #region 属性

        /// <summary>
        /// 参数列表
        /// </summary>
        protected List<object> innerValues = new List<object>();

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// 条件是否求反
        /// 尽量少使用Not条件，对性能影响较大
        /// </summary>
        public bool IsNot
        {
            get;
            private set;
        }

        /// <summary>
        /// 关系运算符，标识与上个条件的关系
        /// </summary>
        public LogicalOperator LogicalOperator
        {
            get;
            private set;
        }


        /// <summary>
        /// 比较运算符
        /// </summary>
        public Operator Operator
        {
            get;
            private set;
        }

        /// <summary>
        /// 开始参数
        /// </summary>
        public object StartParameter
        {
            get
            {
                return innerValues[0];
            }
        }

        /// <summary>
        /// 结束参数
        /// </summary>
        public object EndParameter
        {
            get
            {
                return innerValues[1];
            }
        }


        /// <summary>
        /// 得到参数列表，按添加顺序
        /// </summary>
        public object[] Parameters
        {
            get
            {
                return innerValues.ToArray();
            }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 转换为HQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToHql()
        {
            
            var paramName1 = string.Format("{0}{1}", PropertyName.Trim().Replace('.', '_'), random.Next());
            var paramName2 = string.Format("{0}{1}", PropertyName.Trim().Replace('.', '_'), random.Next());
            switch (Operator)
            {
                case Operator.BetweenAnd:
                    return string.Format("{0} BETWEEN :{1} AND :{2}", PropertyName, paramName1,paramName2);
                default:
                    throw new ArgumentOutOfRangeException(string.Format("不支持的操作符", Operator));
            }
        }

        /// <summary>
        /// 得到字符串描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToHql();
        }

        #endregion
    }


    /// <summary>
    /// 多值查询条件
    /// </summary>
    public class MultiValueCondition : IMultiValueCondition
    {

        private static Random random = new Random();
        #region 构造函数

        /// <summary>
        /// 多值 条件构造函数
        /// </summary>
        /// <param name="logicalOperator">与上一条件的逻辑关系</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="_operator">当前条件的比较运算符</param>
        /// <param name="parameters">参数数组</param>
        /// <exception cref="TypeInitializationException">比较操作类型与参数不匹配</exception>
        internal MultiValueCondition(LogicalOperator logicalOperator, bool isNot, string propertyName,
                                     Operator _operator, params object[] parameters)
        {
            if (_operator != Operator.In)
            {
                throw new TypeInitializationException(typeof (Operator).GetType().Name,
                                                      new ArgumentException("不是一个多值操作类型"));
            }
            LogicalOperator = logicalOperator;
            PropertyName = propertyName;
            Operator = _operator;
            innerValues.AddRange(parameters);
            IsNot = isNot;
        }

        #endregion  构造函数

        #region 属性

        /// <summary>
        /// 参数列表
        /// </summary>
        protected List<object> innerValues = new List<object>();

        /// <summary>
        /// 第一个参数
        /// 单值和双值条件使用
        /// </summary>
        public object Parameter1
        {
            get
            {
                return innerValues[0];
            }
        }

        /// <summary>
        /// 第二参数
        /// 双值条件使用
        /// </summary>
        public object Parameter2
        {
            get
            {
                return innerValues[1];
            }
        }

        /// <summary>
        /// 参数是否是列表参数
        /// </summary>
        public bool IsListParameters
        {
            get;
            internal set;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// 条件是否求反
        /// 尽量少使用Not条件，对性能影响较大
        /// </summary>
        public bool IsNot
        {
            get;
            private set;
        }

        /// <summary>
        /// 关系运算符，标识与上个条件的关系
        /// </summary>
        public LogicalOperator LogicalOperator
        {
            get;
            private set;
        }


        /// <summary>
        /// 比较运算符
        /// </summary>
        public Operator Operator
        {
            get;
            private set;
        }


        /// <summary>
        /// 得到参数列表，按添加顺序
        /// </summary>
        public object[] Parameters
        {
            get
            {
                return innerValues.ToArray();
            }
        }

        /// <summary>
        /// 得到字符串描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToHql();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 转换为HQL字符串
        /// </summary>
        /// <returns></returns>
        public string ToHql()
        {
            var paramName = string.Format("{0}{1}", PropertyName.Trim().Replace('.', '_'), random.Next());
            switch (Operator)
            {
                case Operator.In:
                    return string.Format("{0} IN (:{1})", PropertyName, paramName);
                default:
                    throw new ArgumentOutOfRangeException(string.Format("不支持的操作符 {0}", Operator));
            }
        }

        #endregion
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    public static class Conditions
    {
        #region 条件

        /// <summary>
        /// 创建一个等于条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        /// <returns>创建的条件对象</returns>
        public static ICondition Eq(string propertyName,
                                    object value, LogicalOperator logicalOperator = LogicalOperator.And,
                                    bool isNot = false)
        {
            return new SingleValueCondition(logicalOperator, isNot, propertyName, Operator.Equals, value);
        }


        /// <summary>
        /// 创建一个Like条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        /// <returns>创建的条件对象</returns>
        public static ICondition Like(string propertyName,
                                      string value, LogicalOperator logicalOperator = LogicalOperator.And,
                                      bool isNot = false)
        {
            return new SingleValueCondition(logicalOperator, isNot, propertyName, Operator.Like, value);
        }


        /// <summary>
        /// 条件一个小于条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        /// <returns>创建的条件对象</returns>
        public static ICondition LessThan(string propertyName,
                                          object value, LogicalOperator logicalOperator = LogicalOperator.And,
                                          bool isNot = false)
        {
            return new SingleValueCondition(logicalOperator, isNot, propertyName, Operator.LessThan, value);
        }

        /// <summary>
        /// 创建一个小于等于条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        /// <returns>创建的条件对象</returns>
        public static ICondition LessThanOrEquals(string propertyName,
                                                  object value, LogicalOperator logicalOperator = LogicalOperator.And,
                                                  bool isNot = false)
        {
            return new SingleValueCondition(logicalOperator, isNot, propertyName, Operator.LessThanOrEquals, value);
        }

        /// <summary>
        /// 创建一个大于条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        /// <returns>创建的条件对象</returns>
        public static ICondition GreaterThan(string propertyName,
                                             object value, LogicalOperator logicalOperator = LogicalOperator.And,
                                             bool isNot = false)
        {
            return new SingleValueCondition(logicalOperator, isNot, propertyName, Operator.GreaterThan, value);
        }

        /// <summary>
        /// 创建一个大于等于条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">值</param>
        /// <returns>创建的条件对象</returns>
        public static ICondition GreaterThanOrEquals(string propertyName, object value,
                                                     LogicalOperator logicalOperator = LogicalOperator.And,
                                                     bool isNot = false)
        {
            return new SingleValueCondition(logicalOperator, isNot, propertyName, Operator.GreaterThanOrEquals, value);
        }

        /// <summary>
        /// 创建一个Between And条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="startValue">起始值</param>
        /// <param name="endValue">结束值</param>
        /// <returns>创建的条件</returns>
        public static ICondition BetweenAnd(string propertyName,
                                            object startValue, object endValue,
                                            LogicalOperator logicalOperator = LogicalOperator.And, bool isNot = false)
        {
            return new DoubleValueCondition(logicalOperator, isNot, propertyName, Operator.BetweenAnd, startValue,
                                            endValue);
        }

        /// <summary>
        /// 创建一个In条件
        /// </summary>
        /// <param name="logicalOperator">与上个条件的关系</param>
        /// <param name="isNot">是否求反</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="values">值数组</param>
        /// <returns>创建的条件</returns>
        public static ICondition In(LogicalOperator logicalOperator, bool isNot, string propertyName,
                                    params object[] values)
        {
            return new MultiValueCondition(logicalOperator, isNot, propertyName, Operator.In, values);
        }

        #endregion

        #region 查询条件组

        /// <summary>
        /// 得到新的查询条件组
        /// </summary>
        /// <param name="logicalOperator">与上一条件逻辑关系</param>
        /// <param name="isNot">是否求反</param>
        /// <returns></returns>
        public static IConditionGroup ConditionGroup(LogicalOperator logicalOperator = LogicalOperator.And,
                                                     bool isNot = false)
        {
            return new ConditionGroup(logicalOperator, isNot);
        }

        #endregion
    }

    #endregion

    #region  排序

    /// <summary>
    /// 排序条件列表
    /// </summary>
    public class OrderByGroup
    {
        private List<OrderBy> orderByList = new List<OrderBy>();
        /// <summary>
        /// 构造函数
        /// </summary>
        internal OrderByGroup()
        {
        }

        /// <summary>
        /// 添加排序条件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="orderByMode">排序条件</param>
        /// <returns></returns>
        public OrderByGroup Add(string propertyName, OrderByMode orderByMode)
        {
            orderByList.Add(new OrderBy(propertyName, orderByMode));
            return this;
        }

        /// <summary>
        /// 转换为HQL语句
        /// </summary>
        /// <returns></returns>
        public string ToHql()
        {
            string str = "";
            orderByList.ForEach(term =>
                        {
                            string mode = term.OrderByMode == OrderByMode.DESC ? "DESC" : "ASC";
                            str += string.IsNullOrEmpty(str)
                                       ? string.Format("{0} {1}", term.PropertyName, mode)
                                       : string.Format(", {0} {1}", term.PropertyName, mode);
                        });
            return str;
        }
    }

    /// <summary>
    /// 排序条件
    /// </summary>
    public class OrderBy
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="orderByMode">排序方式</param>
        internal OrderBy(string propertyName, OrderByMode orderByMode = OrderByMode.ASC)
        {
            PropertyName = propertyName;
            OrderByMode = orderByMode;
        }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName
        {
            get;
            private set;
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public OrderByMode OrderByMode
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 排序
    /// </summary>
    public static class Orders
    {
        public static OrderByGroup OrderGroup()
        {
            return new OrderByGroup();
        }
    }


    #endregion

    internal static class QueryHelper
    {
        #region 静态方法

        /// <summary>
        /// 得到逻辑关系字条串
        /// </summary>
        /// <param name="logic"></param>
        /// <returns></returns>
        public static string LogicalOperatorToString(LogicalOperator logic)
        {
            switch (logic)
            {
                case LogicalOperator.And:
                    return "AND";
                case LogicalOperator.Or:
                    return "OR";
                default:
                    throw new ArgumentOutOfRangeException("LogicalOperator");
            }
        }

        /// <summary>
        /// 得到 是否求反字符串
        /// </summary>
        /// <param name="isNot">是否求反</param>
        /// <returns>是否求反字符串</returns>
        public static string IsNotToString(bool isNot)
        {
            return isNot ? "NOT" : "";
        }

        #endregion
    }
}