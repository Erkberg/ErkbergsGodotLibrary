using Godot;
using System;

namespace ErkbergsGodotLibrary
{
    public static class RandomSpawnPosition
    {
        public static Vector3 GetRandomSpawnPositionWithinBounds3D(Vector3 min, Vector3 max)
        {
            float x = (float)GD.RandRange(min.X, max.X);
            float y = (float)GD.RandRange(min.X, max.X);
            float z = (float)GD.RandRange(min.Z, max.Z);

            return new Vector3(x, y, z);
        }

        public static Vector3 GetRandomSpawnPosition3D(Vector2 spawnBounds)
        {
            return GetRandomSpawnPosition3D(spawnBounds.X, spawnBounds.Y);
        }

        public static Vector3 GetRandomSpawnPosition3D(float spawnX, float spawnZ, float spawnY = 0f)
        {
            Vector3 spawnPosi = Vector3.Zero;
            spawnPosi.Y = spawnY;

            float random = (float)GD.RandRange(0f, 1f);

            if (random < 0.25f)
            {
                spawnPosi.X = spawnX;
                spawnPosi.Z = (float)GD.RandRange(-spawnZ, spawnZ);
            }
            else
            {
                if (random < 0.5f)
                {
                    spawnPosi.X = -spawnX;
                    spawnPosi.Z = (float)GD.RandRange(-spawnZ, spawnZ);
                }
                else
                {
                    if (random < 0.75f)
                    {
                        spawnPosi.X = (float)GD.RandRange(-spawnX, spawnX);
                        spawnPosi.Z = spawnZ;
                    }
                    else
                    {
                        spawnPosi.X = (float)GD.RandRange(-spawnX, spawnX);
                        spawnPosi.Z = -spawnZ;
                    }
                }
            }

            return spawnPosi;
        }
    }
}