using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private float currentXAxis = 60f;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float currentDistance = 8f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float zoomRate = 1f;
    private PlayerInput input;
    float currentDistanceValue;
    float currentRotationXAxis = 0;
    private void Start()
    {
        input = GetComponent<PlayerInput>();
        currentDistanceValue = currentDistance;
        currentRotationXAxis = currentXAxis;
    }

    private void Update()
    {
        if (input != null)
        {
            float scrollValue = input.onFoot.ScrollMouse.ReadValue<Vector2>().y;
            float v = scrollValue > 0 ? -1f : 1f;
            if (scrollValue != 0f)
            {
                currentDistanceValue = Mathf.Clamp(currentDistanceValue + v * zoomRate * Time.deltaTime, minDistance, maxDistance);
                CinemachineComponentBase cinemachineCompose = mainCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
                if (cinemachineCompose is CinemachineFramingTransposer)
                {
                    (cinemachineCompose as CinemachineFramingTransposer).m_CameraDistance = currentDistanceValue;
                }
            }
            if (input.onFoot.SwitchCamera.triggered)
            {
                currentRotationXAxis = currentRotationXAxis == currentXAxis ? 90f : currentXAxis;
                mainCamera.transform.rotation = Quaternion.Euler(new Vector3(currentRotationXAxis, 0f, 0f));
            }
        }
    }
}
