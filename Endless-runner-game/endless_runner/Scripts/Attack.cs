using Godot;
using System;

public partial class Attack : Area2D
{

	[Signal]
	public delegate void AnimationFinishedEventHandler();
	private AnimatedSprite2D _sprite;
	private CollisionShape2D _collisionShape;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape = GetNode<CollisionShape2D>("hitCollision");
		_sprite.AnimationFinished += OnAnimationFinished;
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
	}

	private void OnAnimationFinished()
	{
		_collisionShape.SetDeferred("disabled", true);
		EmitSignal(SignalName.AnimationFinished);
	}

	/*private void OnBodyEntered(Node body)
	{

		if (body is Fox fox)
		{
		
			GD.Print("Hit Fox!");
			_sprite.Play("hit");
			// Stop fox and play its hit animation if it has one
			fox.OnHit();
			fox.stop();


			_collisionShape.SetDeferred("disabled", true);
			}*/

			// Optional: if your fox has an AnimatedSprite2D with "hit" animation
			/*
			var foxSprite = fox.GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
            if (foxSprite != null)
            {
				_sprite.Play("hit");
		
            }if (foxSprite != null)
				{
					foxSprite.Modulate = new Color(1, 0.5f, 0.5f); // light red flash
				}

			// Disable the attack to prevent multiple hits

		
	}*/
	
	private async void HandleFoxHit(Fox fox)
    {
        // If fox already hit, skip
        if (fox.IsDefeated)
            return;

        GD.Print("Hit Fox!");
		_sprite.Play("hit");
        
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
