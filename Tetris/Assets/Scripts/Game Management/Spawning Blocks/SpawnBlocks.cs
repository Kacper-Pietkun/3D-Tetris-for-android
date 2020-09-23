using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour
{
    private BlockInput blockInput;
    private ListOfPrefabs listOfPrefabs;

    public GameObject newBlock;
    private Block block;

    private float block_falling_rate;
    private float block_death_rate;

    // If Block's start_pos_y is equal to its end pos_y then we know that player has lost the game
    private float start_pos_y;
    private float end_pos_y;

    [SerializeField]
    private GameObject game_over_menu;

    [SerializeField]
    private Material[] materials; // List of accessible materials for spawned blocks

    private string[] blocksNames = new string[4];

    private GameObject row1; // We need this row because we will refresh each hotspot when new block will be created
    private RowManager rowManager; 

    private void Awake()
    {
        blockInput = GameObject.FindGameObjectWithTag(ConstVar.tag_controller).GetComponent<BlockInput>();
        row1 = GameObject.Find("ROW_1");
        rowManager = row1.GetComponent<RowManager>();

        // Getting list of prefabs to randomly choose one that will be created
        listOfPrefabs = transform.GetComponent<ListOfPrefabs>();

        // Checking what type of game user has chosen, then we set block_falling_rate 
        switch(ActiveGame.gameLevel)
        {
            case ActiveGame.difficulty.easy:
                block_falling_rate = 0.8f;
                block_death_rate = 0.8f;
                break;
            case ActiveGame.difficulty.medium:
                block_falling_rate = 0.6f;
                block_death_rate = 0.6f;
                break;
            case ActiveGame.difficulty.hard:
                block_falling_rate = 0.4f;
                block_death_rate = 0.4f;
                break;
        }

        blocksNames[0] = "Cube.000";
        blocksNames[1] = "Cube.001";
        blocksNames[2] = "Cube.002";
        blocksNames[3] = "Cube.003";

    }

    private void Start()
    {
        newBlock = listOfPrefabs.GetOnePrefab();
        newBlock = Instantiate(newBlock, transform.position, newBlock.transform.rotation); // Creating one block in the beginning of the game

        block = newBlock.GetComponent<Block>(); // We need access to canFall (bool) variable, when one block touches ground then canFall = false and then
                                                // we will create new block
        block.fallingRate = block_falling_rate; // Setting original falling_rate which is in Script: Block, with this private float block_falling_rate
        block.deathRate = block_death_rate;
        start_pos_y = newBlock.transform.position.y;
        setBlockMaterial();
    }

    private void Update()
    {
        SpawnNewBlock();
    }

    private void SpawnNewBlock()
    {
        // Everytime one block is dead the ground ten new block will be created
        if (block.isBlockDead == true)
        {
            end_pos_y = newBlock.transform.position.y;
            // If Block's start_pos_y is equal to its end pos_y then we know that player has lost the game
            if (start_pos_y == end_pos_y)
            {
                ActiveGame.GameOver();
                game_over_menu.SetActive(true);
            }

            newBlock = listOfPrefabs.GetOnePrefab();
            newBlock = Instantiate(newBlock, transform.position, newBlock.transform.rotation);
            block = newBlock.GetComponent<Block>();
            block.fallingRate = block_falling_rate;
            block.deathRate = block_death_rate;
            blockInput.buttonsBlocked = false;
            blockInput.buttonLeftBlocked = false;
            blockInput.buttonRightBlocked = false;
            blockInput.buttonRotateBlocked = false;
            blockInput.buttonFastDropBlocked = false;
            setBlockMaterial(); // Setting color of the new block
            rowManager.RefreshRows(); // Refreshing every hot spot on the map
        }
    }

    private void setBlockMaterial()
    {
        int index;
        index = UnityEngine.Random.Range(0, 7);

        foreach (Transform child in newBlock.transform)
        {
            if(child.name == blocksNames[0] ||
                child.name == blocksNames[1] ||
                child.name == blocksNames[2] ||
                child.name == blocksNames[3])
            {  
                child.GetComponent<Renderer>().material = materials[index];
            }
        }
    }
}
