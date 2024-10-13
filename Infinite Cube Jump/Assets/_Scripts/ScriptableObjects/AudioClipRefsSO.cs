using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class AudioClipRefsSO : ScriptableObject
    {
        [field: SerializeField] public AudioClip JumpedClip { get; private set; }
        [field: SerializeField] public AudioClip LandedClip { get; private set; }
        [field: SerializeField] public AudioClip FellClip { get; private set; }
    }
}
