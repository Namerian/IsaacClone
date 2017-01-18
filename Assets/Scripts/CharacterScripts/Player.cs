using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	//================================

	private Rigidbody2D _rigidBody;

	private Animator _headAnimator;
	private SpriteRenderer _headRenderer;

	private Animator _legsAnimator;
	private SpriteRenderer _legsRenderer;

	private Vector2 _movementDirection = new Vector2 (0, 0);
	private Vector2 _firingDirection = new Vector2 (0, -1);

	private bool _gameRunning = true;
	private bool _hasFired = false;
	private bool _isAlive = true;

	private WeaponType _currentWeapon = WeaponType.BlueTear;

	private int _healthPoints;
	private float _movementSpeed;
	private float _firingSpeed;
	private float _projectileRange;
	private float _projectileSpeed;
	private int _projectileDamage;

	//=================================================================
	//
	//=================================================================

	public Team Team { get { return Team.Player; } }

	public WeaponType WeaponType{ get { return _currentWeapon; } }

	public Vector3 Position{ get { return this.transform.position; } }

	public Vector2 FiringDirection { 
		get { return _firingDirection; }
		set {
			if (_firingDirection != value) {
				_firingDirection = value;

				//UP
				if (value.x == 0 && value.y == 1) {
					_headRenderer.flipX = false;
					_headAnimator.SetTrigger ("LookUp");

					_legsRenderer.flipX = true;
					_legsAnimator.SetTrigger ("LookUp");
				}
				//DOWN
				else if (value.x == 0 && value.y == -1) {
					_headRenderer.flipX = false;
					_headAnimator.SetTrigger ("LookDown");

					_legsRenderer.flipX = false;
					_legsAnimator.SetTrigger ("LookDown");
				}
				//LEFT
				else if (value.x == -1 && value.y == 0) {
					_headRenderer.flipX = true;
					_headAnimator.SetTrigger ("LookLeft");

					_legsRenderer.flipX = true;
					_legsAnimator.SetTrigger ("LookLeft");
				}
				//RIGHT
				else if (value.x == 1 && value.y == 0) {
					_headRenderer.flipX = false;
					_headAnimator.SetTrigger ("LookRight");

					_legsRenderer.flipX = false;
					_legsAnimator.SetTrigger ("LookRight");
				}
			}
		}
	}

	public float ProjectileRange{ get { return _projectileRange; } }

	public float ProjectileSpeed{ get { return _projectileSpeed; } }

	public int ProjectileDamage{ get { return _projectileDamage; } }

	//=================================================================
	//
	//=================================================================

	// Use this for initialization
	void Start ()
	{
		_rigidBody = this.GetComponent<Rigidbody2D> ();

		Transform head = this.transform.Find ("Head");
		_headAnimator = head.GetComponent<Animator> ();
		_headRenderer = head.GetComponent<SpriteRenderer> ();

		Transform legs = this.transform.Find ("Legs");
		_legsAnimator = legs.GetComponent<Animator> ();
		_legsRenderer = legs.GetComponent<SpriteRenderer> ();

		_healthPoints = __healthPoints;
		_movementSpeed = __movementSpeed;
		_firingSpeed = __firingSpeed;
		_projectileRange = __projectileRange;
		_projectileSpeed = __projectileSpeed;
		_projectileDamage = __projectileDamage;

		Event.Instance.OnGameEndedEvent += OnGameEndedEvent;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_isAlive && _gameRunning) {
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
				_movementDirection = newMovementDirection;

				_rigidBody.velocity = newMovementDirection * _movementSpeed;
				_legsAnimator.SetBool ("Walking", true);
			} else {
				//_rigidBody.velocity = Vector2.zero;

				_legsAnimator.SetBool ("Walking", false);
			}

			//===========================================
			// Firing

			if (!_hasFired) {
				Vector2 newFiringDirection = Vector2.zero;

				if (Input.GetKey (KeyCode.UpArrow)) {
					newFiringDirection.y = 1;
				} else if (Input.GetKey (KeyCode.DownArrow)) {
					newFiringDirection.y = -1;
				} else if (Input.GetKey (KeyCode.LeftArrow)) {
					newFiringDirection.x = -1;
				} else if (Input.GetKey (KeyCode.RightArrow)) {
					newFiringDirection.x = 1;
				}

				if (newFiringDirection.magnitude > 0) {
					FiringDirection = newFiringDirection;

					ProjectileFactory.CreateProjectiles (this);
					_headAnimator.SetTrigger ("FireTrigger");


					_hasFired = true;
					Invoke ("OnReloadingOver", _firingSpeed);
				}
			}
		}
	}

	//=================================================================
	//
	//=================================================================

	public void ApplyDamage (int damage)
	{
		if (!_isAlive) {
			return;
		}

		_healthPoints -= damage;

		if (_healthPoints <= 0) {
			_isAlive = false;
			GameController.Instance.CharacterDied (this);
		}
	}

	//=================================================================
	//
	//=================================================================

	private void OnReloadingOver ()
	{
		_hasFired = false;
	}

	private void OnGameEndedEvent ()
	{
		_gameRunning = false;
		_rigidBody.velocity = Vector2.zero;
	}

	//=================================================================
	//
	//=================================================================

	public void OnTearSelected (bool value)
	{
		//Debug.Log ("OnTearSelected:called!");

		if (value) {
			_currentWeapon = WeaponType.BlueTear;
		}
	}

	public void OnMissileSelected (bool value)
	{
		//Debug.Log ("OnMissileSelected:called!");

		if (value) {
			_currentWeapon = WeaponType.BlueMissile;
		}
	}

	public void OnSlalomTearSelected (bool value)
	{
		if (value) {
			_currentWeapon = WeaponType.SlalomTear;
		}
	}

	//=================================================================
	//
	//=================================================================

}
