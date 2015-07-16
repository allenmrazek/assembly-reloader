using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public class GetMethodDefinitionsExcluding : IGetMethodDefinitions
    {
        private readonly IGetMethodDefinitions _toExcludeFrom;
        private readonly IGetMethodDefinitions _definitionsesToExclude;

        public GetMethodDefinitionsExcluding(IGetMethodDefinitions toExcludeFrom, IGetMethodDefinitions definitionsesToExclude)
        {
            if (toExcludeFrom == null) throw new ArgumentNullException("toExcludeFrom");
            if (definitionsesToExclude == null) throw new ArgumentNullException("definitionsesToExclude");

            _toExcludeFrom = toExcludeFrom;
            _definitionsesToExclude = definitionsesToExclude;
        }


        public IEnumerable<MethodDefinition> Get(TypeDefinition definition)
        {
            return _toExcludeFrom.Get(definition).Except(_definitionsesToExclude.Get(definition));
        }
    }
}
