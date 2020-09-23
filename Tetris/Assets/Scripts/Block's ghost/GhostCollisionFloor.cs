using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCollisionFloor : MonoBehaviour
{
    private GameObject parent;
    private Block block;

    private void Awake()
    {
        parent = transform.parent.gameObject;
        block = parent.GetComponent<Block>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // If we hit the ground then block stops falling
        if(IsItSibling(other.transform) == false && other.name != "HotSpot")
        {
            block.deathTime = true;
            block.canFall = false;
            block.touchedGround = true;
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
