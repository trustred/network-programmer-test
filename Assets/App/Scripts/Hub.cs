using UnityEngine;
using UnityEngine.UI;

namespace Gabsee
{
    public class Hub : MonoBehaviour
    {
        [SerializeField] ConnectionBroadcasts broadcasts;
        [SerializeField] Button serverButton;
        [SerializeField] Button clientButton;
        [SerializeField] Button disconnectButton;
        [SerializeField] Button flashButton;

        private readonly int port = 8080;
        private NetManager netManager;

        private void Awake()
        {
            netManager = new NetManager();
            broadcasts.Init(netManager);
            SetListeners();
        }

        private void SetListeners()
        {
            serverButton.onClick.AddListener(StartServer);
            clientButton.onClick.AddListener(StartClient);
            disconnectButton.onClick.AddListener(Disconnect);
            flashButton.onClick.AddListener(SendFlash);
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            serverButton.onClick.RemoveListener(StartServer);
            clientButton.onClick.RemoveListener(StartClient);
            disconnectButton.onClick.RemoveListener(Disconnect);
            flashButton.onClick.RemoveListener(SendFlash);
        }

        private void StartServer()
        {
            netManager.StartServer(port);
            SetButtonInteracable(false);
            broadcasts.IsHost = true;
            Debug.Log($"Starting Server - Waiting for player");
        }

        private void StartClient()
        {
            netManager.StartClient("localhost", port);
            SetButtonInteracable(false);
            broadcasts.IsHost = false;
            Debug.Log($"Starting Client - Waiting for player");
        }

        private void Disconnect()
        {
            netManager.Stop();
            SetButtonInteracable(true);
            Debug.Log($"Disconnected");
        }

        private void SetButtonInteracable(bool interactable)
        {
            serverButton.interactable = interactable;
            clientButton.interactable = interactable;
            disconnectButton.interactable = !interactable;
        }

        private void SendFlash()
        {
            var color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
            var flash = new FlashPackage(PackageType.Flash, Random.Range(1,4), color);
            broadcasts.SendPackage(flash);
        }

        private void Update()
        {
            netManager.Update();
        }
    }
}