using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IAttackable
{
    public HighScore score;
    public int initialHealth = 2;
    public float pushbackForce = 2f;
    public double attackPeriodInMillis = 1000f;
    public LayerMask enemyLayer;

    public TimeSpan attackTimeSpan;
    public Image healthBar;

    public Action OnDestroy
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public int Health => health;

    public GameObject GameObject => gameObject;

    private int health;

    private DateTime lastAttackTime;

    private Rigidbody2D rigidbody;
    private Animator animator;

    private void Awake()
    {
        attackTimeSpan = TimeSpan.FromMilliseconds(attackPeriodInMillis);
        health = initialHealth;

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void ReceveAttack(int damage)
    {

        health -= damage;
        UpdateHealthBar(damage);

        animator.SetTrigger(AnimationParameters.ATTACK);
        AudioManager.Instance.enemyHit.Play();

        if (health <= 0)
            _ = KillEnemyAsync(500);

    }
    private async Task KillEnemyAsync(int aDelay = 0)
    {
        animator.SetTrigger(AnimationParameters.DIE);
        await Task.Delay(aDelay);
        Destroy(this.gameObject);
        score.KillEnemy();
    }

    private void UpdateHealthBar(int aDamage)
    {
        healthBar.fillAmount -= 0.5f * aDamage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAttackableObjects(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckAttackableObjects(other);
    }

    private void CheckAttackableObjects(Collider2D other)
    {
        IAttackable attackableObj = other.GetComponent<IAttackable>();

        if (attackableObj != null && attackableObj.GameObject.layer != enemyLayer)
        {
            Attack(attackableObj, (other.transform.position - transform.position).normalized);
        }
    }

    private void Attack(IAttackable @object, Vector3 direction)
    {

        if (!IsLastAttackedDone())
            return;


        animator.SetTrigger(AnimationParameters.ATTACK);

        @object.ReceveAttack(1);
        rigidbody.AddForce(-pushbackForce * direction);

        lastAttackTime = DateTime.Now;
    }

    private bool IsLastAttackedDone()
    {
        return DateTime.Now - lastAttackTime >= attackTimeSpan;
    }
}