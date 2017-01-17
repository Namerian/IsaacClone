using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
	Team Team{ get; }

	void ApplyDamage (int damage);
}

public enum Team
{
	Player,
	Enemy
}