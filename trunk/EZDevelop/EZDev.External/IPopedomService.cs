using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using EZDev.Data;
using EZDev.Data.Popedom;
using EZDev.Data.QueryModel;

namespace EZDev.External
{
    [ServiceContract]
    public interface IPopedomService
    {
        #region 用户管理
        [OperationContract]
        User GetUser(Guid id);

        [OperationContract]
        User GetUser(string loginName, string password);

        User[] GetUsers();

        //User[] GetUsers(WhereList where);

        //User[] GetUsers(WhereList where, OrderByList orderBy);

        #endregion

        #region 模块管理

        #endregion

        #region 角色管理

        #endregion

        #region 权限管理

        #endregion
    }
}
