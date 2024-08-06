namespace CatFishScripts.Spells {
    class DieOff : Spell {
        public DieOff() : base(150, true, true, false) { }
        protected override void OnCast(Characters.Character character, uint power) {
            if (character.Condition == Characters.Character.ConditionType.dead) {
                character.Hp = 1;
                character.Condition = Characters.Character.ConditionType.healthy;
            }
        }
    }
}
