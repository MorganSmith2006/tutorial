using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class playercontrollertask2 : MonoBehaviour
{
    [SerializeField] int score = 0;
    [SerializeField] float timer = 60;
    [SerializeField] bool levelCompleted = false;

    [SerializeField] TextMeshProUGUI ScoreTextBox;
    [SerializeField] TextMeshProUGUI TimerTextBox;
    [SerializeField] TextMeshProUGUI CompletionTextBox;

    [SerializeField] float moveSpeed = 10;

    [SerializeField] AudioSource playerSound;
    [SerializeField] AudioClip coinClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] Rigidbody physicsBody;

    private void OnMouseDown()
    {
        playerSound.PlayOneShot(coinClip);

        // int pointsToAdd = Random.Range(1,20);
        //AddPoints(pointsToAdd); 

    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerSound != null)
        {
            playerSound.Play();
        }
        score = score + 1;
        Destroy(other.gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physicsBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(0, 0, 0.1f * Time.deltaTime);
        //float zSpeed = 0;
        //if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        //{
        //    zSpeed += moveSpeed;
        //}
        //if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        //{
        //    zSpeed -= moveSpeed;
        //}

        //float xSpeed = 0;
        //if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        //{
        //    xSpeed -= moveSpeed;
        //}
        //if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        //{
        //    xSpeed += moveSpeed;

        float zInput = (Keyboard.current.upArrowKey.value - Keyboard.current.downArrowKey.value);
        float xInput = (Keyboard.current.rightArrowKey.value - Keyboard.current.leftArrowKey.value);
    
       // velocity = new Vector3(xSpeed, 0, zSpeed) * Time.deltaTime;

        if (transform.position.z < -5)
            transform.position += new Vector3(0, 0, 10);
        if (transform.position.z > 5)
            transform.position -= new Vector3(0, 0, 10);

        if (transform.position.x > 5)
            transform.position -= new Vector3(10, 0, 0);
        if (transform.position.x < -5)
            transform.position += new Vector3(10, 0, 0);

        physicsBody.AddForce(new Vector3(xInput, 0, zInput) * moveSpeed);

        if(Keyboard.current.uKey.wasReleasedThisFrame)
        {
            transform.position = new Vector3(3, 0, 0);
        }

        if(timer <= 0 && score != 3)
        {
            Destroy(gameObject);
        }

        if(score>=3)
        {
            levelCompleted = true;
        }
        if(timer<=0 && !levelCompleted)
        {
            Destroy(gameObject);
        }

        timer = timer - Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, 60);
        UpdateTextBoxes();

        if (timer <= 0 && score != 3)
        {
            playerSound.PlayOneShot(gameOverClip);
            this.enabled = false;
        }

        if(Keyboard.current.f1Key.wasPressedThisFrame)
        {
            PlayerPrefs.SetInt("PlayerScore", score);
        }
        if(Keyboard.current.f2Key.wasPressedThisFrame)
        {
            score = PlayerPrefs.GetInt("PlayerScore");
        }

        float bounceSpeed = moveSpeed / 4;
        float rightBoundary = 3.75f;
        if(transform.position.x >  rightBoundary)
        {
            float newXspeed = -bounceSpeed;
            float maintainZspeed = physicsBody.linearVelocity.x;
            physicsBody.linearVelocity = new Vector3(newXspeed, 0, maintainZspeed);
        }
        float leftBoundary = 1.5f;
        if (transform.position.x < leftBoundary)
        {
            float newXspeed = +bounceSpeed;
            float maintainZspeed = physicsBody.linearVelocity.z;
            physicsBody.linearVelocity = new Vector3(newXspeed, 0, maintainZspeed);
        }
    }

    void UpdateTextBoxes()
    {
        ScoreTextBox.text = "Your Score: " + score.ToString();
        TimerTextBox.text = "Your Time: " + timer.ToString("F0");
        CompletionTextBox.text = "Level Complete? " + levelCompleted.ToString();
    }

    void AddPoints(int amount)
    {
        score += amount;
    }

}
