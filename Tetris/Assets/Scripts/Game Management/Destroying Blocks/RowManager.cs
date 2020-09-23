using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    [SerializeField]
    private int index; // Information which row is it

    private GameObject[] hotSpots = new GameObject[10];
    private HotSpotManager[] hotSpotManager = new HotSpotManager[10];


    // We need it to have access to every row in the game
    private GameObject[] allRows = new GameObject[22];
    private RowManager[] rowManager = new RowManager[22];

    private Block block;

    private float time;

    private void Awake()
    {
        // Getting children of that row
        if(transform.childCount >= 10)
        {
            for(int i=0; i<10; i++)
            {
                hotSpots[i] = transform.GetChild(i).gameObject;
                hotSpotManager[i] = hotSpots[i].GetComponent<HotSpotManager>();
            }
        }

        // Getting siblings of that row
        if (transform.parent.childCount >= 22)
        {
            for (int i = 0; i < 22; i++)
            {
                allRows[i] = transform.parent.GetChild(i).gameObject;
                rowManager[i] = allRows[i].GetComponent<RowManager>();
            }
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    public void TryToDestroyRow()
    {
        // Destroys Row as soon as it is ready
        if (RowReadyToDestroy())
        {
            // then it destroys everything in this row
            DestroyRow();
            // We pass the time because if the difference between one row and the next row being destroyed is low then we know that they were
            // destroyed in the same move
            ActiveGame.AddPoints(time);
            ActiveGame.UpdateTime(time); // Updating time when the last row was destroyed

            // then it makes other blocks upstairs fall down
            TetrisDecrease(index + 1); // + 1 because we want to fall only that blocks which are higher than our destroyed one
        }
    }

    // Checks if row is ready to be destroyed
    // Row is ready to be destroyed only if every hot spot in that row is ready to be destroyed (In that hot spot there is a part of a block)
    private bool RowReadyToDestroy()
    {
        for(int i=0; i<hotSpots.Length; i++)
        {
            if (hotSpotManager[i].ready == false)
                return false;

        }
        return true;
    }

    // Destroy all the parts of blocks that are in this row
    private void DestroyRow()
    {
        // We destroy every cube in row and then we set target of hotSpot to null
        for (int i = 0; i < hotSpots.Length; i++)
        {
            if(hotSpotManager[i].target != null)
            {
                Destroy(hotSpotManager[i].target.gameObject);
                hotSpotManager[i].target = null;
                hotSpotManager[i].ready = false;
            }
        }
    }

    // Triggers RowCubeFall for every row that is higher than this one
    public void TetrisDecrease(int index)
    {
        for (int i = index; i < 22; i++)
        {
            rowManager[i].RowCubeFall();
        }

        // Make every hotSpot in every Row unactive and then active to see if they are ready to be destroyed
        RefreshRows();
    }

    public void RefreshRows()
    {
        for(int i=0; i<22; i++)
        {
            for(int j=0; j<10; j++)
            {
                rowManager[i].hotSpotManager[j].refresh();
            }
        }
    }

    // Every cube that is in this row goes down by one (It is used when row which is under this row is destroyed)
    public void RowCubeFall()
    {
        for (int i = 0; i < hotSpots.Length; i++)
        {
            // Decreasing block's position.y by 1 position down (it means pos_y or pos_z increase or decrease by 2 (It depends from block's rotation))
            if (hotSpotManager[i].target != null)
            {
                Vector3 position = hotSpotManager[i].target.localPosition;
                float pos_y = position.y;
                float pos_z = position.z;
                block = hotSpotManager[i].target.parent.GetComponent<Block>();
                int rot = block.rotationForBlock;

                switch(rot)
                {
                    case 0:
                        pos_y -= 2;
                        break;
                    case 90:
                        pos_z += 2;
                        break;
                    case 180:
                        pos_y += 2;
                        break;
                    case 270:
                        pos_z -= 2;
                        break;
                }

                hotSpotManager[i].target.localPosition = new Vector3(0, pos_y, pos_z);
                hotSpotManager[i].target = null;
                hotSpotManager[i].ready = false;
            }

        }
    }     
}