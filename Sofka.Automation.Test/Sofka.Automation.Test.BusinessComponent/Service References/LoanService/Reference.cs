﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sofka.Automation.Test.BusinessComponent.LoanService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoanRequestRequest", Namespace="http://schemas.datacontract.org/2004/07/Sofka.Automation.Dummy.Entities.Loan")]
    [System.SerializableAttribute()]
    public partial class LoanRequestRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmmountRequestedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Sofka.Automation.Test.BusinessComponent.LoanService.LoanType TypeField;
        
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
        public decimal AmmountRequested {
            get {
                return this.AmmountRequestedField;
            }
            set {
                if ((this.AmmountRequestedField.Equals(value) != true)) {
                    this.AmmountRequestedField = value;
                    this.RaisePropertyChanged("AmmountRequested");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Sofka.Automation.Test.BusinessComponent.LoanService.LoanType Type {
            get {
                return this.TypeField;
            }
            set {
                if ((this.TypeField.Equals(value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoanType", Namespace="http://schemas.datacontract.org/2004/07/Sofka.Automation.Dummy.Entities.Enumerati" +
        "ons")]
    public enum LoanType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Car = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Mortgages = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Appliance = 2,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoanRequestResponse", Namespace="http://schemas.datacontract.org/2004/07/Sofka.Automation.Dummy.Entities.Loan")]
    [System.SerializableAttribute()]
    public partial class LoanRequestResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmmountApprovedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmmountRequestedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ApprovedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ControlRiskStateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
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
        public decimal AmmountApproved {
            get {
                return this.AmmountApprovedField;
            }
            set {
                if ((this.AmmountApprovedField.Equals(value) != true)) {
                    this.AmmountApprovedField = value;
                    this.RaisePropertyChanged("AmmountApproved");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal AmmountRequested {
            get {
                return this.AmmountRequestedField;
            }
            set {
                if ((this.AmmountRequestedField.Equals(value) != true)) {
                    this.AmmountRequestedField = value;
                    this.RaisePropertyChanged("AmmountRequested");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Approved {
            get {
                return this.ApprovedField;
            }
            set {
                if ((this.ApprovedField.Equals(value) != true)) {
                    this.ApprovedField = value;
                    this.RaisePropertyChanged("Approved");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string ControlRiskState {
            get {
                return this.ControlRiskStateField;
            }
            set {
                if ((object.ReferenceEquals(this.ControlRiskStateField, value) != true)) {
                    this.ControlRiskStateField = value;
                    this.RaisePropertyChanged("ControlRiskState");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage {
            get {
                return this.ErrorMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true)) {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="LoanService.ILoan")]
    public interface ILoan {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoan/LoanRequest", ReplyAction="http://tempuri.org/ILoan/LoanRequestResponse")]
        Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestResponse LoanRequest(Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILoan/LoanRequest", ReplyAction="http://tempuri.org/ILoan/LoanRequestResponse")]
        System.Threading.Tasks.Task<Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestResponse> LoanRequestAsync(Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestRequest request);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILoanChannel : Sofka.Automation.Test.BusinessComponent.LoanService.ILoan, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LoanClient : System.ServiceModel.ClientBase<Sofka.Automation.Test.BusinessComponent.LoanService.ILoan>, Sofka.Automation.Test.BusinessComponent.LoanService.ILoan {
        
        public LoanClient() {
        }
        
        public LoanClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LoanClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoanClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LoanClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestResponse LoanRequest(Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestRequest request) {
            return base.Channel.LoanRequest(request);
        }
        
        public System.Threading.Tasks.Task<Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestResponse> LoanRequestAsync(Sofka.Automation.Test.BusinessComponent.LoanService.LoanRequestRequest request) {
            return base.Channel.LoanRequestAsync(request);
        }
    }
}
