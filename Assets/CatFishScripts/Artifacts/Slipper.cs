using System;

namespace CatFishScripts.Artifacts {
    public class Slipper : Artifact {
        public Slipper(string name, string description, uint power) :
            base(name, description, power, true, false) { }
        protected override void OnCast(Characters.Character character, uint power) {
            if (character.Condition != Characters.Character.ConditionType.invulnerable) {
                character.Hp -= (uint)new System.Random().Next(0, Math.Min((int)this.Power, (int)character.MaxHp) + 1);
                this.Power++;
            }
        }
    }
}
