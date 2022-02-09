using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    public Rigidbody2D enemyRB;
    public int enemyHealth = 2;
    public Image healthBar;

    public Action OnDestroy
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }


    /// <summary>
    /// Takes damage after player attack
    /// </summary>
    public void TakeDamage(int aDamage, Transform aPlayerPosition)
    {
        AudioManager.Instance.enemyHit.Play();

        enemyHealth -= aDamage;
        UpdateHealthBar(aDamage);
        if (enemyHealth == 0)
        {
            _ = KillEnemyAsync();
            return;
        }

        KnockbackEnemy(aPlayerPosition.position);
    }

    /// <summary>
    /// Updates healthBar.fillAmount
    /// </summary>
    private void UpdateHealthBar(int aDamage)
    {
        healthBar.fillAmount -= 0.5f * aDamage;
    }

    /// <summary>
    /// Knockbacks enemy after collision with player
    /// </summary>
    private void KnockbackEnemy(Vector3 aPlayerPosition)
    {
        Vector3 knockbackDirection = aPlayerPosition - transform.position;
        enemyRB.AddForce(knockbackDirection.normalized * -25f);
        _ = ResetVelocityAsync(500);
    }

    /// <summary>
    /// Destroys Enemy gameObject after given time
    /// </summary>
    private async Task KillEnemyAsync(int aDelay = 0)
    {
        await Task.Delay(aDelay);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Resets enemy velocity after given time
    /// </summary>
    private async Task ResetVelocityAsync(int aDelay)
    {
        await Task.Delay(aDelay);
        enemyRB.velocity = Vector2.zero;
    }

    private void OnCollisionExit2D(Collision2D aCollision)
    {
        if (aCollision.gameObject.tag == "Player")
        {
            _ = ResetVelocityAsync(500);
        }
    }
}
