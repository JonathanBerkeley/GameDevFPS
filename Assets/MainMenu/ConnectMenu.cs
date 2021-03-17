using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectMenu : MonoBehaviour
{
    void Update()
    {
        if (!Client.instance.GetClientConnected())
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if ((int)StaticError.CCT != -1)
            {
                GameManager.instance.ProcessClientMessage(StaticError.CCT);
                StaticError.CCT = (FlagTranslations.ClientCodeTranslations)(-1);
            }
            if ((int)StaticError.SCT != -1)
            {
                GameManager.instance.ProcessServerMessage(StaticError.SCT);
                StaticError.SCT = (FlagTranslations.ServerCodeTranslations)(-1);
            }
            
        }
    }
}
