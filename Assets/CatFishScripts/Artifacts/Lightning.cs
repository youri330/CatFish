namespace CatFishScripts.Artifacts {
    public class Lightning : Artifact {
        public Lightning(string name, string description, uint power) :
            base(name, description, power, true, true) { }
        protected override void OnCast(Characters.Character character, uint power) {
            if (character.Condition != Characters.Character.ConditionType.invulnerable) {
                character.Hp -= power;
            }
        }
    }
}
