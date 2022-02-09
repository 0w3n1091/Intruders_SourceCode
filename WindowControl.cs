using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowControl : MonoBehaviour, IAttackable
{
    public int windowHealth;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public Collider2D collider2d;
    public Image progressBar;
    public const int MAX_HEALTH = 4;

    public Action OnDestroy
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public GameObject GameObject => gameObject;

    private void Start()
    {
        progressBar.fillAmount = 1f;
    }

    public IEnumerator BoardWindow()
    {
        yield return new WaitForSeconds(2f);

        sprite1.enabled = true;
        sprite2.enabled = true;
        collider2d.enabled = true;

        windowHealth++;
        progressBar.fillAmount += 0.25f;

        StartCoroutine("BoardWindow");
    }

    public void ReceveAttack(int damage)
    {
        windowHealth -= damage;
        progressBar.fillAmount = (float)windowHealth / MAX_HEALTH;

        if (windowHealth <= 0)
        {
            sprite1.enabled = false;
            sprite2.enabled = false;
            collider2d.enabled = false;
        }
    }
}
