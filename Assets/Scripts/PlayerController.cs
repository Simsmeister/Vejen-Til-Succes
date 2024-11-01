using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;

    public float jumpForce = 10f;
    public LayerMask groundLayer;

    private bool isGrounded;

    private Animator animator;

    public Transform groundCheck; // Empty GameObject at the player’s feet for ground detection

    // NetworkVariable to sync position across clients
    //private NetworkVariable<Vector2> networkPosition = new NetworkVariable<Vector2>();


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (IsOwner)
        {
            HandleInput();
            //networkPosition.Value = rb.position; // Update networked position
            CheckIfGrounded();

            animator.SetBool("IsRunning", Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded);
        }
        else
        {
            //rb.position = networkPosition.Value; // Sync position for non-owners
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.W)) // "W" or "Space" for jump
        {
            Jump();
            animator.SetBool("IsJumping", true); // Set jumping animation
        }
        else if (isGrounded)
        {
            animator.SetBool("IsJumping", false); // Reset jumping animation when grounded
        }
    }

    private void HandleInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
            // Flip the character's direction based on movement direction
        if (moveHorizontal > 0)
        {
            transform.localScale = new Vector3(4, 4, 4); // Face right
        }
        else if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-4, 4, 4); // Face left
        }

        // Apply horizontal movement to Rigidbody2D
        Vector2 movement = new Vector2(moveHorizontal * speed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void Jump()
    {
        // Apply jump force only if grounded
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CheckIfGrounded()
    {
        // Check if player is touching the ground using a small overlap circle at the player's feet
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        
        // Debug to check ground state
        Debug.Log("Is Grounded: " + isGrounded);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the ground check circle in the editor for visual aid
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
        }
    }
}
