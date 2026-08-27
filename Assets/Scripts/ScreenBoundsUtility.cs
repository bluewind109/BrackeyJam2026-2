using UnityEngine;

public static class ScreenBoundsUtility
{
    public static Vector3 ClampPositionInsideCamera(Camera camera, Transform target, Vector3 desiredPosition)
    {
        if (camera == null || target == null)
        {
            return desiredPosition;
        }

        GetViewportBounds(camera, out Vector2 viewportMin, out Vector2 viewportMax);
        Vector2 worldMin = GetWorldPoint(camera, desiredPosition.z, viewportMin.x, viewportMin.y);
        Vector2 worldMax = GetWorldPoint(camera, desiredPosition.z, viewportMax.x, viewportMax.y);

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

    public static bool IsOutsideCamera(Camera camera, Transform target)
    {
        if (camera == null || target == null)
        {
            return false;
        }

        GetViewportBounds(camera, out Vector2 viewportMin, out Vector2 viewportMax);
        Vector2 worldMin = GetWorldPoint(camera, target.position.z, viewportMin.x, viewportMin.y);
        Vector2 worldMax = GetWorldPoint(camera, target.position.z, viewportMax.x, viewportMax.y);

        if (!TryGetTargetBounds(target, out Bounds bounds))
        {
            Vector3 position = target.position;
            return position.x < worldMin.x ||
                   position.x > worldMax.x ||
                   position.y < worldMin.y ||
                   position.y > worldMax.y;
        }

        return bounds.max.x < worldMin.x ||
               bounds.min.x > worldMax.x ||
               bounds.max.y < worldMin.y ||
               bounds.min.y > worldMax.y;
    }

    private static void GetViewportBounds(Camera camera, out Vector2 viewportMin, out Vector2 viewportMax)
    {
        viewportMin = Vector2.zero;
        viewportMax = Vector2.one;

        CameraBoundsOffsets offsets = camera.GetComponent<CameraBoundsOffsets>();
        if (offsets == null)
        {
            return;
        }

        Rect pixelRect = camera.pixelRect;
        float width = Mathf.Max(1f, pixelRect.width);
        float height = Mathf.Max(1f, pixelRect.height);

        float minX = offsets.LeftPixels / width;
        float maxX = 1f - (offsets.RightPixels / width);
        float minY = offsets.BottomPixels / height;
        float maxY = 1f - (offsets.TopPixels / height);

        viewportMin.x = Mathf.Clamp01(minX);
        viewportMax.x = Mathf.Clamp01(maxX);
        viewportMin.y = Mathf.Clamp01(minY);
        viewportMax.y = Mathf.Clamp01(maxY);

        if (viewportMin.x > viewportMax.x)
        {
            float centerX = (viewportMin.x + viewportMax.x) * 0.5f;
            viewportMin.x = centerX;
            viewportMax.x = centerX;
        }

        if (viewportMin.y > viewportMax.y)
        {
            float centerY = (viewportMin.y + viewportMax.y) * 0.5f;
            viewportMin.y = centerY;
            viewportMax.y = centerY;
        }
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
