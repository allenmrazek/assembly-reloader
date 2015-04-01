namespace AssemblyReloader.TypeInstallers
{
    public interface ITypeInstaller
    {
        void Install(AssemblyLoader.LoadedAssembly assembly);
    }
}
