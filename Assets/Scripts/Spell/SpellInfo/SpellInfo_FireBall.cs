using UnityEngine;

[CreateAssetMenu(fileName = "SpellInfo_FireBall", menuName = "Spell/FireBall")]
public class SpellInfo_FireBall : SpellInfo
{
	[Header("FireBall Settings")]
	[SerializeField] private float projectileSpeed = 10f;

	public override void Cast(Player player, Vector3 to)
	{
		Vector3 from = player.transform.position;
		Vector3 direction = (to - from).normalized;
		PlayerProjectileManager.Instance.SpawnProjectile<FireBall>(from, direction, projectileSpeed, Damage);
	}
}
