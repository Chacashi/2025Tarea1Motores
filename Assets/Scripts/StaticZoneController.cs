
using System.Collections;
using UnityEngine;
using System;

public class StaticZoneController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 1f;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private bool PlayerTakeZone = false;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int timeForShoot;
    private Coroutine shootBulletCoroutine;

    private void Update()
    {
        if (PlayerTakeZone)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, velocityModifier*Time.deltaTime);

        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, initialPosition, velocityModifier *Time.deltaTime);          
        }
    }

    private IEnumerator loopShoot()
    {
        while (true)
        {
            ShootBullet();
            yield return new WaitForSeconds(timeForShoot);
        }
       
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody2D>().velocity = (playerTarget.position - transform.position).normalized * bulletSpeed;
        Destroy(bullet,10);
    }
    

    void ActiveActionsOfDetectorZone()
    {
        shootBulletCoroutine = StartCoroutine(loopShoot());
        PlayerTakeZone = true;
    }

    void DesactiveActionsOfDetectorZone()
    {
        PlayerTakeZone = false;
        StopCoroutine(shootBulletCoroutine);
    }

    private void OnEnable()
    {
        DetectorCotroller.OnDetectorActivate += ActiveActionsOfDetectorZone;
        DetectorCotroller.OnDetectorDesactivate += DesactiveActionsOfDetectorZone;
    }
    private void OnDisable()
    {
        DetectorCotroller.OnDetectorActivate -= ActiveActionsOfDetectorZone;
        DetectorCotroller.OnDetectorDesactivate -= DesactiveActionsOfDetectorZone;
    }
}




