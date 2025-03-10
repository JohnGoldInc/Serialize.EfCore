﻿using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Serialize.EfCore.Factories;
using Serialize.EfCore.Interfaces;
using Serialize.EfCore.Internals;
using Serialize.EfCore.Nodes;

namespace Serialize.EfCore.Serializers
{
    public class ExpressionConverter
    {
        private readonly ExpressionCompressor _expressionCompressor;

        public ExpressionConverter()
        {
            _expressionCompressor = new ExpressionCompressor();
        }

        public ExpressionNode Convert(Expression expression, FactorySettings factorySettings = null)
        {
            expression = _expressionCompressor.Compress(expression);

            var factory = CreateFactory(expression, factorySettings);
            return factory.Create(expression);
        }

        protected virtual INodeFactory CreateFactory(Expression expression, FactorySettings factorySettings)
        {
            if (expression is LambdaExpression lambda)
                return new DefaultNodeFactory(lambda.Parameters.Select(p => p.Type), factorySettings);
            return new NodeFactory(factorySettings);
        }
    }
}
