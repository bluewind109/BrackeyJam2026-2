using UnityEngine;
using MoreMountains.Tools;
[CreateAssetMenu(fileName = "SpellInfo_IceLances", menuName = "Spell/IceLances")]
public class SpellInfo_IceLances : SpellInfo
{
	[Header("IceLances Settings")]
	[SerializeField] private float _projectileSpeed = 10f;
	[SerializeField] private int _numberOfLances = 5;
	[SerializeField] private int _damagePerLance = 1;

	public override void Cast(int level, Player player, Vector3 to)
	{
		Vector3 from = player.transform.position;
		Vector3 direction = (to - from).normalized;
		int damage = GetLevelInfo(level)?.Damage ?? 1;
		_numberOfLances = damage;

		for (int i = 0; i < _numberOfLances; i++)
		{
			float angleOffset = (i - (_numberOfLances - 1) / 2f) * 10f; // Adjust the angle offset for each lance
			Vector3 rotatedDirection = Quaternion.Euler(0, 0, angleOffset) * direction;
			PlayerProjectileManager.Instance.SpawnProjectile<IceLance>(
				from, 
				rotatedDirection, 
				_projectileSpeed, 
				_damagePerLance
			);
		}

		MMGameEvent.Trigger(GameDefine.SpellEvents.OnSpellCast, stringParameter: GameDefine.SpellEvents.Spell_IceLances);
	}
}
