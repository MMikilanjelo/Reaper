using Godot;
using System;

namespace Game.Components
{
	public partial class RandomAudioPlayer : AudioStreamPlayer2D
	{
		[Export] Godot.Collections.Array<AudioStream> streams =new Godot.Collections.Array<AudioStream>();
		public void PlayRandom()
		{
			if (streams == null || streams.Count == 0)
			{
				return;
			}
			Stream = streams.PickRandom();
			Play();
		}
	}
}

