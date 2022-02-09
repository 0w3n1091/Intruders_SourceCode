using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float startTimeBetweenAttacks;
    public float timeBetweenAttacks;
    public AttackPointsProvider attackPointsProvider;
    public Transform playerPosition;
    public LayerMask enemiesLayer;
    public int damage;

    private Animator animator;
    private AttackPoints attackPoints;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerControl = GetComponent<PlayerControl>();
    }
    void Update()
    {
        if (timeBetweenAttacks <= 0 && GlobalParams.weaponDurability > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackPoints = attackPointsProvider.GetAttackPoints(playerControl.lastTriggerName);
                Debug.Log($"{playerControl.lastTriggerName}");

                animator.SetTrigger(AnimationParameters.ATTACK);
                Collider2D[] enemiesToDamage = Physics2D.OverlapAreaAll(attackPoints.A.position,
                                                                        attackPoints.B.position,
                                                                        enemiesLayer);

                foreach (Collider2D enemy in enemiesToDamage)
                {
                    enemy.GetComponent<IAttackable>().ReceveAttack(damage);

                    Vector3 knockbackDirection = enemy.transform.position - transform.position;
                    enemy.attachedRigidbody.AddForce(knockbackDirection.normalized * 250f);
                    GetComponent<PlayerControl>().UpdateWeaponDurability(1);

                }

                AudioManager.Instance.swordHit.Play();

                timeBetweenAttacks = startTimeBetweenAttacks;
            }
        }
        else
        {
            timeBetweenAttacks -= Time.deltaTime;
        }
    }

    private Vector3 RotateAPointOnACircle(int angle, Transform attackPoint)
    {
        float distance = Vector3.Distance(transform.position, attackPoint.position);

        float x = distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = distance * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3(x, y, 0);
    }
    private PlayerControl playerControl;
    public string GetAnimationTrigger()
    {
        if (playerControl.horizontal > 0.5)
            return AnimationParameters.RIGHT;

        if (playerControl.horizontal < -0.5)
            return AnimationParameters.LEFT;

        if (playerControl.vertical > 0.5)
            return AnimationParameters.UP;

        if (playerControl.vertical < -0.5)
            return AnimationParameters.DOWN;

        return null;
    }
}
