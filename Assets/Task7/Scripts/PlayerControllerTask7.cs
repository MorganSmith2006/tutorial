using System.Reflection.Metadata;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTask7 : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject reticle;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip scoreSound;
    [SerializeField] AudioClip shootSound;
    [SerializeField] float speed = 20;
    [SerializeField] float ProjectileSpeed = 500;

    Vector2 projectileDirection = new Vector2(1, 0);

    private int lives = 3;
    private int score = 0;
    Rigidbody2D physicsBody;
    AudioSource speaker; 
    void Start()
    {
        physicsBody = GetComponent<Rigidbody2D>();
        speaker = GetComponent<AudioSource>(); 

        //string newname = "";
        //foreach (char letter in name)
        //{
        //    if (char.IsUpper(letter)) newname += " ";
        //    newname += char.ToUpper(letter);
        //}
        //name = newname; 
    }

    void Update()
    {
        if (physicsBody == null) return; 
        HandleMovement();
        HandleFiring();
        CheckWinAndLoseStates();
    }

    private void OnGUI()
    {
        int startX = 1700;
        int spacing = 40;
        Texture livesTexture = GetComponent<SpriteRenderer>().sprite.texture;
        for (int drawn = 0; drawn < lives; drawn = drawn + 1)
        {
            int xPosition = startX - spacing * drawn;
            int yPosition = 900;
            int xSize = 50;
            int ySize = 50;
            Rect drawLocation = new Rect(x: xPosition, y: yPosition, width: xSize, height: ySize);
            GUI.DrawTexture(drawLocation, livesTexture); 
        }
        Texture scoreSprite = projectilePrefab.GetComponent<SpriteRenderer>().sprite.texture;
        startX = 100;
        spacing = 25;
        for (int drawnSoFar = 0; drawnSoFar < score; drawnSoFar++)
        {
            int xPosition = startX + drawnSoFar * spacing;
            int yPosition = 900;
            int xSize = 20;
            int ySize = 20;
            Rect drawLocation = new Rect(xPosition, yPosition, xSize, ySize);
            GUI.DrawTexture(drawLocation, scoreSprite); 
        }
    }

    private void HandleMovement()
    {
        float xInput = Keyboard.current.rightArrowKey.value - Keyboard.current.leftArrowKey.value;
        float yInput = Keyboard.current.upArrowKey.value - Keyboard.current.downArrowKey.value;
        Vector2 force = new Vector2(xInput, yInput) * speed;
        physicsBody.AddForce(force);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -8, 8), Mathf.Clamp(transform.position.y, -4, 4), 0);
    }

    private void CheckWinAndLoseStates()
    {
        if (score >= 10)
        {
            GameObject enemyObject = GameObject.Find("Enemy");
            Destroy(enemyObject);
            speaker.PlayOneShot(winSound);
            Destroy(physicsBody);
        }
        if (lives <= 0)
        {
            speaker.PlayOneShot(deathSound);
            Destroy(gameObject);
            GetComponent<SpriteRenderer>().color = Color.red;
            Invoke(nameof(RemoveCorpse), 1.5f);
        }
    }

    private void HandleFiring()
    {
        if (physicsBody.linearVelocity != Vector2.zero)
        {
            projectileDirection = physicsBody.linearVelocity.normalized;
        }
        reticle.transform.localPosition = projectileDirection;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Vector2 spawnlocation = transform.position;
            Quaternion spawnrotation = projectilePrefab.transform.rotation;
            GameObject spawnedprojectile = Instantiate(projectilePrefab, spawnlocation, spawnrotation);
            spawnedprojectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * ProjectileSpeed);
            speaker.PlayOneShot(shootSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyControllerTask7>() != null)
        {
            lives--;
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
    }

    private void RemoveCorpse()
    {
        Destroy(gameObject);
    }

    public void AddPoints(int points)
    {
        score += points;
        speaker.PlayOneShot(scoreSound);
    }
}
