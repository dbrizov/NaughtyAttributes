using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HexData {
	public static readonly float hexRatio = 0.86603f;
	public static readonly float outerRadius = 0.5f;
	public static readonly float innerRadius = outerRadius * hexRatio;
	public static readonly float levelHeigth = 0.6f;

	public static readonly Vector3 yAxis = Vector3.forward;
	public static readonly Vector3 xAxis = Quaternion.AngleAxis(60, Vector3.up) * yAxis;
	public static readonly Vector3 ortho = Vector3.Cross(yAxis, xAxis);

}

[System.Serializable]
public struct HexVector {
	public enum direction { up, upR, downR, down, downL, upL, zero }

	public int x, y;

	public int z { get { return -x - y; } }
	public int magnitude {
		get {
			return (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z)) / 2;
		}
	}


	public int angle {
		get {
			return GetRotation(up, this.normalize);
		}
	}

	public HexVector(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public HexVector(Vector3 pos) {
		this = GetHex(pos);
	}

	public override string ToString() {
		return "HexVector(" + x + ", " + y + ")";
	}
	public string ToShortString() {
		return "(" + x + ", " + y + ")";
	}

	public bool TryParse(string val, out HexVector v) {
		if(val.Contains("HexVector"))
			val = val.Remove(0, "HexVector".Length);

		val.Trim('(', ')', ',');
		string[] xy = val.Split(' ');
		int x = int.Parse(xy[0]);
		int y = int.Parse(xy[1]);
		v = new HexVector(x, y);
		return true;
	}

	public Vector3 Cartesian {
		get {
			return (HexData.xAxis * x + HexData.yAxis * y) * HexData.innerRadius * 2;
		}
	}

	public HexVector Rotate(int tiles) {
		tiles = tiles % 6;


		int xR = x,
			yR = y,
			zR = z;
		int xTemp, yTemp, zTemp;
		if(tiles >= 0) {
			for(int i = 0; i < tiles; i++) {
				xTemp = xR;
				yTemp = yR;
				zTemp = zR;

				xR = -zTemp;
				yR = -xTemp;
				zR = -yTemp;

			}
		} else {
			for(int i = 0; i < -tiles; i++) {
				xTemp = xR;
				yTemp = yR;
				zTemp = zR;

				xR = -yTemp;
				yR = -zTemp;
				zR = -xTemp;

			}
		}
		return new HexVector(xR, yR);
	}

	public direction GetDirection() {
		if(this == HexVector.up)
			return direction.up;
		if(this == downR)
			return direction.downR;
		if(this == upR)
			return direction.upR;
		if(this == -upR)
			return direction.downL;
		if(this == -downR)
			return direction.upL;
		if(this == -up)
			return direction.down;
		return direction.zero;
	}

	public HexVector CircleRotate(int tiles) {
		int d = magnitude;
		HexVector g = this;
		//skrót przez rotację o pełne hexy

		for(int i = 0; i < tiles; i++) {
			if(Mathf.Abs(g.x) == d) {
				if(Mathf.Abs(g.y) == d) {
					g += downL * Math.Sign(g.x);
					continue;
				}
				g += down * Math.Sign(g.x);
				continue;
			}

			if(Mathf.Abs(g.y) == d) {
				if(Mathf.Abs(g.z) == d) {
					g += downR * Math.Sign(g.y);
					continue;
				}
				g += upR * Math.Sign(g.y);
				continue;
			}
			g += downR * Math.Sign(g.y);
		}


		for(int i = 0; i > tiles; i--) {

			if(Mathf.Abs(g.y) == d) {
				if(Mathf.Abs(g.x) == d) {
					g += up * Math.Sign(g.x);
					continue;
				}
				g += downL * Math.Sign(g.y);
				continue;
			}
			if(Mathf.Abs(g.x) == d) {
				if(Mathf.Abs(g.z) == d) {
					g += upL * Math.Sign(g.x);
					continue;
				}
				g += up * Math.Sign(g.x);
				continue;
			}

			g += upL * Math.Sign(g.y);
		}

		return g;

	}

	public HexVector normalize {
		get {
			return GetHex(Cartesian.normalized);
			//return new HexVector(Mathf.Clamp(this.x, -1, 1), Mathf.Clamp(this.y, -1, 1));
		}
	}
	public bool CanNormalize() {
		if(x == 0 || y == 0 || z == 0)
			return true;
		else
			return false;
	}
	public Vector3 GetCorner(int index) {

		return Cartesian + Quaternion.AngleAxis(-30 + index * 60, HexData.ortho) * (HexData.yAxis * HexData.outerRadius);
	}

	public Vector3 GetEdgeCenter(int index) {
		return Cartesian + Quaternion.AngleAxis(-60 * index, HexData.ortho) * (HexData.yAxis * HexData.innerRadius);
	}

	public HexVector[] subdivide {
		get {
			HexVector[] p = new HexVector[2];
			p[0] = new HexVector(x / 2, 0);
			p[1] = new HexVector(0, y / 2);

			p[0].y = y - p[1].y;
			p[1].x = x - p[0].x;
			//p[1] = new HexVector(x - p[0].x, y - p[0].y);
			return p;
		}
	}


	//statics
	public readonly static HexVector zero = new HexVector(0, 0);
	public readonly static HexVector up = new HexVector(0, 1);
	public readonly static HexVector down = new HexVector(0, -1);

	public readonly static HexVector upR = new HexVector(1, 0);
	public readonly static HexVector upL = new HexVector(-1, 1);

	public readonly static HexVector downL = new HexVector(-1, 0);
	public readonly static HexVector downR = new HexVector(1, -1);


	public static HexVector operator +(HexVector a, HexVector b) {
		return new HexVector(a.x + b.x, a.y + b.y);
	}
	public static HexVector operator -(HexVector a, HexVector b) {
		return new HexVector(a.x - b.x, a.y - b.y);
	}
	public static HexVector operator -(HexVector a) {
		return new HexVector(-a.x, -a.y);
	}
	public static HexVector operator *(HexVector a, int v) {
		return new HexVector(a.x * v, a.y * v);
	}

	public static bool operator ==(HexVector a, HexVector b) {
		return (a.x == b.x && a.y == b.y);
	}
	public static bool operator !=(HexVector a, HexVector b) {
		return !(a.x == b.x && a.y == b.y);
	}

	public static explicit operator Vector2Int(HexVector v) {
		return new Vector2Int(v.x, v.y);
	}

	public static HexVector GetHex(Vector3 pos) {

		float y = Vector3.Dot(pos, HexData.yAxis) / (HexData.innerRadius * 2);
		float z = -y;

		float offset = pos.x / (HexData.outerRadius * 3);

		y -= offset;
		z -= offset;

		float x = -z - y;



		int iX = Mathf.RoundToInt(x);
		int iY = Mathf.RoundToInt(y);
		int iZ = Mathf.RoundToInt(z);

		if(iX + iY + iZ != 0) {
			float dX = Mathf.Abs(x - iX);
			float dY = Mathf.Abs(y - iY);
			float dZ = Mathf.Abs(z - iZ);

			if(dX > dY && dX > dZ) {
				iX = -iY - iZ;
			} else if(dY > dZ) {
				iY = -iZ - iX;
			}
		}


		return new HexVector(iX, iY);
	}

	public static HexVector Direction(Vector3 v, int row) {
		v = Vector3.ProjectOnPlane(v, HexData.ortho).normalized * HexData.hexRatio * row;
		return GetHex(v);      //nieładnie :/ 
	}

	public static Vector3 SnapDirection(Vector3 v) {
		return (Direction(v, 1)).Cartesian;
	}

	public static Quaternion HexRotation(int v, int row) {
		return Quaternion.AngleAxis(v * 60f / row, HexData.ortho);
	}
	public static Vector3 Snap(Vector3 pos) {
		return (GetHex(pos)).Cartesian;
	}
	public Quaternion Rotation {
		get {
			return Quaternion.LookRotation(this.Cartesian.normalized, HexData.ortho);
		}
	}
	public static int GetRotation(HexVector r, HexVector v) {

		if(r == v)
			return 0;

		for(int i = 1; i < 4; i++) {
			r = r.Rotate(1);
			if(r == v)
				return i;
		}

		for(int i = 0; i < 2; i++) {
			r = r.Rotate(1);
			if(r == v)
				return i - 2;
		}

		return 0;
	}

	public override bool Equals(object obj) {
		if(!(obj is HexVector)) {
			return false;
		}

		var vector = (HexVector)obj;
		return x == vector.x &&
			   y == vector.y;
	}

	public override int GetHashCode() {
		var hashCode = -1476677902;
		hashCode = hashCode * -1521134295 + x.GetHashCode();
		hashCode = hashCode * -1521134295 + y.GetHashCode();
		return hashCode;
	}

	public static Quaternion SnappedRotation(Transform t) {
		return Quaternion.LookRotation(SnapDirection(t.forward));
	}

	public static HexVector[] Line(HexVector from, HexVector to) {
		HexVector l = to - from;
		if(l == HexVector.zero)
			return new HexVector[] { from };

		Vector3 step = l.Cartesian / l.magnitude;
		HexVector[] points = new HexVector[l.magnitude + 1];
		for(int i = 0; i < l.magnitude + 1; i++) {
			points[i] = from + GetHex(step * (i));
		}
		return points;
	}

	public static HexVector[] neighbours = new HexVector[]{
		up, upR, downR, down, downL,upL    };

}
public struct HexBorder {
	public int value { get; private set; }
	public HexBorder(HexVector[] open) {
		value = 0b00111111;
		foreach(HexVector a in open) {
			value &= ~DirToBorder[a];
		}
	}
	public HexBorder(int val) {
		value = val;
	}

	public void Add(HexVector v) {
		value |= DirToBorder[v];
	}
	public void Remove(HexVector v) {
		value &= ~DirToBorder[v];
	}
	public bool Allowed(HexVector v) {
		int s = (DirToBorder[v] & value);
		return s != 0;
	}

	static Dictionary<HexVector, byte> DirToBorder = new Dictionary<HexVector, byte> { // równiedobrze można by brać rotację wektora i przekęcać o << 1 
		{ HexVector.zero,   0b0     },
		{ HexVector.up,     0b1     },
		{ HexVector.upR,    0b10    },
		{ HexVector.downR,  0b100   },
		{ HexVector.down,   0b1000  },
		{ HexVector.downL,  0b10000 },
		{ HexVector.upL,    0b100000},
	};


}

/*
 public static int[] options = new int[] {
		0b000000,
		0b000001,
		0b000011,
		0b000111,
		0b001111,
		0b011111 ,

		0b000101,
		0b001101,
		0b011101,
		0b010101,
		0b001001,
		0b001011,
		0b011011,
		0b111111
	};
*/