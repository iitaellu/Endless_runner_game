using Godot;
using System;

public partial class Attack : Area2D
{

	[Signal]
	public delegate void AnimationFinishedEventHandler();
	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionShape;

	private AudioStreamPlayer2D _soundEffect;
	private AudioStream _hitStream;
	private AudioStream _attackStream;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("hitCollision");
		_sprite.AnimationFinished += OnAnimationFinished;

		_soundEffect = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		_hitStream = GD.Load<AudioStream>("res://Music/sounds/cinematic-whoosh-hit-impact-238932.mp3");
		_attackStream = GD.Load<AudioStream>("res://Music/sounds/whoosh-effect-382717.mp3");

		//BodyEntered += OnBodyEntered;
		_collisionShape.Disabled = true;

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
    {
        if (!_collisionShape.Disabled)
        {
            foreach (var body in GetOverlappingBodies())
            {
                if (body is Fox fox)
                {
                    HandleFoxHit(fox);
                    break; // only one fox per frame
                }
            }
        }
    }

	public void StartAnimation()
	{
		_collisionShape.Disabled = false;
		_sprite.Play("default");
		_soundEffect.Stream = _attackStream;
		_soundEffect.Play();
	}

	private void OnAnimationFinished()
	{
		_collisionShape.SetDeferred("disabled", true);
		EmitSignal(SignalName.AnimationFinished);
	}
	
	private async void HandleFoxHit(Fox fox)
    {
        // If fox already hit, skip
        if (fox.IsDefeated)
            return;

        GD.Print("Hit Fox!");
		_sprite.Play("hit");
		_soundEffect.Stream = _hitStream;
		_soundEffect.Play();
        
        // Stop and disable the fox
        fox.OnHit();
		fox.stop();

        // Disable attack to prevent multi-hit spam
		 await ToSignal(GetTree().CreateTimer(0.05f), SceneTreeTimer.SignalName.Timeout);
        _collisionShape.SetDeferred("disabled", true);
    }

		// TODO: If hit fox collision shape play hit animation
	// AND Sens fox kill message
}
