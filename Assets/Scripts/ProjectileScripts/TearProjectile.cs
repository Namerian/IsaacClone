using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearProjectile : MonoBehaviour, IProjectile
{
	private Rigidbody2D _rigidBody;

	private Team _team;
	private Vector2 _movementDirection = new Vector2 (0, 0);
	private float _range;
	private float _speed;
	private int _damage;

	// Use this for initialization
	void Start ()
	{
		_rigidBody = this.GetComponent<Rigidbody2D> ();

		this.transform.up = _movementDirection;
		_rigidBody.velocity = _movementDirection;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//_rigidBody.velocity = _movementDirection;
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
