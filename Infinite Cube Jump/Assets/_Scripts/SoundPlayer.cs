using ScriptableObjects;
using UnityEngine;

public class SoundPlayer : Singleton<SoundPlayer>
{
    [SerializeField] private Cube _cube;
    [SerializeField] private AudioClipRefsSO _audioClips;

    private readonly float _volume = 1f;

    private void OnEnable()
    {
        Platform.OnCubeGrounded += Platform_OnCubeGrounded;
        CubeController.OnCubeStartedJump += CubeController_OnCubeStartedJump;
        Cube.OnCubeFell += Cube_OnCubeFell;
    }

    private void OnDisable()
    {
        Platform.OnCubeGrounded -= Platform_OnCubeGrounded;
        CubeController.OnCubeStartedJump -= CubeController_OnCubeStartedJump;
        Cube.OnCubeFell -= Cube_OnCubeFell;
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
        => AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * _volume);

    private void Cube_OnCubeFell(object sender, System.EventArgs e)
    {
        PlaySound(_audioClips.FellClip, _cube.transform.position);
    }

    private void CubeController_OnCubeStartedJump(object sender, System.EventArgs e)
    {
        PlaySound(_audioClips.JumpedClip, _cube.transform.position);
    }

    private void Platform_OnCubeGrounded(object sender, OnCubeGroundedArgs e)
    {
        PlaySound(_audioClips.LandedClip, _cube.transform.position);
    }
}
