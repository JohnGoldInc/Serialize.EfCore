﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Serialize.EfCore.Factories;
using Serialize.EfCore.Interfaces;
using Serialize.EfCore.Nodes;

namespace Serialize.EfCore.Serializers
{
    public class ExpressionSerializer : ExpressionConverter
    {
        private readonly ISerializer _serializer;
        private readonly FactorySettings _factorySettings;

        public ExpressionSerializer(ISerializer serializer, FactorySettings factorySettings = null)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _factorySettings = factorySettings;
        }

        public bool AutoAddKnownTypesAsArrayTypes
        {
            get => _serializer.AutoAddKnownTypesAsArrayTypes;
            set => _serializer.AutoAddKnownTypesAsArrayTypes = value;
        }

        public bool AutoAddKnownTypesAsListTypes
        {
            get => _serializer.AutoAddKnownTypesAsListTypes;
            set => _serializer.AutoAddKnownTypesAsListTypes = value;
        }

        public bool CanSerializeText => _serializer is ITextSerializer;

        public bool CanSerializeBinary => _serializer is IBinarySerializer;

        public void AddKnownType(Type type)
        {
            _serializer.AddKnownType(type);
        }

        public void AddKnownTypes(IEnumerable<Type> types)
        {
            _serializer.AddKnownTypes(types);
        }

        public void Serialize(Stream stream, Expression expression, FactorySettings factorySettings = null)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            _serializer.Serialize(stream, Convert(expression, factorySettings ?? _factorySettings));
        }

        public Expression Deserialize(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var node = _serializer.Deserialize<ExpressionNode>(stream);
            return node?.ToExpression();
        }

        public string SerializeText(Expression expression, FactorySettings factorySettings = null)
        {
            return TextSerializer.Serialize(Convert(expression, factorySettings ?? _factorySettings));
        }

        public Expression DeserializeText(string text)
        {
            var node = TextSerializer.Deserialize<ExpressionNode>(text);
            return node?.ToExpression();
        }

        public Expression DeserializeText(string text, IExpressionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var node = TextSerializer.Deserialize<ExpressionNode>(text);
            return node?.ToExpression(context);
        }

        public byte[] SerializeBinary(Expression expression, FactorySettings factorySettings = null)
        {
            return BinarySerializer.Serialize(Convert(expression, factorySettings ?? _factorySettings));
        }

        public Expression DeserializeBinary(byte[] bytes)
        {
            var node = BinarySerializer.Deserialize<ExpressionNode>(bytes);
            return node?.ToExpression();
        }

        public Expression DeserializeBinary(byte[] bytes, IExpressionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var node = BinarySerializer.Deserialize<ExpressionNode>(bytes);
            return node?.ToExpression(context);
        }

        private ITextSerializer TextSerializer
        {
            get
            {
                if (!(_serializer is ITextSerializer textSerializer))
                    throw new InvalidOperationException("Unable to serialize text.");
                return textSerializer;
            }
        }

        private IBinarySerializer BinarySerializer
        {
            get
            {
                if (!(_serializer is IBinarySerializer binarySerializer))
                    throw new InvalidOperationException("Unable to serialize binary.");
                return binarySerializer;
            }
        }
    }
}