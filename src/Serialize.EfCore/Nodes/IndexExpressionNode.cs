﻿using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "X")]
#endif
    [Serializable]
    #endregion
    public class IndexExpressionNode : ExpressionNode<IndexExpression>
    {
        public IndexExpressionNode() { }

        public IndexExpressionNode(INodeFactory factory, IndexExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "A")]
#endif
        #endregion
        public ExpressionNodeList Arguments { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "I")]
#endif
        #endregion
        public PropertyInfoNode Indexer { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "O")]
#endif
        #endregion
        public ExpressionNode Object { get; set; }

        protected override void Initialize(IndexExpression expression)
        {
            Arguments = new ExpressionNodeList(Factory, expression.Arguments);
            Indexer = new PropertyInfoNode(Factory, expression.Indexer);
            Object = Factory.Create(expression.Object);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.MakeIndex(
                Object.ToExpression(context), 
                Indexer.ToMemberInfo(context),
                Arguments.GetExpressions(context));
        }
    }
}
