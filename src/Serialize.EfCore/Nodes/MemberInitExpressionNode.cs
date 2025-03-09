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
    [DataContract(Name = "MIE")]
#endif
    [Serializable]
    #endregion
    public class MemberInitExpressionNode : ExpressionNode<MemberInitExpression>
    {
        public MemberInitExpressionNode() { }

        public MemberInitExpressionNode(INodeFactory factory, MemberInitExpression expression)
            : base(factory, expression) { }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "B")]
#endif
        #endregion
        public MemberBindingNodeList Bindings { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "N")]
#endif
        #endregion
        public NewExpressionNode NewExpression { get; set; }

        protected override void Initialize(MemberInitExpression expression)
        {
            Bindings = new MemberBindingNodeList(Factory, expression.Bindings);
            NewExpression = (NewExpressionNode)Factory.Create(expression.NewExpression);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            return Expression.MemberInit((NewExpression)NewExpression.ToExpression(context), Bindings.GetMemberBindings(context));
        }
    }
}
