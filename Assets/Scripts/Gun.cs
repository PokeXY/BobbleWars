using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform launchPosition;
    public bool isUpgraded;
    public float upgradeTime = 10.0f;
    private float currentTime;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(SoundManager.Instance.gunFire);
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

        currentTime += Time.deltaTime;
        if (currentTime > upgradeTime && isUpgraded == true)
        {
            isUpgraded = false;
        }

    }

    //void FireBullet()
    //{
    //    // Creates instance of bullet prefab
    //    GameObject bullet = Instantiate(bulletPrefab) as GameObject;
    //    // Where the bullet launches from
    //    bullet.transform.position = launchPosition.position;
    //    // The bullet is a constant speed thanks to the rigid body
    //    bullet.GetComponent<Rigidbody>().velocity =
    //    transform.parent.forward * 100;
    //}
    void FireBullet()
    {
        Rigidbody bullet = createBullet();
        bullet.velocity = transform.parent.forward * 100;

        if (isUpgraded)
        {
            Rigidbody bullet2 = createBullet();
            bullet2.velocity =
            (transform.right + transform.forward / 0.5f) * 100;
            Rigidbody bullet3 = createBullet();
            bullet3.velocity =
            ((transform.right * -1) + transform.forward / 0.5f) * 100;
        }

        if (isUpgraded)
        {
            audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
        }
        else
        {
            audioSource.PlayOneShot(SoundManager.Instance.gunFire);
        }
    }

    private Rigidbody createBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = launchPosition.position;
        return bullet.GetComponent<Rigidbody>();
    }

    public void UpgradeGun()
    {
        isUpgraded = true;
        currentTime = 0;
    }



}
