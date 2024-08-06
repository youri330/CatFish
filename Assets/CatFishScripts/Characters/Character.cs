using System;
using System.Text;
using System.Threading;
using UnityEngine;

namespace CatFishScripts.Characters {
    public delegate void DeathEventHandler(object sender, EventArgs args);
    public class Character : IComparable {
        public event DeathEventHandler DeathEvent;
        static private uint nextId = 1;
        public string Name {
            get;
        }
        public uint Id {
            get;
        }
        public enum ConditionType { invulnerable, healthy, weakened, ill, poisoned, paralyzed, dead };
        private ConditionType _condition;
        public ConditionType Condition {
            get {
                return _condition;
            }
            set {
                _condition = value;
                CheckCondition();
            }
        }
        public bool IsTalkable {
            get;
            set;
        }
        public bool IsMovable {
            get;
            set;
        }
        public enum RaceType { human, gnome, elf, orc, goblin };
        public RaceType Race {
            get;
        }
        public enum GenderType { male, female };
        public GenderType Gender {
            get;
        }
        public uint Age {
            get;
            set;
        }
        public GameObject gameObject;

        private uint _hp;
        public uint Hp {
            get {
                return _hp;
            }
            set {
                if ((int)value <= 0) {
                    value = 0;
                }
                _hp = value;
                if (_hp > MaxHp) {
                    _hp = MaxHp;
                }
                CheckCondition();
            }
        }
        private Thread poisoningThread;
       /* private void Poisoning() {
            while (Condition == ConditionType.poisoned && Hp > 0) {
                lock (conditionLocker) {
                    _hp--;
                    if (_hp == 0) {
                        Condition = ConditionType.dead;
                    }
                    Monitor.Wait(conditionLocker, 2000);
                }
            }
        }*/
        private void CheckCondition() {
            if (Hp == 0) {
                _condition = ConditionType.dead;
                DeathEvent?.Invoke(this, new EventArgs());
            }
           /* if (Condition == ConditionType.poisoned) {
                poisoningThread = new Thread(Poisoning);
                poisoningThread.Start();
                return;
            } else if (poisoningThread != null) {
                lock (conditionLocker) {
                    poisoningThread.Abort();
                }
            }*/
            if (Condition != ConditionType.healthy && Condition != ConditionType.weakened && 
                Condition != ConditionType.dead) {
                return;
            }
            if (Hp > 0 && Hp < 0.1f * MaxHp) {
                _condition = ConditionType.weakened;
            }
            if (Hp >= 0.1f * MaxHp) {
                _condition = ConditionType.healthy;
            }
        }
        public uint MaxHp {
            get;
            set;
        }
        public uint Xp {
            get;
            set;
        }
        public Inventory.Inventory Inventory {
            get;
            private set;
        }
        public Character(string name, RaceType race, GenderType gender, uint age,
            uint maxHp, uint hp, uint xp = 0, bool isTalkable = true, bool isMovable = true) {
            this.Name = name;
            this.Id = nextId++;
            this.IsTalkable = isTalkable;
            this.IsMovable = isMovable;
            this.Race = race;
            this.Age = age;
            this.MaxHp = maxHp;
            this.Hp = hp;
            this.Xp = xp;
            this.Gender = gender;
            this.Inventory = new Inventory.Inventory(this);
            this.Condition = ConditionType.healthy;
            conditionLocker = new object();
        }
        public int CompareTo(object obj) {
            if (!(obj is Character)) {
                throw new ArgumentException("Неверный тип: ожидался объект Character");
            }
            Character tmp = obj as Character;
            if (this.Xp < tmp.Xp) {
                return -1;
            }
            if (this.Xp > tmp.Xp) {
                return 1;
            }
            return 0;
        }
        public override string ToString() {
            StringBuilder s = new StringBuilder();
            s.Append("ID : " + this.Id.ToString() + "\n");
            s.Append("Имя : " + this.Name + "\n");
            s.Append("Раса : " + this.Race.ToString() + "\n");
            s.Append("Пол : " + this.Gender.ToString() + "\n");
            s.Append("Возраст : " + this.Age.ToString() + "\n");
            s.Append("Максимальное здоровье : " + this.MaxHp.ToString() + "\n");
            s.Append("Здоровье : " + this.Hp.ToString() + "\n");
            s.Append("Состояние : " + this.Condition.ToString() + "\n");
            s.Append("Опыт : " + this.Xp.ToString() + "\n");
            s.Append("Может говорить : " + this.IsTalkable.ToString() + "\n");
            s.Append("Может двигаться : " + this.IsMovable.ToString() + "\n");
            s.Append("Количество артефактов : " + this.Inventory.Artifacts.Count.ToString());
            return s.ToString();
        }
        public object conditionLocker {
            get;
        }

    }
}
