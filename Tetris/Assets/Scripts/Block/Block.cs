using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Rotation))]
public class Block : MonoBehaviour
{
    // Keeps passed time
    public float time { get; set; }

    // Determines how fast block will fall down
    public float fallingRate { get; set; }
    // Determines how many time block will have in deatTime
    public float deathRate { get; set; }

    public int rotationForBlock { get; set; }

    // Keeps block's position
    private Vector3 position;
    public float pos_y { get; set; }
    public float pos_x { get; set; }

    // Three variables determine if we can move to any direction or rotate
    public bool canFall { get; set; }
    public bool touchedGround { get; set; } // Information for us (for Death time "function" )

    public bool canRight { get; set; }
    public bool canRightHelper { get; set; }
    public bool canLeft { get; set; }
    public bool canLeftHelper { get; set; }
    public bool canRotate { get; set; }
    public bool canRotateHelper { get; set; } // Helper is because we first have to know if block can rotate then we can change canRotate bool (We are avoiding bugs by doing this)
    public bool canFastDrop { get; set; }

    public bool canGoVertical { get; set; } public bool canGoVertical2 {get;set;}

    private bool ghostDestroyed;

    // Used to detect if our block will touch the ground in the next move DIRECTION - DOWN
    public GameObject ghost_floor { get; set; }
    // Used to detect if we can move to the right
    public GameObject ghost_right { get; set; }
    //Used to detect if we can move to the left
    public GameObject ghost_left { get; set; }

    // Deterines the position of ghosts (floor, right, left) (Imitating rotation by setting different position)
    public int ghost_change { get; set; }

    public int howManyRotations; // It is information how many times block should rotate for instance flar block (****) can rotate only two times because 
                                 // if he rotate third time then he will repeat one of his previous moves

    private Movement movement;
    private Rotation rotation;
    private BlockInput blockInput;


    public bool readyForDeathTime { get; set; }

    // If it is true it means that block has already touched the ground and it has got about 0.5 sec to move horizontal
    public bool deathTime { get; set; }
    public bool isBlockDead { get; set; }


    private void Awake()
    {
        ghostDestroyed = false;
        readyForDeathTime = true;

        // Getting components of our block
        movement = transform.GetComponent<Movement>();
        rotation = transform.GetComponent<Rotation>();
        blockInput = GameObject.FindGameObjectWithTag(ConstVar.tag_controller).GetComponent<BlockInput>();

        // Reseting all variables in the beginning
        canFall = true;
        canFastDrop = true;
        canRotate = true;        
        canRight = true;
        canLeft = true;
        canGoVertical = true;
        canGoVertical2 = true;

        canLeftHelper = true;
        canRightHelper = true;
        canRotateHelper = true;

        time = 0;

        rotationForBlock = 0;

        deathTime = false;
        isBlockDead = false;
    }

    void Start()
    {
        // getting block's postion
        position = transform.position;
        pos_y = position.y;
        pos_x = position.x;

        FindChildren();
    }

    void Update()
    {
        // Incrementing time
        time += Time.deltaTime;

        if(canGoVertical && canGoVertical2)
            movement.Vertical(); // move down

        DeathTimeHandler(); // After block touches ground it has got 1 second to move left or right
    }

    // Getting block's children and setting its position
    private void FindChildren()
    {
        if (transform.childCount >= 3)
        {
            ghost_floor = this.gameObject.transform.GetChild(4).gameObject;
            ghost_floor.transform.localPosition = new Vector3(0, -2, 0);

            ghost_right = this.gameObject.transform.GetChild(5).gameObject;
            ghost_right.transform.localPosition = new Vector3(0, 0, 2);

            ghost_left = this.gameObject.transform.GetChild(6).gameObject;
            ghost_left.transform.localPosition = new Vector3(0, 0, -2);

            ghost_change = 0;
        }
    }

    // As the name says it makes ghost active
    public void MakeGhostsActive()
    {
        if (ghostDestroyed == true)
            return;

        ghost_floor.SetActive(true);
        ghost_right.SetActive(true);
        ghost_left.SetActive(true);
    }

    // As the name says it makes ghost not active
    public void MakeGhostsNotActive()
    {
        if (ghostDestroyed == true)
            return;

        ghost_floor.SetActive(false);
        ghost_right.SetActive(false);
        ghost_left.SetActive(false);
    }


    private void DeathTimeHandler()
    {
        if(canFall == false && readyForDeathTime == true && canFastDrop == true)
        {
            blockInput.buttonRotateBlocked = true;
            switch (ActiveGame.gameLevel)
            {
                case ActiveGame.difficulty.easy:
                    fallingRate = 0.8f;
                    break;
                case ActiveGame.difficulty.medium:
                    fallingRate = 0.6f;
                    break;
                case ActiveGame.difficulty.hard:
                    fallingRate = 0.4f;
                    break;
            }

            readyForDeathTime = false;
            StartCoroutine(DeathCoroutine());
        }
        else if(canFall == false && readyForDeathTime == true && canFastDrop == false)
        {
            isBlockDead = true;
            deathTime = false;
            ghostDestroyed = true;

            Destroy(ghost_floor.gameObject);
            Destroy(ghost_right.gameObject);
            Destroy(ghost_left.gameObject);
        }
    }

    private IEnumerator DeathCoroutine()
    {
        // Block has got some time to move left and right
        yield return new WaitForSeconds(deathRate * 0.75f);

        // Blocking buttons
        blockInput.buttonsBlocked = true;

        yield return new WaitForSeconds(deathRate * 0.25f);

        touchedGround = false;

        MakeGhostsNotActive();
        MakeGhostsActive();

        // We have to wait about 0.08f because we need to give time to GhostCollisionFloor's OnTriggeEnter to figure out if block is touching the ground
        yield return new WaitForSeconds(0.08f);

        // If block touches the ground then is stops to move
        if (touchedGround == true)
        {
            isBlockDead = true;
            ghostDestroyed = true;

            Destroy(ghost_floor.gameObject);
            Destroy(ghost_right.gameObject);
            Destroy(ghost_left.gameObject);
        }
        // Else it can fall again until it touches ground again
        else
        {
            canFall = true;
            readyForDeathTime = true;
        }

        deathTime = false;
        blockInput.buttonsBlocked = false;
        blockInput.buttonRotateBlocked = false;
    }
}
