using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReplayableExtension;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class Example : MonoBehaviour
{
    public int movePixel = 20;
    public float moveSpeed = 0.1f;

    public Toggle record;
    public Button prefabList;
    public Button replayList;

    public Transform target;
    public Text tips;
    new Camera camera;

    private void Start()
    {
        camera = Camera.main;
        tips.gameObject.SetActive(false);
        prefabList.gameObject.SetActive(false);
        replayList.gameObject.SetActive(false);

        record.onValueChanged.AddListener((b) =>
        {
            if (b)
            {
                RecordManager.instance.StartRecord();
                record.GetComponentInChildren<Text>().text = "结束录制";
            }
            else
            {
                RecordManager.instance.EndRecord(Path.Combine(Application.streamingAssetsPath, "Replay"), System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                record.GetComponentInChildren<Text>().text = "开始录制";
                RefreshReplayList();
            }
        });

        RefreshPrefabList();


        RefreshReplayList();
    }
    void Update()
    {
        if (RecordAndReplayBase.currentState == RecordAndReplayBase.State.Replaying) return;
        float speedX, speedZ;
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        if (mousePosition.x <= movePixel)
            speedX = -1;
        else if (mousePosition.x >= Screen.width - movePixel)
            speedX = 1;
        else
            speedX = 0;

        if (mousePosition.y <= movePixel)
            speedZ = -1;
        else if (mousePosition.y >= Screen.height - movePixel)
            speedZ = 1;
        else
            speedZ = 0;
        if (speedX != 0 || speedZ != 0)
            camera.transform.RePosition(new Vector3(Mathf.Clamp(camera.transform.position.x + speedX * moveSpeed, -30, 30), camera.transform.position.y, Mathf.Clamp(camera.transform.position.z + speedZ * moveSpeed, -30, 30)));

        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Physics.Raycast(camera.ScreenPointToRay(mousePosition), out RaycastHit hitInfo))
            {
                if (hitInfo.transform.TryGetComponent<ReplayableUnit>(out _))
                {
                    tips.gameObject.SetActive(true);
                    target.gameObject.ReActive(true);
                    target.ReParent(hitInfo.transform);
                    target.ReLoaclPosition(Vector3.zero);
                }
            }
        }
        if (Mouse.current.rightButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            tips.gameObject.SetActive(false);
            target.gameObject.ReActive(false);
            target.ReParent(null);
        }
        if(target.parent != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                target.parent.GetComponent<Animation>()?.RePlay();
            if (Keyboard.current.digit2Key.wasPressedThisFrame)
                target.parent.Find("1").GetComponent<Renderer>()?.ReColor("_Color",Random.ColorHSV());
            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                GameObject parent = target.parent.gameObject;
                tips.gameObject.SetActive(false);
                target.gameObject.ReActive(false);
                target.ReParent(null);
                ReplayableAPI.ReDestroy(parent);
            }
            if (Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed)
                speedZ = 1;
            else if (Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed)
                speedZ = -1;
            else
                speedZ = 0;
            if (Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed)
                speedX = 1;
            else if (Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
                speedX= -1;
            else
                speedX = 0;
            if (speedX != 0 || speedZ != 0)
                target.parent.RePosition(new Vector3(Mathf.Clamp(target.parent.position.x + speedX * moveSpeed, -30, 30), target.parent.position.y, Mathf.Clamp(target.parent.position.z + speedZ * moveSpeed, -30, 30)));
        }
    }

    List<GameObject> buttonPrefabList = new List<GameObject>();
    void RefreshPrefabList()
    {
        List<ReplayableUnit> list = CreateManager.instance.replayablePrefabs;
        foreach (var item in buttonPrefabList)
        {
            Destroy(item);
        }
        buttonPrefabList.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            Button oneReplay = Instantiate(prefabList, prefabList.transform.parent);
            oneReplay.gameObject.SetActive(true);
            ((RectTransform)oneReplay.transform).position += Vector3.down * i * ((RectTransform)oneReplay.transform).sizeDelta.y;
            oneReplay.GetComponentInChildren<Text>().text = list[i].name;
            int index = i;
            oneReplay.onClick.AddListener(() =>
            {
                ReplayableUnit unit = ReplayableAPI.ReInstantiate(list[index]);
                unit.transform.RePosition(camera.transform.position + new Vector3(0, -5, 7));
            });
            buttonPrefabList.Add(oneReplay.gameObject);
        }
        ((RectTransform)prefabList.transform.parent.transform).sizeDelta = new Vector2(((RectTransform)prefabList.transform.parent.transform).sizeDelta.x, ((RectTransform)prefabList.transform).sizeDelta.y * list.Count);
    }

    List<GameObject> buttonReplayList = new List<GameObject>();
    void RefreshReplayList()
    {
        List<string> list = new List<string>(Directory.GetFiles(Path.Combine(Application.streamingAssetsPath, "Replay"), "*.replay"));

        foreach (var item in buttonReplayList)
        {
            Destroy(item);
        }
        buttonReplayList.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            Button oneReplay = Instantiate(replayList, replayList.transform.parent);
            oneReplay.gameObject.SetActive(true);
            ((RectTransform)oneReplay.transform).position += Vector3.down * i * ((RectTransform)oneReplay.transform).sizeDelta.y;
            oneReplay.GetComponentInChildren<Text>().text = Path.GetFileNameWithoutExtension(list[i]);
            int index = i;
            oneReplay.onClick.AddListener(() =>
            {
                ReplayManager.instance.StartReplay(list[index]);
            });
            buttonReplayList.Add(oneReplay.gameObject);
        }
        ((RectTransform)replayList.transform.parent.transform).sizeDelta = new Vector2(((RectTransform)replayList.transform.parent.transform).sizeDelta.x, ((RectTransform)replayList.transform).sizeDelta.y * list.Count);
    }
}
