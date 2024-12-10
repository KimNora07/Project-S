using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float xInput;
    private float yInput;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float acceleration;
    [SerializeField] private float drag;
    [SerializeField] private float playerSize;
   
    [SerializeField] private bool isGrounded;
    private bool isShot;
    private bool characterFacingRight;
    private bool handFacingRight;

    private Rigidbody2D body;
    public BoxCollider2D groundCheckCollider;
    public LayerMask groundCheckMask;

    // Test
    [SerializeField] private Transform gunShot;
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private Transform hand;
    private float handDistance;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();   
        handDistance = hand.transform.position.x - transform.position.x;
    }

    private void Update()
    {
        if (!isShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isShot = true;
                StartCoroutine(ShotCoolTime());
            }
        }

        Move();
        MouseAim();
    }

    private void FixedUpdate()
    {
        // PlayerMove
        if (Mathf.Abs(xInput) > 0)
        {
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.velocity.x + increment, -moveSpeed, moveSpeed);

            Vector3 vec = new Vector3(newSpeed, body.velocity.y);
            body.velocity = vec;
        }

        if(Mathf.Abs(yInput) > 0 && isGrounded)
        {
            Vector3 vec = new Vector3(body.velocity.x, yInput * jumpPower) ;
            body.velocity = vec;
        }
        

        // GroundCheck
        GroundCheck();
        if (isGrounded && xInput == 0 && body.velocity.y <= 0)
        {
            body.velocity *= drag;
        }
    }

    private void MouseAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - hand.transform.position;

        hand.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));

        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        hand.position = transform.position + Quaternion.Euler(0,0,angle) * new Vector3(handDistance,0,0);

        HandFlipController(mousePos);
        CharacterFlipController(mousePos);
    }

    private void HandFlipController(Vector3 mousePos)
    {
        if(mousePos.x < hand.position.x && handFacingRight)
        {
            HandFlip();
        }
        else if(mousePos.x > hand.position.x && !handFacingRight)
        {
            HandFlip();
        }
    }

    private void HandFlip()
    {
        handFacingRight = !handFacingRight;
        hand.localScale = new Vector3(hand.localScale.x, hand.localScale.y * -1, hand.localScale.z);
    }

    private void CharacterFlipController(Vector3 mousePos)
    {
        if(mousePos.x < transform.position.x && characterFacingRight)
        {
            CharacterFlip();
        }
        else if(mousePos.x > transform.position.x && !characterFacingRight)
        {
            CharacterFlip();
        }
    }

    private void CharacterFlip()
    {
        characterFacingRight = !characterFacingRight;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
    }

    private void Move()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    private void GroundCheck()
    {
        isGrounded = Physics2D.OverlapAreaAll(groundCheckCollider.bounds.min, groundCheckCollider.bounds.max, groundCheckMask).Length > 0;
    }

    private IEnumerator ShotCoolTime()
    {
        GameObject go = Instantiate(bulletPrefab, gunShot.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        isShot = false;
    }
}
