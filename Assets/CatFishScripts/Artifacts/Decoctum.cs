namespace CatFishScripts.Artifacts {
    public class Decoctum : Artifact {
        public Decoctum(string name, string description) :
            base(name, description, 0, false, false) { }
        protected override void OnCast(Characters.Character character, uint power) {
            if (character.Condition == Characters.Character.ConditionType.poisoned) {
                character.Condition = Characters.Character.ConditionType.healthy;
            }
        }
    }
}
