using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpotManager : MonoBehaviour
{
    // List of every block's name
    private string[] blocksNames = new string[4];
    public bool ready;// { get; set; }

    private Block block;

    public Transform target { get; set; } // If sth stays in this hotSpot this variable will help to remember what to destroy in this row

    private RowManager rowManager;

    private void Awake()
    {
        ready = false;

        blocksNames[0] = "Cube.000";
        blocksNames[1] = "Cube.001";
        blocksNames[2] = "Cube.002";
        blocksNames[3] = "Cube.003";

        rowManager = transform.parent.GetComponent<RowManager>();
    }

    private void OnEnable()
    {
        // Reseting hotSpot it is done after TetrisDecrease
        target = null;
        ready = false;
    }

    // When sth comes to hot spot we check just for security (but i am sure it would work without this loop) if it is block and if block's canFall is
    // false then it means that block will stay in this hot spot so it is ready to be destroyed
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < 4; i++)
        {
            if (other.name == blocksNames[i])
            {
                // We have to give time to block so it can set its canLeft and canRight value to false or true so hotSpot can be sure if it is ready to
                // be destroyed
                
                block = other.transform.parent.GetComponent<Block>(); // Getting parent because "Collider other" it is cube - one part of the block
                                                                      // So this cube is a child of the block

                if (block.canFall == false && block.deathTime == false && ready == false)
                {
                    target = other.transform;
                    ready = true;
                    StartCoroutine(SearchDelay());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < 4; i++)
        {
            if (other.name == blocksNames[i] && other.transform.parent.GetComponent<Block>().canFall == false)
            {
                target = null;
                ready = false;
            }
        }
    }

    private IEnumerator SearchDelay()
    {
        yield return new WaitForSeconds(0.05f);
        rowManager.TryToDestroyRow();

    }

    public void refresh()
    {
        target = null;
        ready = false;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
