using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data
{

    #region 实体类型

    /// <summary>
    /// 数据实体标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public abstract class BaseEntityAttribute : Attribute
    {

    }

    /// <summary>
    /// 列表实体对象标识特性
    /// 数据列表呈现为数据列表，支持一对多一、一对多等数据结构
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ListEntityAttribute : BaseEntityAttribute
    {

    }


    /// <summary>
    /// 树型结构实体特性
    /// 树型数据结构，必须指定父属性、子属性、名称属性等
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TreeEntityAttribute: BaseEntityAttribute
    {

    }

    /// <summary>
    /// 单据数据实体特性
    /// 一对多数据变形，支持单据头及单据明细数据，可直接在明细Grid中编辑数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class BillEnittyAttribute : BaseEntityAttribute
    {
    }


    #endregion

    #region 树型结构
    /// <summary>
    /// 树型结构名称特性
    /// </summary>
    public class TreeNameAttribute : DTOAttribute
    {
    }

    /// <summary>
    /// 树形结构父对象特性
    /// </summary>
    public class TreeParentAttribute : DTOAttribute
    {
    }

    /// <summary>
    /// 树型结构子对象集合特性
    /// </summary>
    public class TreeChildrenAttribute : DTOAttribute
    {
    }

    #endregion

    #region DTO 数据
    /// <summary>
    /// 属性标识特性基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DTOAttribute : Attribute
    {


        /// <summary>
        /// 无参数构造函数
        /// </summary>
        protected DTOAttribute()
        {
        }

         /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dtoName">DTO名称</param>
         public DTOAttribute(string dtoName)
        {
            DTOName = dtoName;
        }   
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dtoName">DTO名称</param>
        /// <param name="index">序号，默认数据排列方式</param>
        public DTOAttribute(string dtoName, int index)
        {
            Index = index;
            DTOName = dtoName;
        }


        /// <summary>
        /// 显示顺序
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// DTO数据名称
        /// </summary>
        public string DTOName
        {
            get;
            set;
        }
    }
    
    /// <summary>
    /// 列表属性显示特性，主要显示在GridView或TreeView中使用
    /// </summary>
    public class ListAttribute : DTOAttribute
    {
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public ListAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index"></param>
        public ListAttribute(int index): base("EZGridList", index)
        {
        }
    }

    /// <summary>
    /// Lookup属性显示特性，主要显示在LookupEdit、Combobox或弹出的选择窗体中使用
    /// </summary>
    public class LookupAttribute : DTOAttribute
    {
          /// <summary>
        /// 无参数构造函数
        /// </summary>
        public LookupAttribute()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">默认排序</param>
        public LookupAttribute(int index)
            : base("EZLookup", index)
        {
        }
 }


    #endregion

    #region  详细信息

    /// <summary>
    /// 数据的显示类型
    /// </summary>
    [Flags]
    public enum DataView
    {
        /// <summary>
        /// 不在明细中显示
        /// </summary>
        None= 0,
        /// <summary>
        /// 查看，在查看详细信息中显示
        /// </summary>
        View = 1,
        /// <summary>
        /// 新建，在新建对象中可编辑值
        /// </summary>
        New = 2,
        /// <summary>
        /// 编辑，在编辑对象中可编辑值
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 全部，在详细信息中总是显示
        /// </summary>
        All = View| New| Edit
    }


    /// <summary>
    /// 明细属性特性
    /// 在查看详细信息时显示
    /// 考虑在查看明细时某些数据不需要显示的情况
    /// </summary>
    [AttributeUsage(AttributeTargets.Property| AttributeTargets.Field, AllowMultiple= false)]
    public class DetailAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataView"></param>
        public DetailAttribute(DataView dataView)
        {
            DataView = dataView;
        }

        /// <summary>
        /// 数据查看方式
        /// </summary>
        public DataView DataView
        {
            get;
            private set;
        }
    }

 
    /// <summary>
    /// 子对象集合特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ChildrenAttribute : Attribute
    {
    }

    /// <summary>
    /// 单据明细特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class BillDetailAttribute : Attribute
    {

    }

    #endregion

    #region 显示


    /// <summary>
    /// 标签特性，用于显示字段在显示时的标签内容
    /// </summary>
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LabelAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        public LabelAttribute(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 标题文本
        /// </summary>
        public string Text
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 格式化显示特性
    /// 支持数据、日期
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FormatAttribute : Attribute
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatString">格式化字符串</param>
        public FormatAttribute(string formatString)
        {
            FormatString = formatString;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        public string FormatString
        {
            get;
            private set;
        }
    
    }

    /// <summary>
    /// 指定编辑控件
    /// 如果没有指定，在需要显示时则使用默认控件
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EditorControlAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="editorType">编辑控件类型</param>
        public EditorControlAttribute(Type editorType)
        {
            EditorType = editorType;
        }

        /// <summary>
        /// 编辑控件
        /// </summary>
        public Type EditorType
        {
            get;
            private set;
        }
    }

    #endregion

    #region 搜索

    /// <summary>
    /// 是否支持检索特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class QueryAttribute : Attribute
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryAttribute() : this("", null)
        {
        } 
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="queryString">关联的查询字符串HQL</param>
        public QueryAttribute(string queryString): this(queryString, null)
        {
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="queryString">关联的查询字符串HQL</param>
        /// <param name="parameters">HQL相应的参数</param>
        public QueryAttribute(string queryString, params object[] parameters)
        {
            QueryString = queryString;
            Parameters = parameters;
        }

        /// <summary>
        /// 相关查询HQL语句
        /// </summary>
        public string QueryString
        {
            get;
            private set;
        }

        /// <summary>
        /// 查询HQL相应的参数
        /// </summary>
        public object[] Parameters
        {
            get;
            private set;
        }
    }

    #endregion

    #region  数据源

    /// <summary>
    /// 数据源特性
    /// 指定属性的数据来源
    /// 如果是使用属性类型，则不需要指定此特性
    /// 场景：对象使用接口，可指定相应的实体类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DataSourceAttribute : Attribute
    {

        /// <summary>
        /// <param name="sourceType">源类型</param>
        /// 构造函数
        /// </summary>
        public DataSourceAttribute(Type sourceType)
            : this(sourceType, "", null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="queryString">关联的查询字符串HQL</param>
        public DataSourceAttribute(Type sourceType, string queryString)
            : this(sourceType, queryString, null)
        {
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceType">源类型</param>
        /// <param name="queryString">关联的查询字符串HQL,如果不使用，直接设置为null</param>
        /// <param name="parameters">HQL相应的参数</param>
        public DataSourceAttribute(Type sourceType, string queryString, params object[] parameters)
        {
            SourceType = sourceType;
            QueryString = queryString;
            Parameters = parameters;
        }

        /// <summary>
        /// 源类型
        /// </summary>
        public Type SourceType
        {
            get;
            private set;
        }

        /// <summary>
        /// 相关查询HQL语句
        /// </summary>
        public string QueryString
        {
            get;
            private set;
        }

        /// <summary>
        /// 查询HQL相应的参数
        /// </summary>
        public object[] Parameters
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 枚举数据源特性，指定属性的枚举数据源
    /// 场景：属性使用枚举，但选择时不需要从全部枚举中选择
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumListDataSource : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enumList">指定的枚举值列表，使用枚举名称或枚举值标识</param>
        public EnumListDataSource(params Enum[] enumList)
        {
        }
    }

    /// <summary>
    /// 自定义数据源
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomDataSource : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="invokeType">调用的类型</param>
        /// <param name="meodthName">调用的方法</param>
        /// <param name="parameters">方法参数
        /// 参数支持当前对象的属性，标识方法为{PropertyName}
        /// </param>
        public CustomDataSource(Type invokeType, string meodthName, params object[] parameters)
        {
            InvokeType = invokeType;
            MeodthName = meodthName;
            Parameters = parameters;
        }  
        
        /// <summary>
        /// 调用的类型
        /// </summary>
        public Type InvokeType
        {
            get;
            private set;
        }

        /// <summary>
        /// 调用的方法名称
        /// </summary>
        public string MeodthName
        {
            get;
            private set;
        }

        /// <summary>
        /// 调用方法的参数，必须与声明顺序一致
        /// </summary>
        public object[] Parameters
        {
            get;
            private set;
        }

    }

    #endregion

}
