using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace Gabsee
{
    public class ConnectionBroadcasts : MonoBehaviour
    {
        [SerializeField] Image flashImage;
        private Coroutine flashRoutine;
        private NetManager netManager;
        private bool isHost;

        public bool IsHost { get => isHost; set => isHost = value; }

        public void Init(NetManager netManager)
        {
            this.netManager = netManager;
            Subscribe();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            netManager.OnConnection += OnConnected;
            netManager.OnDisconnection += OnDisconnected;
            netManager.OnNetworkError += OnNetworkError;
            netManager.OnNetworkReceive += OnNetworkReceive;
        }

        private void UnSubscribe()
        {
            netManager.OnConnection -= OnConnected;
            netManager.OnDisconnection -= OnDisconnected;
            netManager.OnNetworkError -= OnNetworkError;
            netManager.OnNetworkReceive -= OnNetworkReceive;
        }

        private void OnConnected(IPEndPoint ipEndPoint)
        {
            if (IsHost)
            {
                Debug.Log($"A Player Connected");
            }
            else
            {
                Debug.Log($"Connected");
            }
        }

        private void OnDisconnected(IPEndPoint ipEndPoint)
        {
            Debug.Log($"A Player Disconnected");
        }

        private void OnNetworkError(IPEndPoint ipEndPoint, System.Net.Sockets.SocketError error)
        {
            Debug.Log($"OnNetworkError: {ipEndPoint}\n{error}");
        }

        private void OnNetworkReceive(IPEndPoint ipEndPoint, byte[] data)
        {
            var package = PackageOpener.OpenPackage(data);

            if(package != null)
            {
                if (IsHost)
                {
                    SendPackage(package);
                }
                else
                {
                    OpenPackage(package);
                }
            }
        }

        public void SendPackage(IPackage package)
        {
            var packedPackage = package.Pack();

            netManager.Send(packedPackage);
            if (IsHost)
            {
                OpenPackage(package);
            }
        }

        private void OpenPackage(IPackage package)
        {
            var flashPackage = package as FlashPackage;

            if (flashPackage != null)
            {
                OpenFlashPackage(flashPackage);
            }
        }

        private void OpenFlashPackage(FlashPackage flashPackage)
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
                flashRoutine = null;
            }
            flashRoutine = StartCoroutine(ActivateFlash(flashPackage));
        }

        private IEnumerator ActivateFlash(FlashPackage flash)
        {
            var alpha = 1f;
            var lifeTime = flash.LifeTime;
            while (alpha > 0)
            {
                alpha -= Time.deltaTime / lifeTime;
                flashImage.color = Color.Lerp(Color.clear, flash.Color, alpha);
                yield return null;
            }
        }
    }
}