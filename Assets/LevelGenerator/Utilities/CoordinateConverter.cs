using UnityEngine;

public static class CoordinateConverter {
    public static Vector3Int GetTileCoordinate(Vector3Int cell, BoundsInt bounds) {
        int col = cell.x - bounds.xMin - 5;
        int row = bounds.yMax - cell.y + 10;

        return new Vector3Int(col, row, 0);
    }
}