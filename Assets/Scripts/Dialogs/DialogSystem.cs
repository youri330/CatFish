using System;
using System.IO;
using UnityEngine;
public class Speech {
    public string Message;
    public Color Color;
    public int Delay;
}
public class DialogEventArgs{
    readonly public string dialogFile;
    public DialogEventArgs(string dialogFile) {
        this.dialogFile = dialogFile;
    }
}
public delegate void DialogEventHandler (object sender, DialogEventArgs eventArgs);
public class DialogSystem : MonoBehaviour {
    public event DialogEventHandler DialogEndedEvent;
    public GameObject DialogMessageBox;
    bool isActive;
    string fileName;
    private DialogMessage message;
    const char speechSeparator = '~';
    const char separator = '^';
    private void Awake() {
        message = DialogMessageBox.GetComponent<DialogMessage>();
    }
    private Speech[] Parse(string fileName) {
        Speech[] speeches;
        this.fileName = fileName;
        using (StreamReader sr = new StreamReader(@"Assets\Dialogs\" + fileName)) {
            var speechStrings = sr.ReadToEnd().Split(speechSeparator);
            speeches = new Speech[speechStrings.Length];
            int i = 0;
            foreach (var speechString in speechStrings) {
                var chars = speechString.Split(separator);
                speeches[i] = new Speech();
                speeches[i].Delay = Int32.Parse(chars[0]);
                switch (chars[1]) {
                    case ("black"):
                        speeches[i].Color = Color.black;
                        break;
                    case ("grey"):
                        speeches[i].Color = Color.grey;
                        break;
                    case ("magenta"):
                        speeches[i].Color = Color.magenta;
                        break;
                    case ("green"):
                        speeches[i].Color = Color.green;
                        break;
                    case ("blue"):
                        speeches[i].Color = Color.cyan;
                        break;
                }
                speeches[i].Message = chars[2];
                i++;
            }
        }
        return speeches;
    }
    Speech[] speeches;
    private int index = 0;
    public void Show(string fileName) {
        if (isActive)
            return;
        speeches = Parse(fileName);
        index = 0;
        isActive = true;
        SpeechesLoop();
    }
    private void SpeechesLoop() {
        if (index < speeches.Length && speeches[index] != null) {
            message.Show(speeches[index]);
            Debug.Log(speeches[index].Delay);
            Invoke("SpeechesLoop", speeches[index++].Delay);
            return;
        }
        isActive = false;
        message.Hide();
        DialogEndedEvent?.Invoke(this, new DialogEventArgs(this.fileName));
    }
}

