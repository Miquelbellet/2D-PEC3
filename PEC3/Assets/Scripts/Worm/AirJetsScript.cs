using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirJetsScript : MonoBehaviour
{
    public float airJetsVelocity;
    public GameObject airJetMissilesPrefab;
    void Start()
    {
        transform.position = new Vector2(15, 3);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x - airJetsVelocity, transform.position.y);
        if (transform.position.x < -15) Destroy(gameObject);
    }
    public void ActivateMissiles()
    {
        GameObject airJetMissiles = Instantiate(airJetMissilesPrefab, transform.position, Quaternion.Euler(new Vector2(0, 0)));
        Destroy(airJetMissiles, 5f);
    }
}
