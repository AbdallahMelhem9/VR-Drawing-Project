/*
 Public file of VoxelCore. Every public methods and fields are here.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Voxel4.Internal;

namespace Voxel4
{
    /*
     Handles:
    - default fields
    - initilisation
    - Main Unity Callbacks
    - Other callbacks (if any)
     */
    public partial class VoxelCore : MonoBehaviour
    {
        // ---------------- API ----------------
        #region API

        /// <summary>
        /// Size use to display the voxels. Different from the size
        /// of the voxel brush.
        /// Can be used for 2 things:
        /// 1) as a zoom level to help with painting
        /// 2) as an export settings for the paint.
        ///
        /// Does not control the size of the brush. This is a different
        /// brush parameter.
        /// </summary>
        public float VoxelSize
        {
            get => _voxelSize;
            set
            {
                float pastVoxelSize = _voxelSize;
                _voxelSize = Mathf.Max(MinVoxelSize, value);
                if (pastVoxelSize != _voxelSize)
                {
                    _renderingController.VoxelSizeHasChanged();
                }
            }
        }
        public const float MinVoxelSize = 0.1f;
        [SerializeField]
        float _voxelSize = 1.0f;

        /// <summary>
        /// Controls what action is triggered by the paint button,
        /// and its underlying settings
        /// </summary>
        public VoxelTool Tool = new VoxelTool();
        /*
        VoxelTool _tool = new VoxelTool();
        public VoxelTool Tool
        {
            get => _tool;
            set
            {
                Debug.Log("tool setter");
                // settings all member variables to apply mutators
                _tool.BrushOffset = Tool.BrushOffset;
                _tool.ToolSize = Tool.ToolSize;
                _tool.Action = Tool.Action;
                _tool.Shape = Tool.Shape;
                _tool.Color = Tool.Color;
            }
        }*/


        /// <summary>
        /// Transform to the device used as a brush
        /// </summary>
        public Transform BrushDeviceTransform;

        /// <summary>
        /// A prefab to use as a Chunk.
        /// What really matters is associated Material.
        /// </summary>
        public GameObject ChunkTemplateGO;

        /// <summary>
        /// Button watcher for key inputs
        /// </summary>
        public PrimaryButtonWatcher watcher;

        void Start(){
            watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
        }

        void onPrimaryButtonEvent(bool pressed){
            if (pressed){
                Paint();
            }
        }

        /// <summary>
        /// Iterator to export the voxels
        /// </summary>
        public List<(int, int, int, Color)> GetAllVoxels()
        {
            var l = new List<(int, int, int, Color)>();
            foreach(var kvp in _chunkNet.chunks)
            {
                Common.ActOnMatrixIterate(Chunk.Dimensions.x, Chunk.Dimensions.y, Chunk.Dimensions.z, (x, y, z) =>
                {
                    if(kvp.Value[x, y, z] != null)
                    {
                        l.Add((
                        Chunk.Dimensions.x * kvp.Key.Item1 + x,
                        Chunk.Dimensions.y * kvp.Key.Item2 + y,
                        Chunk.Dimensions.z * kvp.Key.Item3 + z,
                        kvp.Value[x, y, z].Color));
                    }
                });
            }
            return l;
        }

        public VoxelIterator GetVoxelIterator()
        {
            return new VoxelIterator(_chunkNet);
        }

        /// <summary>
        /// Paint signal, actually what it does depends on the
        /// Tool parameters
        /// </summary>
        public void Paint()
        {
            _brushController.Paint();
        }

        /// <summary>
        /// A Simple writing method that writes voxels and keeps that of which chunk have been
        /// modified, so they can be re-rendered later.
        /// </summary>
        /// <param name="x">x coordinate of the voxel</param>
        /// <param name="y">y coordinate of the voxel</param>
        /// <param name="z">z coordinate of the voxel</param>
        /// <param name="color"></param>
        public void WriteVoxel(int x, int y, int z, Color color)
        {
            _voxelWriter.WriteVoxel(x, y, z, color);
        }

        /// <summary>
        /// A Simple writing method that erase voxels and keeps that of which chunk have been
        /// modified, so they can be re-rendered later.
        /// </summary>
        /// <param name="x">x coordinate of the voxel</param>
        /// <param name="y">y coordinate of the voxel</param>
        /// <param name="z">z coordinate of the voxel</param>
        /// <param name="color"></param>
        public void EraseVoxel(int x, int y, int z)
        {
            _voxelWriter.EraseVoxel(x, y, z);
        }

        #endregion

        // ------------ Chunk Net + VC Systems -------------
        #region Systems
        private ChunkNet _chunkNet = new ChunkNet();

        private BrushController _brushController;
        private ChunkMeshing _chunkMeshing;
        private RenderingController _renderingController;
        private VoxelWriter _voxelWriter;

        #endregion
        // ---------- Unity Callbacks ----------
        #region UnityCallbacks

        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("VoxelCore Awake Init");

            _brushController = new BrushController(this);
            _chunkMeshing = new ChunkMeshing(this);
            _renderingController = new RenderingController(this);
            _voxelWriter = new VoxelWriter(this);

            Chunk.CommonChunkTemplate = ChunkTemplateGO;
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log("voxel core update");

            // Brush Preview Update
            _brushController.FrameUpdate();

            // Renderer Update
            _renderingController.FrameUpdate();
        }

        #endregion
    }
}


