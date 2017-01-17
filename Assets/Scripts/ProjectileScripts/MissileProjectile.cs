using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour, IProjectile
{
	public float _lockRange;

	private Rigidbody2D _rigidBody;

	private Team _team;
	private Vector2 _movementDirection;
	private float _range;
	private float _speed;
	private int _damage;

	private bool _targetSet = false;
	private ICharacter _target;

	// Use this for initialization
	void Start ()
	{
		_rigidBody = this.GetComponent<Rigidbody2D> ();

		this.transform.up = _movementDirection;
		_rigidBody.velocity = _movementDirection * _speed;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_targetSet) {
			if (_target == null) {
				_targetSet = false;
			}

			_movementDirection = _target.Position - this.transform.position;
			_movementDirection.Normalize ();

			this.transform.up = _movementDirection;
			_rigidBody.velocity = _movementDirection * _speed;
		} else {
			ICharacter target = FindTarget ();

			if (target != null) {
				_targetSet = true;
				_target = target;
			}
		}
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

	private ICharacter FindTarget ()
	{
		Collider2D[] hits = Physics2D.OverlapCircleAll (new Vector2 (this.transform.position.x, this.transform.position.y), _lockRange);

		ICharacter nearestTarget = null;
		float shortestDistance = float.MaxValue;

		foreach (Collider2D hit in hits) {
			if (hit.tag == "Player" || hit.tag == "Enemy") {
				ICharacter target = hit.GetComponent<ICharacter> ();

				if (_team != target.Team) {
					float distance = (target.Position - this.transform.position).magnitude;

					if (distance < shortestDistance) {
						nearestTarget = target;
						shortestDistance = distance;
					}
				}
			}
		}

		return nearestTarget;
	}
}
