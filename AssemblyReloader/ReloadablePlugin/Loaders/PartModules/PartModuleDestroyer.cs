extern alias KSP;
using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IPartModuleDestroyer))]
    public class PartModuleDestroyer : IPartModuleDestroyer
    {
        private readonly SignalAboutToDestroyMonoBehaviour _mbDestructionSignal;


        public PartModuleDestroyer(SignalAboutToDestroyMonoBehaviour mbDestructionSignal)
        {
            if (mbDestructionSignal == null) throw new ArgumentNullException("mbDestructionSignal");

            _mbDestructionSignal = mbDestructionSignal;
        }


        public void Destroy(IPart owner, KSP::PartModule target)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            if (target == null) throw new ArgumentNullException("target");

            _mbDestructionSignal.Dispatch(target);

            owner.RemoveModule(target);
        }
    }
}
