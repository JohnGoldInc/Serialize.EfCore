using System;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Serializers
{
    public class XmlSerializer : TextSerializer, IXmlSerializer
    {
        protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
        {
            return new DataContractSerializer(type, GetKnownTypes());
        }
    }
}