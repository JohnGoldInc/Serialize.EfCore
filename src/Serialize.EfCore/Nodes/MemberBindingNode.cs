﻿using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "MB")]
#endif
    [Serializable]
    #endregion
    public abstract class MemberBindingNode : Node
    {
        protected MemberBindingNode() { }

        protected MemberBindingNode(INodeFactory factory)
            : base(factory) { }

        protected MemberBindingNode(INodeFactory factory, MemberBindingType bindingType, MemberInfo memberInfo)
            : base(factory)
        {
            BindingType = bindingType;
            Member = new MemberInfoNode(Factory, memberInfo);
        }
        
        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "BT")]
#endif
        #endregion
        public MemberBindingType BindingType { get; set; }

        #region DataMember
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
        [DataMember(EmitDefaultValue = false)]
#else
        [DataMember(EmitDefaultValue = false, Name = "M")]
#endif
        #endregion
        public MemberInfoNode Member { get; set; }

        internal abstract MemberBinding ToMemberBinding(IExpressionContext context);

        internal static MemberBindingNode Create(INodeFactory factory, MemberBinding memberBinding)
        {
            MemberBindingNode memberBindingNode = null;

            if (memberBinding is MemberAssignment)
                memberBindingNode = new MemberAssignmentNode(factory, (MemberAssignment)memberBinding);
            else if (memberBinding is MemberListBinding)
                memberBindingNode = new MemberListBindingNode(factory, (MemberListBinding)memberBinding);
            else if (memberBinding is MemberMemberBinding)
                memberBindingNode = new MemberMemberBindingNode(factory, (MemberMemberBinding)memberBinding);
            else if (memberBinding != null)
                throw new ArgumentException("Unknown member binding of type " + memberBinding.GetType(), nameof(memberBinding));

            return memberBindingNode;
        }
    }
}
