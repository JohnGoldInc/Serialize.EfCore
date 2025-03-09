﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.EfCore.Extensions;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "L")]
#endif
    [Serializable]
    #endregion
    public class LambdaExpressionNode : ExpressionNode<LambdaExpression>
    {
        public LambdaExpressionNode() { }

        public LambdaExpressionNode(INodeFactory factory, LambdaExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "B")]
#endif
        #endregion
        public ExpressionNode Body { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "P")]
#endif
        #endregion
        public ExpressionNodeList Parameters { get; set; }

        protected override void Initialize(LambdaExpression expression)
        {
            Parameters = new ExpressionNodeList(Factory, expression.Parameters);
            Body = Factory.Create(expression.Body);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            var body = Body.ToExpression(context);
            var parameters = Parameters.GetParameterExpressions(context).ToArray();

            var bodyParameters = body.GetNodes().OfType<ParameterExpression>().ToArray();
            for (var i = 0; i < parameters.Length; ++i)
            {
                var matchingParameter = bodyParameters.Where(p => p.Name == parameters[i].Name && p.Type == parameters[i].Type).ToArray();
                if (matchingParameter.Length == 1)
                    parameters[i] = matchingParameter.First();
            }

            return Expression.Lambda(Type.ToType(context), body, parameters);
        }
    }
}
