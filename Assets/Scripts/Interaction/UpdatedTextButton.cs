using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Interaction
{
    public class UpdatedTextButton : MonoBehaviour
    {
        public Button button;

        private string deviceType;

        void Update()
        {
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            
            deviceType = PlayerPrefs.GetString("DeviceType", "Pc");
            
            if (deviceType == "Mobile")
            {
                buttonText.text = "INTERAGIR";
                button.interactable = true;
            }
            else
            {
                buttonText.text = "PRESSIONE ESPAÇO";
                button.interactable = false;
            }
        }
    }
}