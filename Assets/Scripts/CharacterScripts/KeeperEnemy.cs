using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeperEnemy : MonoBehaviour, ICharacter
{
	public int _healthPoints;
	public float _movementSpeed;
	public float _firingSpeed;
	public float _projectileRange;
	public float _projectileSpeed;
	public int _projectileDamage;
	public float _hoverRange;

	private Rigidbody2D _rigidBody;
	private ICharacter _player;

	private bool _gameRunning = true;
	private Vector2 _direction;
	private bool _hasFired = false;

	//====================================================
	//
	//====================================================

	public Team Team{ get { return Team.Enemy; } }

	public WeaponType WeaponType{ get { return WeaponType.RedTear; } }

	public Vector3 Position{ get { return this.transform.position; } }

	public Vector2 FiringDirection{ get { return _direction; } }

	public float ProjectileRange{ get { return _projectileRange; } }

	public float ProjectileSpeed{ get { return _projectileSpeed; } }

	public int ProjectileDamage{ get { return _projectileDamage; } }

	//====================================================
	//
	//====================================================

	// Use this for initialization
	void Start ()
	{
		_rigidBody = this.GetComponent<Rigidbody2D> ();
		_player = GameObject.Find ("Player").GetComponent<ICharacter> ();

		_direction = (_player.Position - Position).normalized;

		this.transform.up = _direction;

		Event.Instance.OnGameEndedEvent += OnGameEndedEvent;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_gameRunning) {
			return;
		}

		Vector2 playerDirection = _player.Position - Position;
		_direction = playerDirection.normalized;

		Vector2 randomDirection = new Vector2 (UnityEngine.Random.value, UnityEngine.Random.value);

		Vector2 velocity;

		if (playerDirection.magnitude > _projectileRange) {
			velocity = _direction * _movementSpeed;
		} else if (playerDirection.magnitude < _hoverRange) {
			velocity = -_direction * _movementSpeed;
		} else {
			velocity = randomDirection * _movementSpeed;
		}

		this.transform.up = _direction;
		_rigidBody.velocity = velocity;

		if (!_hasFired) {
			ProjectileFactory.CreateProjectiles (this);

			_hasFired = true;
			Invoke ("OnReloadingOver", _firingSpeed);
		}
	}

	//====================================================
	//
	//====================================================

	public void ApplyDamage (int damage)
	{
		_healthPoints -= damage;

		if (_healthPoints <= 0) {
			GameController.Instance.CharacterDied (this);
			Destroy (this.gameObject);
		}
	}

	private void OnReloadingOver ()
	{
		_hasFired = false;
	}

	private void OnGameEndedEvent ()
	{
		_gameRunning = false;
		_rigidBody.velocity = Vector2.zero;
	}
}
