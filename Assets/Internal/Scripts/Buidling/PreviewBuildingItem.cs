using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewBuildingItem : MonoBehaviour
{
    [HideInInspector] public bool isCollision;
    [SerializeField] private float checkRadious = 0.3f;
    [SerializeField] private float offsetY = 0f;
    [SerializeField] private LayerMask colliderMask;
    Vector3 newPos;
    private void Start()
    {
        isCollision = false;
    }
    private void Update()
    {
        isCollision = false;
        Ray forwardRay = new Ray(transform.position + Vector3.up * offsetY, transform.forward);
        Ray rightRay = new Ray(transform.position + Vector3.up * offsetY, transform.right);
        Ray leftRay = new Ray(transform.position + Vector3.up * offsetY, -transform.right);
        Ray backwardRay = new Ray(transform.position + Vector3.up * offsetY,-transform.forward);
        Ray topRay = new Ray(transform.position + Vector3.up * offsetY,transform.up);
        Ray downRay = new Ray(transform.position + Vector3.up * offsetY, -transform.up);
        DrawRayCastList(forwardRay,"forward");
        DrawRayCastList(leftRay,"left");
        DrawRayCastList(rightRay,"right");
        DrawRayCastList(backwardRay,"backward");
        DrawRayCastList(topRay,"top");
        DrawRayCastList(downRay,"down");
    }
    private void DrawRayCastList(Ray ray,string name)
    {
        Debug.DrawRay(ray.origin,ray.direction * checkRadious);
        if(Physics.Raycast(ray,out RaycastHit hit,checkRadious, colliderMask))
        {
            if (hit.collider.gameObject.TryGetComponent<ItemOffset>(out ItemOffset target))
            {
                isCollision = true;
                switch (name)
                {
                    case "forward":
                        newPos = target.GetPosition(ItemSide.Backward);
                        break;
                    case "backward":
                        newPos = target.GetPosition(ItemSide.Forward);
                        break;
                    case "left":
                        newPos = target.GetPosition(ItemSide.Right);
                        break;
                    case "right":
                        newPos = target.GetPosition(ItemSide.Left);
                        break;
                    case "top":
                        newPos = target.GetPosition(ItemSide.Bottom);
                        break;
                    case "down":
                        newPos = target.GetPosition(ItemSide.Top);
                        break;
                }
            }
        }
    }
    public bool IsColliding()
    {
        return isCollision;
    }
    public Vector3 TargetPos()
    {
        return newPos;
    }
}