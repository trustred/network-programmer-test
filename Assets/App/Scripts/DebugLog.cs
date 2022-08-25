using System.Text;
using TMPro;
using UnityEngine;

namespace Gabsee
{
    public class DebugLog : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI debugText;
        private StringBuilder log = new StringBuilder();

        private void Awake() => Application.logMessageReceived += LogMessageReceived;
        
        private void LogMessageReceived(string condition, string stackTrace, LogType type)
        {
            log.AppendLine($"{condition}");
            debugText.text = log.ToString();
        }
    }
}