using UnityEngine;

[CreateAssetMenu(fileName = "Spell_FireBall", menuName = "Spell/FireBall")]
public class Spell_FireBall : Spell
{
	[Header("FireBall Settings")]
	[SerializeField] private FireBall _fireBallPrefab;

	public override void Cast(Vector3 from, Vector3 to)
	{
		if (_fireBallPrefab == null) return;
		FireBall spellInstance = Instantiate(_fireBallPrefab, from, Quaternion.identity);
		Vector3 direction = (to - from).normalized;
		float speed = 10f;
		spellInstance.Initialize(direction, speed);
	}
}
