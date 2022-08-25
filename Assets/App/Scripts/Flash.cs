using System.Collections.Generic;
using UnityEngine;

namespace Gabsee
{
    public class FlashPackage : IPackage
    {
        private float lifeTime;
        private Color32 color;
        private PackageType packageType;

        public FlashPackage(PackageType packageType, float lifeTime, Color color)
        {
            this.packageType = packageType;
            this.lifeTime = lifeTime;
            this.color = color;
        }
        public FlashPackage(byte[] package)
        {
            packageType = (PackageType)package[0];
            lifeTime = package[1];
            color = new Color32(package[2], package[3], package[4], 255);
        }
        public byte[] Pack()
        {
            List<byte> package = new List<byte>();
            package.Add((byte)packageType);
            package.Add((byte)lifeTime);
            package.Add(color.r);
            package.Add(color.g);
            package.Add(color.b);

            return package.ToArray();
        }

        public float LifeTime => lifeTime;
        public Color32 Color => color;
        public PackageType PackageType => packageType;
    }
}