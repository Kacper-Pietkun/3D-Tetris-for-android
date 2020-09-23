using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCollisionHorizontal : MonoBehaviour
{
    private GameObject parent;
    private Block block;

    [SerializeField]
    private bool left;

    private void Awake()
    {
        parent = transform.parent.gameObject;
        block = parent.GetComponent<Block>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If we hit the wall or another bock then block cannot move lef or right
        if (IsItSibling(other.transform) == false && other.name != "HotSpot")
        {
            if (left)
            {
                block.canLeft = false;
                block.canLeftHelper = false;
            }
            else
            {
                block.canRight = false;
                block.canRightHelper = false;
            }

            // If we are in the range of collider then block cannot rotate
            block.canRotateHelper = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Making our block able to move left or right again
        if (IsItSibling(other.transform) == false && other.name != "HotSpot")
        {
            if (left)
                block.canLeft = true;
            else
                block.canRight = true;

            // If we exit collider then block can rotate
            block.canRotate = true;
        }
    }

    private bool IsItSibling(Transform other)
    {
        foreach (Transform child in transform.parent)
        {
            if (child == other)
                return true;
        }
        return false;
    }
}
