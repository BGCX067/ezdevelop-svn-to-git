using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EZDev
{
    /// <summary>
    /// 参数接口
    /// </summary>
    public interface IStringValue
    {
        /// <summary>
        /// 从字符串加载 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool LoadFromString(String value);

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        string ConvertToString();

        /// <summary>
        /// 校验值
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }

}
