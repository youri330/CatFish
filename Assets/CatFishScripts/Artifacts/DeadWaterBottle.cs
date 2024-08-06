using CatFishScripts.Characters;
using System;

namespace CatFishScripts.Artifacts {
    public class DeadWaterBottle : Bottle {
        public DeadWaterBottle(string name, string description, VolumeType volume) :
            base(name, description, 0, false, volume) { }
        protected override void OnCast(Character character, uint power = 0) {
            if (character.GetType() != typeof(Magician)) {
                throw new ArgumentException("Вы можете использовать данный артефакт только на магов!");
            }
            if (character.Condition != Character.ConditionType.dead) {
                (character as Magician).Mana += (uint)this.Volume;
            }
        }
    }
}
