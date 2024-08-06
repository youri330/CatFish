using System.Text;

namespace CatFishScripts.Characters {
    public class Magician : Character {
        private uint _mana;
        public uint Mana {
            get { return _mana; }
            set {
                if ((int)value <= 0) {
                    value = 0;
                }
                _mana = value;
                if (_mana > MaxMana) {
                    _mana = MaxMana;
                }
            }
        }

        public uint MaxMana {
            get;
            set;
        }
        public Inventory.SpellsList SpellsList {
            get;
            set;
        }

        public Magician(string name, RaceType race, GenderType gender,
            uint age, uint maxHp, uint hp, uint mana, uint maxMana,
            uint xp = 0, bool isTalkable = true, bool isMovable = true)
             : base(name, race, gender, age, maxHp, hp, xp, isTalkable, isMovable) {
            this.MaxMana = maxMana;
            this.Mana = mana;
            this.SpellsList = new Inventory.SpellsList(this);
        }

        public override string ToString() {
            StringBuilder s = new StringBuilder();
            s.Append(base.ToString() + '\n');
            s.Append("Количество заклинаний : " + this.SpellsList.Spells.Count.ToString() + '\n');
            s.Append("Мана: " + this.Mana.ToString() + '\n');
            s.Append("Максимальная мана: " + this.MaxMana.ToString());
            return s.ToString();
        }
    }
}
