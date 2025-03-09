﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Serialize.EfCore.Interfaces;

namespace Serialize.EfCore.Nodes
{
    #region CollectionDataContract
#if !SERIALIZE_LINQ_OPTIMIZE_SIZE
    [CollectionDataContract]
#else
    [CollectionDataContract(Name = "EIL")]
#endif
    [Serializable]
    #endregion
    public class ElementInitNodeList : List<ElementInitNode>
    {
        public ElementInitNodeList() { }

        public ElementInitNodeList(INodeFactory factory, IEnumerable<ElementInit> items)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            AddRange(items.Select(item => new ElementInitNode(factory, item)));
        }

        internal IEnumerable<ElementInit> GetElementInits(IExpressionContext context)
        {
            return this.Select(item => item.ToElementInit(context));
        }
    }
}
