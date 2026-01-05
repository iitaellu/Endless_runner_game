using Godot;
using System;

public partial class Fox : StaticBody2D
{

	
	[Export]

	private float _speed = 125.0f;

	private AnimatedSprite2D _animatedSprite2D;

	private VisibleOnScreenNotifier2D _visibleOnScreenNotifier2D;

	private CollisionShape2D _collisionShape2D;

	private bool _isDefeated = false;

	private AudioStreamPlayer2D _soundEffect;

	public bool IsDefeated => _isDefeated;

	private RectangleShape2D _dieShape = GD.Load<RectangleShape2D>("res://collisionShapes/obstacles/fox.tres");
	private RectangleShape2D _runShape = GD.Load<RectangleShape2D>("res://collisionShapes/obstacles/foxRun.tres");

	public override void _Ready()
	{

		AddToGroup("obstacles");

		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_visibleOnScreenNotifier2D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_collisionShape2D.Shape = _runShape;
		_soundEffect = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_visibleOnScreenNotifier2D.ScreenExited += OnScreenExcited;

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	/*public override void _Process(double delta)
	{
		Position += new Vector2(-_speed * (float)delta, 0);
	}*/

	public void stop()
	{
		_speed = 0;
		_animatedSprite2D.Stop();
		_soundEffect.Play();
	}

	/*public void DisableCollision()
	{
		if (_collisionShape2D != null)
		{
			_collisionShape2D.Disabled = true;
		}
	}*/
	
	public void OnHit()
{
    if (_isDefeated) return; // Already hit
    _isDefeated = true;
	_collisionShape2D.Shape = _dieShape;
    _collisionShape2D.SetDeferred("disabled", true);
	_collisionShape2D.Shape = _dieShape;
	_animatedSprite2D.Play("death");
    _animatedSprite2D.Modulate = new Color(1, 0.5f, 0.5f); // if you have a hit animation
	stop();
	var ui = GetNode<Ui>("/root/Main/UI");
		ui.AddPoints(50);
		GetNode<GlobalScore>("/root/GlobalScore").AddScore(50);
}

	private void OnScreenExcited() => QueueFree();
}
