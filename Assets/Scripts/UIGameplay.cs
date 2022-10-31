using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIGameplay : MonoBehaviour
{
    public GameObject dumbell_player_object;
    public GameObject dumbell_require_object;

    private Transform[] dumbell_positions;
    public Transform dumbell_prefab;

    private TextMeshProUGUI dumbell_player;
    private TextMeshProUGUI dumbell_require;

    private Attributes attributes;

    private GameObject dumbell_parent;

    private void Start()
    {
        attributes = GameObject.Find("player_sprite").GetComponent<Attributes>();
        dumbell_parent = GameObject.Find("Spawn Points");
        dumbell_player = dumbell_player_object.GetComponent<TextMeshProUGUI>();
        dumbell_require = dumbell_require_object.GetComponent<TextMeshProUGUI>();
        dumbell_require.SetText(dumbell_parent.transform.childCount.ToString());
        dumbell_positions = dumbell_parent.GetComponentsInChildren<Transform>();
        instantiate_dumbell();
    }

    // Update is called once per frame
    void Update()
    {
        dumbell_player.SetText(attributes.dumbell.ToString());
        if (attributes.dumbell == dumbell_parent.transform.childCount)
        {
            attributes.level_complete = true;
        }
    }

    private void instantiate_dumbell()
    {
        for(int i = 1; i < dumbell_positions.Length; i++)
        {
            Instantiate(dumbell_prefab, dumbell_positions[i].position, Quaternion.identity);
        }
    }
}
