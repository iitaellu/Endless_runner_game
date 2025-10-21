using Godot;
using System;

public partial class Hawk : StaticBody2D
{

	[Export]

	private float _speed = 125.0f;

	private AnimatedSprite2D _animatedSprite2D;

	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");

		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExcited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	/*public override void _Process(double delta)
	{
		Position += new Vector2(-_speed * (float)delta, 0);
	}*/

	public void stop() {
		_speed = 0;
		_animatedSprite2D.Stop();
	}

	private void OnScreenExcited() => QueueFree();
}
