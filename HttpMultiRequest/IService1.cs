using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace HttpMultiRequest
{
    [ServiceContract]
    public interface IService1
    {
        [WebInvoke(Method = "*", UriTemplate = "/NotificationPreference/", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        [CustomOperationBehaviour]
        ResponseObj NotificationPreference(ReqObj value);

        [WebInvoke(Method = "GET", UriTemplate = "/OtherPreference/{value}", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        ResponseObj OtherPreference(String value);
    }

}
