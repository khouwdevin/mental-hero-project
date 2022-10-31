using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Movement : MonoBehaviour
{

    private Attributes attributes;
    [Range(0, .3f)] [SerializeField] private float movement_smooth = .05f;  // Smoothness for the movement
	[SerializeField] private bool air_control = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask ground_layer_mask;                          // A mask determining what is ground to the character
	[SerializeField] private Transform ground_check;                           // A position marking where to check if the player is grounded.

	const float grounded_radius = .2f; // Radius of the overlap circle to determine if grounded
	private bool isGrounded = false;            // Whether or not the player is grounded.
	private Rigidbody2D rb;
	private bool isRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;

	// Start is called before the first frame update
	void Start()
    {
        attributes = GetComponent<Attributes>();
		rb = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	void FixedUpdate()
	{
		if (!isGrounded)
		{
			sw.Start();
		}
		isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(ground_check.position, grounded_radius, ground_layer_mask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				if (sw.ElapsedMilliseconds > (long)60)
					OnLandEvent.Invoke();
				sw.Reset();
			}
		}
	}


	public void Move(float direction, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (isGrounded || air_control)
		{

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(direction * 10f, rb.velocity.y);
			// And then smoothing it out and applying it to the character
			rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movement_smooth);

			// If the input is moving the player right and the player is facing left...
			if (direction > 0 && !isRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (direction < 0 && isRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (isGrounded && jump)
		{
			// Add a vertical force to the player.
			isGrounded = false;
			rb.AddForce(new Vector2(0f, attributes.jump_height));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		isRight = !isRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
