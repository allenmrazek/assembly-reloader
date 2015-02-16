using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using ReeperCommon.Extensions;

namespace AssemblyReloaderUnitTests.Fixture
{
    class Fixture_TestAssembly
    {
        private readonly AssemblyDefinition _assemblyDefinition;

        public Fixture_TestAssembly(string filename = "TestProject.dll")
        {
            filename = filename.TrimStart('/', '\\');

            filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + filename;

            if (!System.IO.File.Exists(filename))
                throw new FileNotFoundException(filename);

            _assemblyDefinition = AssemblyDefinition.ReadAssembly(filename);
        }


        public AssemblyDefinition Definition
        {
            get { return _assemblyDefinition; }
        }
    }
}
