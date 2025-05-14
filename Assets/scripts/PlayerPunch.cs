using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Var")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float punchingRange = 5f;

    [Header("Punch Effects")]
    public GameObject woodedEffect;

    public void Punch()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, punchingRange))
        {
            Debug.Log(hitInfo.transform.name);

            // هل الجسم الذي ضربه يحتوي على سكريبت ObjectToHit؟
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);

                // إنشاء التأثير الخشبي في مكان الضربة
                GameObject woodGo = Instantiate(woodedEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(woodGo, 1f); // حذف التأثير بعد ثانية
            }
        }
    }
}
