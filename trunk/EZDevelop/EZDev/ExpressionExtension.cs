using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EZDev
{
    public static class ExpressionExtension
    {
        #region 表达式

        public static Expression ContactExpressions(this Expression exp, params Expression[] exps)
        {
            foreach (var e in exps)
            {
                if (null == e) continue;
                exp = Expression.And(exp, e);
            }
            return exp;
        }

        public static Expression<Func<T>> ContactExpressions<T>(this Expression exp, params Expression[] exps)
        {
            foreach (var e in exps)
            {
                if (null == e) continue;
                exp = Expression.And(exp, e);
            }
            return (Expression<Func<T>>)exp;
        }

        public static Expression<Func<T1, T>> ContactExpressions<T1, T>(this Expression exp, params Expression[] exps)
        {
            foreach (var e in exps)
            {
                if (null == e) continue;
                exp = Expression.And(exp, e);
            }
            return (Expression<Func<T1, T>>)exp;
        }

        public static Expression<Func<T1, T2, T>> ContactExpressions<T1, T2, T>(this Expression exp,
                                                                                params Expression[] exps)
        {
            foreach (var e in exps)
            {
                if (null == e) continue;
                exp = Expression.And(exp, e);
            }
            return (Expression<Func<T1, T2, T>>)exp;
        }

        public static Expression<Func<T1, T2, T3, T>> ContactExpressions<T1, T2, T3, T>(this Expression exp,
                                                                                        params Expression[] exps)
        {
            foreach (var e in exps)
            {
                if (null == e) continue;
                exp = Expression.And(exp, e);
            }
            return (Expression<Func<T1, T2, T3, T>>)exp;
        }

        #endregion

        private static Tuple<string, bool> ParseOrderbyString(string orderbyString)
        {
            string realStr = orderbyString.Trim();
            if (realStr == "")
            {
                throw new ArgumentNullException("orderbyString");
            }
            int idx = realStr.IndexOf(' ');
            string prop = idx == -1 ? realStr : realStr.Substring(0, idx);
            bool isDesc = idx != -1 && realStr.Substring(idx).Trim().Equals("desc", StringComparison.OrdinalIgnoreCase);
            return new Tuple<string, bool>(prop, isDesc);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, params string[] orderbyArray)
        {
            IOrderedQueryable<T> result = null;
            foreach (string prop in orderbyArray)
            {
                var tuple = ParseOrderbyString(prop);
                dynamic keySelector = QueryableHelper<T>.GetLambdaExpression(tuple.Item1);
                if (result == null)
                {
                    result = tuple.Item2
                                 ? Queryable.OrderByDescending(queryable, keySelector)
                                 : Queryable.OrderBy(queryable, keySelector);
                }
                else
                {
                    result = tuple.Item2
                             ? Queryable.ThenByDescending(result, keySelector)
                             : Queryable.ThenBy(result, keySelector);
                }
            }
            return result;
        }

        static class QueryableHelper<T>
        {
            private static Dictionary<string, LambdaExpression> cache = new Dictionary<string, LambdaExpression>();


            public static LambdaExpression GetLambdaExpression(string propertyName)
            {
                if (cache.ContainsKey(propertyName)) return cache[propertyName];
                var param = Expression.Parameter(typeof(T));
                var body = Expression.Property(param, propertyName);
                var keySelector = Expression.Lambda(body, param);
                cache[propertyName] = keySelector;
                return keySelector;
            }
        }
    }
}
