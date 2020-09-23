using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Block))]
public class Movement : MonoBehaviour
{
    private Block block;
    private BlockInput blockInput;


    private void Awake()
    {
        // Getting block's component
        block = transform.GetComponent<Block>();
        blockInput = GameObject.FindGameObjectWithTag(ConstVar.tag_controller).GetComponent<BlockInput>();
    }

    // Updating position of block
    public void Move()
    {
        transform.position = new Vector3(block.pos_x, block.pos_y, 0);
    }

    // Moving left
    public void HorizontalLeft()
    {
        block.pos_x--;
        Move(); // Update block's position
        blockInput.buttonLeftBlocked = false;
        block.canGoVertical2 = true;
    }

    // Moving right
    public void HorizontalRight()
    {
        block.pos_x++;
        Move(); // Update block's position
        blockInput.buttonRightBlocked = false;
        block.canGoVertical2 = true;
    }

    // After falling rate we move our block down
    public void Vertical()
    {
        StartCoroutine(verticalCoroutine());
    }

    private IEnumerator verticalCoroutine()
    {
        if (block.canFall)
        {
            if ( Time.timeScale != 0)
            {
                block.canGoVertical = false;
                blockInput.buttonsBlocked = true;

                if (block.canFastDrop == false)
                {
                    // Moving block dowon
                    block.pos_y--;

                    // Update block's position
                    Move();
                    yield return new WaitForSeconds(block.fallingRate * 0.2f);
                    block.canGoVertical = true;
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                    block.touchedGround = false;
                    block.MakeGhostsNotActive();
                    block.MakeGhostsActive();
                    yield return new WaitForEndOfFrame();

                    if (block.touchedGround == true)
                    {
                        block.canFall = false;
                    }
                    else
                    {
                        // Moving block dowon
                        block.pos_y--;

                        // Update block's position
                        Move();

                        yield return new WaitForEndOfFrame();
                        blockInput.buttonsBlocked = false;

                        yield return new WaitForSeconds(block.fallingRate);
                        block.canGoVertical = true;
                    }
                    
                } 
            }
        }
    }

    public void FastDrop()
    {
        if (block.canFall)
        {
            block.fallingRate = 0.03f;
            block.canFastDrop = false;
        }
    }
}
