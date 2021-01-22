using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;
    public GameObject AudioManager;
    public HealthBar healthBar;
    public float health = 100;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool isPlayingRun = false;
    bool ableToMove = true;
    Vector3 _velocity;
    
    public int kills = 0;

    void Awake()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (kills == 9)
        {
            Cursor.visible = true;
            SceneManager.LoadScene("Gratz");
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        animator.SetFloat("velocity", direction.magnitude);

        if (direction.magnitude >= 0.1f && ableToMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            if (!isPlayingRun)
            {
                AudioManager.GetComponent<AudioManager>().Play("PlayerRun");
                isPlayingRun = true;
            }

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            isPlayingRun = false;
            AudioManager.GetComponent<AudioManager>().Stop("PlayerRun");
        }
        _velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
        if (controller.isGrounded) _velocity.y = 0;
    }

    public void NotAbleMove()
    {
        ableToMove = false;
    }

    public void AbleMove()
    {
        ableToMove = true;
    }

    public void TakeDamage(int damage)
    {
        AudioManager.GetComponent<AudioManager>().Play("Oof");
        health -= damage;
        if (health <= 0)
        {
            Invoke(nameof(PlayerDeath), 0.5f);
            ableToMove = false;
        }
            healthBar.SetHealth(health);
        Debug.Log("taking damage!");
    }

    private void PlayerDeath()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("Game Over");
        Destroy(gameObject);
    }
}
