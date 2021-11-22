using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexArray<T> {
	public readonly HexVector offset;
	public T[,] data { get; private set; }

	public HexArray(int minX, int maxX, int minY, int maxY) {
		offset = new HexVector(-minX, -minY);
		data = new T[maxX - minX, maxY - minY];
	}
	public HexArray(T[,] data, HexVector offset) {
		this.data = data;
		this.offset = offset;
	}
	public HexArray(int width, int heigth, HexVector offset) {
		this.offset = offset;
		data = new T[width, heigth];
	}

	public int width { get { return data.GetLength(0); } }
	public int height { get { return data.GetLength(1); } }

	public T GetVal(HexVector v) {
		v += offset;
		try {
			return data[v.x, v.y];
		} catch {
			throw new Exception($"HexArray out of range. Size: ({data.GetLength(0)},{data.GetLength(1)}) requested: {(v - offset).ToShortString()}");
		}
	}

	public void SetVal(HexVector v, T val) {
		v += offset;
		data[v.x, v.y] = val;
	}

	public static Texture2D MapFloat(HexArray<float> arr) {
		Texture2D map = new Texture2D(arr.data.GetLength(0), arr.data.GetLength(1), TextureFormat.RGBA32, false);

		arr.data[0, 0] = arr.offset.x;
		arr.data[1, 0] = arr.offset.y;

		List<byte> b = new List<byte>();


		for(int y = 0; y < map.height; y++) {
			for(int x = 0; x < map.width; x++) {
				b.AddRange(BitConverter.GetBytes(arr.data[x, y]));
			}
		}

		map.SetPixelData(b.ToArray(), 0, 0);
		map.filterMode = FilterMode.Point;
		map.Apply(updateMipmaps: false);
		return map;
	}

	public static Texture2D MapInt(HexArray<int> arr) {
		Texture2D map = new Texture2D(arr.data.GetLength(0), arr.data.GetLength(1), TextureFormat.RGBA32, false);

		arr.data[0, 0] = arr.offset.x;
		arr.data[1, 0] = arr.offset.y;

		List<byte> b = new List<byte>();


		for(int y = 0; y < map.height; y++) {
			for(int x = 0; x < map.width; x++) {
				b.AddRange(BitConverter.GetBytes(arr.data[x, y]));
			}
		}

		map.SetPixelData(b.ToArray(), 0, 0);
		map.filterMode = FilterMode.Point;
		map.Apply(updateMipmaps: false);
		return map;
	}

	public static HexArray<float> FromFloatMap(Texture2D map) {
		int counter = 0;

		byte[] b = map.GetRawTextureData();
		HexVector offset = new HexVector((int)BitConverter.ToSingle(b, 0), (int)BitConverter.ToSingle(b, 4));

		HexArray<float> arr = new HexArray<float>(map.width, map.height, offset);

		for(int y = 0; y < map.height; y++) {
			for(int x = 0; x < map.width; x++) {
				arr.data[x, y] = BitConverter.ToSingle(b, counter);
				counter += 4;
			}
		}

		arr.data[0, 0] = arr.data[0, 3];
		arr.data[1, 0] = arr.data[0, 3];

		return arr;
	}

	public static HexArray<int> FromIntMap(Texture2D map) {
		int counter = 0;

		byte[] b = map.GetRawTextureData();

		HexVector offset = new HexVector((int)BitConverter.ToInt32(b, 0), BitConverter.ToInt32(b, 4));

		HexArray<int> arr = new HexArray<int>(map.width, map.height, offset);
		for(int y = 0; y < map.height; y++) {
			for(int x = 0; x < map.width; x++) {
				arr.data[x, y] = BitConverter.ToInt32(b, counter);
				counter += 4;
			}
		}

		arr.data[0, 0] = arr.data[0, 3];
		arr.data[1, 0] = arr.data[0, 3];

		return arr;
	}

	public static HexArray<bool> FloatToBool(HexArray<float> g) {

		HexArray<bool> v = new HexArray<bool>(g.width, g.height, g.offset);
		for(int x = 0; x < g.width; x++) {
			for(int y = 0; y < g.height; y++) {
				v.data[x, y] = g.data[x, y] > 0;
			}
		}
		return v;
	}

}
