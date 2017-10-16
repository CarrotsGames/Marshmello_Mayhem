using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public bool isSwinging;

    public GameObject meleeHitbox;
    public float rotationSpeed;

    public float timeBetweenSwing;
    public float swingTimer;
    private float swingCount;
    public float rotateDegrees;
    public Transform swingPoint;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSwinging)
        {
            swingCount -= Time.deltaTime;
            if (swingCount <= 0)
            {
                swingCount = timeBetweenSwing;
                GameObject newAttack = Instantiate(meleeHitbox, swingPoint.position, (swingPoint.rotation)) as GameObject;


                newAttack.GetComponent<Rigidbody>().AddForce(newAttack.transform.forward * rotationSpeed, ForceMode.Impulse);
                swingTimer += 0.1f * Time.deltaTime;

                if (swingTimer >= 1)
                {
                    DestroyObject(meleeHitbox.gameObject);
                }
            }
        }
        else
        {
            swingCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (meleeHitbox.gameObject.tag == "Player")
        {
            DestroyObject(meleeHitbox.gameObject);
        }
    }
}
