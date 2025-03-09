﻿using System;
using System.IO;
using Serialize.EfCore.Nodes;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Serializers
{
    public abstract class DataSerializer : SerializerBase, ISerializer
    {
        public virtual void Serialize<T>(Stream stream, T obj) where T : Node
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var serializer = CreateXmlObjectSerializer(typeof(T));
            serializer.WriteObject(stream, obj);
        }

        public virtual T Deserialize<T>(Stream stream) where T : Node
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var serializer = CreateXmlObjectSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        protected abstract XmlObjectSerializer CreateXmlObjectSerializer(Type type);
    }
}
