using UnityEngine;

[CreateAssetMenu(fileName = "Spell_IceLances", menuName = "Spell/IceLances")]
public class Spell_IceLances : Spell
{
	[Header("IceLances Settings")]
	[SerializeField] private int _numberOfLances = 5;

	public override void Cast(Player player, Vector3 to)
	{
		Vector3 from = player.transform.position;
		Vector3 direction = (to - from).normalized;
		float speed = 10f;

		for (int i = 0; i < _numberOfLances; i++)
		{
			float angleOffset = (i - (_numberOfLances - 1) / 2f) * 10f; // Adjust the angle offset for each lance
			Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * direction;
			PlayerProjectileManager.Instance.SpawnProjectile<IceLance>(from, rotatedDirection, speed, Damage);
		}
	}
}
