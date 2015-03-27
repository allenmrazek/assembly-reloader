namespace AssemblyReloader.Game
{
    public interface IPartActionWindowController
    {
        void Close();
        void Add(UIPartActionWindow window);
        void Remove(UIPartActionWindow window);
        void Refresh();
    }
}
