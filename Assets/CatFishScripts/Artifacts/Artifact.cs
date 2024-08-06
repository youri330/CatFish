using CatFishScripts.Characters;
using System;
using UnityEngine;

namespace CatFishScripts.Artifacts {
    public abstract class Artifact : IMagic {
        //Сила артефакта
        public uint Power {
            get;
            protected set;
        }
        //"Восстанавливаемость" артефакта
        public bool IsRechargeable {
            get;
        }
        //Имеет ли силу
        public bool HasPower {
            get;
        }
        public string Name {
            get;
            private set;
        }
        public string Description {
            get;
            private set;
        }
        public Sprite ArtifactSprite;
        public UnityArtifact unityShell;
        //конструктор
        public Artifact(string name, string description,
                uint power, bool isRechargeable, bool hasPower) {
            this.Power = power;
            this.IsRechargeable = isRechargeable;
            this.HasPower = hasPower;
            this.Name = name;
            this.Description = description;
        }
        public bool IsFightOnly;
        public bool Used = false;
        //абстрактный метод активации артефакта
        protected abstract void OnCast(Character character, uint power = 0);
        //Реализация интерфейса IMagic (выполнение артефакта)
        public void Cast(Character character, uint power) {
            if (!this.HasPower) {
                throw new ArgumentException("Данный артефакт не может обладать мощностью!");
            }
            var deltaPower = Math.Min(this.Power, power);
            this.Power -= deltaPower;
            OnCast(character, deltaPower);
        }
        public void Cast(Character character) {
            if (this.HasPower) {
                throw new ArgumentException("Данный артефакт должен обладать мощностью!");
            }
            OnCast(character, 0);
        }
        public void Cast(Magician initiator, Character character, uint power) {
            throw new NotImplementedException("Инициатор не нужен!");
        }
        public void Cast(Magician initiator, Character character) {
            throw new NotImplementedException("Инициатор не нужен!");
        }


    }
}

