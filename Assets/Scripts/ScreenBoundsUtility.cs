using UnityEngine;

public static class ScreenBoundsUtility
{
    public static Vector3 ClampPositionInsideCamera(Camera camera, Transform target, Vector3 desiredPosition)
    {
        if (camera == null || target == null)
        {
            return desiredPosition;
        }

        Vector2 worldMin = GetWorldPoint(camera, desiredPosition.z, 0f, 0f);
        Vector2 worldMax = GetWorldPoint(camera, desiredPosition.z, 1f, 1f);

        if (!TryGetTargetOffsets(target, out Vector2 leftBottomOffset, out Vector2 rightTopOffset))
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, worldMin.x, worldMax.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, worldMin.y, worldMax.y);
            return desiredPosition;
        }

        desiredPosition.x = ClampAxis(
            desiredPosition.x,
            worldMin.x + leftBottomOffset.x,
            worldMax.x - rightTopOffset.x,
            (worldMin.x + worldMax.x) * 0.5f + (leftBottomOffset.x - rightTopOffset.x) * 0.5f
        );
        desiredPosition.y = ClampAxis(
            desiredPosition.y,
            worldMin.y + leftBottomOffset.y,
            worldMax.y - rightTopOffset.y,
            (worldMin.y + worldMax.y) * 0.5f + (leftBottomOffset.y - rightTopOffset.y) * 0.5f
        );

        return desiredPosition;
    }

    private static float ClampAxis(float value, float min, float max, float fallback)
    {
        if (min > max)
        {
            return fallback;
        }

        return Mathf.Clamp(value, min, max);
    }

    private static Vector2 GetWorldPoint(Camera camera, float targetZ, float viewportX, float viewportY)
    {
        float depth = Mathf.Abs(targetZ - camera.transform.position.z);
        if (depth < camera.nearClipPlane)
        {
            depth = camera.nearClipPlane;
        }

        Vector3 worldPoint = camera.ViewportToWorldPoint(new Vector3(viewportX, viewportY, depth));
        return new Vector2(worldPoint.x, worldPoint.y);
    }

    private static bool TryGetTargetOffsets(Transform target, out Vector2 leftBottomOffset, out Vector2 rightTopOffset)
    {
        leftBottomOffset = Vector2.zero;
        rightTopOffset = Vector2.zero;

        if (!TryGetTargetBounds(target, out Bounds bounds))
        {
            return false;
        }

        Vector3 targetPosition = target.position;
        leftBottomOffset = new Vector2(
            targetPosition.x - bounds.min.x,
            targetPosition.y - bounds.min.y
        );
        rightTopOffset = new Vector2(
            bounds.max.x - targetPosition.x,
            bounds.max.y - targetPosition.y
        );
        return true;
    }

    private static bool TryGetTargetBounds(Transform target, out Bounds bounds)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>(false);
        Collider2D[] colliders = target.GetComponentsInChildren<Collider2D>(false);

        bool hasBounds = false;
        bounds = new Bounds(target.position, Vector3.zero);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (!hasBounds)
            {
                bounds = renderers[i].bounds;
                hasBounds = true;
                continue;
            }

            bounds.Encapsulate(renderers[i].bounds);
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (!hasBounds)
            {
                bounds = colliders[i].bounds;
                hasBounds = true;
                continue;
            }

            bounds.Encapsulate(colliders[i].bounds);
        }

        return hasBounds;
    }
}
