using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceModel.Web;

namespace HttpMultiRequest
{
    public class CustomOperaionInvoker<T> : IOperationInvoker
    {
        private IOperationInvoker invoker = null;
        private MethodInfo exeMethod = null;
        private OperationDescription opsDesc = null;

        public CustomOperaionInvoker(MethodInfo targetmethod, IOperationInvoker invoker, OperationDescription operationDescription) 
        {
            this.invoker = invoker;
            opsDesc = operationDescription;
        }

        public object[] AllocateInputs()
        {
            return this.invoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            outputs = new object[] { };
            object result = null;
            var targetType = instance.GetType();

            if (WebOperationContext.Current.IncomingRequest.Method == "GET")
            {
                exeMethod = targetType.GetMethod("Get" + opsDesc.SyncMethod.Name, opsDesc.SyncMethod.GetParameters().Select(mi => mi.ParameterType).ToArray());
            }
            else if (WebOperationContext.Current.IncomingRequest.Method == "POST")
            {
                exeMethod = targetType.GetMethod("Post" + opsDesc.SyncMethod.Name, opsDesc.SyncMethod.GetParameters().Select(mi => mi.ParameterType).ToArray());
            }
            else if (WebOperationContext.Current.IncomingRequest.Method == "DELETE")
            {
                exeMethod = targetType.GetMethod("Delete" + opsDesc.SyncMethod.Name, opsDesc.SyncMethod.GetParameters().Select(mi => mi.ParameterType).ToArray());
            }

            if (exeMethod != null)
            {                
                try
                {
                    result = exeMethod.Invoke(instance, inputs);
                }
                catch (Exception exception)
                {
                    throw new FaultException(new FaultReason(exception.InnerException.Message));
                }
            }
            else
            {
                throw new FaultException(new FaultReason("Method Not implemented for this Http Verb"));
            }
            return result;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }

    public class CustomOperationBehaviour : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            var openType = typeof(CustomOperaionInvoker<>);
            var closedType = openType.MakeGenericType(new Type[] { operationDescription.SyncMethod.ReturnType });
            dispatchOperation.Invoker = Activator.CreateInstance(closedType, null, dispatchOperation.Invoker, operationDescription) as IOperationInvoker;
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }

    //public class CustomServiceBehaviour : Attribute, IServiceBehavior
    //{
    //    public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
    //    {
    //    }

    //    public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    //    {
    //        foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
    //        {
    //            foreach (OperationDescription operation in endpoint.Contract.Operations)
    //            {
    //                IOperationBehavior behavior = new CustomOperationBehaviour();
    //                operation.Behaviors.Add(behavior);
    //            }
    //        }
    //    }

    //    public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
    //    {
    //    }
    //}

}