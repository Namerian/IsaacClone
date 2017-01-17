using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
	void InitializeValues (Team team, Vector2 direction, float range, float speed, int damage);
}
