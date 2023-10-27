using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkNet
{
    Chunk[,,] chunks;
    public Vector3Int Dimensions{ get; private set; }

    public ChunkNet(Vector3Int dimensions)
    {
        Dimensions = dimensions;
        chunks = new Chunk[Dimensions.x, Dimensions.y, Dimensions.z];
        Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
        {
            chunks[x, y, z] = new Chunk(chunkLocation(x, y, z));
        });
    }

    public void insertVoxel(VoxelData data, int x, int y, int z)
    {
        enforcePositiveCoordinates(x, y, z);
        if(isResizingRequiredForVoxel(x, y, z))
        {
            resizeToFitVoxel(x, y, z);
        }

        /*
        Vector3Int chunkCoordinates = new Vector3Int(
            x / Chunk.Dimensions.x,
            y / Chunk.Dimensions.y,
            z / Chunk.Dimensions.z);
        Vector3Int voxelCoordinates = new Vector3Int(
            x - chunkCoordinates.x * Chunk.Dimensions.x,
            y - chunkCoordinates.y * Chunk.Dimensions.y,
            z - chunkCoordinates.z * Chunk.Dimensions.z);
        */
        var (chunkCoordinates, voxelCoordinates) = chunkAndLocVoexCoordfromGlobVoxCoord(x, y, z);

        Chunk c = chunks[chunkCoordinates.x, chunkCoordinates.y, chunkCoordinates.z];
        c[voxelCoordinates.x, voxelCoordinates.y, voxelCoordinates.z] = data;
    }

    public Chunk RetrieveChunk(int x, int y, int z)
    {
        enforcePositiveCoordinates(x, y, z);
        if (isResizingRequiredForChunk(x, y, z))
        {
            resizeToFitChunk(x, y, z);
        }
        return chunks[x, y, z];
    }

    public void SetChunkRendering(bool b)
    {
        Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
        {
            chunks[x, y, z].Rendering = b;
        });
    }

    void resizeToFitVoxel(int x, int y, int z)
    {
        var scalingFactor = new Vector3Int(
            x / (Dimensions.x * Chunk.Dimensions.x) + 1,
            y / (Dimensions.y * Chunk.Dimensions.y) + 1,
            z / (Dimensions.z * Chunk.Dimensions.z) + 1);

        var newDimensions = new Vector3Int(
            Dimensions.x * scalingFactor.x,
            Dimensions.y * scalingFactor.y,
            Dimensions.z * scalingFactor.z);

        var newChunks = new Chunk[newDimensions.x, newDimensions.y, newDimensions.z];
        Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
        {
            newChunks[x, y, z] = chunks[x, y, z];
        });

        chunks = newChunks;
        Dimensions = newDimensions;

        fillEmptyChunks();
    }

    void resizeToFitChunk(int x, int y, int z)
    {
        var scalingFactor = new Vector3Int(
            x / (Dimensions.x) + 1,
            y / (Dimensions.y) + 1,
            z / (Dimensions.z) + 1);

        var newDimensions = new Vector3Int(
            Dimensions.x * scalingFactor.x,
            Dimensions.y * scalingFactor.y,
            Dimensions.z * scalingFactor.z);

        var newChunks = new Chunk[newDimensions.x, newDimensions.y, newDimensions.z];
        Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
        {
            newChunks[x, y, z] = chunks[x, y, z];
        });

        chunks = newChunks;
        Dimensions = newDimensions;

        fillEmptyChunks();
    }

    Vector3 chunkLocation(int x, int y, int z)
    {
        return new Vector3(
            x * Chunk.Dimensions.x,
            y * Chunk.Dimensions.y,
            z * Chunk.Dimensions.z);
    }

    (Vector3Int, Vector3Int) chunkAndLocVoexCoordfromGlobVoxCoord(int x, int y, int z)
    {
        Vector3Int chunkCoordinates = new Vector3Int(
            x / Chunk.Dimensions.x,
            y / Chunk.Dimensions.y,
            z / Chunk.Dimensions.z);
        Vector3Int voxelCoordinates = new Vector3Int(
            x - chunkCoordinates.x * Chunk.Dimensions.x,
            y - chunkCoordinates.y * Chunk.Dimensions.y,
            z - chunkCoordinates.z * Chunk.Dimensions.z);
        return (chunkCoordinates, voxelCoordinates);
    }

    void fillEmptyChunks()
    {
        Common.ActOnMatrixIterate(Dimensions.x, Dimensions.y, Dimensions.z, (x, y, z) =>
        {
            if (chunks[x, y, z] == null)
            {
                chunks[x, y, z] = new Chunk(chunkLocation(x, y, z));
            }
        });
    }

    void enforcePositiveCoordinates(int x, int y, int z)
    {
        if(x < 0 || y < 0 || z < 0)
        {
            throw new NotImplementedException("Negative Coordinates in Net are not supported yet");
        }
    }

    bool areCoordinatesInNet(int x, int y, int z)
    {
        return x >= 0 && y >= 0 && z >= 0;
    }

    bool isResizingRequiredForVoxel(int x, int y, int z)
    {
        return (x >= Dimensions.x * Chunk.Dimensions.x)
            || (y >= Dimensions.y * Chunk.Dimensions.y)
            || (z >= Dimensions.z * Chunk.Dimensions.z);
    }

    bool isResizingRequiredForChunk(int x, int y, int z)
    {
        return (x >= Dimensions.x)
            || (y >= Dimensions.y)
            || (z >= Dimensions.z);
    }
}
