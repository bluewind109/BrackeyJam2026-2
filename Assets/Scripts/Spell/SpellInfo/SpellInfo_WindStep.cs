using UnityEngine;
using MoreMountains.Tools;
[CreateAssetMenu(fileName = "SpellInfo_WindStep", menuName = "Spell/WindStep")]
public class SpellInfo_WindStep : SpellInfo
{
	[Header("WindStep Settings")]
	[SerializeField] private PlayerShadow _playerShadowPrefab;
	[SerializeField] private float _castRange = 5f;
	[SerializeField] private float _shadowSpeed = 30f;

	public float CastRange => _castRange;

	public override void Cast(int level, Player player, Vector3 to)
	{
		float distance = Vector3.Distance(player.transform.position, to);
		if (distance > _castRange)
		{
			distance = _castRange; // Clamp the distance to the maximum cast range
		}

		int damage = GetLevelInfo(level)?.Damage ?? 1;

		PlayerShadow playerShadow = Instantiate(_playerShadowPrefab, player.transform.position, Quaternion.identity);
		playerShadow.Initialize(player, _shadowSpeed, damage);

		Vector3 direction = (to - player.transform.position).normalized;
		Vector3 targetPosition = player.transform.position + direction * distance;
		player.SetPosition(targetPosition);

		MMGameEvent.Trigger(GameDefine.SpellEvents.OnSpellCast, stringParameter: GameDefine.SpellEvents.Spell_WindStep);
	}
}
