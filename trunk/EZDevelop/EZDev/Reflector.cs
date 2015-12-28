#region  修订历史
/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：对象反射器
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EZDev
{
    /// <summary>
    /// 对象反射器
    /// </summary>
    public class Reflector
    {
        /// <summary>
        /// 反射操作搜索标准，搜索所有公有或非公有成员。
        /// </summary>
        public static readonly BindingFlags DefaultCriteria = (BindingFlags.NonPublic | BindingFlags.Public);
        /// <summary>
        /// 反射操作搜索标准，搜索所有公有或非公有的实例成员。
        /// </summary>
        public static readonly BindingFlags InstanceCriteria = (DefaultCriteria | BindingFlags.Instance);
        /// <summary>
        /// 反射操作搜索标准，搜索所有公有或非公有的静态成员，并且搜索其父类的保护和公有的静态成员。.
        /// </summary>
        public static readonly BindingFlags StaticCriteria = ((DefaultCriteria | BindingFlags.Static) | BindingFlags.FlattenHierarchy);
        /// <summary>
        /// 反射操作搜索标准，搜索所有公有或非公有的静态和实例成员，并且搜索其父类的保护和公有的静态和实例成员。
        /// </summary>
        public static readonly BindingFlags AllCriteria = (InstanceCriteria | StaticCriteria);

        /// <summary>
        /// 利用晚期绑定执行指定对象的指定公共方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <param name="methodName">对象的方法名称</param>
        /// <param name="paramValues">方法的参数列表</param>
        /// <returns>该方法的返回值</returns>
        public static object ExecuteMethod(object obj, string methodName, params object[] paramValues)
        {
            try
            {
                return obj.GetType().InvokeMember(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, obj, paramValues);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 在一个枚举值中查找一个指定属性类型的属性
        /// </summary>
        /// <param name="enumValue">要搜索属性的枚举值</param>
        /// <param name="attributeType">属性类型</param>
        /// <returns>发现的属性</returns>
        public static Attribute FindAttribute(Enum enumValue, Type attributeType)
        {
            MemberInfo[] member = enumValue.GetType().GetMember(enumValue.ToString(), StaticCriteria);
            if ((member != null) && (member.Length > 0))
            {
                MemberInfo memberInfo = member[0];
                Attribute[] attributeArray = FindAttributes(memberInfo, false, new Type[] { attributeType });
                if ((attributeArray != null) && (attributeArray.Length > 0))
                {
                    return attributeArray[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 查找并获取指定成员的属性
        /// </summary>
        /// <param name="memberInfo">要获取属性的成员</param>
        /// <param name="allowDuplicate">是否允许返回重复的多个属性。为true表示，如果一个成员有多个相同类型的属性，则返回多个，false则不管成员是否有多个属性，都只取其第一个属性。</param>
        /// <param name="attributeTypes">要搜索的1个或多个属性类型</param>
        /// <returns>成员的属性数组</returns>
        public static Attribute[] FindAttributes(MemberInfo memberInfo, bool allowDuplicate, params Type[] attributeTypes)
        {
            List<Attribute> list = new List<Attribute>();
            foreach (Type type in attributeTypes)
            {
                object[] customAttributes = memberInfo.GetCustomAttributes(type, true);
                int num = allowDuplicate ? customAttributes.Length : 1;
                if ((customAttributes != null) && (customAttributes.Length > 0))
                {
                    for (int i = 0; i < num; i++)
                    {
                        list.Add(customAttributes[i] as Attribute);
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 查询指定成员的所有特性
        /// </summary>
        /// <param name="memberInfo">成员</param>
        /// <param name="inherit">是否按继承关系搜索</param>
        /// <returns>返回的特性数组</returns>
        public static Attribute[] FindAttributes(MemberInfo memberInfo, bool inherit)
        {
            return Array.ConvertAll<object, Attribute>(memberInfo.GetCustomAttributes(inherit),
                                                       obj => obj == null ? null : obj as Attribute);
        }


        /// <summary>
        /// 查找并获取指定类型的可用的构造函数(不包括虚拟的构造函数)。
        /// </summary>
        /// <param name="type">指定的类型</param>
        /// <returns>可用的构造函数数组</returns>
        public static ConstructorInfo[] FindConstructors(Type type)
        {
            return type.GetConstructors(InstanceCriteria).Where(info => !info.IsAbstract).ToArray();
        }

        /// <summary>
        /// 查找并获取指定名称的成员
        /// </summary>
        /// <param name="criteria">搜索成员的标准</param>
        /// <param name="name">要查找的成员名称</param>
        /// <param name="type">指定的要搜索的类型</param>
        /// <returns>成员</returns>
        public static MemberInfo FindMember(BindingFlags criteria, string name, Type type)
        {
            MemberInfo[] member = type.GetMember(name, criteria);
            return (((member != null) && (member.Length > 0)) ? member[0] : null);
        }

        /// <summary>
        /// 查找并获取在类型只那个包含指定的属性的成员。
        /// </summary>
        /// <param name="criteria">搜索成员的标准</param>
        /// <param name="type">指定的要搜索的类型</param>
        /// <param name="allowDuplicate">是否允许返回重复的多个属性。为true表示，如果一个成员有多个相同类型的属性，则返回多个，false则不管成员是否有多个属性，都只取其第一个属性。</param>
        /// <param name="attributeTypes">要搜索的1个或多个属性类型</param>
        /// <returns>成员属性信息数组</returns>
        public static MemberAttributeInfo[] FindMembers(BindingFlags criteria, Type type, bool allowDuplicate, params Type[] attributeTypes)
        {
            List<MemberAttributeInfo> list = new List<MemberAttributeInfo>();
            foreach (MemberInfo info in type.GetMembers())
            {
                Attribute[] attributes = FindAttributes(info, allowDuplicate, attributeTypes);
                if ((attributes != null) && (attributes.GetLength(0) > 0))
                {
                    list.Add(new MemberAttributeInfo(info, attributes));
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 利用晚期绑定实现对指定对象指定公共属性的读取
        /// </summary>
        /// <param name="obj">要调用属性的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>返回值</returns>
        public static object GetPropertyValue(object obj, string propertyName)
        {
            try
            {
                return obj.GetType().InvokeMember(propertyName, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, obj, null);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定字段或属性的值。
        /// </summary>
        /// <param name="memberInfo">成员</param>
        /// <param name="instance">含有成员的对象实例，静态成员时忽略该参数</param>
        /// <returns>字段或属性的值</returns>
        public static object GetValue(MemberInfo memberInfo, object instance)
        {
            if (memberInfo is PropertyInfo)
            {
                return (memberInfo as PropertyInfo).GetValue(instance, null);
            }
            if (memberInfo is FieldInfo)
            {
                return (memberInfo as FieldInfo).GetValue(instance);
            }
            return null;
        }

        /// <summary>
        /// 得到指定名称的字段或属性的值(仅仅搜索对象实例)。
        /// </summary>
        /// <param name="name">字段或属性的名称</param>
        /// <param name="instance">搜索的实例</param>
        /// <returns>字段或属性的值</returns>
        public static object GetValue(string name, object instance)
        {
            MemberInfo memberInfo = FindMember(InstanceCriteria, name, instance.GetType());
            if (memberInfo == null)
            {
                return null;
            }
            return GetValue(memberInfo, instance);
        }

        /// <summary>
        /// 利用晚期绑定实现对指定对象指定公共属性的值的设置
        /// </summary>
        /// <param name="obj">要设置的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
        {
            try
            {
                Type type = obj.GetType();
                object[] args = new object[] { propertyValue };
                type.InvokeMember(propertyName, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance, null, obj, args);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 得到Nullable类型的基类型
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <returns>如果是Nullable类型，则返回基类型，否则返回空</returns>
        public static Type GetNullable(Type type)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return type.GetGenericArguments()[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 设置指定字段或属性的值
        /// </summary>
        /// <param name="memberInfo">成员</param>
        /// <param name="instance">含有成员的对象实例，静态成员时忽略该参数</param>
        /// <param name="value">设置的值</param>
        public static void SetValue(MemberInfo memberInfo, object instance, object value)
        {
            Type propertyType;
            if (memberInfo is PropertyInfo)
            {
                PropertyInfo info = memberInfo as PropertyInfo;
                propertyType = info.PropertyType;
                if (value == null)
                {
                    if (GetNullable(propertyType) != null)
                    {
                        info.SetValue(instance, null, null);
                    }
                    else if (propertyType.Equals(typeof(DateTime)))
                    {
                        info.SetValue(instance, SysUtils.EmptyDateTime, null);
                    } 
                    else
                    {
                        info.SetValue(instance, value, null);
                    }
                }
                else if (value.GetType().Equals(propertyType))
                {
                    info.SetValue(instance, value, null);
                }
                else
                {
                    info.SetValue(instance, DataConvert.GetTypeValue(propertyType, value), null);
                }
            }
            else if (memberInfo is FieldInfo)
            {
                FieldInfo info2 = memberInfo as FieldInfo;
                propertyType = info2.FieldType;
                if (value == null)
                {
                    if (GetNullable(propertyType) != null)
                    {
                        info2.SetValue(instance, null);
                    }
                    else if (propertyType.Equals(typeof(DateTime)))
                    {
                        info2.SetValue(instance, SysUtils.EmptyDateTime);
                    }
                    else
                    {
                        info2.SetValue(instance, value);
                    }
                }
                else if (value.GetType().Equals(propertyType))
                {
                    info2.SetValue(instance, value);
                }
                else
                {
                    info2.SetValue(instance, DataConvert.GetTypeValue(propertyType, value));
                }
            }
        }
    }
}