using UnityEngine;
using UnityEngine.UI;

public class QuestListDemonstrator : MonoBehaviour {
    public QuestSystem questSystem;
    public Sprite TextBackground;
    public void Show() {
        this.gameObject.SetActive(true);
        var quests = questSystem.GetQuestsList();
        int i;
        for (i = 0; i < 9; i++) {
            this.transform.Find(i.ToString()).gameObject.SetActive(false);
        }
        if (questSystem.ActiveQuests == 0) {
            var slot = transform.Find("0");
            slot.gameObject.SetActive(true);
            slot.Find("Message").GetComponent<Text>().text = "Заданий нет";
            return;
        }
        
        i = 0;
        foreach (var quest in quests) {
            if (!quest.IsFinished) {
                var slot = transform.Find(i.ToString());
                slot.gameObject.SetActive(true);
                slot.Find("Message").GetComponent<Text>().text = quest.Description;
                i++;
            }
        }
    }/*
    GameObject AddSlot(string name) {
        GameObject slot = new GameObject(name, typeof(CanvasRenderer), typeof(Image));
        slot.transform.SetParent(this.transform, false);
        slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        slot.GetComponent<Image>().sprite = TextBackground;
        GameObject text = new GameObject("Message", typeof(CanvasRenderer), typeof(Text));
        new Vector3(1, 1, 1);
        text.transform.SetParent(slot.transform, false); //.transform.parent = slot.transform;
        text.GetComponent<RectTransform>().sizeDelta = new Vector2(320, 40);
        
       text.GetComponent<Text>().color = Color.magenta;
      
        return slot;
    }*/
    public void Hide() {
        this.gameObject.SetActive(false);

    }
}