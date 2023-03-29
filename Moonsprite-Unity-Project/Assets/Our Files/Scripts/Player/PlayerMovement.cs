using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Attribution: Maciej Wolski

    public bool active = true;

    [Header("Required References")]
    public Rigidbody2D rb2D;
    public Animator anim;
    public SpriteRenderer spriteRenderer;

    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode auxLeftKey = KeyCode.LeftArrow;
    [Space]
    public KeyCode rightKey = KeyCode.D;
    public KeyCode auxRightKey = KeyCode.RightArrow;
    [Space]
    public KeyCode downKey = KeyCode.S;
    public KeyCode auxDownKey = KeyCode.DownArrow;
    [Space]
    public KeyCode upKey = KeyCode.W;
    public KeyCode auxUpKey = KeyCode.UpArrow;

    [Header("Movement")]
    public float moveSpeed = 0.35f;
    public float pivotTime = 0.1f;
    [Space]
    public float deccelerationPower = 2;
    public float maxMoveSpeed = 6;
    public float maxBoostedSpeed = 25;
    [Space]
    private Vector3 targetPos;
    private Vector3 currentPos;

    Vector2 velocity = Vector2.zero;
    const float xDeccelerateFactor = 1;

    float xInput;
    float yInput;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        //player input
        GetPlayerInput();
        //GetPlayerMouseInput();
    }

    private void FixedUpdate()
    {
        // deccelerate();

        DampBackTo0v();

        if (active == true)
        {
            MovePlayer();
            AnimatorUpdate(new Vector2(xInput, yInput));
        }
    }

    public void AnimatorUpdate(Vector2 inputVector)
    {
        FlipSpriteBasedOn(inputVector.x);
        anim.SetFloat("xInput", inputVector.x);
        anim.SetFloat("yInput", inputVector.y);
    }

    void FlipSpriteBasedOn(float number)
    {
        if(number > 0.05f)
        {
            spriteRenderer.flipX = true;
        }
        else if(number < -0.05f)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void GetPlayerInput()
    {
        if(active == false)
        {
            xInput = 0;
            yInput = 0;
            return;
        }

        if (Input.GetKey(leftKey) == true || Input.GetKey(auxLeftKey) == true)
        {
            xInput = -1;
        }
        else if (Input.GetKey(rightKey) == true || Input.GetKey(auxRightKey) == true)
        {
            xInput = 1;
        }
        else
        {
            xInput = 0;
        }

        if (Input.GetKey(downKey) == true || Input.GetKey(auxDownKey) == true)
        {
            yInput = -1;
        }
        else if (Input.GetKey(upKey) == true || Input.GetKey(auxUpKey) == true)
        {
            yInput = 1;
        }
        else
        {
            yInput = 0;
        }
    }

    [System.Obsolete("DR: It'd be nice to have mouse click movement, but it could clash with our UI and interaction system (as you might click for those). It could be made to work with them, but lets just leave it.")]
    private void GetPlayerMouseInput()
    {
        currentPos = transform.position;
        float damp = 0.3f;

        if (Input.GetMouseButtonDown(1))
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = transform.position.z;
        }

        if (currentPos.x <= targetPos.x - damp)
        {
            xInput = 1;
        }
        else if (currentPos.x >= targetPos.x + damp)
        {
            xInput = -1;
        }
        else
        {
            xInput = 0;
        }

        if (currentPos.y <= targetPos.y - damp)
        {
            yInput = 1;
        }
        else if (currentPos.y >= targetPos.y + damp)
        {
            yInput = -1;
        }
        else
        {
            yInput = 0;
        }
    }

    private void MovePlayer()
    {
        Vector2 limitedInput = Vector2MagnitudeLimit(new Vector2(xInput, yInput), 1); // this limits the input magnitude, so the player can't abuse vectors to go really fast

        if(limitedInput.x > 0 && rb2D.velocity.x < maxMoveSpeed * limitedInput.x)
        {
            rb2D.AddForce(new Vector2(limitedInput.x, 0) * moveSpeed, ForceMode2D.Impulse);
        }

        if (limitedInput.x < 0 && rb2D.velocity.x > maxMoveSpeed * limitedInput.x)
        {
            rb2D.AddForce(new Vector2(limitedInput.x, 0) * moveSpeed, ForceMode2D.Impulse);
        }

        if (limitedInput.y < 0 && rb2D.velocity.y > maxMoveSpeed * limitedInput.y)
        {
            rb2D.AddForce(new Vector2(0, limitedInput.y) * moveSpeed, ForceMode2D.Impulse);
        }

        if (limitedInput.y < 0 && rb2D.velocity.y > maxMoveSpeed * limitedInput.y)
        {
            rb2D.AddForce(new Vector2(0, limitedInput.y) * moveSpeed, ForceMode2D.Impulse);
        }

        // so instead of pushing back on the player when they're above a certain speed
        // it just stops pushing the player once they're above that speed
        // this makes the movement smoother, which fixes camera jittering

        // nevermind that seemingly wasn't causing the jitter
        // it was instead because the camera was following an attached gameobject (the camera tracking point) instead of the player itself
        // we can still maintain the camera tracking point though, as there is an offset parameter in the cinemachine brain

        //rb2D.AddForce(new Vector2(limitedInput.x, limitedInput.y) * moveSpeed, ForceMode2D.Impulse); 

        /*
        // this slows the player down at a rate equal to their movespeed
        if (rb2D.velocity.x >= maxMoveSpeed * limitedInput.x)
        {
            rb2D.AddForce(new Vector2(-moveSpeed, 0) * xDeccelerateFactor, ForceMode2D.Impulse);
        }
        if (rb2D.velocity.x <= -maxMoveSpeed * limitedInput.x)
        {
            rb2D.AddForce(new Vector2(moveSpeed, 0) * xDeccelerateFactor, ForceMode2D.Impulse);
        }

        if (rb2D.velocity.y >= maxMoveSpeed * limitedInput.y)
        {
            rb2D.AddForce(new Vector2(0, -moveSpeed) * xDeccelerateFactor, ForceMode2D.Impulse);
        }
        if (rb2D.velocity.y <= -maxMoveSpeed * limitedInput.y)
        {
            rb2D.AddForce(new Vector2(0, moveSpeed) * xDeccelerateFactor, ForceMode2D.Impulse);
        }
        */
    }

    Vector2 Vector2MagnitudeLimit(Vector2 vector2, float limit)
    {
        if(vector2.magnitude > limit)
        {
            return vector2.normalized * limit;
        }
        return vector2;
    }

    private void Deccelerate()
    {
        //// This increases the force pushing against the player if they have exceeded their "boost" speed
        //if (rb2D.velocity.x >= maxBoostedSpeed || rb2D.velocity.x <= -maxBoostedSpeed)
        //{
        //    xDeccelerateFactor = deccelerationPower;
        //}
        //else
        //{
        //    xDeccelerateFactor = 1;
        //}


        double distance = Mathf.Sqrt((Mathf.Pow(targetPos.x - currentPos.x, 2) + Mathf.Pow(targetPos.y - currentPos.y, 2)));

        if (distance < 0.5)
        {
            yInput = 0;
            xInput = 0;
        }

    }

    private void DampBackTo0v()
    {
        // if player inputs a direction opposite of their x velocity or neutral then their velocity will damp back to 0

        if (xInput == 0)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
        }
        if (rb2D.velocity.x > 0 && xInput == -1)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
        }
        if (rb2D.velocity.x < 0 && xInput == 1)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(0, rb2D.velocity.y), ref velocity, pivotTime);
        }


        if (yInput == 0)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(rb2D.velocity.x, 0), ref velocity, pivotTime);
        }
        if (rb2D.velocity.y > 0 && yInput == -1)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(rb2D.velocity.x, 0), ref velocity, pivotTime);
        }
        if (rb2D.velocity.y < 0 && yInput == 1)
        {
            rb2D.velocity = Vector2.SmoothDamp(rb2D.velocity, new Vector2(rb2D.velocity.x, 0), ref velocity, pivotTime);
        }
    }

    public Vector2 GetFacingDirection()
    {
        return new Vector2(xInput, yInput);
    }
}
