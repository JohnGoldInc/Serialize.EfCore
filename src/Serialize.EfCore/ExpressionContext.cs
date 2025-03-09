using Serialize.EfCore.Interfaces;
using Serialize.EfCore.Internals;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Serialize.EfCore
{
    public class ExpressionContext : ExpressionContextBase
    {
        private readonly IAssemblyLoader _assemblyLoader;

        public ExpressionContext()
            : this(new DefaultAssemblyLoader()) { }

        public ExpressionContext(IAssemblyLoader assemblyLoader)
        {
            _assemblyLoader = assemblyLoader 
                ?? throw new ArgumentNullException(nameof(assemblyLoader));
        }

        protected override IEnumerable<Assembly> GetAssemblies()
        {
            return _assemblyLoader.GetAssemblies();
        }
    }
}