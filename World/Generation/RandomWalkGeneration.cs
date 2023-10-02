using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Generation.Alghoritms;
namespace Generation
{
public class RandomWalkGeneration 
{
	
	private int iterations , walkLenght , corridorCount , corridorLenght;
	private float roomPercent;
	private Vector2I startPosition;
	public RandomWalkGeneration(int iterations , int walkLenght ,int corridorCount , int corridorLenght,float roomPercent ,Vector2I startPosition  , WorldTileMap tileMapVisualisator )
	{
		this.iterations = iterations;
		this.walkLenght = walkLenght;
		this.startPosition = startPosition;
		this.corridorCount = corridorCount;
		this.corridorLenght = corridorLenght;
		this.roomPercent = roomPercent;
		RunProceduralGeneration(tileMapVisualisator);
	}
	public void  RunProceduralGeneration(WorldTileMap tileMapVisualisator)
	{
		HashSet<Vector2I> floorPosition = new HashSet<Vector2I>();
		HashSet<Vector2I> potentialRoomPosition = new HashSet<Vector2I>();
		CreatCorridors(floorPosition , potentialRoomPosition);
		HashSet<Vector2I> roomPositions = CreatRooms(potentialRoomPosition);
		List<Vector2I> deadEnds = FindDeadEnd(floorPosition);
		CreatRommsAtDeadEnds(deadEnds , roomPositions);
		floorPosition.UnionWith(roomPositions);
		HashSet<Vector2I> postionsToFill = FindTilesToFill(floorPosition);
		floorPosition.UnionWith(postionsToFill);
		tileMapVisualisator.PainFloorTiles(floorPosition);
	}
	private  HashSet<Vector2I> RunRandomwalk(int iterations , int walkLenght , Vector2I startPosition)
    { 
        var currentposition = startPosition;
        HashSet<Vector2I> floorPosition = new HashSet<Vector2I>();
        for (int i  = 0 ; i < iterations; i++)
        {
            var path  = GenerationAlghoritms.SimpleRandomWalk(currentposition , walkLenght);
            floorPosition.UnionWith(path); 
			currentposition = floorPosition.ElementAt(Directions.random.RandiRange(0 , floorPosition.Count - 1 ));
            
        }
        return floorPosition;
	}
	private void CreatCorridors(HashSet<Vector2I> floorPosition , HashSet<Vector2I> potentialRoomPosition)
	{
		var currentposition = startPosition;
		potentialRoomPosition.Add(currentposition);
		for(int i = 0 ;  i < corridorCount ; i ++)
		{
			var corridor = GenerationAlghoritms.RandomWalkCorridor(currentposition , corridorLenght);
			currentposition = corridor[corridor.Count - 1];
			potentialRoomPosition.Add(currentposition);
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
				roomFloors.UnionWith(roomAtDeadEnd);
			}
		}
	} 
	private HashSet<Vector2I> FindTilesToFill(HashSet<Vector2I> floorPositions)
	{
		HashSet<Vector2I> positionsToFill = new HashSet<Vector2I>();
		foreach (var position in floorPositions)
		{
			foreach(var direction in Directions.cardinalDirectionList)
			{
				var positionOfNeighbourTIle = direction + position;
				if(floorPositions.Contains(positionOfNeighbourTIle) == false)
				{
					positionsToFill.Add(positionOfNeighbourTIle);	
				}
			}
		}
		return positionsToFill;
	}
}

}
