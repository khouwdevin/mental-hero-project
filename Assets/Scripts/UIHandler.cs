using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public AudioSource ui_sound;
    public AudioClip ui_sound_clip;

    public void play_ui_sound()
    {
        ui_sound.PlayOneShot(ui_sound_clip);
    }
}
