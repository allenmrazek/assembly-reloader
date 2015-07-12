using System.Reflection;

namespace AssemblyReloader.Commands
{
    public class AwakenPartModuleCommand : ICommand<PartModule>
    {
        public void Execute(PartModule context)
        {
            var method = typeof(PartModule).GetMethod("Awake",
                 BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            if (method != null)
                method.Invoke(context, null);
        }
    }
}
