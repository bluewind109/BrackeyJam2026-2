using UnityEngine;

public class GameDefine
{
    public class PlayerEvents
    {
        public const string OnGetHit = "Player.OnGetHit";
        public const string OnDeath = "Player.OnDeath";
    }
    public class SpellEvents
    {
        public const string OnSpellCast = "Spell.OnSpellCast";
        public const string Spell_FireBall = "Spell.FireBall";
        public const string Spell_IceLances = "Spell.IceLances";
        public const string Spell_WindStep = "Spell.WindStep";
    }
}
