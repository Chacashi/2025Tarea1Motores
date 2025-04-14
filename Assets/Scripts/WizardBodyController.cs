using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBodyController : MonoBehaviour
{
    public static event Action<string> OnEnemyStaticTakeBullet;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            OnEnemyStaticTakeBullet?.Invoke("BarEnemyStatic");
            Destroy(collision.gameObject);
        }

    }
}
