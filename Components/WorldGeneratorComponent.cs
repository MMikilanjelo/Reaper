using Godot;
using System;
using System.Collections.Generic;
using Generation.Alghoritms;
using Generation;
using System.Linq;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Net;
using System.Runtime.InteropServices;

namespace Game.Components
{
	public partial class WorldGeneratorComponent : Node2D
	{
		[Export]private TileMap worldTileMap;
		
		[ExportGroup("Generation Parameters")]
		[Export] private int iterations;
		[Export] private int walkLenght;
		[Export] private int corridor_count;
		[Export] private int corridor_length;
		[Export (PropertyHint.Range , "0 , 1 , 5")] private float roomPercent;
		[ExportCategory("Extra Parameters")]
		[Export] private bool startRandomlyEachIteration = false;
		[Export] private Vector2I Floor_atlas_tile_coordinates;
		[ExportCategory("Basick Wall Parameters")]
		[Export] private Vector2I Wall_Top_Atlas_Coordinates;
		[Export] private Vector2I Wall_SideLeft_Atlas_Coordinates;
		[Export] private Vector2I Wall_SideRight_Atlas_Coordinates;
		[Export] private Vector2I Wall_Full_Atlas_Coordinates;
		[Export] private Vector2I Wall_Bottom_Atlas_Coordinates;

		[ExportCategory("Corner Wall Parameters")]
		[Export] private Vector2I Inner_Corner_Down_Left_Atlas_Coordinates;
		[Export] private Vector2I Inner_Corner_Down_Right_Atlas_Coordinates;
		[Export] private Vector2I  Diagonal_Corner_Down_Right_Atlas_Coordinates;
		[Export] private Vector2I  Diagonal_Corner_Down_Left_Atlas_Coordinates;
		[Export] private Vector2I Diagonal_Corner_Up_Left_Atlas_Coordinates;
		[Export] private Vector2I Diagonal_Corner_Up_Right_Atlas_Coordinates;
	


		private Dictionary<Vector2I , Room> RoomData = new Dictionary<Vector2I, Room>();
		private HashSet<Vector2I> corridorsPos = new HashSet<Vector2I>();
		
        public override void _Ready()
        {
			//worldTileMap.Clear();
			RunProceduralGeneration();	
			PainSingleTile(worldTileMap ,RoomData.Keys.Last(),new Vector2I(4,8));
			
        }

        public void RunProceduralGeneration()
		{
			HashSet<Vector2I> floorPosition = new HashSet<Vector2I>();
			HashSet<Vector2I> potentialRoomPosition = new HashSet<Vector2I>();
			CreatCorridors(floorPosition , potentialRoomPosition , (Vector2I)this.Position);
			HashSet<Vector2I> roomPositions = CreatRooms(potentialRoomPosition);
			List<Vector2I> deadEnds = FindDeadEnd(floorPosition);
			CreatRommsAtDeadEnds(deadEnds , roomPositions);
			//Generate floor positions
			floorPosition.UnionWith(roomPositions);
			//Generate Walls
			PaintGeneretedWallTiles(floorPosition);
			PaintGeneretedFloorTiles(floorPosition ,Floor_atlas_tile_coordinates);
			List<FloorNode>  walkableNodes =  CreatNodesFronTilePositions(floorPosition);
			GD.Print(RoomData.Count);
			foreach(var item in RoomData)
			{
				GD.Print(GetDistanceToRoom(item.Value , new Vector2I(0,0) , walkableNodes));
			}
			

			
		}

		/// <Generation Logick>
		private  HashSet<Vector2I> RunRandomwalk(int iterations , int walkLenght , Vector2I startPosition)
		{ 
			var currentposition = startPosition;
			HashSet<Vector2I> floorPosition = new HashSet<Vector2I>();
			for (int i  = 0 ; i < iterations; i++)
			{
				var path  = GenerationAlghoritms.SimpleRandomWalk(currentposition , walkLenght);
				floorPosition.UnionWith(path); 
				if(startRandomlyEachIteration){
					currentposition = floorPosition.ElementAt(Directions.random.RandiRange(0 , floorPosition.Count - 1 ));
				}
				
				
			}
			return floorPosition;
		}
		private void CreatCorridors(HashSet<Vector2I> floorPosition , HashSet<Vector2I> potentialRoomPosition , Vector2I startPosition)
		{
			var currentposition = startPosition;
			potentialRoomPosition.Add(currentposition);
			for(int i = 0 ;  i < corridor_count ; i ++)
			{
				var corridor = GenerationAlghoritms.RandomWalkCorridor(currentposition , corridor_length);
				currentposition = corridor[corridor.Count - 1];
				potentialRoomPosition.Add(currentposition);
				corridorsPos.UnionWith(corridor);
				floorPosition.UnionWith(corridor);
				
			}
		}
		private HashSet<Vector2I> CreatRooms (HashSet<Vector2I> potentialRoomPosition)
		{
			HashSet<Vector2I> roomPositions  = new HashSet<Vector2I>();

			int roomToCreatCount = Godot.Mathf.RoundToInt(potentialRoomPosition.Count * roomPercent);

			List<Vector2I> roomToCreat = potentialRoomPosition.OrderBy(x=> Guid.NewGuid()).Take(roomToCreatCount).ToList();

			foreach(var roomPosition in roomToCreat)
			{
				var roomFLoor  = RunRandomwalk(iterations , walkLenght , roomPosition);
				SafeRoomData(roomPosition, roomFLoor);
				roomPositions.UnionWith(roomFLoor);
			}
			return roomPositions;
		}
		private List<Vector2I> FindDeadEnd(HashSet<Vector2I> floorPositions)
		{
			List<Vector2I> deadEnds = new List<Vector2I>();
			foreach(var floorPosition in floorPositions)
			{
				int neighboursCount = 0 ;
				foreach(var direction in Directions.cardinalDirectionList)
				{
					if(floorPositions.Contains(floorPosition + direction))
					{
						neighboursCount++;
					}
				}
				if(neighboursCount == 1)
				{
					deadEnds.Add(floorPosition);
				}
			}
			return deadEnds;
		}
		private void CreatRommsAtDeadEnds(List<Vector2I> deadEnds , HashSet<Vector2I> roomFloors)
			{
				foreach(var position in deadEnds)
				{
					if(roomFloors.Contains(position) == false)
					{
						HashSet<Vector2I> roomAtDeadEnd = RunRandomwalk(iterations ,walkLenght , position);
						SafeRoomData(position ,roomAtDeadEnd);
						roomFloors.UnionWith(roomAtDeadEnd);
					}
				}
			} 
		private HashSet<Vector2I> FindWallsPositionInGivenDirection(IEnumerable<Vector2I> floorPositions , List<Vector2I> directions)
		{
			HashSet<Vector2I> wall_postion = new HashSet<Vector2I>();

			foreach(var floor_pos in floorPositions)
			{
				foreach(var direction in directions)
				{
					var positionOfNeighbourTIle = floor_pos + direction;
					if(floorPositions.Contains(positionOfNeighbourTIle) == false){
						wall_postion.Add(positionOfNeighbourTIle);
					}
					
				}
				
			}
			return wall_postion;
		}
		/// <Generation Logick>
		
		/// <Drawing tiles based on floor postion>///
		private void PaintGeneretedFloorTiles(IEnumerable<Vector2I> tilespostion  , Vector2I atlas_tile_coordinates)
    	{
        	PaintTiles(tilespostion , worldTileMap , atlas_tile_coordinates);
    	}
		private void PaintTiles (IEnumerable<Vector2I> positions , TileMap tileMap , Vector2I atlas_tile_coordinates)
		{
			foreach(var position in positions)
			{
				PainSingleTile(tileMap , position , atlas_tile_coordinates);
			}
		}
		private void PainSingleTile(TileMap tileMap , Vector2I position , Vector2I atlas_tile_coordinates )
		{
			var tilePosition = position;
			tileMap.SetCell(0,tilePosition ,0,atlas_tile_coordinates,0); 	
		}
		private void PaintGeneretedWallTiles(IEnumerable<Vector2I> floor_positions )
		{
			HashSet<Vector2I> get_basick_walls_position =FindWallsPositionInGivenDirection(floor_positions ,Directions.cardinalDirectionList);
			HashSet<Vector2I>get_corner_walls_position = FindWallsPositionInGivenDirection(floor_positions , Directions.diagonalDirectionList);
			GetBasickWallType(get_basick_walls_position , floor_positions);
			GetCornerWallsType(get_corner_walls_position , floor_positions);
		}
		/// <Drawing tiles based on floor postion>///
		
		
		///<Tile desision helper>
		private void GetBasickWallType(IEnumerable<Vector2I>wall_positions , IEnumerable<Vector2I> floor_positions)
		{
			foreach(var wall_pos in wall_positions)
			{
				string neighbours_binary_type = "";
				foreach(var direction in Directions.cardinalDirectionList)
				{
					var neighbour_position = wall_pos + direction;
					if(floor_positions.Contains(neighbour_position))
					{
						neighbours_binary_type += "1";
					}
					else{
						neighbours_binary_type += "0";
					}
					
				}
				PaintSingleWallTile(worldTileMap , wall_pos , neighbours_binary_type);
				
			}
		}
		private void GetCornerWallsType(IEnumerable<Vector2I>corner_wall_positions , IEnumerable<Vector2I> floor_positions)
		{
			foreach(var wall_pos in corner_wall_positions)
			{
				string neighbours_binary_type = "";
				foreach(var direction in Directions.allDirectionsList)
				{
					var neighbour_position = direction + wall_pos;
					if(floor_positions.Contains(neighbour_position))
					{
						neighbours_binary_type += "1";
					}
					else{
						neighbours_binary_type += "0";
					}
				}
				PaintSingleCornerWall(worldTileMap , wall_pos , neighbours_binary_type);
			}
		}
		private void PaintSingleWallTile(TileMap tileMap , Vector2I position  , string binaryType)
		{
			int typeAsInt = Convert.ToInt32(binaryType , 2);
			if(WallByteTypes.wallTop.Contains(typeAsInt))
			{
				tileMap.SetCell(0,position ,0, Wall_Top_Atlas_Coordinates,0); 
			}
			else if (WallByteTypes.wallSideRight.Contains(typeAsInt))
			{
				
				tileMap.SetCell(0,position ,0,  Wall_SideRight_Atlas_Coordinates,0); 
			}
			else if (WallByteTypes.wallSideLeft.Contains(typeAsInt))
			{
				
				tileMap.SetCell(0,position ,0, Wall_SideLeft_Atlas_Coordinates,0); 
			}
			else if (WallByteTypes.wallFull.Contains(typeAsInt))
			{
				
				tileMap.SetCell(0,position ,0,  Wall_Full_Atlas_Coordinates,0); 
			}
			else if (WallByteTypes.wallBottm.Contains(typeAsInt))
			{
				tileMap.SetCell(0,position ,0,  Wall_Bottom_Atlas_Coordinates,0); 
			}
			
			
				
		}
		private void PaintSingleCornerWall(TileMap tileMap , Vector2I position , string binaryType)
		{
			int typeAsInt = Convert.ToInt32(binaryType , 2);
			 
			if(WallByteTypes.wallInnerCornerDownLeft.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Inner_Corner_Down_Left_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if(WallByteTypes.wallInnerCornerDownRight.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Inner_Corner_Down_Right_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if(WallByteTypes.wallDiagonalCornerDownLeft.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Diagonal_Corner_Down_Left_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if(WallByteTypes.wallDiagonalCornerDownRight.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Diagonal_Corner_Down_Right_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if( WallByteTypes.wallDiagonalCornerUpLeft.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Diagonal_Corner_Up_Left_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if( WallByteTypes.wallDiagonalCornerUpRight.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Diagonal_Corner_Up_Right_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if (WallByteTypes.wallFullEightDirections.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Wall_Full_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
			else if (WallByteTypes.wallBottmEightDirections.Contains(typeAsInt))
			{
				var atlas_tile_coordinates = Wall_Bottom_Atlas_Coordinates;
				tileMap.SetCell(0,position ,0,atlas_tile_coordinates,0); 
			}
				
			
		}	
		///<Tile desision helper>


		///<Room Data Extracktor>
		private void SafeRoomData(Vector2I roomCentrePosition, HashSet<Vector2I> room_floor_postion)
		{

			RoomData.Add(roomCentrePosition , new Room(roomCentrePosition , room_floor_postion));
		}

		///<A* alghoritm>
		private List<FloorNode> FindPath(Vector2I startPosition , Vector2I FinalPosition , List<FloorNode> floorNodes)
		{
			FloorNode startNode = GetNodeFromTilePos(startPosition , floorNodes);
			FloorNode destinationNode = GetNodeFromTilePos(FinalPosition , floorNodes);
			List<FloorNode> openSet= new List<FloorNode>();
			HashSet<FloorNode> closedSet = new HashSet<FloorNode>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				FloorNode currentNode = openSet[0];
				for(int i =1 ;i < openSet.Count ; i ++)
				{
					if(openSet[i].fCost < currentNode.fCost||openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
					{
						currentNode = openSet[i];
					}
				}
				openSet.Remove(currentNode);
				closedSet.Add(currentNode);
				if(currentNode == destinationNode)
				{
					return RetracePath(startNode , destinationNode);
				}
				foreach(FloorNode neighbourNode in GetNodeNeighbours(currentNode, floorNodes , Directions.cardinalDirectionList))
				{
					if(closedSet.Contains(neighbourNode))
					{
						continue;
					}
					int newMovmentCost = currentNode.gCost + GetDistance(currentNode, neighbourNode);
					if(newMovmentCost < neighbourNode.gCost || !openSet.Contains(neighbourNode))
					{
						neighbourNode.gCost = newMovmentCost;
						neighbourNode.hCost = GetDistance(neighbourNode , destinationNode);
						neighbourNode.parent = currentNode;
						if(!openSet.Contains(neighbourNode))
						{
							openSet.Add(neighbourNode);
						}
					}

				}

			}
			return null;

		}
		private List<FloorNode> RetracePath(FloorNode startNode , FloorNode endNode)
		{
			List<FloorNode> path = new List<FloorNode>();
			FloorNode currentNode = endNode;
			while(currentNode != startNode)
			{
				path.Add(currentNode);
				currentNode = currentNode.parent;

			}
			path.Reverse();
			return path;
		}
		private int GetDistance(FloorNode start , FloorNode destionation)
		{
			int distanceX = Mathf.Abs(start.TileMapPosition.X -destionation.TileMapPosition.X);
			int distanceY = Mathf.Abs(start.TileMapPosition.Y - destionation.TileMapPosition.Y);
			if(distanceX > distanceY)
			{
				return 14 * distanceY + 10*(distanceX - distanceY);
			}
			return 14 * distanceX + 10 *(distanceY - distanceX);
		}
		private List<FloorNode> CreatNodesFronTilePositions(HashSet<Vector2I> tilePosition)
		{
			List<FloorNode>nodes = new List<FloorNode>();
			foreach(var pos in tilePosition)
			{
				nodes.Add(new FloorNode(pos));
			}
			return nodes;
			
		}
		private List<FloorNode> GetNodeNeighbours(FloorNode floorNode ,  List<FloorNode> availableNodes , List<Vector2I>directionsToCheck)
		{
			List<FloorNode> neighbours = new List<FloorNode>();
			foreach(var direction in directionsToCheck)
			{
				var neighbour_position = floorNode.TileMapPosition + direction;
				var neighbourNode = GetNodeFromTilePos(neighbour_position , availableNodes);
				if(neighbourNode != null)
				{
					neighbours.Add(neighbourNode);
				}
			}
			return neighbours;
		}
		private FloorNode GetNodeFromTilePos(Vector2I tilePos , List<FloorNode> NodesToCheck)
		{
			foreach(var Node in NodesToCheck)
			{
				if(Node.TileMapPosition == tilePos)
				{
					return Node;
				}
				
			}
			 
			return null;
		}
		private int GetDistanceToRoom(Room room , Vector2I startPoint , List<FloorNode> floorNodes)
		{
			if(room.roomCentre == startPoint)
			{
				return 0;
			}
			List<FloorNode> path = FindPath(startPoint , room.roomCentre , floorNodes);
			return path.Count();
		}
		private  class FloorNode 
		{
			public int gCost;
			public int hCost;
			public Vector2I TileMapPosition;
			public int fCost => gCost + hCost;
			public FloorNode parent;

			public FloorNode(Vector2I position)
			{
				TileMapPosition = position;
			}
		}
		private class Room 
		{
			public Vector2I roomCentre;
			public HashSet<Vector2I> floorPositions;
			public int distanceToRoom;
			public Room(Vector2I roomCentre , HashSet<Vector2I> floorPositions)
			{
				this.roomCentre = roomCentre;
				this.floorPositions = floorPositions;
			}
		}

    }
	

}

