using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using EZDev.Data.Coding;
using NHibernate.SqlTypes;

namespace EZDev.Data
{
    /// <summary>
    /// 传输数据对象
    /// </summary>
    [Serializable]
    public class TransferData<TEntity> : DynamicObject, ISerializable where TEntity: BaseEntity
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public TransferData()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="properties">属性信息 </param>
        /// <example>添加方法如 var ss = new TransferData<Code>(Prop("Name", ""), Prop("Coding", "a")); </example>
        public TransferData(params Tuple<string, object>[] properties)
        {
            foreach(var tuple in properties)
            {
                memberList.Add(tuple.Item1, tuple.Item2);
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">序列化信息</param>
        /// <param name="context">流上下文</param>
        protected TransferData(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                memberList.Add(entry.Name, entry.Value);
            }
        }
        #endregion

        #region 私有成员
        /// <summary>
        /// 元数据
        /// </summary>
        private Dictionary<string, object> memberList = new Dictionary<string, object>();
        #endregion

        #region 重载的方法
        /// <summary>
        /// 得到对象成员
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return (memberList.TryGetValue(binder.Name, out result));
        }

        /// <summary>
        /// 设置成员对象
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            memberList.Add(binder.Name, value);
            return true;
        }

        /// <summary>
        /// 得到成员名称列表
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return memberList.Keys;
        }

        #endregion

        #region ISerializable 接口成员
        /// <summary>
        /// 得到对象数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (KeyValuePair<string, object> kvp in memberList)
            {
                info.AddValue(kvp.Key, kvp.Value);
            }
        }
        #endregion

        /// <summary>
        /// 新建一个属性相关元组
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="value">属性值</param>
        /// <returns>元组</returns>
        public static Tuple<string, object> Prop(string name, object value)
        {
            return new Tuple<string, object>(name, value);
        }

        /// <summary>
        /// 得到实体类型
        /// </summary>
        /// <returns></returns>
        public Type GetEntityType()
        {
            return typeof (TEntity);
        }
    }
}
