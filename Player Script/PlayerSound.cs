using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerMovement move;
    public bool isStone = false;
    public bool isWood = false;

    private void Start()
    {
        move = GetComponent<PlayerMovement>();
    }

    public void FootStepOn()
    {
        if(move.isGrounded)
        {
            if(isStone)
            {
                AudioManagerGame.instance.FootStepStone();
            } else if(isWood)
            {
                AudioManagerGame.instance.FootStepWood();
            } else
            {
                AudioManagerGame.instance.FootStep();
            }
        }
    }

    public void JumpOn()
    {
        if(isStone)
        {
            AudioManagerGame.instance.JumpStone();
        }
        else if (isWood)
        {
            AudioManagerGame.instance.JumpWood();
        }
        else
        {
            AudioManagerGame.instance.Jump();
        }
    }
}
