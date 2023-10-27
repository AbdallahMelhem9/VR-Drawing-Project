using System;
using System.IO;
using System.Text;
using UnityEngine;

public class VoxImporter : MonoBehaviour
{
    [SerializeField] VoxelCore vc;
    public string ImportPath = "/Users/paul/Mines_Programmation/Movie/Projet Final/voxFiles/monu10.vox";
    public bool Import = false;

    // Update is called once per frame
    void Update()
    {
        if(Import)
        {
            Import = false;
            importVox();
            
        }
    }

    void importVox()
    {
        // Debug.Log("Importing path: " + ImportPath);
        if (File.Exists(ImportPath))
        {
            using (var stream = File.Open(ImportPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    deserializeVox(reader);
                }
            }
        }
    }

    // Sources:
    // https://github.com/ephtracy/voxel-model/blob/master/MagicaVoxel-file-format-vox.txt
    // >>> https://paulbourke.net/dataformats/vox/#:~:text=vox%20files%20are%20a%20voxel,of%20interest%20can%20be%20skipped. <<<
    void deserializeVox(BinaryReader reader)
    {
        deserializeVoxColor(reader);
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        deserializeVoxChunk(reader);
    }



    void deserializeVoxColor(BinaryReader reader)
    {
        byte c0 = reader.ReadByte();
        byte c1 = reader.ReadByte();
        byte c2 = reader.ReadByte();
        byte c3 = reader.ReadByte();
        
        while (!(c0 == 0x52 && c1 == 0x47 && c2 == 0x42 && c3 == 0x41))
        {
            c0 = c1;
            c1 = c2;
            c2 = c3;
            c3 = reader.ReadByte();
        }

        // Now 2 UInt32 we don't care about
        reader.ReadInt32();
        reader.ReadInt32();


        for (int index = 0; index < 255; index++)
        {
            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();
            byte a = reader.ReadByte();

            VoxDefaultPalette.Palette[index] = BitConverter.ToUInt32(new byte[4] { r, g, b, a });
        }
    }



    void deserializeVoxChunk(BinaryReader reader)
    {
        /*
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < 20; i++)
        {
            sb.Append($"{Convert.ToString(reader.ReadUInt32(), 16)};\n ");
        }
        Debug.Log(sb.ToString());
        */


        byte c0 = reader.ReadByte();
        byte c1 = reader.ReadByte();
        byte c2 = reader.ReadByte();
        byte c3 = reader.ReadByte();
        //Debug.Log($"bytes initially read: {Convert.ToString(c0, 16)} {Convert.ToString(c1, 16)} {Convert.ToString(c2, 16)} {Convert.ToString(c3, 16)}");

        // StringBuilder sb = new StringBuilder();
        while (!(c0 == 0x58 && c1 == 0x59 && c2 == 0x5A && c3 == 0x49))
        {
            c0 = c1;
            c1 = c2;
            c2 = c3;
            c3 = reader.ReadByte();

            // sb.Append($"{Convert.ToString(c3, 16)};\n ");
            // Debug.Log($"Iterated bytes so far: {Convert.ToString(c0, 16)} {Convert.ToString(c1, 16)} {Convert.ToString(c2, 16)} {Convert.ToString(c3, 16)}");
        }
        // Debug.Log("out");
        // Debug.Log(sb.ToString());

        // Now 2 UInt32 we don't care about
        reader.ReadInt32();
        reader.ReadInt32();

        // Number of voxels
        var numberOfVoxels = reader.ReadUInt32();

        //
        // ========== WARNING !!!!! =========
        //
        // Vox file format uses a right-hand coordinate system
        // while Unity uses a Left-hand
        //
        // Consequence:
        // Unity's 2 "horizontal" axises are X/Z while Vox are X/Y
        // Vox's z is Unity's y
        // Vox's y is Unity's z
        //
        // Meaning: when writing voxels, put y where the API expects z
        // and Z where it expects Y.
        StringBuilder sb = new StringBuilder();
        for (int index = 0; index < numberOfVoxels; index++)
        {
            byte x = reader.ReadByte();
            byte y = reader.ReadByte();
            byte z = reader.ReadByte();
            byte colIndex = reader.ReadByte();

            sb.Append($"{x} {y} {z} {colIndex}\n");

            // yes x/z/y not x/y/z see Warning above.
            vc.WriteVoxelNoGen(x, z, y, VoxDefaultPalette.GetColorAtIndex(colIndex));
        }
        Debug.Log(sb.ToString());

        vc.RegenChunks();

        /*
        StringBuilder sb2 = new StringBuilder();
        for (int i = 0; i < 80; i++)
        {
            sb2.Append($"{Convert.ToString(reader.ReadByte(), 16)};\n ");
        }
        Debug.Log(sb2.ToString());
        */

        // vc.WriteVoxelNoGen(10, 20, 50, VoxDefaultPalette.GetColorAtIndex(2));
        // vc.WriteVoxelNoGen(10, 2, 21, VoxDefaultPalette.GetColorAtIndex(3));
        // vc.WriteVoxelNoGen(1, 1, 1, VoxDefaultPalette.GetColorAtIndex(4));
        // vc.WriteVoxelNoGen(2, 6, 100, VoxDefaultPalette.GetColorAtIndex(5));
        // vc.RegenChunks();
    }
}

static class VoxDefaultPalette
{
    public static Color GetColorAtIndex(int index)
    {
        byte[] bytes = BitConverter.GetBytes(Palette[index]); // Left-most first
        return new Color32(bytes[0], bytes[1], bytes[2], bytes[3]);
    }

    public static uint[] Palette = new uint[]
    {
        0x00000000, 0xffffffff, 0xffccffff, 0xff99ffff, 0xff66ffff, 0xff33ffff, 0xff00ffff, 0xffffccff,
        0xffccccff, 0xff99ccff, 0xff66ccff, 0xff33ccff, 0xff00ccff, 0xffff99ff, 0xffcc99ff, 0xff9999ff,
        0xff6699ff, 0xff3399ff, 0xff0099ff, 0xffff66ff, 0xffcc66ff, 0xff9966ff, 0xff6666ff, 0xff3366ff,
        0xff0066ff, 0xffff33ff, 0xffcc33ff, 0xff9933ff, 0xff6633ff, 0xff3333ff, 0xff0033ff, 0xffff00ff,
        0xffcc00ff, 0xff9900ff, 0xff6600ff, 0xff3300ff, 0xff0000ff, 0xffffffcc, 0xffccffcc, 0xff99ffcc,
        0xff66ffcc, 0xff33ffcc, 0xff00ffcc, 0xffffcccc, 0xffcccccc, 0xff99cccc, 0xff66cccc, 0xff33cccc,
        0xff00cccc, 0xffff99cc, 0xffcc99cc, 0xff9999cc, 0xff6699cc, 0xff3399cc, 0xff0099cc, 0xffff66cc,
        0xffcc66cc, 0xff9966cc, 0xff6666cc, 0xff3366cc, 0xff0066cc, 0xffff33cc, 0xffcc33cc, 0xff9933cc,
        0xff6633cc, 0xff3333cc, 0xff0033cc, 0xffff00cc, 0xffcc00cc, 0xff9900cc, 0xff6600cc, 0xff3300cc,
        0xff0000cc, 0xffffff99, 0xffccff99, 0xff99ff99, 0xff66ff99, 0xff33ff99, 0xff00ff99, 0xffffcc99,
        0xffcccc99, 0xff99cc99, 0xff66cc99, 0xff33cc99, 0xff00cc99, 0xffff9999, 0xffcc9999, 0xff999999,
        0xff669999, 0xff339999, 0xff009999, 0xffff6699, 0xffcc6699, 0xff996699, 0xff666699, 0xff336699,
        0xff006699, 0xffff3399, 0xffcc3399, 0xff993399, 0xff663399, 0xff333399, 0xff003399, 0xffff0099,
        0xffcc0099, 0xff990099, 0xff660099, 0xff330099, 0xff000099, 0xffffff66, 0xffccff66, 0xff99ff66,
        0xff66ff66, 0xff33ff66, 0xff00ff66, 0xffffcc66, 0xffcccc66, 0xff99cc66, 0xff66cc66, 0xff33cc66,
        0xff00cc66, 0xffff9966, 0xffcc9966, 0xff999966, 0xff669966, 0xff339966, 0xff009966, 0xffff6666,
        0xffcc6666, 0xff996666, 0xff666666, 0xff336666, 0xff006666, 0xffff3366, 0xffcc3366, 0xff993366,
        0xff663366, 0xff333366, 0xff003366, 0xffff0066, 0xffcc0066, 0xff990066, 0xff660066, 0xff330066,
        0xff000066, 0xffffff33, 0xffccff33, 0xff99ff33, 0xff66ff33, 0xff33ff33, 0xff00ff33, 0xffffcc33,
        0xffcccc33, 0xff99cc33, 0xff66cc33, 0xff33cc33, 0xff00cc33, 0xffff9933, 0xffcc9933, 0xff999933,
        0xff669933, 0xff339933, 0xff009933, 0xffff6633, 0xffcc6633, 0xff996633, 0xff666633, 0xff336633,
        0xff006633, 0xffff3333, 0xffcc3333, 0xff993333, 0xff663333, 0xff333333, 0xff003333, 0xffff0033,
        0xffcc0033, 0xff990033, 0xff660033, 0xff330033, 0xff000033, 0xffffff00, 0xffccff00, 0xff99ff00,
        0xff66ff00, 0xff33ff00, 0xff00ff00, 0xffffcc00, 0xffcccc00, 0xff99cc00, 0xff66cc00, 0xff33cc00,
        0xff00cc00, 0xffff9900, 0xffcc9900, 0xff999900, 0xff669900, 0xff339900, 0xff009900, 0xffff6600,
        0xffcc6600, 0xff996600, 0xff666600, 0xff336600, 0xff006600, 0xffff3300, 0xffcc3300, 0xff993300,
        0xff663300, 0xff333300, 0xff003300, 0xffff0000, 0xffcc0000, 0xff990000, 0xff660000, 0xff330000,
        0xff0000ee, 0xff0000dd, 0xff0000bb, 0xff0000aa, 0xff000088, 0xff000077, 0xff000055, 0xff000044,
        0xff000022, 0xff000011, 0xff00ee00, 0xff00dd00, 0xff00bb00, 0xff00aa00, 0xff008800, 0xff007700,
        0xff005500, 0xff004400, 0xff002200, 0xff001100, 0xffee0000, 0xffdd0000, 0xffbb0000, 0xffaa0000,
        0xff880000, 0xff770000, 0xff550000, 0xff440000, 0xff220000, 0xff110000, 0xffeeeeee, 0xffdddddd,
        0xffbbbbbb, 0xffaaaaaa, 0xff888888, 0xff777777, 0xff555555, 0xff444444, 0xff222222, 0xff111111
    };
}