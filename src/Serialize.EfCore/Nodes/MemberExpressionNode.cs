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
    [DataContract(Name = "M")]
#endif
    [Serializable]
    #endregion
    public class MemberExpressionNode : ExpressionNode<MemberExpression>
    {
        public MemberExpressionNode() { }

        public MemberExpressionNode(INodeFactory factory, MemberExpression expression)
            : base(factory, expression) { }

        #region DataMember
        #if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "E")]
#endif
        #endregion
        public ExpressionNode Expression { get; set; }

        #region DataMember
        #if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MemberInfoNode Member { get; set; }

        protected override void Initialize(MemberExpression expression)
        {
            Expression = Factory.Create(expression.Expression);
            Member = new MemberInfoNode(Factory, expression.Member);
        }

        public override Expression ToExpression(IExpressionContext context)
        {
            var member = Member.ToMemberInfo(context);
            return System.Linq.Expressions.Expression.MakeMemberAccess(Expression?.ToExpression(context), member);
        }
    }
}
