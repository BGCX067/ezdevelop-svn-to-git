using System.Collections;
using NHibernate;

namespace EZDev.Data
{
    public static class NHExtension
    {

        /// <summary>
        /// 做成扩展方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public static IQuery SetParameters(this IQuery query, object[] parameters)
        {
            var paramNames = query.NamedParameters;
            for(int i = 0; i < paramNames.Length; i++)
            {
                if (parameters[i] is IList)
                {
                    query.SetParameterList(paramNames[i], parameters[i] as IList);
                }
                else
                {
                    query.SetParameter(paramNames[i], parameters[i]);
                }
            }
            return query;
        }
    }
}