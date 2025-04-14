using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class PatrolMovementController : MonoBehaviour
{
    [SerializeField] private Transform[] checkpointsPatrol;
    [SerializeField] private Rigidbody2D myRBD2;
    [SerializeField] private AnimatorController animatorController;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float velocityModifier = 1f;
    [SerializeField] private Transform playerTarget;
    private Transform currentPositionTarget;
    private int patrolPos = 0;

    [Header("Raycast")]
    [SerializeField] private Color collision;
    [SerializeField] private Color notCollision;
    [SerializeField] private LayerMask layerScorpion;
    [SerializeField] private float distanceRay;
     private RaycastHit2D hit;
    public static event Action<string> OnEnemyPatrolTakeBullet;

    private void Start() {
        currentPositionTarget = checkpointsPatrol[patrolPos];
        transform.position = currentPositionTarget.position;
    }

    private void Update() {

        CheckNewPoint();
        hit = Physics2D.Raycast(transform.position, (currentPositionTarget.position - transform.position).normalized, distanceRay, layerScorpion);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, hit.distance * (playerTarget.position - transform.position).normalized, collision);
            velocityModifier = 8f;
            myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized * velocityModifier;

        }
        else
        {
            Debug.DrawRay(transform.position, distanceRay * (currentPositionTarget.position - transform.position).normalized, notCollision);
            velocityModifier = 1f;
            

        }
        

        animatorController.SetVelocity(velocityCharacter: myRBD2.velocity.magnitude);
    }

    private void CheckNewPoint(){
        if(Mathf.Abs((transform.position - currentPositionTarget.position).magnitude) < 0.25){
            patrolPos = patrolPos + 1 == checkpointsPatrol.Length? 0: patrolPos+1;
            currentPositionTarget = checkpointsPatrol[patrolPos];
            myRBD2.velocity = (currentPositionTarget.position - transform.position).normalized*velocityModifier;
            CheckFlip(myRBD2.velocity.x);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletPlayer"))
        {
            OnEnemyPatrolTakeBullet?.Invoke("BarEnemyPatrol");
            Destroy(collision.gameObject);
        }
    }

    private void CheckFlip(float x_Position){
        spriteRenderer.flipX = (x_Position - transform.position.x) < 0;
    }
}
