using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlalomTearProjectile : MonoBehaviour, IProjectile
{
	private Rigidbody2D _rigidBody;

	private Team _team;
	private Vector2 _movementDirection;
	private float _range;
	private float _speed;
	private int _damage;

	private Vector2 _sineDirection;

	private Vector3 _previousPosition;
	private float _movedDistance;

	// Use this for initialization
	void Start ()
	{
		_rigidBody = this.GetComponent<Rigidbody2D> ();

		_previousPosition = this.transform.position;

		//set sine direction
		if (_movementDirection.x == 0 && _movementDirection.y == 1) {
			_sineDirection = new Vector2 (1, 0);
		} else if (_movementDirection.x == 0 && _movementDirection.y == -1) {
			_sineDirection = new Vector2 (-1, 0);
		} else if (_movementDirection.x == 1 && _movementDirection.y == 0) {
			_sineDirection = new Vector2 (0, -1);
		} else if (_movementDirection.x == -1 && _movementDirection.y == 0) {
			_sineDirection = new Vector2 (0, 1);
		}

		//
		Vector2 direction = _movementDirection + (_sineDirection * Mathf.Cos (_movedDistance * 3));

		this.transform.up = direction;
		_rigidBody.velocity = direction * _speed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 deltaVector3 = this.transform.position - _previousPosition;
		Vector2 deltaVector2 = Vector2.Scale (new Vector2 (deltaVector3.x, deltaVector3.y), _movementDirection);
		_movedDistance += deltaVector2.magnitude;
		_previousPosition = this.transform.position;

		if (_movedDistance >= _range) {
			Destroy (this.gameObject);
		}

		Vector2 direction = _movementDirection + (_sineDirection * Mathf.Cos (_movedDistance * 3));

		this.transform.up = direction;
		_rigidBody.velocity = direction * _speed;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player" || other.tag == "Enemy") {
			ICharacter charScript = other.GetComponent<ICharacter> ();

			if (_team != charScript.Team) {
				charScript.ApplyDamage (_damage);
				Destroy (this.gameObject);
			}
		}
	}

	public void InitializeValues (Team team, Vector2 direction, float range, float speed, int damage)
	{
		_team = team;
		_movementDirection = direction;
		_range = range;
		_speed = speed;
		_damage = damage;
	}
}
