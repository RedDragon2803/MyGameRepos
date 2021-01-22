using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Animator animator;
    public GameObject AudioManager;
    public Transform AttackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    bool comboPossible;
    bool someOneHited = false;
    int comboStep;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (comboStep == 0)
        {
            //anim
            animator.Play("attack1");
            comboStep = 1;
            AudioManager.GetComponent<AudioManager>().Play("slash");

            //hitCheck
            Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, attackRange, enemyLayers);

            //Damage
            foreach (Collider enemy in hitEnemies)
            {
                someOneHited = true;
                enemy.gameObject.GetComponent<enemy1Controller>().TakeDamage(25);
            }
            if (someOneHited)
            {
                Invoke(nameof(PlaySwordHit), 0.5f);
                someOneHited = false;
            }
            return;
        }
        else
        {
            if (comboPossible)
            {
                comboPossible = false;
                comboStep++;
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
        {
            animator.Play("attack2");

            //hitCheck
            Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, attackRange, enemyLayers);

            //Damage
            foreach (Collider enemy in hitEnemies)
            {
                someOneHited = true;
                enemy.gameObject.GetComponent<enemy1Controller>().TakeDamage(25);
            }
            if (someOneHited)
            {
                Invoke(nameof(PlaySwordHit), 0.5f);
                someOneHited = false;
            }
        }
        if (comboStep == 3)
        {
            animator.Play("attack3");

            //hitCheck
            Collider[] hitEnemies = Physics.OverlapSphere(AttackPoint.position, attackRange, enemyLayers);

            //Damage
            foreach (Collider enemy in hitEnemies)
            {
                someOneHited = true;
                enemy.gameObject.GetComponent<enemy1Controller>().TakeDamage(50);
            }
            if (someOneHited)
            {
                Invoke(nameof(PlaySwordHit), 0.5f);
                someOneHited = false;
            }
        }
    }

    public void ComboReset()
    {
        comboPossible = false;
        comboStep = 0;
    }

    public void Play3()
    {
        AudioManager.GetComponent<AudioManager>().Play("slash3");
    }

    public void Play2()
    {
        AudioManager.GetComponent<AudioManager>().Play("slash2");
    }

    void OnDrawGizmosSelected()
    {
        if (AttackPoint == null) return;
        Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    }

    private void PlaySwordHit()
    {
        AudioManager.GetComponent<AudioManager>().Play("SwordHit");
    }
}
