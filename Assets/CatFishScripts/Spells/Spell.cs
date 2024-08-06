using CatFishScripts.Characters;
using System;
using UnityEngine;

namespace CatFishScripts.Spells {
    public abstract class Spell : IMagic {
        //Минимальное значение маны для выполнения заклинания
        public uint Cost {
            get;
        }
        //Наличие вербальной компоненты
        public bool IsVerbal {
            get;
        }
        //Наличие моторной компоненты
        public bool IsMotor {
            get;
        }
        //Имеет ли заклинание силу
        public bool HasPower {
            get;
        }
        //сила заклинания
        public int Power {
            get; set;
        }
        public Sprite SpellSprite;
        public string Name;
        public string Description;
        public UnitySpell unityShell;
        public bool IsFightOnly;
        public bool Used;
        //
        protected abstract void OnCast(Character character, uint power);
        //Реализация интерфейса IMagic (выполнения заклинания)
        public void Cast(Magician initiator, Character character, uint power) {
            if (initiator.Condition == Character.ConditionType.dead) {
                throw new ArgumentException("Инициатор не может быть мёртв!");
            }
            if (Cost * power > initiator.Mana) {
                throw new ArgumentException("Недостаточно маны");
            }
            if (this.IsMotor && (!character.IsMovable || character.Condition == Character.ConditionType.paralyzed)) {
                throw new ArgumentException("Персонаж должен уметь двигаться для использования этого заклинания!");
            }
            if (this.IsVerbal && !character.IsTalkable) {
                throw new ArgumentException("Персонаж должен уметь говорить для использования этого заклинания!");
            }
            OnCast(character, power);
            initiator.Mana -= Cost * power;
        }
        public void Cast(Magician initiator, Character character) {
            Cast(initiator, character, 1);
        }
        public void Cast(Character character, uint power) {
            throw new NotImplementedException("Не указан инициатор заклинания!");
        }
        public void Cast(Character character) {
            throw new NotImplementedException("Не указан инициатор заклинания!");
        }
        //Конструктор
        public Spell(uint cost, bool isVerbal, bool isMotor, bool hasPower, int power = 0) {
            this.Cost = cost;
            this.IsVerbal = isVerbal;
            this.IsMotor = isMotor;
            this.HasPower = hasPower;
            this.Power = power;
        }
    }
}
