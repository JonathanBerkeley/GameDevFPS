using UnityEngine;

public static class StaticInput
{
    private static float horizontal = 0.0f;
    private static float vertical = 0.0f;
    private static bool jumping = false;
    private static bool shooting = false;
    private static bool pause = false;
    private static bool chatDown = false;
    
    private static bool inputSuspended = false;

    /// <summary>
    /// Updates all the player input values
    /// </summary>
    public static void UpdatePlayerValues()
    {
        if (!inputSuspended)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            jumping = Input.GetButton("Jump");
            shooting = Input.GetButtonDown("Fire1");
        }
    }

    /// <summary>
    /// Updates the horizontal value only
    /// </summary>
    public static void UpdateHorizontal()
    {
        if (!inputSuspended)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
        }
    }

    /// <summary>
    /// Updates the vertical value only
    /// </summary>
    public static void UpdateVertical()
    {
        if (!inputSuspended)
        {
            vertical = Input.GetAxisRaw("Vertical");
        }
    }

    /// <summary>
    /// Updates the jumping value only
    /// </summary>
    public static void UpdateJumping()
    {
        if (!inputSuspended)
        {
            jumping = Input.GetButton("Jump");
        }
    }

    /// <summary>
    /// Updates the shooting value only
    /// </summary>
    public static void UpdateShooting()
    {
        if (!inputSuspended)
        {
            shooting = Input.GetButtonDown("Fire1");
        }
    }

    public static void UpdatePauseCheck()
    {
        if (!inputSuspended)
        {
            pause = Input.GetButtonDown("Cancel");
        }
    }


    //Chat keys below for multiplayer chat
    public static void UpdateChatCheck()
    {
        chatDown = Input.GetButtonDown("Chat");

        if (inputSuspended)
        {
            if (Input.GetButtonDown("ChatSubmit"))
            {
                MultiplayerChat.instance.SubmitText();
            }
            if (Input.GetButtonDown("Cancel"))
            {
                MultiplayerChat.instance.DisableChat();
            }
        }
    }


    /// <summary>
    /// Gets the last saved horizontal value
    /// </summary>
    /// <returns> Float - Horizontal value </returns>
    public static float GetHorizontal()
    {
        return horizontal;
    }

    /// <summary>
    /// Gets the last saved vertical value
    /// </summary>
    /// <returns> Float - Vertical value </returns>
    public static float GetVertical()
    {
        return vertical;
    }

    /// <summary>
    /// Gets the last saved jumping value
    /// </summary>
    /// <returns> Boolean - Jumping input </returns>
    public static bool GetJumping()
    {
        return jumping;
    }

    /// <summary>
    /// Gets the last saved shooting value
    /// </summary>
    /// <returns> Boolean - Shooting input </returns>
    public static bool GetShooting()
    {
        return shooting;
    }

    /// <summary>
    /// Returns if player paused
    /// </summary>
    /// <returns> Boolean - Pause attempt from player </returns>
    public static bool GetPaused()
    {
        return pause;
    }

    public static bool GetChatDown()
    {
        return chatDown;
    }

    public static void SuspendInput(bool _option)
    {
        inputSuspended = _option;
    }
}
