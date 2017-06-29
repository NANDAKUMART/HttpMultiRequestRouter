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
    [DataContract]
    public class ReqObj
    {
        [DataMember]
        public string Details { get; set; }
    }

    [DataContract]
    public class ResponseObj
    {
        [DataMember]
        public string name { get; set; }
    }

    public class Service1 : IService1
    {
        public ResponseObj NotificationPreference(ReqObj value)
        {
            return null;
        }

        public ResponseObj GetNotificationPreference(ReqObj value)
        {
            return new ResponseObj() { name = "Rerouted GetNotificationPreference Call- " + value.Details };
        }

        public ResponseObj PostNotificationPreference(ReqObj value)
        {
            return new ResponseObj() { name = "Rerouted PostNotificationPreference Call- " + value.Details };
        }

        public ResponseObj OtherPreference(string value)
        {
            return new ResponseObj() { name = "Rerouted OtherPreference Call.1- " + value };
        }
    }
}
