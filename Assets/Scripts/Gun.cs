using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checking to see if the player is firing and if they're not firing.

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsInvoking("FireBullet"))
            {
                InvokeRepeating("FireBullet", 0f, 0.1f);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke("FireBullet");
        }

    }

    public GameObject bulletPrefab;
    public Transform launchPosition;
    void FireBullet()
    {
        // Creates instance of bullet prefab
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        // Where the bullet launches from
        bullet.transform.position = launchPosition.position;
        // The bullet is a constant speed thanks to the rigid body
        bullet.GetComponent<Rigidbody>().velocity =
        transform.parent.forward * 100;
    }


}
