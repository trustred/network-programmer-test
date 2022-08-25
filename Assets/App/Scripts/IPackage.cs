namespace Gabsee
{
    public interface IPackage
    {
        PackageType PackageType { get; }
        byte[] Pack();
    }
}