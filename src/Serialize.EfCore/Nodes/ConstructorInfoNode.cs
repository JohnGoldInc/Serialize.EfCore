﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    #region DataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [DataContract]
#else
    [DataContract(Name = "CI")]
#endif
    [Serializable]
    #endregion
    public class ConstructorInfoNode : MemberNode<ConstructorInfo>
    {
        public ConstructorInfoNode() { }

        public ConstructorInfoNode(INodeFactory factory, ConstructorInfo memberInfo)
            : base(factory, memberInfo) { }

        /// <summary>
        /// Gets the member infos for the specified type.
        /// </summary>
        /// <param name="context">The expression context.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        protected override IEnumerable<ConstructorInfo> GetMemberInfosForType(IExpressionContext context, Type type)
        {
            return type.GetConstructors();
        }
    }
}