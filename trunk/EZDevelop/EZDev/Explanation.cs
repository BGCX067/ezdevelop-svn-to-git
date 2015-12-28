#region  修订历史
/*
 * 创建时间：2010-4-6
 * 创建人：董永刚
 * 描述：说明信息类
 */
#endregion
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EZDev
{
    /// <summary>
    /// 说明（解释）
    /// </summary>
    [Serializable]
    public class Explanation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="explanationAttribute">说明信息</param>
        /// <param name="target">说明的目标</param>
        public Explanation(ExplanationAttribute explanationAttribute, object target)
            : this(explanationAttribute.Name, explanationAttribute.Description, explanationAttribute.Tag, target)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="description">描述</param>
        /// <param name="tag">附加信息</param>
        /// <param name="target">说明的目标</param>
        public Explanation(string name, string description, object tag, object target)
        {
            this.Name = "";
            this.Description = "";
            this.Tag = null;
            this.Target = null;
            this.Name = name;
            this.Description = description;
            this.Tag = tag;
            this.Target = target;
        }

        /// <summary>
        /// 得到指定类型所在程序集的说明
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>程序集说明</returns>
        public static Explanation GetAssemblyExplanation(Type type)
        {
            if (type != null)
            {
                object[] customAttributes = Assembly.GetAssembly(type).GetCustomAttributes(false);
                if (customAttributes.GetLength(0) > 0)
                {
                    return new Explanation(customAttributes[0] as ExplanationAttribute, Assembly.GetAssembly(type));
                }
            }
            return null;
        }

        /// <summary>
        /// 得到枚举类型的说明
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举说明</returns>
        public static Explanation GetEnumExplanation(Type enumType)
        {
            if (enumType == null)
            {
                throw new System.ArgumentNullException("enumType 参数不能为空！");
            }
            if (!enumType.IsEnum)
            {
                throw new System.ArgumentException("enumType 不是一个枚举类型！");
            }
            return GetTypeExplanation(enumType);
        }

        /// <summary>
        /// 得到制定枚举数的说明
        /// </summary>
        /// <param name="enumValue">枚举数</param>
        /// <returns>枚举数的说明</returns>
        public static EnumValueExplanation GetEnumValueExplanation(Enum enumValue)
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException("enumType 不能为空！");
            }
            Attribute attribute = Reflector.FindAttribute(enumValue, typeof(ExplanationAttribute));
            if (attribute != null)
            {
                ExplanationAttribute attribute2 = attribute as ExplanationAttribute;
                return new EnumValueExplanation(enumValue, attribute2.Name, attribute2.Description, attribute2.Tag);
            }
            return null;
        }

        /// <summary>
        /// 得到枚举类型的所有枚举数说明
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举类型的所有枚举数说明</returns>
        public static EnumValueExplanation[] GetEnumValueExplanations(Type enumType)
        {
            if (enumType == null)
            {
                throw new System.ArgumentNullException("enumType 参数不能为空！");
            }
            if (!enumType.IsEnum)
            {
                throw new System.ArgumentException("enumType 不是一个枚举类型！");
            }
            Explanation[] memberExplanations = GetMemberExplanations(enumType);
            List<EnumValueExplanation> list = new List<EnumValueExplanation>();
            foreach (Explanation explanation in memberExplanations)
            {
                if ((explanation.Target != null) && (explanation.Target is FieldInfo))
                {
                    FieldInfo target = explanation.Target as FieldInfo;
                    if (target.FieldType.Equals(enumType))
                    {
                        list.Add(new EnumValueExplanation(target.GetValue(null) as Enum, explanation.Name, explanation.Description, explanation.Tag));
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 得到指定成员的说明
        /// </summary>
        /// <param name="memberInfo">成员</param>
        /// <returns>该成员的说明</returns>
        public static Explanation GetMemberExplanation(MemberInfo memberInfo)
        {
            Attribute[] attributeArray = Reflector.FindAttributes(memberInfo, false, new Type[] { typeof(ExplanationAttribute) });
            if (attributeArray.GetLength(0) > 0)
            {
                return new Explanation(attributeArray[0] as ExplanationAttribute, memberInfo);
            }
            return null;
        }

        /// <summary>
        /// 得到类型的指定名称的成员的说明
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">成员名称</param>
        /// <returns>对应成员的说明</returns>
        public static Explanation GetMemberExplanation(Type type, string memberName)
        {
            MemberInfo memberInfo = Reflector.FindMember(Reflector.AllCriteria, memberName, type);
            if (memberInfo != null)
            {
                return GetMemberExplanation(memberInfo);
            }
            return null;
        }

        /// <summary>
        /// 得到指定类型的所有成员的说明
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>该类型的所有成员的说明</returns>
        public static Explanation[] GetMemberExplanations(Type type)
        {
            List<Explanation> list = new List<Explanation>();
            string s = Reflector.AllCriteria.ToString();
            int i = (int)Reflector.AllCriteria;
            MemberAttributeInfo[] infoArray = Reflector.FindMembers(BindingFlags.Public, type, false, new Type[] { typeof(ExplanationAttribute) });
            foreach (MemberAttributeInfo info in infoArray)
            {
                list.Add(new Explanation(info.Attribute as ExplanationAttribute, info.MemberInfo));
            }
            return list.ToArray();
        }

        /// <summary>
        /// 得到指定类型的说明
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型说明</returns>
        public static Explanation GetTypeExplanation(Type type)
        {
            if (type != null)
            {
                object[] customAttributes = type.GetCustomAttributes(typeof(ExplanationAttribute), false);
                if (customAttributes.GetLength(0) > 0)
                {
                    return new Explanation(customAttributes[0] as ExplanationAttribute, type);
                }
            }
            return null;
        }

        /// <summary>
        /// 转换为字符串描述
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Name == "")
            {
                return Description;
            }
            if (Description != "")
            {
                return (Name + ", " + Description);
            }
            return Name;
        }

        /// <summary>
        /// 获取描述
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取附加信息
        /// </summary>
        public object Tag
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取说明的目标
        /// </summary>
        public object Target
        {
            get;
            private set;
        }
    }
}