using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelBrush : MonoBehaviour
{
    [SerializeField] GameObject brushIndicator;
    Transform brushIndicatorTransform;
    public VoxelCore vc;

    public bool PaintPressed = false;
    public Color BrushColor;

[SerializeField] public GameObject rightC;

    public PrimaryButtonWatcher watcher;

    // Start is called before the first frame update
    void Start()
    {
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
        brushIndicatorTransform = brushIndicator.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        moveAndClipIndicator();
        handlePaintSignal();
    }

    void moveAndClipIndicator()
    {
        Transform selfTransform = GetComponent<Transform>();
        selfTransform = rightC.transform;
        brushIndicatorTransform.position = new Vector3(
            Mathf.Floor(selfTransform.position.x),
            Mathf.Floor(selfTransform.position.y),
            Mathf.Floor(selfTransform.position.z));
    }

    void handlePaintSignal()
    {
        if (PaintPressed)
        {
            PaintPressed = false;
            Vector3 brushIndicatorPosition = brushIndicatorTransform.position;
            vc.WriteVoxel(
                (int) brushIndicatorPosition.x,
                (int) brushIndicatorPosition.y,
                (int) brushIndicatorPosition.z,
                BrushColor);
        }
    }

    public void onPrimaryButtonEvent(bool pressed) {
        PaintPressed = true;
    }
}
