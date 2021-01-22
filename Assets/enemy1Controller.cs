using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class enemy1Controller : MonoBehaviour
{
    public GameObject deathParts;
    public GameObject AudioManager;

    public LayerMask playerLayers;

    public Transform attackPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBetweenAttacks;
    public float health;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange, isRunning = false, isStunned = false;
    private Vector3 PlayerPos;

    void Awake()
    {
        AudioManager = GameObject.Find("AudioManager");
        player = GameObject.Find("ThirdPersonPlayer").transform;
        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if(!playerInAttackRange && !playerInSightRange)
        {
            isRunning = false;
            animator.SetBool("isPlayerInSight", false);
            agent.SetDestination(transform.position);
            transform.parent.gameObject.GetComponent<AudioSource>().Stop();
        }
        else
        if (playerInSightRange && !playerInAttackRange && !alreadyAttacked && !isStunned)
        {
            animator.SetBool("isPlayerInSight", true);
            ChasePlayer();
            if (!isRunning)
            {
                isRunning = true;
                animator.Play("enemy1Run");
                transform.parent.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        if (playerInSightRange && playerInAttackRange && !isStunned) AttackPlayer();
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        PlayerPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(PlayerPos);
    }

    void AttackPlayer()
    {
        transform.parent.gameObject.GetComponent<AudioSource>().Stop();
        isRunning = false;
        agent.SetDestination(transform.position);

        PlayerPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(PlayerPos);
        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.Play("enemy1Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        animator.Play("enemy1TakeHit");
        isStunned = true;
        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        transform.parent.gameObject.GetComponent<AudioSource>().Stop();
        Instantiate(deathParts, transform.position, transform.rotation);
        player.Find("smith").GetComponent<ThirdPersonMovement>().kills++;
        Destroy(gameObject);
        
    }

    public void ResetStun()
    {
        isStunned = false;
    }

    public void DamagePlayer()
    {
        //hitCheck
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayers);

        //Damage
        foreach (Collider player in hitPlayer)
        {
            player.gameObject.GetComponent<ThirdPersonMovement>().TakeDamage(25);
        }
    }
}
