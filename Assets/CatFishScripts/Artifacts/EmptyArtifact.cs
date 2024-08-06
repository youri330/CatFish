using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatFishScripts.Characters;

namespace CatFishScripts.Artifacts {
    class EmptyArtifact : Artifact {
        public EmptyArtifact(string name, string description) : base(name, description, 0, false, false) {

        }
        protected override void OnCast(Character character, uint power = 0) {
            
        }
    }
}
