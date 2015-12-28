using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EZDev.Data.MetaDatas
{ 
    /// <summary>
    /// 元数据管理器
    /// </summary>
    public static class MetaInfoManager
    {

        /// <summary>
        /// 元数据字典
        /// </summary>
        private static Dictionary<Type, List<PropertyMetaInfo>> metaDatas = new Dictionary<Type, List<PropertyMetaInfo>>();

        /// <summary>
        /// 得到元数据集合
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static List<PropertyMetaInfo> GetMetaInfos(Type entityType)
        {
            var finded = metaDatas.Keys.FirstOrDefault(t => t == entityType) != null;
            if (finded)
            {
                return metaDatas[entityType];
            }
            lock(metaDatas)
            {
                var result = GetMetaDatas(entityType);
                metaDatas.Add(entityType, result);
                return result;
            }
        }

        /// <summary>
        /// 得到元数据集合
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        private static List<PropertyMetaInfo> GetMetaDatas(Type entityType)
        {
            var propList = new List<PropertyMetaInfo>();
            PropertyInfo[] members = entityType.GetProperties(BindingFlags.Public);

            foreach(PropertyInfo mem in members)
            {
                var prop = new PropertyMetaInfo(entityType);
                prop.DataType = mem.DeclaringType;
                
#warning 通过NH的配置获取部分属性

                // 通过特性标签获取部分特性，主要包括NH.Validator的特性定义和自定义特性
                Attribute[] attrs = Reflector.FindAttributes(mem, true);
                foreach(Attribute attr in attrs)
                {
                    //if (attr is EZDev.Data.MetaDatas)
                    //{
                        
                    //}
                }
            }
            return null;
        }

    }
}
