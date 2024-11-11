using UnityEngine;

namespace Dialog.DefinitionSoftware
{
    public class WhatAreYouSoftware : MonoBehaviour
    {
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                PlayerPrefs.SetString("DeviceType", "Mobile");
            } 
            else if (Input.anyKey)
            {
                PlayerPrefs.SetString("DeviceType", "Pc");
            }
            
            PlayerPrefs.Save();
        }
    }
}