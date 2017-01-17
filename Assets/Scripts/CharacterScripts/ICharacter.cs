using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
	Team Team{ get; }

	WeaponType WeaponType{ get; }

	Vector3 Position{ get; }

	Vector2 FiringDirection{ get; }

	float ProjectileRange{ get; }

	float ProjectileSpeed{ get; }

	int ProjectileDamage{ get; }

	//================================================

	void ApplyDamage (int damage);
}