using UnityEngine;

[CreateAssetMenu(fileName = "Spell_WindStep", menuName = "Spell/WindStep")]
public class Spell_WindStep : Spell
{
	[Header("WindStep Settings")]
	[SerializeField] private PlayerShadow _playerShadowPrefab;
	[SerializeField] private float _castRange = 5f;
    [SerializeField] private float shadowSpeed = 30f;

	public float CastRange => _castRange;

	public override void Cast(Player player, Vector3 to)
	{
		float distance = Vector3.Distance(player.transform.position, to);
		if (distance > _castRange)
		{
			distance = _castRange; // Clamp the distance to the maximum cast range
		}

		PlayerShadow playerShadow = Instantiate(_playerShadowPrefab, player.transform.position, Quaternion.identity);
		playerShadow.Initialize(player, shadowSpeed);

		Vector3 direction = (to - player.transform.position).normalized;
		Vector3 targetPosition = player.transform.position + direction * distance;
		player.transform.position = targetPosition;
	}
}
