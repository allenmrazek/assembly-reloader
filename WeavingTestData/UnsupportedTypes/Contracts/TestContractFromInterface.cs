using System;
using Contracts;

namespace WeavingTestData.UnsupportedTypes.Contracts
{
    public class TestContractFromInterface : IContractParameterHost
    {
        public ContractParameter AddParameter(ContractParameter parameter, string id = null)
        {
            throw new NotImplementedException();
        }

        public ContractParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        public ContractParameter GetParameter(string id)
        {
            throw new NotImplementedException();
        }

        public ContractParameter GetParameter(Type type)
        {
            throw new NotImplementedException();
        }

        public T GetParameter<T>(string id = null) where T : ContractParameter
        {
            throw new NotImplementedException();
        }

        public void RemoveParameter(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveParameter(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveParameter(Type type)
        {
            throw new NotImplementedException();
        }

        public void RemoveParameter(ContractParameter parameter)
        {
            throw new NotImplementedException();
        }

        public void ParameterStateUpdate(ContractParameter p)
        {
            throw new NotImplementedException();
        }

        public string Title { get; private set; }
        public IContractParameterHost Parent { get; private set; }
        public Contract Root { get; private set; }
        public int ParameterCount { get; private set; }

        ContractParameter IContractParameterHost.this[int index]
        {
            get { throw new NotImplementedException(); }
        }

        ContractParameter IContractParameterHost.this[string id]
        {
            get { throw new NotImplementedException(); }
        }

        ContractParameter IContractParameterHost.this[Type type]
        {
            get { throw new NotImplementedException(); }
        }
    }
}