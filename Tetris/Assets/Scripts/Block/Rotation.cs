using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Block))]
public class Rotation : MonoBehaviour
{
    [SerializeField]
    private bool doesItRotateAtAll; // For square block which will not rotate at all

    // Used when we try to rotate block in any other way if the next rotation cannot be made, because the block would hit the wall or another block
    private bool repeat;
    private int i;

    private Block block;
    private BlockInput blockInput;


    private void Awake()
    {
        // Getting block's component
        block = transform.GetComponent<Block>();
        blockInput = GameObject.FindGameObjectWithTag(ConstVar.tag_controller).GetComponent<BlockInput>();
    }

    // Rotating Block and changing position of its ghost
    public void Rotate()
    {
        if (doesItRotateAtAll == true && block.canRotate == true)
        {
            block.canLeft = false;
            block.canRight = false;
            block.canRotate = false;

            if (block.howManyRotations == 4)
                StartCoroutine(DelayRotateCoroutine4());
            else if (block.howManyRotations == 2)
                StartCoroutine(DelayRotateCoroutine2());
        }
    }

    // This function rotates only block which should be rotated only 2 times !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private IEnumerator DelayRotateCoroutine2()
    {
        int i = 1;
        block.rotationForBlock += 270;
        if (block.rotationForBlock == 540)
            block.rotationForBlock = 0;


        // We want these types of blocks (which should rotate 2 times to rotate and then unrotate that's why i is equal only to 1 or -1)
        if (block.rotationForBlock == 0)
            i = 1;
        else if (block.rotationForBlock == 270)
            i = -1;

        // we rotate ghosts and reset their positions to simulate block position if they rotated
        block.ghost_right.transform.Rotate(90 * i, 0, 0);
        block.ghost_left.transform.Rotate(90 * i, 0, 0);
        block.ghost_left.transform.localPosition = new Vector3(0, 0, 0);
        block.ghost_right.transform.localPosition = new Vector3(0, 0, 0);

        block.canRotateHelper = true;

        // I make ghosts not active and then active beceuse I want to trigger OnTriggerEnter one more time to see if block can rotate
        block.MakeGhostsNotActive();
        block.MakeGhostsActive();

        yield return new WaitForSeconds(0.02f);

        if (block.canRotateHelper == true)
            block.canRotate = true;

        // If block can rotate then we rotate block and we set ghost to their adequate position and rotation
        if (block.canRotate)
        {
            // Rotate the block by 90 degrees
            transform.Rotate(90 * i, 0, 0);

            block.ghost_right.transform.Rotate(-90 * i, 0, 0);
            block.ghost_left.transform.Rotate(-90 * i, 0, 0);

            if (block.ghost_change == 0)
                block.ghost_change = 3;
            else
                block.ghost_change = 0;


            Rotate_ghost_floor();
            Rotate_ghost_right();
            Rotate_ghost_left();
        }
        // If block cannot rotate then we change only ghosts position and rotation
        else
        {
            block.rotationForBlock -= 270;
            if (block.rotationForBlock == -270)
                block.rotationForBlock = 270;

            block.ghost_right.transform.Rotate(-90 * i, 0, 0);
            block.ghost_left.transform.Rotate(-90 * i, 0, 0);

            Rotate_ghost_floor();
            Rotate_ghost_right();
            Rotate_ghost_left();
        }

        block.canLeftHelper = true;
        block.canRightHelper = true;
        block.canRotateHelper = true;

        block.MakeGhostsNotActive();
        block.MakeGhostsActive();

        if (block.canLeftHelper == true)
            block.canLeft = true;

        if (block.canRightHelper == true)
            block.canRight = true;

        if (block.canRotateHelper == true)
            block.canRotate = true;

        blockInput.buttonRotateBlocked = false;
    }



    // This function rotates only block which should be rotated only 4 times !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // This coroutine is needed because we have to wait really a little amount of time for ghost's collider OnTriggerEnter to end
    // If we didn't wait this 0.01 sec then we wouldn't have updated canRotate variable and it would lead to serious problems
    private IEnumerator DelayRotateCoroutine4()
    {
        i = 1;
        repeat = true;

        // Loop is for case when block cannot rotate 90 degrees then we check if it can rotate 180 degrees or 270 degrees otherwise it will rotate
        // 360 degrees => 0 degrees it will have the same rotation
        while (i <= 4 && repeat)
        {
            block.rotationForBlock += 90;
            if (block.rotationForBlock ==360)
                block.rotationForBlock = 0;

            // False if we can rotate, then break the loop

            // we rotate ghosts and reset their positions to simulate block position if they rotated
            block.ghost_right.transform.Rotate(90 * i, 0, 0);
            block.ghost_left.transform.Rotate(90 * i, 0, 0);
            block.ghost_left.transform.localPosition = new Vector3(0, 0, 0);
            block.ghost_right.transform.localPosition = new Vector3(0, 0, 0);

            block.canRotateHelper = true;

            // I make ghosts not active and then active beceuse I want to trigger OnTriggerEnter one more time to see if block can rotate
            block.MakeGhostsNotActive();
            block.MakeGhostsActive();


            yield return new WaitForSeconds(0.02f);

            if (block.canRotateHelper == true)
                block.canRotate = true;

            // If block can rotate then we rotate block and we set ghost to their adequate position and rotation
            if (block.canRotate)
            {
                // After ending this iteration of the loop next won't be triggered
                repeat = false;

                // Rotate the block by 90 degrees
                transform.Rotate(90 * i, 0, 0);

                block.ghost_right.transform.Rotate(-90 * i, 0, 0);
                block.ghost_left.transform.Rotate(-90 * i, 0, 0);

                block.ghost_change += i;
                if (block.ghost_change >= 4)
                    block.ghost_change -= 4;

                Rotate_ghost_floor();
                Rotate_ghost_right();
                Rotate_ghost_left();
            }
            // If block cannot rotate then we change only ghosts position and rotation
            else
            {
                block.ghost_right.transform.Rotate(-90 * i, 0, 0);
                block.ghost_left.transform.Rotate(-90 * i, 0, 0);

                Rotate_ghost_floor();
                Rotate_ghost_right();
                Rotate_ghost_left();
            }
            i++;
        }

        block.canLeftHelper = true;
        block.canRightHelper = true;
        block.canRotateHelper = true;

        block.MakeGhostsNotActive();
        block.MakeGhostsActive();

        if (block.canLeftHelper == true)
            block.canLeft = true;

        if (block.canRightHelper == true)
            block.canRight = true;

        if (block.canRotateHelper == true)
            block.canRotate = true;

        blockInput.buttonRotateBlocked = false;
    }

    private void Rotate_ghost_floor()
    {
        // We do not rotate the ghost_floor but we change its position so it can fit to parent's rotation
        switch (block.ghost_change)
        {
            case 0:
                block.ghost_floor.transform.localPosition = new Vector3(0, -2, 0);
                break;
            case 1:
                block.ghost_floor.transform.localPosition = new Vector3(0, 0, 2);
                break;
            case 2:
                block.ghost_floor.transform.localPosition = new Vector3(0, 2, 0);
                break;
            case 3:
                block.ghost_floor.transform.localPosition = new Vector3(0, 0, -2);
                break;
        }
    }

    private void Rotate_ghost_right()
    {
        // We do not rotate the ghost_right but we change its position so it can fit to parent's rotation
        switch (block.ghost_change)
        {
            case 0:
                block.ghost_right.transform.localPosition = new Vector3(0, 0, 2);
                break;
            case 1:
                block.ghost_right.transform.localPosition = new Vector3(0, 2,0);
                break;
            case 2:
                block.ghost_right.transform.localPosition = new Vector3(0, 0, -2);
                break;
            case 3:
                block.ghost_right.transform.localPosition = new Vector3(0, -2, 0);
                break;
        }
    }

    private void Rotate_ghost_left()
    {
        // We do not rotate the ghost_left but we change its position so it can fit to parent's rotation
        switch (block.ghost_change)
        {
            case 0:
                block.ghost_left.transform.localPosition = new Vector3(0, 0, -2);
                break;
            case 1:
                block.ghost_left.transform.localPosition = new Vector3(0, -2, 0);
                break;
            case 2:
                block.ghost_left.transform.localPosition = new Vector3(0, 0, 2);
                break;
            case 3:
                block.ghost_left.transform.localPosition = new Vector3(0, 2, 0);
                break;
        }
    }
}
