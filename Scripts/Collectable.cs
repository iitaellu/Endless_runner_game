using Godot;
using System;

public partial class Collectable : Area2D
{
	private AnimatedSprite2D _animatedSprite2D;

	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	private CollisionShape2D _collisionShape2D;

	private AudioStreamPlayer2D _soundEffect;

	private AudioStream _collectStream;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		AddToGroup("obstacles");
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExcited;

		_soundEffect = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		_collectStream = GD.Load<AudioStream>("res://Music/sounds/retro-coin-4-236671.mp3");

		BodyEntered += OnBodyEntered;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private async void OnBodyEntered(Node body)
	{
		if (body is not Player)
			return;

		// Add points
		var ui = GetNode<Ui>("/root/Main/UI");
		ui.AddPoints(10);
		GetNode<GlobalScore>("/root/GlobalScore").AddScore(10);

		// Disable interaction instantly
		_collisionShape2D.SetDeferred("disabled", true);
		_animatedSprite2D.Visible = false;

		// Play collect sound FROM THIS OBJECT
		_soundEffect.Stream = _collectStream;
		_soundEffect.Play();

		// Wait for sound to finish
		await ToSignal(_soundEffect, AudioStreamPlayer2D.SignalName.Finished);

		QueueFree();
	}

	public void getPoints()
	{
		QueueFree();

	}
	private void OnScreenExcited() => QueueFree();
}
