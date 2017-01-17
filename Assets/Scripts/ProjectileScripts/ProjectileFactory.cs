using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileFactory
{
	public static void CreateProjectiles (ICharacter shooter)
	{
		switch (shooter.WeaponType) {
		case WeaponType.BlueTear:
			CreateBlueTear (shooter);
			break;
		case WeaponType.BlueMissile:
			CreateBlueMissile (shooter);
			break;
		case WeaponType.SlalomTear:
			CreateSlalomTear (shooter);
			break;
		case WeaponType.RedTear:
			CreateRedTear (shooter);
			break;
		case WeaponType.RedMissile:
			CreateRedMissile (shooter);
			break;
		}
	}

	//========================================================
	//
	//========================================================

	private static void CreateBlueTear (ICharacter shooter)
	{
		GameObject projectileObj = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Projectiles/BlueTearProjectile"), shooter.Position, Quaternion.identity) as GameObject;
		IProjectile projectileScript = projectileObj.GetComponent<IProjectile> ();
		projectileScript.InitializeValues (shooter.Team, shooter.FiringDirection, shooter.ProjectileRange, shooter.ProjectileSpeed, shooter.ProjectileDamage);
	}

	private static void CreateBlueMissile (ICharacter shooter)
	{
		GameObject projectileObj = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Projectiles/BlueMissileProjectile"), shooter.Position, Quaternion.identity) as GameObject;
		IProjectile projectileScript = projectileObj.GetComponent<IProjectile> ();
		projectileScript.InitializeValues (shooter.Team, shooter.FiringDirection, shooter.ProjectileRange, shooter.ProjectileSpeed, shooter.ProjectileDamage);
	}

	private static void CreateSlalomTear (ICharacter shooter)
	{
		GameObject projectileObj = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Projectiles/SlalomTearProjectile"), shooter.Position, Quaternion.identity) as GameObject;
		IProjectile projectileScript = projectileObj.GetComponent<IProjectile> ();
		projectileScript.InitializeValues (shooter.Team, shooter.FiringDirection, shooter.ProjectileRange, shooter.ProjectileSpeed, shooter.ProjectileDamage);
	}

	private static void CreateRedTear (ICharacter shooter)
	{
		GameObject projectileObj = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Projectiles/RedTearProjectile"), shooter.Position, Quaternion.identity) as GameObject;
		IProjectile projectileScript = projectileObj.GetComponent<IProjectile> ();
		projectileScript.InitializeValues (shooter.Team, shooter.FiringDirection, shooter.ProjectileRange, shooter.ProjectileSpeed, shooter.ProjectileDamage);
	}

	private static void CreateRedMissile (ICharacter shooter)
	{
		GameObject projectileObj = MonoBehaviour.Instantiate (Resources.Load ("Prefabs/Projectiles/RedMissileProjectile"), shooter.Position, Quaternion.identity) as GameObject;
		IProjectile projectileScript = projectileObj.GetComponent<IProjectile> ();
		projectileScript.InitializeValues (shooter.Team, shooter.FiringDirection, shooter.ProjectileRange, shooter.ProjectileSpeed, shooter.ProjectileDamage);
	}
}
