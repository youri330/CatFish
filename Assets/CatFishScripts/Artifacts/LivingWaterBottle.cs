using CatFishScripts.Characters;

namespace CatFishScripts.Artifacts {
    public class LivingWaterBottle : Bottle {
        public LivingWaterBottle(string name, string description, VolumeType volume) : base(
            name, description, 0, false, volume) { }

        protected override void OnCast(Character character, uint power = 0) {
            if (character.Condition != Character.ConditionType.dead) {
                character.Hp += (uint)this.Volume;
            }
        }
    }
}
