using CatFishScripts.Characters;

namespace CatFishScripts {
    public class Quest {
        public string Tag {
            get;
            set;
        }
        public Character sender {
            get;
        }
        public string Description {
            get;
            set;
        }
        public bool IsFinished {
            get;
            set;
        }
        public Quest(string tag, Character sender, string description) {
            this.Tag = tag;
            this.sender = sender;
            this.Description = description;
            this.IsFinished = false;
        }
        public Quest(string tag, Character sender, string description, bool isFinished) {
            this.Tag = tag;
            this.sender = sender;
            this.Description = description;
            this.IsFinished = isFinished;
        }
    }
}
