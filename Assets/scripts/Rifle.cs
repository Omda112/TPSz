using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 15f;
    public float nextTimeToShoot = 0f;
    public PlayerScript player;
    public Transform hand;

    [Header("Rifle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject woodedEffect;


    [Header("Rifle Ammunition and shooting")] 
    private int maximumAmmunition = 32;
    public int mag = 10;
    private int presentAmmunition;
    public float reloadingTime = 1.3f;
    private bool setReloading = false;

    private void Awake(){
        transform.SetParent(hand);
        presentAmmunition = maximumAmmunition;
    }
    private void Update()
    {

        if(setReloading){
            return;
        } 

        if(presentAmmunition <= 0){
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time>= nextTimeToShoot){
                nextTimeToShoot = Time.time +1f/fireCharge;
                Shoot(); 
            }
    }

    private void Shoot()
    {

        
        //check for mag
        if(mag == 0)
        {
            return;
            //show ammo out text
        }
        presentAmmunition--;
        if(presentAmmunition == 0)
        {
            mag--;
        }
        //updating the UI


        muzzleSpark.Play();
        RaycastHit hitInfo;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);

                // إنشاء تأثير الضرب في مكان الضربة
                GameObject woodGo = Instantiate(woodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(woodGo, 1f); // تدمير التأثير بعد ثانية
            }
        }
    }

    IEnumerator Reload()
{
    // تعطيل السرعة أثناء إعادة التلقيم
    player.playerSpeed = 0f;
    player.playerSprint = 0f;

    setReloading = true;
    Debug.Log("Reloading...");

    // تشغيل أنيميشن إعادة التلقيم (لو موجود)
    // animator.SetTrigger("Reload");

    // تشغيل صوت إعادة التلقيم (لو موجود)
    // audioSource.PlayOneShot(reloadSound);

    yield return new WaitForSeconds(reloadingTime);

    // إعادة ملء الذخيرة
    presentAmmunition = maximumAmmunition;

    // إعادة السرعة
    player.playerSpeed = 1.9f;
    player.playerSprint = 3f;

    setReloading = false;
}

}
