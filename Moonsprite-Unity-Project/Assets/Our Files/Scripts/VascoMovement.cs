using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class VascoMovement : MonoBehaviour
{
    /*
    public float speed = 10.0f;

    private Rigidbody2D rb;
    bool isTalking;
    bool nearNPC;
    [SerializeField] DialogueText Dtext;
    public DialogueState curDialogue;
    [SerializeField] NPC_BasicLogic curNPC;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nearNPC = false;

    }
    void Update()
    {
        if (Dtext != null)
        {
            isTalking = Dtext.isTalking;
        }

        if (nearNPC && !isTalking)
        {
            if (Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.E)))
            {
                curDialogue = curNPC.curDialogue;
                Dtext.StartDialogue(curDialogue);
                curNPC.NPCNextDialogue();

            }
        }
    }

    private void FixedUpdate()
    {

        if (isTalking)
        {
            speed = 0;

        }
        else
        {
            speed = 5;
        }


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            nearNPC = true;

            NPC_BasicLogic npc = other.GetComponent<NPC_BasicLogic>();
            if (npc != null)
            {
                npc.ChangeColor();
                curNPC = npc;
            }


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            nearNPC = false;
            NPC_BasicLogic npc = other.GetComponent<NPC_BasicLogic>();
            if (npc != null)
            {
                npc.ChangeBack();
            }
        }
    }
    */
}

