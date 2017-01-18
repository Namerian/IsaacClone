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
	public int _healthPoints = 10;
	public float _movementSpeed = 1;
	public float _firingSpeed = 1;
	public float _projectileRange = 3;
	public float _projectileSpeed = 2;
	public int _projectileDamage = 1;

	//================================

	private Rigidbody2D _rigidBody;

	private Animator _headAnimator;
	private SpriteRenderer _headRenderer;

	private Animator _legsAnimator;
	private SpriteRenderer _legsRenderer;

	//================================

	private Text _healthPointsViewText;
	private Text _movementSpeedViewText;
	private Text _firingSpeedViewText;
	private Text _projectileRangeViewText;
	private Text _projectileSpeedViewText;
	private Text _projectileDamageViewText;

	//================================

	private Vector2 _movementDirection = new Vector2 (0, 0);
	private Vector2 _firingDirection = new Vector2 (0, -1);

	private bool _gameRunning = true;
	private bool _hasFired = false;
	private bool _isAlive = true;

	private WeaponType _currentWeapon = WeaponType.BlueTear;

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

	public float ProjectileRange { 
		get { return _projectileRange; } 
		set {
			_projectileRange = Mathf.Clamp (value, 1, 5);
			_projectileRangeViewText.text = _projectileRange.ToString ();
		} 
	}

	public float ProjectileSpeed { 
		get { return _projectileSpeed; } 
		set {
			_projectileSpeed = Mathf.Clamp (value, 1, 5);
			_projectileSpeedViewText.text = _projectileSpeed.ToString ();
		}
	}

	public int ProjectileDamage { 
		get { return _projectileDamage; } 
		set {
			_projectileDamage = Mathf.Clamp (value, 1, 5); 
			_projectileDamageViewText.text = _projectileDamage.ToString ();
		}
	}

	private int HealthPoints {
		get{ return _healthPoints; }
		set {
			_healthPoints = Mathf.Clamp (value, 0, 10);
			_healthPointsViewText.text = _healthPoints.ToString ();

			if (_healthPoints <= 0) {
				_isAlive = false;
				GameController.Instance.CharacterDied (this);
			}
		}
	}

	private float MovementSpeed {
		get{ return _movementSpeed; } 
		set {
			_movementSpeed = Mathf.Clamp (value, 1, 5); 
			_movementSpeedViewText.text = _movementSpeed.ToString ();
		}
	}

	private float FiringSpeed {
		get{ return _firingSpeed; }
		set {
			_firingSpeed = Mathf.Clamp (value, 1, 5); 
			_firingSpeedViewText.text = _firingSpeed.ToString ();
		}
	}

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

		//===================

		_healthPointsViewText = this.transform.Find ("Canvas/StatModificationPanel/HealthPointsView/Value").GetComponent<Text> ();
		_movementSpeedViewText = this.transform.Find ("Canvas/StatModificationPanel/MovementSpeedView/Value").GetComponent<Text> ();
		_firingSpeedViewText = this.transform.Find ("Canvas/StatModificationPanel/FiringSpeedView/Value").GetComponent<Text> ();
		_projectileRangeViewText = this.transform.Find ("Canvas/StatModificationPanel/ProjectileRangeView/Value").GetComponent<Text> ();
		_projectileSpeedViewText = this.transform.Find ("Canvas/StatModificationPanel/ProjectileSpeedView/Value").GetComponent<Text> ();
		_projectileDamageViewText = this.transform.Find ("Canvas/StatModificationPanel/ProjectileDamageView/Value").GetComponent<Text> ();

		//==================

		HealthPoints = _healthPoints;
		MovementSpeed = _movementSpeed;
		FiringSpeed = _firingSpeed;
		ProjectileRange = _projectileRange;
		ProjectileSpeed = _projectileSpeed;
		ProjectileDamage = _projectileDamage;

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

				_rigidBody.velocity = newMovementDirection * MovementSpeed;
				_legsAnimator.SetBool ("Walking", true);
			} else {
				_rigidBody.velocity = Vector2.zero;

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
					Invoke ("OnReloadingOver", 1 / FiringSpeed);
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

		HealthPoints -= damage;


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

	public void OnStatDecreaseButtonPressed (string statName)
	{
		switch (statName) {
		case "HealthPoints":
			this.HealthPoints -= 1;
			break;
		case "MovementSpeed":
			this.MovementSpeed -= 1;
			break;
		case "FiringSpeed":
			this.FiringSpeed -= 1;
			break;
		case "ProjectileRange":
			this.ProjectileRange -= 1;
			break;
		case "ProjectileSpeed":
			this.ProjectileSpeed -= 1;
			break;
		case "ProjectileDamage":
			this.ProjectileDamage -= 1;
			break;
		}
	}

	public void OnStatIncreaseButtonPressed (string statName)
	{
		switch (statName) {
		case "HealthPoints":
			this.HealthPoints += 1;
			break;
		case "MovementSpeed":
			this.MovementSpeed += 1;
			break;
		case "FiringSpeed":
			this.FiringSpeed += 1;
			break;
		case "ProjectileRange":
			this.ProjectileRange += 1;
			break;
		case "ProjectileSpeed":
			this.ProjectileSpeed += 1;
			break;
		case "ProjectileDamage":
			this.ProjectileDamage += 1;
			break;
		}
	}

	//=================================================================
	//
	//=================================================================

}
