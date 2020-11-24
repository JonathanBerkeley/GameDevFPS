using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jonathan's Input manager
//N00181859
public class InputManager
{
    public float horizontal;
    public float vertical;
    public bool jumping;

    private int InputManagers = 0;
    public InputManager()
    {
        //Initial
        horizontal = 0.0f;
        vertical = 0.0f;
        jumping = false;

        //For checking InputManagers count in existence
        ++this.InputManagers;
    }

    /// <summary>
    /// Updates all the player input values
    /// </summary>
    public void UpdatePlayerValues()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
    }

    /// <summary>
    /// Updates the horizontal value only
    /// </summary>
    public void UpdateHorizontal()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }

    /// <summary>
    /// Updates the vertical value only
    /// </summary>
    public void UpdateVertical()
    {
        vertical = Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Gets the last saved horizontal value
    /// </summary>
    /// <returns> Float - Horizontal value </returns>
    public float GetHorizontal()
    {
        return this.horizontal;
    }

    /// <summary>
    /// Gets the last saved vertical value
    /// </summary>
    /// <returns> Float - Vertical value </returns>
    public float GetVertical()
    {
        return this.vertical;
    }

    /// <summary>
    /// Gets the last saved jumping value
    /// </summary>
    /// <returns> Boolean - Jumping input </returns>
    public bool GetJumping()
    {
        return this.jumping;
    }

    /// <summary>
    /// For debugging purposes only, returns count of class instantiations
    /// </summary>
    /// <returns> Int - Number of input managers instantiated </returns>
    public int DebugGetInputManagerCount()
    {
        return this.InputManagers;
    }
}
