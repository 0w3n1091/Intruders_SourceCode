using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    public Animator animator;
    public HighScore highScore;
    public HUD hud;
    public Rigidbody2D playerRB;
    public Transform raycastOrigin;
    public Transform grabPoint;
    public List<GameObject> objectsInRange = new List<GameObject>();
    public float Speed = 1.5f;
    public float horizontal;
    public float vertical;
    public bool isKidGrabbed = false;
    public bool isInCollision = false;
    public string lastTriggerName;

    private Vector2 prevSpeed;
    private Vector2 currentSpeed;
    void Start()
    {
        highScore.StartCountingPoints();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isKidGrabbed)
                isKidGrabbed = false;
            else
                GrabKid();
        }

        if (Input.GetKeyDown(KeyCode.Q))
            WindowBoardingControl(true, false);

        if (Input.GetKeyUp(KeyCode.Q))
            WindowBoardingControl(false, true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GrabEquipment();
            // // RaycastHit2D hit = Physics2D.Linecast(raycastOrigin.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            // // if (hit)
            //     GrabEquipment(hit.collider.gameObject);
        }

        PlayerMove();
        SetAnimations();

        if (previousTriggerName != currentTriggerName && currentTriggerName != null)
            lastTriggerName = currentTriggerName;

        if (playerRB.velocity != Vector2.zero && !AudioManager.Instance.stamp.isPlaying)
            AudioManager.Instance.stamp.Play();
    }
    private string previousTriggerName;
    private string currentTriggerName;
    private void SetAnimations()
    {
        animator.SetFloat(AnimationParameters.SPEED, currentSpeed.magnitude);

        previousTriggerName = lastTriggerName;
        currentTriggerName = GetAnimationTrigger();

        if (currentTriggerName != null)
            animator.SetTrigger(currentTriggerName);
    }

    public string GetAnimationTrigger()
    {
        if (horizontal > 0.5)
            return AnimationParameters.RIGHT;

        if (horizontal < -0.5)
            return AnimationParameters.LEFT;

        if (vertical > 0.5)
            return AnimationParameters.UP;

        if (vertical < -0.5)
            return AnimationParameters.DOWN;

        return null;
    }

    /// <summary>
    /// Changes player position by detected inputs
    /// </summary>
    private void PlayerMove()
    {
        if (!isInCollision)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");

            playerRB.velocity = Speed * new Vector2(horizontal, vertical);
            currentSpeed = playerRB.velocity;
        }
    }

    /// <summary>
    /// Grabs kid
    /// </summary>
    private void GrabKid()
    {
        foreach (GameObject item in objectsInRange)
        {
            if (item.tag == "Baby")
                isKidGrabbed = true;
        }
    }

    /// <summary>
    /// Grabs Equipment in range
    /// </summary>
    private void GrabEquipment()
    {
        foreach (GameObject item in objectsInRange)
        {
            if (item.tag == "Sword")
            {
                Destroy(item);
                SetWeaponDurability(8);
                GameManager.Instance.SpawnWeapon();
                break;
            }
        }
    }

    /// <summary>
    /// Controls Boarding Window Coroutine
    /// </summary>
    private void WindowBoardingControl(bool aStart, bool aStop)
    {
        foreach (GameObject item in objectsInRange)
        {
            if (item.tag == "Window" && aStart)
            {
                Debug.Log("start coroutine");
                item.GetComponent<WindowControl>().StartCoroutine("BoardWindow");
            }

            if (item.tag == "Window" && aStop)
            {
                item.GetComponent<WindowControl>().StopCoroutine("BoardWindow");
                Debug.Log("stop coroutine");
            }
        }
    }

    /// <summary>
    /// Sets weaponDurability paremeter and HUD to given value
    /// </summary>
    private void SetWeaponDurability(int aDurability)
    {
        GlobalParams.weaponDurability = aDurability;
        hud.SetWeaponDurability(aDurability);
    }

    /// <summary>
    /// Updates weaponDurability paremeter and HUD by given damage  
    /// </summary>
    public void UpdateWeaponDurability(int aDamage)
    {
        GlobalParams.weaponDurability -= aDamage;
        hud.UpdateWeaponDurability(aDamage);
    }

    /// <summary>
    /// Updates playerHealth parameter and HUD by given damage
    /// </summary>
    private void UpdatePlayerHealth(int aDamage)
    {
        AudioManager.Instance.playerHit.Play();

        GlobalParams.playerHealth -= aDamage;
        hud.UpdatePlayerHealth(aDamage);

        animator.SetTrigger(AnimationParameters.HURT);

        if (GlobalParams.playerHealth <= 0)
            _ = DeathAsync(500);
    }

    private async Task DeathAsync(int dellayMilliseconds)
    {

        animator.SetTrigger(AnimationParameters.DIE);
        await Task.Delay(dellayMilliseconds);
        highScore.SaveResult();

        Destroy(gameObject);
    }

    /// <summary>
    /// Knockback player and blocks his movement after collision with Enemy 
    /// </summary>
    private void KnockbackPlayer(Vector3 aEnemyPosition)
    {
        isInCollision = true;
        Vector3 knockbackDirection = aEnemyPosition - transform.position;

        UpdatePlayerHealth(1);
        playerRB.AddForce(knockbackDirection.normalized * -25f);
        _ = ResetVelocityAsync(500);
    }

    /// <summary>
    /// Resets player velocity and movement blockade flag
    /// </summary>
    private async Task ResetVelocityAsync(int aDelay)
    {
        await Task.Delay(aDelay);
        playerRB.velocity = Vector2.zero;
        isInCollision = false;
    }

    private void OnTriggerEnter2D(Collider2D aCollider)
    {
        objectsInRange.Add(aCollider.gameObject);
    }

    private void OnTriggerExit2D(Collider2D aCollider)
    {
        objectsInRange.Remove(aCollider.gameObject);

        if (aCollider.tag == "Window")
        {
            Debug.Log("stop coroutine");
            aCollider.gameObject.GetComponent<WindowControl>().StopCoroutine("BoardWindow");
        }
    }

    private void OnCollisionEnter2D(Collision2D aCollision)
    {
        if (aCollision.gameObject.tag == "Enemy")
        {
            KnockbackPlayer(aCollision.transform.position);
        }
    }
}
