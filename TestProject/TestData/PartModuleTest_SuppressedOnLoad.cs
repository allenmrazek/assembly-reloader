using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject.TestData
{
    class PartModuleTest_SuppressedOnLoad : PartModule
    {
        // intentionally hides correct OnLoad for this **INCORRECT** version to try and trick
        // Art's PartModule method definition query
        new public void OnLoad(ConfigNode node)
        {
            
        }
    }
}
