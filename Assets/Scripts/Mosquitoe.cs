using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Mosquitoe : MonoBehaviour
{
    public static Mosquitoe instance;
    public ScriptedMosquitoe _scriptedMosquitoe;
    public SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    public string id {get; private set;}
    public List<string> assignedText = new List<string>();
    public List<string> currentAssignedText;
    private List<string> lettersToAssign = new List<string>() {
        "a", "b", "c", "d", "e", "f", "g",
        "h", "i", "j", "k", "l", "m", "n",
        "o", "p", "q", "r", "s", "t", "u",
        "v", "w", "x", "y", "z"
    };
    private Dictionary<string, int> letterToIndex = new Dictionary<string, int>() {
        {"a", 0}, {"b", 1}, {"c", 2}, {"d", 3}, {"e", 4}, {"f", 5}, {"g", 6},
        {"h", 7}, {"i", 8}, {"j", 9}, {"k", 10}, {"l", 11}, {"m", 12}, {"n", 13},
        {"o", 14}, {"p", 15}, {"q", 16}, {"r", 17}, {"s", 18}, {"t", 19}, {"u", 20},
        {"v", 21}, {"w", 22}, {"x", 23}, {"y", 24}, {"z", 25}
    };
    public bool isDestoyed = false;
    public float speed;
    public int timesHit = 0;
    public bool isFlying {get; private set;} = false;
    Sprite[] spriteArray;
    List<Vector3> imagePositions = new List<Vector3>();
    List<GameObject> childrenImages = new List<GameObject>();

    void Awake()
	{
        instance = this;
        _rb = GetComponent<Rigidbody2D>();
	}

    // Start is called before the first frame update
    void Start()
    {
        InstanciateLetterImages();
        AssignLetters();
        SetImagePositions();
        AssignImagesToLetters();
    }

    void Update()
    {
        if(this.isFlying && !CollisionManager.instance.isGameOver)
        {
            _rb.velocity = (new Vector3(1,0,0) * this.speed);
        } else {
            _rb.velocity = new Vector3(0,0,0);
        }
    }

    public void RemoveLetter(string letterToRemove)
    {
        int index = this.currentAssignedText.IndexOf(letterToRemove);
        this.currentAssignedText.RemoveAt(index);
        RemoveChildrenImage(letterToRemove);
    }

    public async Task FlyMosquitoes()
    {
        // Fly and then pass turn to spawner
        if(this && !this.isDestoyed)
        {
            this.isFlying = true;
            await Task.Delay(1500);
            this.isFlying = false;
        }

        await Task.Yield();
    }

    void InstanciateLetterImages()
    {
        spriteArray = Resources.LoadAll<Sprite>("letterstrim");
    }

    void AssignLetters()
    {
        this.id =  System.Guid.NewGuid().ToString();
        _spriteRenderer.sprite = _scriptedMosquitoe._sprite;
        this.speed = _scriptedMosquitoe.speed;
        while(this.assignedText.Count < _scriptedMosquitoe.assignedTextLength)
        {
            int randomLetterIndex = Random.Range(0, lettersToAssign.Count);
            this.assignedText.Add(lettersToAssign[randomLetterIndex]);
            Debug.Log("Mosquitoe " + id + " has letter " + assignedText);
        }
        currentAssignedText = this.assignedText;
    }

    void AssignImagesToLetters()
    {
        int childrenImageCount = 0;
        var mosquitoeCanvas = this.GetComponentInChildren<Canvas>();
        while(childrenImageCount < _scriptedMosquitoe.assignedTextLength)
        {
            var childrenImageGameObject = new GameObject("ChildrenImage" + this.assignedText[childrenImageCount]);
            childrenImageGameObject.transform.SetParent(mosquitoeCanvas.transform, false);
            childrenImageGameObject.AddComponent<Image>();
            Vector3 imageLocalScale = new Vector3(0.0125f, 0.0125f, 0f);
            if (this._scriptedMosquitoe.name == "ResistentMosquitoe")
            {
                imageLocalScale = new Vector3(0.0175f, 0.0175f, 0f);
            }
            childrenImageGameObject.transform.localScale = imageLocalScale;
            childrenImageGameObject.transform.localPosition = this.imagePositions[childrenImageCount];
            var childrenImage = childrenImageGameObject.GetComponent<Image>();
            childrenImage.sprite = AssignLetterImage(childrenImageCount);
            childrenImages.Add(childrenImageGameObject);
            childrenImageCount++;
        }
    }

    Sprite AssignLetterImage(int childrenImageCount)
    {
        string assignedTextLetter = this.assignedText[childrenImageCount];
        return spriteArray[this.letterToIndex[assignedTextLetter]];
    }

    void SetImagePositions()
    {
        switch(currentAssignedText.Count)
        {
            case 2:
                this.imagePositions.Add(new Vector3(-0.7f, 0f, 0f));
                this.imagePositions.Add(new Vector3(0.7f, 0f, 0f));
                break;
            case 3:
                this.imagePositions.Add(new Vector3(-1.4f, 0f, 0f));
                this.imagePositions.Add(new Vector3(0f, 0f, 0f));
                this.imagePositions.Add(new Vector3(1.4f, 0f, 0f));
                break;
            default:
                this.imagePositions.Add(new Vector3(0f, 0f, 0f));
                break;
        }
    }

    void RemoveChildrenImage(string letterToRemove)
    {
        foreach (GameObject children in this.childrenImages)
        {
            if (children.name == "ChildrenImage" + letterToRemove)
            {
                this.childrenImages.Remove(children);
                Destroy(children);
                break;
            }
        }
        ReassignImagePositions();
    }

    public void ReduceChildrenImageSize(string letterToReduce)
    {
        foreach (GameObject children in this.childrenImages)
        {
            if (children.name == "ChildrenImage" + letterToReduce)
            {
                children.transform.localScale = new Vector3(0.0175f - (0.0025f * this.timesHit), 0.0175f - (0.0025f * this.timesHit), 0f);
                break;
            }
        }
    }

    void ReassignImagePositions()
    {
        Debug.Log("children Images count here " + this.childrenImages.Count);
        this.imagePositions = new List<Vector3>();
        SetImagePositions();
        int childrenImageCount = 0;
        foreach (GameObject children in this.childrenImages)
        {
            children.transform.localPosition = this.imagePositions[childrenImageCount];
            childrenImageCount++;
        }
    }

    public void CheckScriptedLetterActions(Mosquitoe mosquitoe, string letterToCheck)
    {
        if (_scriptedMosquitoe.checkLetterActions.Count == 1)
        {
            _scriptedMosquitoe.checkLetterActions[0].CheckLetters(mosquitoe, letterToCheck);
        }
    }
}
