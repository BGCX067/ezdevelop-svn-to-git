#region  修订历史
/*
 * 创建时间：2010-4-17
 * 创建人：董永刚
 * 描述：数据库参数抽象类，实现参数从数据库加载和将参数保存到数据库的操作
 *       目前支持简单数据类型，字符串、数值、日期、枚举等
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EZDev;

namespace EZDev.Data
{
    /// <summary>
    /// 数据库参数类
    /// </summary>
    public abstract class DBParameters
    {
        /// <summary>
        /// 对象的参数项列表
        /// </summary>
        private List<ParameterItem> paramList = new List<ParameterItem>();
        private List<ParameterItem> changeParamList = new List<ParameterItem>();
        /// <summary>
        /// 从数据库加载参数
        /// </summary>
        public void LoadFromDB()
        {
            
            MemberAttributeInfo[] members = Reflector.FindMembers(Reflector.AllCriteria, base.GetType(), false,
                                                                  typeof(ParameterAttribute));
            IList<ParameterItem> parameterList =
                NHHelper.Instance.GetCurrentSession().CreateCriteria<ParameterItem>().List<ParameterItem>();
            ParameterItem item = null;
            foreach(var member in members)
            {
                item = null;
                // 得到成员的特性
                var paramaterAttr =
                    (ParameterAttribute) member.Attributes.First(a => a.GetType().Equals(typeof (ParameterAttribute)));
                if (parameterList.Count > 0)
                {
                    item = parameterList.FirstOrDefault<ParameterItem>(paraItem => paraItem.ID == paramaterAttr.ID);
                }
                // 查找不到参数的对象
                if (item == null)
                {
                    Reflector.SetValue(member.MemberInfo, this, paramaterAttr.DefaultValue);
                }
                else
                {
                    Reflector.SetValue(member.MemberInfo, this, item.Value);
                    if (paramList.Find(p => p.ID == item.ID) == null)
                    {
                        paramList.Add(item);
                    }
                }

            }
        }

        /// <summary>
        /// 将当前对象的参数保存到数据库
        /// </summary>
        public void SaveToDB()
        {
            MemberAttributeInfo[] members = Reflector.FindMembers(Reflector.AllCriteria, base.GetType(), false,
                                                                  typeof(ParameterAttribute));
            foreach (var member in members)
            {
                // 得到成员的特性
                ParameterAttribute paramaterAttr =
                    (ParameterAttribute)member.Attributes.First(a => a.GetType().Equals(typeof(ParameterAttribute)));

                // 在数据库查找参数项
                ParameterItem item = paramList.Find(p => p.ID == paramaterAttr.ID);

                // 查找不到参数的对象
                if (item == null)
                {
                    item = new ParameterItem
                               {
                                   ID = paramaterAttr.ID,
                                   Value = DataConvert.GetString(Reflector.GetValue(member.MemberInfo, this)),
                                   Remark = paramaterAttr.Explanation,
                               };
                    
                    NHHelper.Instance.GetCurrentSession().SaveOrUpdate(item);
                    paramList.Add(item);
                }
                else
                {
                    string newstr = DataConvert.GetString(Reflector.GetValue(member.MemberInfo, this));

                    if (item.Value != newstr)
                    {
                        item.Value = newstr;
                        NHHelper.Instance.GetCurrentSession().SaveOrUpdate(item);
                    }
                }

            }
        }

    }
}
