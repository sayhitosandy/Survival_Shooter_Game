using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpHeight = 10f;

    PlayerShooting playerShooting;
    bool isGrounded;
    bool jumpCollected = false;
    bool damageCollected = false;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    float jumpTimer = 0f;
    float damageTimer = 0f;

    void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);

    }

    void Update()
    {
        if (jumpTimer >= 10f && jumpCollected)
        {
            jumpCollected = false;
            transform.position += new Vector3(3, 0, 3);
        } 
        if (jumpCollected)
        {
            jumpTimer += Time.deltaTime;
            //Debug.Log(jumpTimer);
        }
        Jump();

        if (damageTimer >= 10f && damageCollected)
        {
            playerShooting.damagePerShot = 20;
            damageCollected = false;
        }
        if (damageCollected)
        {
            //Debug.Log(damageTimer);
            damageTimer += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !isGrounded)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8 && isGrounded)
        {
            isGrounded = false;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && isGrounded && jumpCollected)
        {
            playerRigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            //isGrounded = false;
        }
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            //playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JumpCollectible"))
        {
            other.gameObject.SetActive(false);
            jumpCollected = true;
            jumpTimer = 0f;
        }
        
        if (other.gameObject.CompareTag("DamageCollectible"))
        {
            playerShooting.damagePerShot = 100;
            other.gameObject.SetActive(false);
            damageCollected = true;
            damageTimer = 0f;
        }
    }
}
