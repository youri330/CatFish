using UnityEngine;
using UnityEngine.UI;
public class ButtonEndTurn : MonoBehaviour {
    public FightSystem fightSystem;
    private void Awake() {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
    private void OnClick() {
        fightSystem.EndTurn();
    }
}
