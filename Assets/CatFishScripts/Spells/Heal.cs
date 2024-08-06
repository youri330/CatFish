using CatFishScripts.Characters;

namespace CatFishScripts.Spells {
    class Heal : Spell {
        public Heal() : base(20, true, false, false) { }
        protected override void OnCast(Character character, uint power) {
            if (character.Condition == Character.ConditionType.ill) {
                character.Condition = Character.ConditionType.healthy;
            }
        }
    }
}
