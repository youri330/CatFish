using CatFishScripts.Characters;

namespace CatFishScripts.Artifacts {
    public class BasiliskEye : Artifact {
        public BasiliskEye(string name, string description) :
            base(name, description, 0, false, false) { }

        protected override void OnCast(Character character, uint power) {
            if (character.Condition != Character.ConditionType.dead) {
                character.Condition = Character.ConditionType.paralyzed;
            }
        }
    }
}
