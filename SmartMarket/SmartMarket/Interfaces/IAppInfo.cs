namespace SmartMarket.Interfaces
{
    public interface IAppInfo
    {
        string PackageName { get; }

        string Name { get; }

        string VersionString { get; }

        //Version Version { get; }

        string VersionCode { get; }

        void ShowSettingsUI();
    }
}
