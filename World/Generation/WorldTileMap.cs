using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Generation
{
public partial class WorldTileMap : TileMap
{
	Vector2I tileCoordinates = new Vector2I(0 , 0);
	int idTile = 0;
	int tileLayer = 0;
    public   int tilescount = 0;

	public void PainFloorTiles(IEnumerable<Vector2I> floorposition)
    {
        PaintTiles(floorposition , this);
        var array = new Godot.Collections.Array<Vector2I>(floorposition);
//        SetCellsTerrainConnect(0, array , 0 , 0 ,true);
    }

    private void PaintTiles (IEnumerable<Vector2I> positions , TileMap tileMap)
    {
        foreach(var position in positions)
        {
            GD.Print(position);
            PainSingleTile(tileMap , position);
            
        }
    }

    private void PainSingleTile(TileMap tileMap , Vector2I position )
    {
        var tilePosition = position;
        tileMap.SetCell(0,tilePosition ,0,new Vector2I(0,0),0);
        tilescount++;
    }
 

}

}
