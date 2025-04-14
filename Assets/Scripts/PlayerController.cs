using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private float velocityModifier = 5f;
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject prefabGun;
    [SerializeField] private float speedGun;
    [SerializeField] private Vector2 positionMin;
    [SerializeField] private Vector2 positionMax;
    private Vector2 movementPlayer;
    public static event Action<string> OnPlayerTakeBullet;

    private void Update() {
        

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);

        Vector2 mouseInput = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        CheckFlip(mouseInput.x);
    
        Debug.DrawRay(transform.position, mouseInput.normalized * rayDistance, Color.red);
        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(prefabGun, transform.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = mouseInput.normalized * speedGun;
            Destroy(bullet,10);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameObject bullet = Instantiate(prefabGun);
            bullet.GetComponent<Rigidbody2D>().velocity = mouseInput.normalized * speedGun;
            Destroy(bullet, 10);
        }

    }

   
    private void FixedUpdate()
    {
        myRBD2.velocity = movementPlayer * velocityModifier;
        Vector2 clampedPosition = new Vector2(
            Mathf.Clamp(transform.position.x, positionMin.x, positionMax.x),
            Mathf.Clamp(transform.position.y, positionMin.y, positionMax.y)
        );

        transform.position = clampedPosition;

    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementPlayer = context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletWizard"))
        {
            OnPlayerTakeBullet?.Invoke("BarPlayer");
            Destroy(collision.gameObject);
        }


    }


    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
