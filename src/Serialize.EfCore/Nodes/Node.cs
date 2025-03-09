﻿using System;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    #region DataContract
    [DataContract]
    [Serializable]
    #region KnownTypes
    [KnownType(typeof(BinaryExpressionNode))]
    [KnownType(typeof(ConditionalExpressionNode))]
    [KnownType(typeof(ConstantExpressionNode))]
    [KnownType(typeof(ConstructorInfoNode))]
    [KnownType(typeof(DefaultExpressionNode))]
    [KnownType(typeof(ElementInitNode))]
    [KnownType(typeof(ElementInitNodeList))]
    [KnownType(typeof(ExpressionNode))]
    [KnownType(typeof(ExpressionNodeList))]
    [KnownType(typeof(FieldInfoNode))]
    [KnownType(typeof(IndexExpressionNode))]
    [KnownType(typeof(InvocationExpressionNode))]
    [KnownType(typeof(LambdaExpressionNode))]
    [KnownType(typeof(ListInitExpressionNode))]
    [KnownType(typeof(MemberAssignmentNode))]
    [KnownType(typeof(MemberBindingNode))]
    [KnownType(typeof(MemberBindingNodeList))]
    [KnownType(typeof(MemberExpressionNode))]
    [KnownType(typeof(MemberInfoNode))]
    [KnownType(typeof(MemberInfoNodeList))]    
    [KnownType(typeof(MemberInitExpressionNode))]
    [KnownType(typeof(MemberListBindingNode))]
    [KnownType(typeof(MemberMemberBindingNode))]
    [KnownType(typeof(MethodCallExpressionNode))]
    [KnownType(typeof(NewArrayExpressionNode))]
    [KnownType(typeof(NewExpressionNode))]
    [KnownType(typeof(ParameterExpressionNode))]
    [KnownType(typeof(PropertyInfoNode))]    
    [KnownType(typeof(TypeBinaryExpressionNode))]
    [KnownType(typeof(TypeNode))]
    [KnownType(typeof(UnaryExpressionNode))]
    [KnownType(typeof(EntityQueryRootExpressionNode))]
    #endregion
    #endregion
    public abstract class Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        protected Node() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <exception cref="System.ArgumentNullException">factory</exception>
        protected Node(INodeFactory factory)
        {
            Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>
        /// The factory.
        /// </value>
        [IgnoreDataMember]
        [NonSerialized]
        public readonly INodeFactory Factory;        
    }
}
