using UnityEngine;

public class IceLance : Projectile
{
	public override void GameUpdate()
	{
		base.GameUpdate();
		// look towards the direction of movement
		if (_direction != Vector3.zero)
		{
			float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(0f, 0f, angle);
		}
	}
}
