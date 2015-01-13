using AssemblyReloader.Messages;

namespace AssemblyReloader.Tracking.Messages
{
    class DestroyAllTrackedObjects : IMessage
    {
        public bool InvokeDestructionEvent { get; private set; }

        public DestroyAllTrackedObjects(bool invokeDestructionEvent = false)
        {
            InvokeDestructionEvent = invokeDestructionEvent;
        }
    }
}
