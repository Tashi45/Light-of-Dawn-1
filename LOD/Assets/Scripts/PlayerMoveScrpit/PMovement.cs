using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PMovement : MonoBehaviour
{
    private Collision col;
    private TrailRenderer trailRenderer;
    
    public Rigidbody2D rb;

    [SerializeField] private float dashingVelocity = 5f;
    [SerializeField] private float dashingTime = 0.2f;
    private Vector2 dashingDir;
    private bool isDashing;
    private bool canDash = true;
    
    
    public float speed = 8f;
    public float jumpForce = 5f;
    public float slideSpeed = 3;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    public bool canMove;
    public bool wallSlideing;
    public bool wallJumped;
    public bool wallGrab;
    private bool doubleJump;
    private bool canWallJump;

    private bool groundTouch;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collision>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

// Update is called once per frame
    void Update()
    {
        if (col.onGround)
        {
            wallJumped = false;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        var dashInput = Input.GetButtonDown("Dash");

        wallGrab = col.onWall && Input.GetKey(KeyCode.P);

        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashingDir = new Vector2(x, Input.GetAxisRaw("Vertical"));
            if (dashingDir == Vector2.zero)
            {
                dashingDir = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            rb.velocity = dashingDir.normalized * dashingVelocity;
            return;
        }

        if(wallGrab) {
            rb.velocity = new Vector2(rb.velocity.x, y * speed);
        }

        if(col.onWall && !col.onGround) {
            if(x != 0 && !wallGrab) {
                wallSlide();
            }
       
        }

        if (col.onGround || col.onWall)
        {
            canDash = true;
        }

        if(Input.GetButtonDown("Jump"))  {

            if(col.onGround) 
            {
                Jump(Vector2.up, false);
            }
            if(col.onWall && !col.onGround) 
            {
                wallJump();
            }
        }
        
        if (col.onWall && Input.GetButtonDown("Jump")) {
            Debug.Log("Wall jump detected!");
            Jump(Vector2.up * 1f, true); 
        }
    }

    private void wallSlide() {
        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
    
    }

    private void Walk(Vector2 dir)
    {

        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
        if(!canMove) 
            return;
        if(wallGrab)
            return;

        if(!wallJumped) {
            rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
        } else{
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), .5f * Time.deltaTime);
        }
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        trailRenderer.emitting = false;
        isDashing = false;
    }

    /*private void Dash(float x, float y)
    {
        wallJumped = true;
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(x, y).normalized * 30;
    }*/

    private void Jump(Vector2 dir, bool wall) {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;
    }

    private void wallJump() {

        StartCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
    
        Vector2 wallDir = col.onRightWall ? Vector2.left : Vector2.right;

        float wallJumpHorizontalForce = 0;
        float horizontalForce = wallDir.x * wallJumpHorizontalForce;
        float verticalForce = jumpForce;

        rb.velocity = new Vector2(horizontalForce, verticalForce);

        Jump((Vector2.up / 2f + wallDir / 2f), true);
        wallJumped = true;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }
}
