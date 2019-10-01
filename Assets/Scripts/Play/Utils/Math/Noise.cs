using System;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public static class Noise
    {
        private const int PERLIN_NOISE_SAMPLE_REGION = 10000;

        /// <summary>
        /// Generates a 2D Perlin noise array.
        /// </summary>
        /// <param name="width">
        /// Width.
        /// </param>
        /// <param name="height">
        /// Height.
        /// </param>
        /// <param name="seed">
        /// Seed used for randomness.
        /// </param>
        /// <param name="scale">
        /// Strength of the noise (frequency of the wave). Higher value means noisier results.
        /// Must be higher than 0.
        /// </param>
        /// <param name="nbIterations">
        /// Number of iterations.
        /// </param>
        /// <param name="persistence">
        /// Multiplier of the amplitude for each iteration. Usually between 0 and 1.
        /// Higher value means noisier results.
        /// Must be higher than 0.
        /// </param>
        /// <param name="lacunarity">
        /// Multiplier of the frequency for each iteration. Usually higher than 1.
        /// Higher value means higher number of small variations.
        /// Must be higher than 0.
        /// </param>
        /// <returns>2D float array.</returns>
        public static float[,] PerlinNoiseMap(
            int width,
            int height,
            int seed,
            float scale,
            int nbIterations,
            float persistence,
            float lacunarity
        )
        {
            if (scale <= 0)
                throw new ArgumentException("\"" + nameof(scale) + "\" must be > 0.");
            if (lacunarity <= 0)
                throw new ArgumentException("\"" + nameof(lacunarity) + "\" must be > 0.");

            var random = new Random(seed);
            var octavesOffsets = new Vector2[nbIterations];
            for (var i = 0; i < nbIterations; i++)
                octavesOffsets[i] = new Vector2(
                    random.Next(-PERLIN_NOISE_SAMPLE_REGION, PERLIN_NOISE_SAMPLE_REGION),
                    random.Next(-PERLIN_NOISE_SAMPLE_REGION, PERLIN_NOISE_SAMPLE_REGION)
                );

            var noise = new float[width, height];
            var minNoise = float.MaxValue;
            var maxNoise = float.MinValue;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var currentNoise = 0f;

                    var amplitude = 1f;
                    var frequency = 1f;

                    for (int i = 0; i < nbIterations; i++)
                    {
                        var octavesOffset = octavesOffsets[i];
                        var sampleX = x / scale * frequency + octavesOffset.x;
                        var sampleY = y / scale * frequency + octavesOffset.y;

                        var perlinNoise = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        currentNoise += perlinNoise * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (currentNoise > maxNoise) maxNoise = currentNoise;
                    else if (currentNoise < minNoise) minNoise = currentNoise;

                    noise[x, y] = currentNoise;
                }
            }

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                noise[x, y] = Mathf.InverseLerp(minNoise, maxNoise, noise[x, y]);

            return noise;
        }
    }
}