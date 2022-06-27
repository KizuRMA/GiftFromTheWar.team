using UnityEngine;

/// <summary>
/// ����|�X�g�G�t�F�N�g��K�p����
/// ImageEffectAllowedInSceneView�Ƃ����A�g���r���[�g���g�����ƂŃV�[���r���[�ɂ����f�����
/// </summary>
[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class CustomColorPostEffect : MonoBehaviour
{
    [SerializeField] private Material colorEffectMaterial;

    private enum UsePass
    {
        UsePass1,
        UsePass2
    }

    [SerializeField] private UsePass usePass;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, colorEffectMaterial, (int)usePass);
    }
}