using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 7f;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private AudioSource jumpSound;

    [SerializeField]
    private AudioSource collectSound;

    [SerializeField]
    private AudioSource hitSound;

    [SerializeField]
    private AudioSource deathSound;

    [SerializeField]
    private AudioSource finishSound;

    private HealthBar healthBar;
    private float movementX;
    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator animator;
    private bool onGround;
    private enum State { Idle, Running, Jumping, Falling }
    private State state;
    private int count;
    
    private const int LevelNumber = 3;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxValue(GameManager.Instance.playerMaxLife);
        healthBar.SetValue(GameManager.Instance.playerLife);
    }

    private void Update()
    {
        PlayerMove();
        PlayerJump();
        AnimatePlayer();
        healthBar.SetValue(GameManager.Instance.playerLife);
    }

    private void PlayerMove()
    {
        if (body.bodyType == RigidbodyType2D.Dynamic)
        {
            movementX = Input.GetAxis("Horizontal");
            transform.position += speed * Time.deltaTime * new Vector3(movementX, 0f, 0f);
        }
    }

    private void PlayerJump()
    {
        if (Input.GetButton("Jump") && state != State.Jumping && state != State.Falling 
            && onGround && body.bodyType == RigidbodyType2D.Dynamic)
        {
            onGround = false;
            body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpSound.Play();
        }
    }

    private void AnimatePlayer()
    {
        if (movementX > 0)
        {
            state = State.Running;
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            state = State.Running;
            sr.flipX = true;
        }
        else
            state = State.Idle;

        if (body.velocity.y > .1f)
            state = State.Jumping;
        else if (body.velocity.y < -.1f)
            state = State.Falling;

        animator.SetInteger("State", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            onGround = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            Destroy(collision.gameObject);
            ++count;
            GameObject appleText = GameObject.FindWithTag("Score");
            appleText.GetComponent<Text>().text = count.ToString();
            collectSound.Play();
        }

        if (collision.gameObject.CompareTag("Trap"))
            PlayerHit(GameManager.Instance.trapDamage);

        if (collision.gameObject.CompareTag("Enemy"))
            PlayerHit(GameManager.Instance.enemyDamage);

        if (collision.gameObject.CompareTag("Checkpoint") &&
            GameObject.FindGameObjectsWithTag("Apple").Length == 0)
        {
            finishSound.Play();
            StartCoroutine(FinishLevel());
        }
    }

    private IEnumerator PlayerDeath()
    {
        body.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Death");
        deathSound.Play();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("GameOver");
    }

    private void PlayerHit(int damage)
    {
        GameManager.Instance.playerLife -= damage;
        animator.SetTrigger("Hit");
        hitSound.Play();
    }

    private void NextLevel()
    {
        string currLevelName = SceneManager.GetActiveScene().name;
        string[] strings = currLevelName.Split("_");
        int currLevel = int.Parse(strings[^1]);

        if(currLevel == LevelNumber)
            SceneManager.LoadScene("GameFinished");
        else
        {
            ++currLevel;
            strings[^1] = currLevel.ToString();
            var nextLevelName = string.Join("_", strings);
            SceneManager.LoadScene(nextLevelName);
        }
    }

    private IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(1);
        NextLevel();
    }

    public void ToIdle()
    {
        animator.ResetTrigger("Hit");
        state = State.Idle;

        if (GameManager.Instance.playerLife <= 0)
        {
            StartCoroutine(PlayerDeath());
        }
    }
}
