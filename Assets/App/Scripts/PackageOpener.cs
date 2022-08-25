namespace Gabsee
{
    public static class PackageOpener
    {
        public static IPackage OpenPackage(byte[] data)
        {
            var packageType = (PackageType)data[0];
            return packageType switch
            {
                PackageType.Flash => new FlashPackage(data),
                _ => null,
            };
        }
    }
}