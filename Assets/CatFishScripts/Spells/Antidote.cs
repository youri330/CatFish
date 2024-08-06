using CatFishScripts.Characters;

namespace CatFishScripts.Spells {
    class Antidote : Spell {
        public Antidote() : base(30, false, true, false) { }
        protected override void OnCast(Character character, uint power) {
            if (character.Condition == Character.ConditionType.poisoned) {
                character.Condition = Character.ConditionType.healthy;
            }
        }
    }
}
