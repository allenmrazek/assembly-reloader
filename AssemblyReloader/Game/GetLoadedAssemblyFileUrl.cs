using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGetLoadedAssemblyFileUrl))]
    // for some reason, the DLL urls in ksp's loaded assembly list consists of only the directory name
    // it's best to emulate that instead of fixing it for reloaded plugins in case third party plugins
    // rely those urls for any reason
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetLoadedAssemblyFileUrl : IGetLoadedAssemblyFileUrl
    {
        public string Get(IFile assemblyLocation)
        {
            new DebugLog("GetLoadedAssemblyFileUrl").Normal("Url of " + assemblyLocation.FileName + " is " +
                                                              assemblyLocation.Directory.Url);

            return assemblyLocation.Directory.Url;
        }
    }
}
