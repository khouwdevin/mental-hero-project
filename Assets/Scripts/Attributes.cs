using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attributes : MonoBehaviour
{
    public int health;
    public int dumbell;
    public float speed;
    public float jump_height;
    public float mental = 1f;
    public float mental_charge = 0.05f;
    public float mental_reduce = 0.02f;
    public bool jump;
    public bool level_complete = false;
    public bool game_finish = false;
    public GameObject position_a;
    public GameObject position_b;
}
