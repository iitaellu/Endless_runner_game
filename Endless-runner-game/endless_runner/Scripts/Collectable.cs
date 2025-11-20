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
		if (body is Player player)
		{

			var soundPlayer = GetNode<AudioStreamPlayer2D>("/root/Main/Player/AudioStreamPlayer2D");
			soundPlayer.Stream = _collectStream;
			soundPlayer.Play();
			/*_soundEffect.Stream = _collectStream;
			_soundEffect.Play();*/

			var ui = GetNode<Ui>("/root/Main/UI");
			ui.AddPoints(10);
			GetNode<GlobalScore>("/root/GlobalScore").AddScore(10);

				// Hide instantly
			Visible = false;
			_collisionShape2D.SetDeferred("disabled", true);   // so it won't collide again

			await ToSignal(_soundEffect, "finished");
			QueueFree(); // disappear immediately
		}
	}

	public void getPoints()
	{
		QueueFree();

	}
	private void OnScreenExcited() => QueueFree();
}
