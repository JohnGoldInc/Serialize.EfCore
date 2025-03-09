using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Serializers
{
    public class JsonSerializer : TextSerializer, IJsonSerializer
    {
        protected override XmlObjectSerializer CreateXmlObjectSerializer(Type type)
        {
            return new DataContractJsonSerializer(type, GetKnownTypes());
        }
    }
}