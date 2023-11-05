using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Voxel4;

public class FakeBrushController : MonoBehaviour
{
    public float VoxelSize = 1.0f;
    // public Color Color = Color.green;
    public VoxelTool Tool;

    public VoxelCore vc;

    public bool paintSignal = false;
    public bool getIterator = false;
    public bool realPaintSignal = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Fakebrush Init");

        /*
        var Colors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.cyan,
            Color.yellow,
            Color.magenta,
        };
        int tempIndex = 0;
        int cIndex = 0;
        for(int z = -32; z < 32; z++)
        {
            GetComponent<Transform>().position = new Vector3(0, 0, z);
            if (tempIndex >= 16)
            {
                tempIndex = 0;
                cIndex++;
            }
            Tool.Color = Colors[cIndex%Colors.Length]; 
            simpleWrite();
            tempIndex++;
            
        }
        */

        var colors = new List<Color>();
        for (int i = 0; i < 16; i++)
        {
            colors.Add(new Color(i * 0.60f, i * 0.60f, i * 0.60f));
        }
        // int index = 0;
        /*
        for(int z = -32; z <= 31; z++)
        {
            for(int x = -50; x <= 50; x++)
            {
                GetComponent<Transform>().position = new Vector3(x, 0, 0);
                Tool.Color = colors[index % colors.Count];
                simpleWrite();
                index++;
                //Debug.Log(colors[index % colors.Count]);
            }
        }
        */


        /*
        for(int y = -24; y < 24; y++)
        {
            GetComponent<Transform>().position = new Vector3(0, y, 0);
            Tool.Color = colors[index % colors.Count];
            simpleWrite();
            index++;
        }
        */

        /*
        Debug.Log("===========");
        for (int z = -6; z < 6; z++)
        {
            GetComponent<Transform>().position = new Vector3(0, 0, z);
            Tool.Color = colors[index % colors.Count];
            simpleWrite();
            index++;
        }
        */

        /*
        vc.WriteVoxel(0, 0, 0, Color.red);
        vc.WriteVoxel(1, 0, 0, Color.green);
        vc.WriteVoxel(2, 0, 0, Color.blue);
        vc.WriteVoxel(3, 0, 0, Color.cyan);
        vc.WriteVoxel(4, 0, 0, Color.yellow);
        vc.WriteVoxel(5, 0, 0, Color.magenta);

        vc.WriteVoxel(-6, 0, 0, Color.red);
        vc.WriteVoxel(-5, 0, 0, Color.green);
        vc.WriteVoxel(-4, 0, 0, Color.blue);
        vc.WriteVoxel(-3, 0, 0, Color.cyan);
        vc.WriteVoxel(-2, 0, 0, Color.yellow);
        vc.WriteVoxel(-1, 0, 0, Color.magenta);
        */

        /*
        vc.WriteVoxel(0, 0, 0, Color.red);
        vc.WriteVoxel(0, 1, 0, Color.green);
        vc.WriteVoxel(0, 2, 0,Color.blue);
        vc.WriteVoxel(0, 3, 0,Color.cyan);
        vc.WriteVoxel(0, 4, 0,Color.yellow);
        vc.WriteVoxel(0, 5, 0,Color.magenta);

        vc.WriteVoxel(0, -6, 0, Color.red);
        vc.WriteVoxel(0, -5, 0, Color.green);
        vc.WriteVoxel(0, -4, 0,Color.blue);
        vc.WriteVoxel(0, -3, 0,Color.cyan);
        vc.WriteVoxel(0, -2, 0,Color.yellow);
        vc.WriteVoxel(0, -1, 0,Color.magenta);
        */

        vc.WriteVoxel(0, 0, 0, Color.red);
        vc.WriteVoxel(0, 0, 1, Color.green);
        vc.WriteVoxel(0, 0, 2, Color.blue);
        vc.WriteVoxel(0, 0, 3, Color.cyan);
        vc.WriteVoxel(0, 0, 4, Color.yellow);
        vc.WriteVoxel(0, 0, 5, Color.magenta);

        vc.WriteVoxel(0, 0, -6, Color.red);
        vc.WriteVoxel(0, 0, -5, Color.green);
        vc.WriteVoxel(0, 0, -4, Color.blue);
        vc.WriteVoxel(0, 0, -3, Color.cyan);
        vc.WriteVoxel(0, 0, -2, Color.yellow);
        vc.WriteVoxel(0, 0, -1, Color.magenta);

        var voxIter = vc.GetVoxelIterator();
        while(voxIter.MoveNext())
        {
            var n = voxIter.Current();
            Debug.Log($"x{n.Item1.x} y{n.Item1.y} z{n.Item1.z} c{n.Item2}");
        }

        /*
        foreach(var n in vc.GetAllVoxels())
        {
            Debug.Log($"x{n.Item1} y{n.Item2} z{n.Item3} c{n.Item4}");
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        

        if(vc.Tool.Shape != Tool.Shape)
        {
            // shape has just changed, update fake offset
            
            vc.Tool.ToolSize = Tool.ToolSize;
            vc.Tool.Action = Tool.Action;
            vc.Tool.Shape = Tool.Shape;
            vc.Tool.Color = Tool.Color;
            vc.VoxelSize = VoxelSize;
            
            Tool.BrushOffset = vc.Tool.BrushOffset;
        } else
        {
            // shape is so same, update as normal
            
            vc.Tool.BrushOffset = Tool.BrushOffset;
            vc.Tool.ToolSize = Tool.ToolSize;
            vc.Tool.Action = Tool.Action;
            vc.Tool.Shape = Tool.Shape;
            vc.Tool.Color = Tool.Color;
            vc.VoxelSize = VoxelSize;
        }


        if (paintSignal)
        {
            paintSignal = false;
            // vc.Paint();
            simpleWrite();
        }
        if(getIterator)
        {
            getIterator = false;
            vc.GetAllVoxels();
        }
        if(realPaintSignal)
        {
            realPaintSignal = false;
            vc.Paint();
        }
    }

    void simpleWrite()
    {
        Vector3Int pos = Vector3Int.RoundToInt(GetComponent<Transform>().position);
        // Debug.Log($"ordering to write at: {pos}");
        if(Tool.Action == ToolAction.Write)
        {
            vc.WriteVoxel(pos.x, pos.y, pos.z, Tool.Color);
        } else if(Tool.Action == ToolAction.Erase)
        {
            vc.EraseVoxel(pos.x, pos.y, pos.z);
        }
        
    }
}