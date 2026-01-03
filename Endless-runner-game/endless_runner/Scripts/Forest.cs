using Godot;
using System;

public partial class Forest : ParallaxBackground
{

	 private float _scrollSpeed = 0f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void SetScrollSpeed(float groundSpeed)
    {
        _scrollSpeed = groundSpeed;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ScrollOffset += new Vector2(_scrollSpeed * (float)delta, 0);
	}
}
