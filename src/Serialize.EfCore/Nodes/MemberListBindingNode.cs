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
    [DataContract(Name = "MLB")]
#endif
    [Serializable]
    #endregion
    public class MemberListBindingNode : MemberBindingNode
    {
        public MemberListBindingNode() { }

        public MemberListBindingNode(INodeFactory factory, MemberListBinding memberListBinding)
            : base(factory, memberListBinding.BindingType, memberListBinding.Member)
        {
            Initializers = new ElementInitNodeList(Factory, memberListBinding.Initializers);
        }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "I")]
#endif
        #endregion
        public ElementInitNodeList Initializers { get; set; }

        internal override MemberBinding ToMemberBinding(IExpressionContext context)
        {
            return Expression.ListBind(Member.ToMemberInfo(context), Initializers.GetElementInits(context));
        }
    }
}
