extern alias KSP;
using System;
using strange.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
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
