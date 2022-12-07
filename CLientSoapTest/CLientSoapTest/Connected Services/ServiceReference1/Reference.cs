﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CLientSoapTest.ServiceReference1 {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/ServeurSoapBiking")]
    [System.SerializableAttribute()]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BoolValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StringValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool BoolValue {
            get {
                return this.BoolValueField;
            }
            set {
                if ((this.BoolValueField.Equals(value) != true)) {
                    this.BoolValueField = value;
                    this.RaisePropertyChanged("BoolValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue {
            get {
                return this.StringValueField;
            }
            set {
                if ((object.ReferenceEquals(this.StringValueField, value) != true)) {
                    this.StringValueField = value;
                    this.RaisePropertyChanged("StringValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IServiceBiking")]
    public interface IServiceBiking {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceBiking/getRoute", ReplyAction="http://tempuri.org/IServiceBiking/getRouteResponse")]
        string getRoute(string departure, string arrival);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceBiking/getRoute", ReplyAction="http://tempuri.org/IServiceBiking/getRouteResponse")]
        System.Threading.Tasks.Task<string> getRouteAsync(string departure, string arrival);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceBiking/NextStep", ReplyAction="http://tempuri.org/IServiceBiking/NextStepResponse")]
        bool NextStep(string queue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceBiking/NextStep", ReplyAction="http://tempuri.org/IServiceBiking/NextStepResponse")]
        System.Threading.Tasks.Task<bool> NextStepAsync(string queue);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceBiking/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IServiceBiking/GetDataUsingDataContractResponse")]
        CLientSoapTest.ServiceReference1.CompositeType GetDataUsingDataContract(CLientSoapTest.ServiceReference1.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceBiking/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IServiceBiking/GetDataUsingDataContractResponse")]
        System.Threading.Tasks.Task<CLientSoapTest.ServiceReference1.CompositeType> GetDataUsingDataContractAsync(CLientSoapTest.ServiceReference1.CompositeType composite);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceBikingChannel : CLientSoapTest.ServiceReference1.IServiceBiking, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceBikingClient : System.ServiceModel.ClientBase<CLientSoapTest.ServiceReference1.IServiceBiking>, CLientSoapTest.ServiceReference1.IServiceBiking {
        
        public ServiceBikingClient() {
        }
        
        public ServiceBikingClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceBikingClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceBikingClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceBikingClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string getRoute(string departure, string arrival) {
            return base.Channel.getRoute(departure, arrival);
        }
        
        public System.Threading.Tasks.Task<string> getRouteAsync(string departure, string arrival) {
            return base.Channel.getRouteAsync(departure, arrival);
        }
        
        public bool NextStep(string queue) {
            return base.Channel.NextStep(queue);
        }
        
        public System.Threading.Tasks.Task<bool> NextStepAsync(string queue) {
            return base.Channel.NextStepAsync(queue);
        }
        
        public CLientSoapTest.ServiceReference1.CompositeType GetDataUsingDataContract(CLientSoapTest.ServiceReference1.CompositeType composite) {
            return base.Channel.GetDataUsingDataContract(composite);
        }
        
        public System.Threading.Tasks.Task<CLientSoapTest.ServiceReference1.CompositeType> GetDataUsingDataContractAsync(CLientSoapTest.ServiceReference1.CompositeType composite) {
            return base.Channel.GetDataUsingDataContractAsync(composite);
        }
    }
}
