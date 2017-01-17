using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacter
{
	
	/*Stats to implement:
	 * hp joueur
	 * vitesse joueur
	 * cadence tir
	 * portée tir
	 * vitesse projectile
	 * dégats projectile
	 */
	public int __healthPoints = 10;
	public float __movementSpeed = 1;
	public float __firingSpeed = 1;
	public float __projectileRange = 5;
	public float __projectileSpeed = 1.5f;
	public int __projectileDamage = 1;

	private Rigidbody2D _rigidBody;

	private int _healthPoints;
	private float _movementSpeed;
	private float _firingSpeed;
	private float _projectileRange;
	private float _projectileSpeed;
	private int _projectileDamage;

	private Vector2 _movementDirection = new Vector2 (0, 0);
	private Vector2 _firingDirection = new Vector2 (0, -1);

	private bool _hasFired = false;

	//
	public Team Team { get { return Team.Player; } }

	// Use this for initialization
	void Start ()
	{
		_rigidBody = this.GetComponent<Rigidbody2D> ();

		_healthPoints = __healthPoints;
		_movementSpeed = __movementSpeed;
		_firingSpeed = __firingSpeed;
		_projectileRange = __projectileRange;
		_projectileSpeed = __projectileSpeed;
		_projectileRange = __projectileRange;
	}
	
	// Update is called once per frame
	void Update ()
	{


		//=============================================
		// Movement

		Vector2 newMovementDirection = Vector2.zero;

		if (Input.GetKey (KeyCode.Z)) {
			newMovementDirection.y = 1;
		} else if (Input.GetKey (KeyCode.S)) {
			newMovementDirection.y = -1;
		} else if (Input.GetKey (KeyCode.Q)) {
			newMovementDirection.x = -1;
		} else if (Input.GetKey (KeyCode.D)) {
			newMovementDirection.x = 1;
		}

		if (newMovementDirection.magnitude > 0) {
			Vector2 velocity = newMovementDirection * _movementSpeed /* Time.deltaTime*/;
			_rigidBody.velocity = velocity;
			_movementDirection = newMovementDirection;
		} else {
			_rigidBody.velocity = Vector2.zero;
		}

		//===========================================
		// Firing

		if (!_hasFired) {
			Vector2 newFiringDirection = Vector2.zero;

			if (Input.GetKey (KeyCode.UpArrow)) {

			} else if (Input.GetKey (KeyCode.DownArrow)) {
				newFiringDirection.y = -1;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {

			} else if (Input.GetKey (KeyCode.RightArrow)) {

			}

			if (newFiringDirection.magnitude > 0) {
				GameObject projectileObj = Instantiate (Resources.Load ("Prefabs/Projectiles/BlueTearProjectile"), this.transform.position, Quaternion.identity) as GameObject;
				IProjectile projectileScript = projectileObj.GetComponent<IProjectile> ();
				projectileScript.InitializeValues (Team.Player, _firingDirection, _projectileRange, _projectileSpeed, _projectileDamage);

				_hasFired = true;
				Invoke ("OnReloadingOver", _firingSpeed);
			}
		}
	}


	public void ApplyDamage (int damage)
	{
		
	}


	private void OnReloadingOver ()
	{
		_hasFired = false;
	}
}
