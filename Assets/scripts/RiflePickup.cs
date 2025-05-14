using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public GameObject PickupRifle;
    public PlayerPunch playerPunch;

    [Header("Rifle Assign Things")]
    public PlayerScript player;

    private float radius = 2.5f;
    private float nextTimeToPunch = 0f;
    public float punchCharge = 15f;

    private void Awake()
    {
        PlayerRifle.SetActive(false); // السلاح في يد اللاعب مغلق بالبداية
    }

    private void Update()
    {
        // تنفيذ الضربة لو ضغط Fire1 (مثلاً كليك شمال) مع تأخير بين كل ضربة
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToPunch)
        {
            nextTimeToPunch = Time.time + 1f / punchCharge;
            playerPunch.Punch();
        }

        // لو اللاعب قريب من السلاح
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            // وضغط F
            if (Input.GetKeyDown(KeyCode.F))
            {
                PlayerRifle.SetActive(true);     // تفعيل السلاح في يد اللاعب
                PickupRifle.SetActive(false);    // إخفاء السلاح اللي على الأرض

                // تقدر تضيف صوت أو إكمال مهمة هنا
                // AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                // ObjectiveManager.Complete("Picked up rifle");
            }
        }
    }
}
