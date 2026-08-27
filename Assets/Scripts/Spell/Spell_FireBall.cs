using UnityEngine;

[CreateAssetMenu(fileName = "Spell_FireBall", menuName = "Spell/FireBall")]
public class Spell_FireBall : Spell
{
	// [Header("FireBall Settings")]

	public override void Cast(Player player, Vector3 to)
	{
		float speed = 10f;
		Vector3 from = player.transform.position;
		Vector3 direction = (to - from).normalized;
		PlayerProjectileManager.Instance.SpawnProjectile<FireBall>(from, direction, speed, Damage);
	}
}
