using System.Threading;

namespace CatFishScripts.Spells {
    class Armor : Spell {
        int power;
        Characters.Character character;
        public Armor() : base(50, false, true, true) { }
        protected override void OnCast(Characters.Character character, uint power) {
            if (character.Condition != Characters.Character.ConditionType.invulnerable &&
            character.Condition != Characters.Character.ConditionType.dead) {
                this.power = (int)power;
                this.character = character;
                Thread waitingThread = new Thread(LockCondition);
                waitingThread.Start();
            }
        }
        private void LockCondition() {
            lock (character.conditionLocker) {
                var condition = character.Condition;
                character.Condition = Characters.Character.ConditionType.invulnerable;
                Monitor.Wait(character.conditionLocker, 2000 * power);
                character.Condition = condition;
            }
        }
    }
}

