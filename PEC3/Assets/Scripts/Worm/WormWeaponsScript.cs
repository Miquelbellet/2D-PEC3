using Dweiss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WormWeaponsScript : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject missilePrefab;
    public GameObject airJetsPrefab;
    public GameObject uziHitPrefab;

    [Header("Weapons configuration")]
    public float missileForce;
    public float maxMissileDistance;
    public float missileDamage;
    public float missileExplosionRange;
    public float axHitRange;
    public float uziBulletDamage;
    public float uziExplosionRadius;

    [HideInInspector] public enum weapons { None, AxHit, Jetpack, Uzi, Bazooka, AirAttack }
    weapons currentWeapon;
    List<ParticleCollisionEvent> collisionEvents;

    private GameControllerScript gameController;
    private WormAnimationsScript animScript;
    private WormMovementScript moveScript;
    private GameObject uziParticleSys;
    private bool isDebuging, canShoot, airJetsFlying, shootingUzi;

    List<ParticleSystem.Particle> particlesEnter = new List<ParticleSystem.Particle>();
    GameObject airJets;
    Vector3 attackAirJetPosition;
    float angleUziSooting;
    void Start()
    {
        initParams();
    }

    void Update()
    {
        WeaponsStateMachine();
    }

    void initParams()
    {
        animScript = GetComponent<WormAnimationsScript>();
        moveScript = GetComponent<WormMovementScript>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        GetComponent<LineRenderer>().enabled = false;
        collisionEvents = new List<ParticleCollisionEvent>();
        uziParticleSys = transform.GetChild(2).gameObject;
        uziParticleSys.SetActive(false);
    }
    void WeaponsStateMachine()
    {
        if (currentWeapon != weapons.None) animScript.UsingWeapon(true);
        else animScript.UsingWeapon(false);

        switch (currentWeapon)
        {
            case weapons.None:
                if (isDebuging) Debug.Log("None");
                break;
            case weapons.AxHit:
                if (isDebuging) Debug.Log("AxHit");
                AxHitState();
                break;
            case weapons.Jetpack:
                if (isDebuging) Debug.Log("Jetpack");
                break;
            case weapons.Uzi:
                if (isDebuging) Debug.Log("Uzi");
                UziState();
                break;
            case weapons.Bazooka:
                if (isDebuging) Debug.Log("Bazooka");
                BazookaState();
                break;
            case weapons.AirAttack:
                if (isDebuging) Debug.Log("AirAttack");
                AirAttackState();
                break;
            default:
                currentWeapon = weapons.None;
                break;
        }
    }
    public void ChangeWeaponState(string newWeaponStateStr)
    {
        weapons newWeaponState = weapons.None;
        try { newWeaponState = (weapons)System.Enum.Parse(typeof(weapons), newWeaponStateStr); }
        catch { newWeaponState = weapons.None; }
        if (currentWeapon == newWeaponState) return;

        animScript.ActivateAnimator();
        JetpackChange(newWeaponState);
        BazookaChange(newWeaponState);
        AirAttackChange(newWeaponState);
        AxHitChange(newWeaponState);
        UziChange(newWeaponState);
        currentWeapon = newWeaponState;
    }
    void JetpackChange(weapons newState)
    {
        if (newState == weapons.Jetpack)
        {
            animScript.PutJetpack();
            moveScript.isUsingJetpack = true;
        }
        else if (currentWeapon == weapons.Jetpack)
        {
            if (newState == weapons.None) animScript.TakeoffJetpack();
            moveScript.isUsingJetpack = false;
        }
    }
    void BazookaChange(weapons newState)
    {
        if (newState == weapons.Bazooka)
        {
            animScript.PutBazooka();
            moveScript.allowMovement = false;
            GetComponent<LineRenderer>().enabled = true;
        }
        else if (currentWeapon == weapons.Bazooka)
        {
            if (newState == weapons.None) animScript.TakeoffBazooka();
            moveScript.allowMovement = true;
            GetComponent<LineRenderer>().enabled = false;
        }
    }
    void AirAttackChange(weapons newState)
    {
        if (newState == weapons.AirAttack)
        {
            animScript.PutAirAttack();
            moveScript.allowMovement = false;
        }
        else if (currentWeapon == weapons.AirAttack)
        {
            if (newState == weapons.None) animScript.TakeOffAirAttack();
            moveScript.allowMovement = true;
        }
    }
    void AxHitChange(weapons newState)
    {
        if (newState == weapons.AxHit)
        {
            animScript.PuAxHit();
            moveScript.allowMovement = false;
        }
    }
    void UziChange(weapons newState)
    {
        if (newState == weapons.Uzi)
        {
            animScript.PutUzi();
            shootingUzi = false;
            moveScript.allowMovement = false;
        }
        else if (currentWeapon == weapons.Uzi)
        {
            if (newState == weapons.None) animScript.TakeoffUzi();
            moveScript.allowMovement = true;
        }
    }
    void AirAttackState()
    {
        if (Input.GetMouseButtonDown(0) && !airJetsFlying)
        {
            attackAirJetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            canShoot = true;
            foreach (RaycastResult result in results) if (result.gameObject.tag == "Panel") canShoot = false;
        }

        if (canShoot)
        {
            if (!airJetsFlying)
            {
                airJetsFlying = true;
                airJets = Instantiate(airJetsPrefab);
                animScript.ActivateAnimator();
                animScript.CallAirAttack();
            }
            if (airJets.transform.position.x <= attackAirJetPosition.x)
            {
                canShoot = false;
                airJetsFlying = false;
                airJets.GetComponent<AirJetsScript>().ActivateMissiles();
                ChangeWeaponState("None");
            }
        }
    }
    void BazookaState()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        animScript.SetSpritesAiming(angle);

        Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 velocity = BallisticVelocity(new Vector3(ray.x, ray.y, 0), angle);
        DrawParabolicLine(velocity, new Vector3(0, 0, 0));

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            bool canShoot = true;
            foreach (RaycastResult result in results) if (result.gameObject.tag == "Panel") canShoot = false;

            if (canShoot)
            {
                GameObject missile = Instantiate(missilePrefab, transform.position, rotation);
                missile.transform.position = new Vector2(transform.position.x, transform.position.y + 0.1f);
                missile.GetComponent<Rigidbody2D>().velocity = velocity;
                ChangeWeaponState("None");
            }
        }
    }
    void AxHitState()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x) GetComponent<SpriteRenderer>().flipX = false;
        else GetComponent<SpriteRenderer>().flipX = true;

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            bool canShoot = true;
            foreach (RaycastResult result in results) if (result.gameObject.tag == "Panel") canShoot = false;

            if (canShoot)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);
                    if (player.transform.position == transform.position) { }
                    else if (mousePos.x < transform.position.x && dist <= axHitRange && player.transform.position.x < transform.position.x)
                    {
                        player.GetComponent<WormHealthScript>().ReduceHealth(15);
                    }
                    else if (mousePos.x > transform.position.x && dist <= axHitRange && player.transform.position.x > transform.position.x)
                    {
                        player.GetComponent<WormHealthScript>().ReduceHealth(15);
                    }
                }
                animScript.AttackAx();
                ChangeWeaponState("None");
            }
        }
    }
    void UziState()
    {
        if (!shootingUzi)
        {
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            float angle = Mathf.Atan2(positionOnScreen.y - mouseOnScreen.y, positionOnScreen.x - mouseOnScreen.x) * Mathf.Rad2Deg;
            angleUziSooting = angle;
            animScript.SetSpritesUziAiming(angle);
        }
        else if (shootingUzi)
        {
            animScript.SetSpriteShotUzi();
            Quaternion uziParticleRot = Quaternion.Euler(new Vector3(0, 0, angleUziSooting));
            uziParticleSys.transform.rotation = uziParticleRot;
            uziParticleSys.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            bool canShoot = true;
            foreach (RaycastResult result in results) if (result.gameObject.tag == "Panel") canShoot = false;

            if (canShoot)
            {
                shootingUzi = true;
                Invoke("StopShooting", 3f);
            }
        }
    }
    void StopShooting()
    {
        uziParticleSys.SetActive(false);
        animScript.ActivateAnimator();
        ChangeWeaponState("None");
    }
    Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        if (angle < 0) angle *= -1;
        if (angle >= 100) { angle -= 90; angle /= 2; }
        if (angle < 40) angle = 40;
        if (angle > 80 && angle < 100) angle = 80;
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences
        if (dist > maxMissileDistance) dist = maxMissileDistance;
        else if (dist < 0.1) dist = 0.1f;
        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }
    void DrawParabolicLine(Vector3 addedV, Vector3 addedF)
    {
        var _rb = missilePrefab.GetComponent<Rigidbody2D>();
        var _lr = GetComponent<LineRenderer>();
        var res = _rb.CalculateMovement(100, 1, addedV, addedF);
        _lr.positionCount = 100;
        for (int i = 0; i < res.Length; ++i)
        {
            _lr.SetPosition(i, res[i]);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem uziPartSys = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>().activeWorm.transform.GetChild(2).GetComponent<ParticleSystem>();
        uziPartSys.GetCollisionEvents(gameObject, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            GameObject uziHit = Instantiate(uziHitPrefab, pos, Quaternion.Euler(new Vector2(0, 0)));
            gameController.FindExplosionHit(pos, uziBulletDamage, uziExplosionRadius);
            Destroy(uziHit, 1f);
        }
    }
}
