using CatFishScripts;
using System.Collections.Generic;
using UnityEngine;

public class QuestSystem : MonoBehaviour {
    List<Quest> quests;
    ChangeMessage message;

    public QuestListDemonstrator demonstrator;
    public int ActiveQuests {
        get;
        set;
    }
    public List<Quest> GetQuestsList() {
        return quests;
    }
    public void RemoveAllQuests() {
        quests = new List<Quest>();
        ActiveQuests = 0;
    }
    void Awake() {
        message = GameObject.Find("Canvas").transform.Find("MessageBox").GetComponent<ChangeMessage>();
        quests = new List<Quest>();
    }

    public void Add(Quest quest) {
        if (this.GetByTag(quest.Tag) != null) {
            return;
        }
        quests.Add(quest);
        message.Hide();
        if (!quest.IsFinished) {
            message.Show("Задание: " + quest.Description);

            ActiveQuests++;
        }
        if (demonstrator.gameObject.activeSelf) {
            demonstrator.Hide();
            demonstrator.Show();
        }
    }
    public Quest GetByTag(string tag) {
        return quests.Find(obj => obj.Tag == tag);
    }
}
