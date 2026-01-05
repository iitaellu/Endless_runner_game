using Godot;
using System;

public partial class Main : Node
{
	private Levels _levels;
	private ObstacleSpawner _spawner;

	private AudioStreamPlayer2D _audio;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello, Godot!");
		_audio = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void StopMusic()
    {
        if (_audio != null && _audio.Playing)
            _audio.Stop();
    }
}
