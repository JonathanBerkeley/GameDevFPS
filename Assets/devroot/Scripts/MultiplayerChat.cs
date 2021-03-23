using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MultiplayerChat : MonoBehaviour
{
    public static MultiplayerChat instance;

    public GameObject chatbox;
    public Text textHandle;
    public static bool state = true;

    private InputField entryBox;
    

    private void Awake()
    {
        //Make this a singleton
        if (instance == null)
        {
            instance = this;
        } 
        else if (instance != this)
        {
            Debug.LogError("MultiplayerChat tried to reinstantiate");
            Destroy(this);
        }

        chatbox.SetActive(false);
        entryBox = chatbox.GetComponent<InputField>();
    }

    void Update()
    {
        StaticInput.UpdateChatCheck();
        if (StaticInput.GetChatDown())
        {
            if (state)
            {
                EnableChat();
            }
            else
            {
                DisableChat();
            }
            state = !state;
        }
    }

    public void EnableChat()
    {
        chatbox.SetActive(true);
        StaticInput.SuspendInput(true);
        entryBox.ActivateInputField();
    }

    public void DisableChat()
    {
        entryBox.DeactivateInputField();
        chatbox.SetActive(false);
        StaticInput.SuspendInput(false);
    }

    public void SubmitText()
    {
        string enteredText = textHandle.text;
        if (enteredText.Length < 100)
        {
            ClientSend.ClientChatData(enteredText);
        }
        entryBox.text = "";
        state = true;
        DisableChat();
    }

}
