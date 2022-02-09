using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KidControl : MonoBehaviour, IAttackable
{
    public HUD hud;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public Action OnDestroy
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    public GameObject GameObject => gameObject;

    public void ReceveAttack(int damage)
    {
        if (GlobalParams.kidHealth <= 0)
            return;

        AudioManager.Instance.kidHit.Play();

        animator.SetTrigger(AnimationParameters.HURT);
        GlobalParams.kidHealth -= damage;
        hud.UpdateKidHealth(damage);

        if (GlobalParams.kidHealth <= 0)
        {
            try
            {
                GameManager.Instance.highscore.SaveResult();
            }
            finally
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    private async Task DeathAsync(int delay)
    {
        animator.SetTrigger(AnimationParameters.DIE);
        await Task.Delay(delay);
        Destroy(gameObject);
    }
}
