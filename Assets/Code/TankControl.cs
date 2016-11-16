using UnityEngine;

/// <summary>
/// Implements player control of tanks, as well as collision detection.
/// </summary>
public class TankControl : MonoBehaviour {
    /// <summary>
    /// How fast to drive
    /// </summary>
    public float ForwardSpeed = 300f;
    /// <summary>
    /// Gain for velocity control.
    /// Force is this times the difference between our target velocity and our actual velocity
    /// </summary>
    public float Acceleration = 2f;

    /// <summary>
    /// How fast to turn
    /// </summary>
    public float RotationSpeed = 80f;
    /// <summary>
    /// Delay between shooting
    /// </summary>
    public float FireCooldown = 0.5f;
    /// <summary>
    /// Pushback on the tank when it fires
    /// </summary>
    public float Recoil = 10;

    /// <summary>
    /// Axis for controlling driving
    /// </summary>
    public string VerticalAxis;
    /// <summary>
    /// Axis for controlling rotation
    /// </summary>
    public string HorizontalAxis;
    /// <summary>
    /// Button to fire projectile
    /// </summary>
    public KeyCode FireButton;
    /// <summary>
    /// Button to deploy powerup
    /// </summary>
    public KeyCode PowerUpButton;

    public AudioClip FireSound;
    public AudioClip DeployPowerUpSound;
    public AudioClip PickUpPowerUpSound;

    /// <summary>
    /// Prefab for the bullets we fire.
    /// </summary>
    public GameObject Projectile;
    /// <summary>
    /// Color to tint the projections fired by this tank
    /// </summary>
    public Color ProjectileColor = Color.white;

    /// <summary>
    /// Time at which we will next be allowed to fire.
    /// </summary>
    private float coolDownTimer;

    /// <summary>
    /// Rigid body component for tank.
    /// </summary>
    private Rigidbody2D tankRb;

    /// <summary>
    /// The powerup we've collected.
    /// Null if we don't have one.
    /// </summary>
    public PowerUpController PowerUp;

	public bool isFrozen=false;

    /// <summary>
    /// Initialize
    /// </summary>
    internal void Start() {
        tankRb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Joystick values less than this will be treated as zero
    /// </summary>
    const float DeadZoneSize = 0.2f;

    float DeadZone(float axis)
    {
        if (Mathf.Abs(axis) < DeadZoneSize)
            return 0;
        return axis;
    }

    /// <summary>
    /// The player pushed fire.
    /// Launch if we aren't cooling down.
    /// </summary>
    void FireProjectileIfPossible(){
        if (Time.time > coolDownTimer) {
            FireProjectile();
            coolDownTimer = Time.time + FireCooldown;
        }
    }

    /// <summary>
    /// Really and truly fire the projectile.
    /// </summary>
    void FireProjectile() {
        var go = Instantiate(Projectile) ;
        var ps = go.GetComponent<Projectile>();
        var up = transform.up.normalized;  // shouldn't this be normalized already?  -ian
        ps.Init(gameObject, transform.position + up * 2f, up);
        GetComponent<AudioSource>().PlayOneShot(FireSound);
        tankRb.AddForce(-Recoil*transform.up,ForceMode2D.Impulse);
    }

	internal void Update(){
		if (!isFrozen) {
			var k = Acceleration;
			var Sd = DeadZone (Input.GetAxis (VerticalAxis)) * ForwardSpeed * Time.deltaTime;
			var Sa = Vector3.Dot (tankRb.velocity, transform.up);
			var F = k * (Sd - Sa);
			var vel = F * transform.up;
			tankRb.velocity = (Vector3)tankRb.velocity + vel;
			var rot = DeadZone (Input.GetAxis (HorizontalAxis)) * RotationSpeed * Time.deltaTime;
			tankRb.rotation = tankRb.rotation + rot;
			var Sr = Vector3.Dot (tankRb.velocity, transform.right);
			var Fr = -Sr;
			tankRb.velocity = (Vector3)tankRb.velocity + (Fr * transform.right);

			if (Input.GetKey (FireButton)) {
				FireProjectileIfPossible ();
			}
		} else {
			tankRb.velocity = new Vector3 (0, 0, 0);
		}

		if (Input.GetKeyDown (PowerUpButton) && PowerUp!=null) {
			PowerUp.Fire ();
		}
			
	}
}
