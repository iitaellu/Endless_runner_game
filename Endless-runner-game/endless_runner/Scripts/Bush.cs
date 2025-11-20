using Godot;
using System;

public partial class Bush : StaticBody2D
{
public Sprite2D Sprite2D;
public CollisionShape2D CollisionShape2D;

	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	private AudioStreamPlayer2D _soundEffect;
	private AudioStream _hitStream;

	public override void _Ready()
	{
		base._Ready();

		AddToGroup("obstacles");

		_visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		Sprite2D = GetNode<Sprite2D>("Sprite2D");
		CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");

		_soundEffect = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExisted;
	}
	
	public void SoundEffect()
    {
		_soundEffect.Play();
    }

	private void OnScreenExisted(){
		QueueFree();
	}

}
