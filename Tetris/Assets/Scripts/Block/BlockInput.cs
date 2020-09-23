using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class is responsible for buttons(these on the screen) clicked by player
public class BlockInput : MonoBehaviour
{
    private GameObject tetris_block;
    private Block block;
    private SpawnBlocks spawnBlocks;
    private Movement movement;
    private Rotation rotation;

    public bool buttonsBlocked { get; set; } // If it is true then we cannot click buttons

    public bool buttonRightBlocked { get; set; }
    public bool buttonLeftBlocked { get; set; }
    public bool buttonRotateBlocked { get; set; }
    public bool buttonFastDropBlocked { get; set; }

    private void Awake()
    {
        tetris_block = null;

        buttonsBlocked = false;
        buttonRightBlocked = false;
        buttonLeftBlocked = false;
        buttonRotateBlocked = false;
        buttonFastDropBlocked = false;
    }

    private void Start()
    {
        // We need to give some time to spanwe to spawn first block, then we can be sure that there is sth in spawnBlocks.newBlock
        StartCoroutine(GetFirstBlockDelay()); 
    }

    private IEnumerator GetFirstBlockDelay()
    {
        yield return new WaitForSeconds(0.05f);
        spawnBlocks = GameObject.Find("Spawn").GetComponent<SpawnBlocks>();
        tetris_block = spawnBlocks.newBlock;
        movement = tetris_block.GetComponent<Movement>();
        rotation = tetris_block.GetComponent<Rotation>();
        block = tetris_block.GetComponent<Block>();
    }

    private void Update()
    {
        if (tetris_block != null && tetris_block != spawnBlocks.newBlock)
        {
            tetris_block = spawnBlocks.newBlock;
            movement = tetris_block.GetComponent<Movement>();
            rotation = tetris_block.GetComponent<Rotation>();
            block = tetris_block.GetComponent<Block>();
        }
    }

    IEnumerator blockButtonsCoroutine()
    {
        yield return new WaitForEndOfFrame();
        buttonsBlocked = false;
    }

    public void MoveLeft()
    {
        StartCoroutine(CanLeftCoroutine());
    }

    private IEnumerator CanLeftCoroutine()
    {
        block.canLeft = false;
        block.canLeftHelper = true;
        block.MakeGhostsNotActive();
        block.MakeGhostsActive();
        yield return new WaitForSeconds(0.05f);

        if (block.canLeftHelper == true)
            block.canLeft = true;

        if (buttonsBlocked == false && block.canLeft && buttonLeftBlocked == false && Time.timeScale != 0)
        {
            block.canGoVertical2 = false;
            buttonLeftBlocked = true;
            movement.HorizontalLeft();
            buttonsBlocked = true;
            StartCoroutine(blockButtonsCoroutine());
        }

    }

    public void MoveRight()
    {
        StartCoroutine(CanRightCoroutine());
    }

    private IEnumerator CanRightCoroutine()
    {
        block.canRight = false;
        block.canRightHelper = true;
        block.MakeGhostsNotActive();
        block.MakeGhostsActive();
        yield return new WaitForSeconds(0.05f);


        if (block.canRightHelper == true)
            block.canRight = true;

        if (buttonsBlocked == false && block.canRight && buttonRightBlocked == false && Time.timeScale != 0)
        {
            block.canGoVertical2 = false;
            buttonRightBlocked = true;
            movement.HorizontalRight();
            buttonsBlocked = true;
            StartCoroutine(blockButtonsCoroutine());
        }
    }

    public void Rotate()
    {
        if (buttonsBlocked == false && block.canRotate && buttonRotateBlocked == false && Time.timeScale != 0)
        {
            buttonRotateBlocked = true;
            rotation.Rotate();
            buttonsBlocked = true;
            StartCoroutine(blockButtonsCoroutine());
        }
    }

    public void FastDrop()
    {
        if (buttonsBlocked == false && block.canFastDrop && buttonFastDropBlocked == false && Time.timeScale != 0)
        {
            movement.FastDrop();
            buttonsBlocked = true;
        }
    }
}
