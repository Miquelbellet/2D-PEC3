using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMovementScript : MonoBehaviour
{
    public float movementForce, jumpForce, jetpackUpDivider, jetpackMovementMultiplier;
    [HideInInspector] public bool isUsingJetpack, allowMovement;

    private GameControllerScript gameController;
    private WormAnimationsScript animScript;
    private SoundEffectsScript soundScript;
    private bool isInGround;
    void Start()
    {
        InitParams();
    }

    void Update()
    {
        if (gameController.playingWithAI && gameController.activeWorm.GetComponent<WormHealthScript>().teamNumber == 2) return;

        if (gameController.activeWorm == gameObject)
        {
            if (allowMovement && !GetComponent<WormHealthScript>().isDead)
                Movement();
        }
        else animScript.isWalking = false;
    }
    void InitParams()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        soundScript = GameObject.FindWithTag("GameController").GetComponent<SoundEffectsScript>();
        animScript = GetComponent<WormAnimationsScript>();
        allowMovement = true;
    }
    void Movement()
    {
        if (isInGround && !isUsingJetpack && Input.GetKeyDown(KeyCode.W))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            soundScript.JumpClip();
            isInGround = false;
            animScript.isJumping = true;
        }
        if (isUsingJetpack && Input.GetKey(KeyCode.W))
        {
            soundScript.JetpackClip();
            if(GetComponent<Rigidbody2D>().velocity.y <= 4)GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce / jetpackUpDivider));
            animScript.VerticalFire(true);
        }
        else animScript.VerticalFire(false);

        if (Input.GetAxis("Horizontal") > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animScript.isWalking = true;
            if (!isUsingJetpack)
            {
                soundScript.WalkClip();
                transform.position = new Vector2(transform.position.x + movementForce, transform.position.y);
            }
            if (isUsingJetpack)
            {
                soundScript.JetpackClip();
                transform.position = new Vector2(transform.position.x + movementForce * jetpackMovementMultiplier, transform.position.y);
                transform.GetChild(0).localPosition = new Vector2(-0.163f, -0.035f);
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                transform.GetChild(1).localPosition = new Vector2(-0.079f, -0.161f);
                animScript.HorizontalFire(true);
            }

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animScript.isWalking = true;
            if (!isUsingJetpack)
            {
                soundScript.WalkClip();
                transform.position = new Vector2(transform.position.x - movementForce, transform.position.y);
            }
            if (isUsingJetpack)
            {
                soundScript.JetpackClip();
                transform.position = new Vector2(transform.position.x - movementForce * jetpackMovementMultiplier, transform.position.y);
                transform.GetChild(0).localPosition = new Vector2(0.161f, -0.027f);
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                transform.GetChild(1).localPosition = new Vector2(0.079f, -0.158f);
                animScript.HorizontalFire(true);
            }
        }
        else
        {
            soundScript.SetNormalPitchAudio();
            animScript.isWalking = false;
            if(isUsingJetpack) animScript.HorizontalFire(false);
        }
    }
    public void MovmentForAI(string direction, bool jump)
    {
        if (isInGround && !isUsingJetpack && jump)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
            soundScript.JumpClip();
            isInGround = false;
            animScript.isJumping = true;
        }
        if (isUsingJetpack && Input.GetKey(KeyCode.W))
        {
            soundScript.JetpackClip();
            if (GetComponent<Rigidbody2D>().velocity.y <= 4) GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce / jetpackUpDivider));
            animScript.VerticalFire(true);
        }
        else animScript.VerticalFire(false);

        if (direction == "Right")
        {
            GetComponent<SpriteRenderer>().flipX = true;
            animScript.isWalking = true;
            if (!isUsingJetpack)
            {
                soundScript.WalkClip();
                transform.position = new Vector2(transform.position.x + movementForce, transform.position.y);
            }
            if (isUsingJetpack)
            {
                soundScript.JetpackClip();
                transform.position = new Vector2(transform.position.x + movementForce * jetpackMovementMultiplier, transform.position.y);
                transform.GetChild(0).localPosition = new Vector2(-0.163f, -0.035f);
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                transform.GetChild(1).localPosition = new Vector2(-0.079f, -0.161f);
                animScript.HorizontalFire(true);
            }

        }
        else if (direction == "Left")
        {
            GetComponent<SpriteRenderer>().flipX = false;
            animScript.isWalking = true;
            if (!isUsingJetpack)
            {
                soundScript.WalkClip();
                transform.position = new Vector2(transform.position.x - movementForce, transform.position.y);
            }
            if (isUsingJetpack)
            {
                soundScript.JetpackClip();
                transform.position = new Vector2(transform.position.x - movementForce * jetpackMovementMultiplier, transform.position.y);
                transform.GetChild(0).localPosition = new Vector2(0.161f, -0.027f);
                transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                transform.GetChild(1).localPosition = new Vector2(0.079f, -0.158f);
                animScript.HorizontalFire(true);
            }
        }
        else
        {
            soundScript.SetNormalPitchAudio();
            animScript.isWalking = false;
            if (isUsingJetpack) animScript.HorizontalFire(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isInGround = true;
            animScript.isJumping = false;
        }
    }
}
