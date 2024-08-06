namespace CatFishScripts.Artifacts {
    public class PoisonousSaliva : Artifact {
        public PoisonousSaliva(string name, string description, uint power) :
            base(name, description, power, true, true) { }
        protected override void OnCast(Characters.Character character, uint power) {
            if (character.Condition != Characters.Character.ConditionType.invulnerable) {
                character.Hp -= power;
            }
            if (character.Condition == Characters.Character.ConditionType.healthy ||
                character.Condition == Characters.Character.ConditionType.weakened) {
                character.Condition = Characters.Character.ConditionType.poisoned;
            }

        }
    }
}
