using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WormAnimationsScript : MonoBehaviour
{
    public Sprite[] bazookaPointing;
    public Sprite[] uziAiming;
    public Sprite[] uziSooting;
    [HideInInspector] public bool isWalking, isJumping;

    private Animator animController, animFireHorizontal, animFireVertical;
    private WormHealthScript healthScript;
    private int currentUziAimSprite, shootingUziCount;
    private bool isUsingWeapon;
    private void Awake()
    {
        InitParams();
    }
    void Start()
    {
        InvokeRepeating("SelectIdleAnim", 2f, Random.Range(10f, 20f));
    }

    void Update()
    {
        if (!isUsingWeapon) MovementAnimations();
        if (animController.GetCurrentAnimatorStateInfo(0).IsName("DeadExplosion")) healthScript.WormAnimationDied();

    }
    void InitParams()
    {
        animController = GetComponent<Animator>();
        healthScript = GetComponent<WormHealthScript>();
        animFireHorizontal = transform.GetChild(0).GetComponent<Animator>();
        animFireVertical = transform.GetChild(1).GetComponent<Animator>();
    }
    void MovementAnimations()
    {
        if (isJumping) animController.SetBool("jumping", true);
        else animController.SetBool("jumping", false);

        if (isWalking) animController.SetBool("walking", true);
        else animController.SetBool("walking", false);
    }
    void SelectIdleAnim()
    {
        if (animController.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            var numAnim = Random.Range(0, 6);
            animController.SetInteger("numIdleAnim", numAnim);
            animController.SetTrigger("doIdleAnim");
        }
    }
    public void UsingWeapon(bool isUsing)
    {
        animController.SetBool("isUsingWeapon", isUsing);
        isUsingWeapon = isUsing;
    }
    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
    public void ActivateAnimator()
    {
        GetComponent<Animator>().enabled = true;
    }
    public void SetSpritesAiming(float angle)
    {
        int numSprite = (((int)angle / 6) - 15) * -1;
        if (numSprite >= 0 && numSprite <= 29)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<SpriteRenderer>().sprite = bazookaPointing[numSprite];
        }
        else if (numSprite < 0 && numSprite >= -14)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<SpriteRenderer>().sprite = bazookaPointing[numSprite * -1];
        }
        else if (numSprite >= 30 && numSprite <= 44)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (numSprite == 44) GetComponent<SpriteRenderer>().sprite = bazookaPointing[15];
            if (numSprite == 43) GetComponent<SpriteRenderer>().sprite = bazookaPointing[16];
            if (numSprite == 42) GetComponent<SpriteRenderer>().sprite = bazookaPointing[17];
            if (numSprite == 41) GetComponent<SpriteRenderer>().sprite = bazookaPointing[18];
            if (numSprite == 40) GetComponent<SpriteRenderer>().sprite = bazookaPointing[19];
            if (numSprite == 39) GetComponent<SpriteRenderer>().sprite = bazookaPointing[20];
            if (numSprite == 38) GetComponent<SpriteRenderer>().sprite = bazookaPointing[21];
            if (numSprite == 37) GetComponent<SpriteRenderer>().sprite = bazookaPointing[22];
            if (numSprite == 36) GetComponent<SpriteRenderer>().sprite = bazookaPointing[23];
            if (numSprite == 35) GetComponent<SpriteRenderer>().sprite = bazookaPointing[24];
            if (numSprite == 34) GetComponent<SpriteRenderer>().sprite = bazookaPointing[25];
            if (numSprite == 33) GetComponent<SpriteRenderer>().sprite = bazookaPointing[26];
            if (numSprite == 32) GetComponent<SpriteRenderer>().sprite = bazookaPointing[27];
            if (numSprite == 31) GetComponent<SpriteRenderer>().sprite = bazookaPointing[28];
            if (numSprite == 30) GetComponent<SpriteRenderer>().sprite = bazookaPointing[29];
        }
    }
    public void SetSpritesUziAiming(float angle)
    {
        if (animController.GetCurrentAnimatorStateInfo(0).IsName("SettingUzi")) DisableAnimator();
        int numSprite = (((int)angle / 6) - 15) * -1;
        currentUziAimSprite = numSprite;
        if (numSprite >= 0 && numSprite <= 29)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<SpriteRenderer>().sprite = uziAiming[numSprite];
        }
        else if (numSprite < 0 && numSprite >= -14)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<SpriteRenderer>().sprite = uziAiming[numSprite * -1];
        }
        else if (numSprite >= 30 && numSprite <= 44)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (numSprite == 44) GetComponent<SpriteRenderer>().sprite = uziAiming[15];
            if (numSprite == 43) GetComponent<SpriteRenderer>().sprite = uziAiming[16];
            if (numSprite == 42) GetComponent<SpriteRenderer>().sprite = uziAiming[17];
            if (numSprite == 41) GetComponent<SpriteRenderer>().sprite = uziAiming[18];
            if (numSprite == 40) GetComponent<SpriteRenderer>().sprite = uziAiming[19];
            if (numSprite == 39) GetComponent<SpriteRenderer>().sprite = uziAiming[20];
            if (numSprite == 38) GetComponent<SpriteRenderer>().sprite = uziAiming[21];
            if (numSprite == 37) GetComponent<SpriteRenderer>().sprite = uziAiming[22];
            if (numSprite == 36) GetComponent<SpriteRenderer>().sprite = uziAiming[23];
            if (numSprite == 35) GetComponent<SpriteRenderer>().sprite = uziAiming[24];
            if (numSprite == 34) GetComponent<SpriteRenderer>().sprite = uziAiming[25];
            if (numSprite == 33) GetComponent<SpriteRenderer>().sprite = uziAiming[26];
            if (numSprite == 32) GetComponent<SpriteRenderer>().sprite = uziAiming[27];
            if (numSprite == 31) GetComponent<SpriteRenderer>().sprite = uziAiming[28];
            if (numSprite == 30) GetComponent<SpriteRenderer>().sprite = uziAiming[29];
        }
    }
    public void SetSpriteShotUzi()
    {
        shootingUziCount++;
        int numSprite = currentUziAimSprite;
        if (shootingUziCount % 5 == 0)
        {
            if (numSprite >= 0 && numSprite <= 29)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().sprite = uziSooting[numSprite];
            }
            else if (numSprite < 0 && numSprite >= -14)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<SpriteRenderer>().sprite = uziSooting[numSprite * -1];
            }
            else if (numSprite >= 30 && numSprite <= 44)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (numSprite == 44) GetComponent<SpriteRenderer>().sprite = uziSooting[15];
                if (numSprite == 43) GetComponent<SpriteRenderer>().sprite = uziSooting[16];
                if (numSprite == 42) GetComponent<SpriteRenderer>().sprite = uziSooting[17];
                if (numSprite == 41) GetComponent<SpriteRenderer>().sprite = uziSooting[18];
                if (numSprite == 40) GetComponent<SpriteRenderer>().sprite = uziSooting[19];
                if (numSprite == 39) GetComponent<SpriteRenderer>().sprite = uziSooting[20];
                if (numSprite == 38) GetComponent<SpriteRenderer>().sprite = uziSooting[21];
                if (numSprite == 37) GetComponent<SpriteRenderer>().sprite = uziSooting[22];
                if (numSprite == 36) GetComponent<SpriteRenderer>().sprite = uziSooting[23];
                if (numSprite == 35) GetComponent<SpriteRenderer>().sprite = uziSooting[24];
                if (numSprite == 34) GetComponent<SpriteRenderer>().sprite = uziSooting[25];
                if (numSprite == 33) GetComponent<SpriteRenderer>().sprite = uziSooting[26];
                if (numSprite == 32) GetComponent<SpriteRenderer>().sprite = uziSooting[27];
                if (numSprite == 31) GetComponent<SpriteRenderer>().sprite = uziSooting[28];
                if (numSprite == 30) GetComponent<SpriteRenderer>().sprite = uziSooting[29];
            }
        }
        else
        {
            if (numSprite >= 0 && numSprite <= 29)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                GetComponent<SpriteRenderer>().sprite = uziAiming[numSprite];
            }
            else if (numSprite < 0 && numSprite >= -14)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                GetComponent<SpriteRenderer>().sprite = uziAiming[numSprite * -1];
            }
            else if (numSprite >= 30 && numSprite <= 44)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                if (numSprite == 44) GetComponent<SpriteRenderer>().sprite = uziAiming[15];
                if (numSprite == 43) GetComponent<SpriteRenderer>().sprite = uziAiming[16];
                if (numSprite == 42) GetComponent<SpriteRenderer>().sprite = uziAiming[17];
                if (numSprite == 41) GetComponent<SpriteRenderer>().sprite = uziAiming[18];
                if (numSprite == 40) GetComponent<SpriteRenderer>().sprite = uziAiming[19];
                if (numSprite == 39) GetComponent<SpriteRenderer>().sprite = uziAiming[20];
                if (numSprite == 38) GetComponent<SpriteRenderer>().sprite = uziAiming[21];
                if (numSprite == 37) GetComponent<SpriteRenderer>().sprite = uziAiming[22];
                if (numSprite == 36) GetComponent<SpriteRenderer>().sprite = uziAiming[23];
                if (numSprite == 35) GetComponent<SpriteRenderer>().sprite = uziAiming[24];
                if (numSprite == 34) GetComponent<SpriteRenderer>().sprite = uziAiming[25];
                if (numSprite == 33) GetComponent<SpriteRenderer>().sprite = uziAiming[26];
                if (numSprite == 32) GetComponent<SpriteRenderer>().sprite = uziAiming[27];
                if (numSprite == 31) GetComponent<SpriteRenderer>().sprite = uziAiming[28];
                if (numSprite == 30) GetComponent<SpriteRenderer>().sprite = uziAiming[29];
            }
        }    }
    public void PutJetpack()
    {
        animController.SetTrigger("putJetpack");
    }
    public void TakeoffJetpack()
    {
        animController.SetTrigger("takeoffJetpack");
    }
    public void HorizontalFire(bool activate)
    {
        animFireHorizontal.SetBool("jetpackFlyingH", activate);
    }
    public void VerticalFire(bool activate)
    {
        animFireVertical.SetBool("jetpackFlyingV", activate);
    }
    public void PutBazooka()
    {
        animController.SetTrigger("putBazooka");
    }
    public void TakeoffBazooka()
    {
        animController.SetTrigger("takeoffBazooka");
    }
    public void PutAirAttack()
    {
        animController.SetTrigger("putAirAttack");
    }
    public void TakeOffAirAttack()
    {
        animController.SetTrigger("takeoffAirAttack");
    }
    public void CallAirAttack()
    {
        animController.SetTrigger("callAirAttack");
    }
    public void PuAxHit()
    {
        animController.SetTrigger("putAx");
    }
    public void AttackAx()
    {
        animController.SetTrigger("attackAx");
    }
    public void PutUzi()
    {
        animController.SetTrigger("putUzi");
    }
    public void TakeoffUzi()
    {
        animController.SetTrigger("takeOffUzi");
    }
    public void Surrender()
    {
        animController.SetTrigger("surrenderFlag");
    }
    public void Winner()
    {
        animController.SetTrigger("winner");
    }
    public void Died()
    {
        animController.SetTrigger("died");
    }
}
