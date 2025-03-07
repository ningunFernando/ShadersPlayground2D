using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float deceleration = 3f;
    public float rotationSpeed = 120f;

    private Rigidbody2D rb;
    private float moveInput;
    private float turnInput;

    [SerializeField] private SpriteRenderer colorChangeRenderer;
    [SerializeField] private GameObject camera;

    private bool canMove = true;
    private float energyLossTimer = 0f;
    private float energyLossInterval = 3f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab; // Prefab del proyectil
    public Transform shootPoint; // Punto desde donde dispara
    public float bulletSpeed = 10f;
    public float fireRate = 0.5f; // Tiempo entre disparos
    private float lastShotTime = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove) return;

        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            Color newColor = Random.ColorHSV();
            colorChangeRenderer.color = newColor;
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            HandleMovement();
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                energyLossTimer += Time.fixedDeltaTime;
                if (energyLossTimer >= energyLossInterval)
                {
                    GameManager.Instance.ChangeEnergy(-1);
                    energyLossTimer = 0f;
                }
            }
            else
            {
                energyLossTimer = 0f;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        camera.transform.position = new Vector3(transform.position.x, transform.position.y, -3.29f);
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    private void Shoot()
    {
        if (Time.time - lastShotTime < fireRate) return; // Controla la cadencia de disparo
        lastShotTime = Time.time;

        if (bulletPrefab != null && shootPoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootPoint.up * bulletSpeed; // Dispara hacia adelante (top-down usa up)
            }
        }
    }

    private void HandleMovement()
    {
        if (moveInput > 0)
        {
            rb.linearVelocity += (Vector2)transform.up * acceleration * moveInput * Time.fixedDeltaTime;
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        if (moveInput <= 0)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        if (rb.linearVelocity.magnitude > 0.2f)
        {
            rb.MoveRotation(rb.rotation - turnInput * rotationSpeed * Time.fixedDeltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Isla"))
        {
            GameManager.Instance.ChangeEnergy(-5);
        }
    }
}
