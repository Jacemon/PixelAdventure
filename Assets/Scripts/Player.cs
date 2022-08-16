using System;
using System.Collections;
using System.Collections.Generic;
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
    private AudioSource deathSound;

    [SerializeField]
    private AudioSource finishSound;

    private HealthBar healthBar;
    private float movementX;
    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator animator;
    private bool onGround;
    private int levelNum = 2;
    private enum State { IDLE, RUNNING, JUMPING, FALLING }
    private State state;
    private int count = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxValue(300);
        healthBar.SetValue(GameManager.instance.PlayerLife);
    }

    void Update()
    {
        PlayerMove();
        PlayerJump();
        AnimatePlayer();
        healthBar.SetValue(GameManager.instance.PlayerLife);
    }

    void PlayerMove()
    {
        if (body.bodyType == RigidbodyType2D.Dynamic)
        {
            movementX = Input.GetAxis("Horizontal");
            transform.position += speed * Time.deltaTime * new Vector3(movementX, 0f, 0f);
        }
    }

    void PlayerJump()
    {
        if (Input.GetButton("Jump") && onGround && body.bodyType == RigidbodyType2D.Dynamic)
        {
            onGround = false;
            body.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpSound.Play();
        }
    }

    void AnimatePlayer()
    {
        if (movementX > 0)
        {
            state = State.RUNNING;
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            state = State.RUNNING;
            sr.flipX = true;
        }
        else
            state = State.IDLE;

        if (body.velocity.y > .1f)
            state = State.JUMPING;
        else if (body.velocity.y < -.1f)
            state = State.FALLING;

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
            PlayerDeath();

        if (collision.gameObject.CompareTag("Checkpoint") &&
            GameObject.FindGameObjectsWithTag("Apple").Length == 0)
        {
            finishSound.Play();
            StartCoroutine(FinishLevel());
        }
    }

    private void PlayerDeath()
    {
        GameManager.instance.PlayerLife -= 100;
        body.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Death");
        deathSound.Play();
    }

    private void NextLevel()
    {
        string currLevelName = SceneManager.GetActiveScene().name;
        string[] strings = currLevelName.Split("_");
        int currLevel = int.Parse(strings[strings.Length - 1]);

        if(currLevel == levelNum)
            SceneManager.LoadScene("GameFinished");
        else
        {
            ++currLevel;
            strings[strings.Length - 1] = currLevel.ToString();
            string nextLevelName = string.Join("_", strings);
            GameManager.instance.PlayerLife = 300;
            SceneManager.LoadScene(nextLevelName);
        }
    }

    private void RestartLevel()
    {
        if (GameManager.instance.PlayerLife == 0)
            SceneManager.LoadScene("GameOver");
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(1);
        NextLevel();
    }
}
