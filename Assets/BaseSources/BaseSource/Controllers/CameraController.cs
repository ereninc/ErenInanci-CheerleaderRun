using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : ControllerBaseModel
{
    [SerializeField] CinemachineVirtualCamera[] virtualCameras;
    public CinemachineVirtualCamera ActiveCamera;

    public void ChangeCamera(int index)
    {
        ActiveCamera.SetActiveGameObject(false);
        ActiveCamera = virtualCameras[index];
        ActiveCamera.SetActiveGameObject(true);
    }

    private void Reset()
    {
        ActiveCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (ActiveCamera != null)
        {
            if (virtualCameras.Length == 0)
            {
                virtualCameras = new CinemachineVirtualCamera[]
                {
                    ActiveCamera
                };
            }
        }
        base.Reset();
    }


}
