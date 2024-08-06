namespace CatFishScripts.Artifacts {
    public abstract class Bottle : Artifact {
        public enum VolumeType { Small = 10, Medium = 25, Big = 50 };
        public VolumeType Volume {
            get;
            set;
        }
        public Bottle(string name, string description, uint power, bool isRechargeable, VolumeType volume)
            : base(name, description, power, isRechargeable, false) {
            Volume = volume;
        }
        abstract protected override void OnCast(Characters.Character character, uint power);
    }
}
