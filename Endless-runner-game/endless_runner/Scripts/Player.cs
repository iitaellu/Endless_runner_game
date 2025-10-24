using Godot;
using System;
using System.Net;

public partial class Player : CharacterBody2D
{

	[Signal]
	public delegate void ObstacleHitEventHandler();

	[Export]
	private float _jumpVelocity = -800.0f;

	[Export]
	private float _gravityMultiplier = 3.0f;


	//Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private RectangleShape2D _runShape = GD.Load<RectangleShape2D>("res://collisionShapes/player/player_run.tres");
	private RectangleShape2D _crouchShape = GD.Load<RectangleShape2D>("res://collisionShapes/player/player_crouching.tres");

	private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _collisionShape2D;

	private PackedScene _attackScene;

	private PackedScene _endScene;
	private Attack _activeAttack;

    private float _yOffsetCrouch = 30.0f;
    private float _collisionShapeStartYPosition;


    public override void _Ready()
    {
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_attackScene = GD.Load<PackedScene>("res://Scenes/attack.tscn");
		_endScene = GD.Load<PackedScene>("res://Scenes/end.tscn");
        _collisionShape2D.Shape = _runShape;
        _collisionShapeStartYPosition = _collisionShape2D.Position.Y;
    }

    public override void _Process(double delta)
	{
		if (_animatedSprite.Animation == "attack" && _animatedSprite.IsPlaying()){
			//_attack.Visible = true;
			//_attack.StartAnimation();
			return;
		}
        if (IsOnFloor()) {
			if (Input.IsActionPressed("down")){
                if (_collisionShape2D.Shape != _crouchShape){
                    _collisionShape2D.Shape = _crouchShape;
                    _collisionShape2D.Position = new Vector2(_collisionShape2D.Position.X, _collisionShapeStartYPosition);
                    _animatedSprite.Offset = new Vector2(0, 0);
                }
                _animatedSprite.Play("crouch");
            } else {
				if (_collisionShape2D.Shape != _runShape){
					_collisionShape2D.Shape = _runShape;
					_collisionShape2D.Position = new Vector2(_collisionShape2D.Position.X, _collisionShapeStartYPosition);
					_animatedSprite.Offset = new Vector2(0,0);
				}
				_animatedSprite.Play("run");
			}
		}
	}

	public override async void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (!IsOnFloor())
		{

			 if (velocity.Y < 0 && !Input.IsActionPressed("jump"))
        {
            // extra gravity to jump shorter
            velocity.Y += gravity * _gravityMultiplier * 5.0f * (float)delta;
        }
        else
        {
            // normal gravity while rising or falling
            velocity.Y += gravity * _gravityMultiplier * (float)delta;
        }
			//velocity.Y += gravity * _gravityMultiplier * (float)delta;
		}
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = _jumpVelocity;
			_animatedSprite.Play("jump");
		}
		if (Input.IsActionJustPressed("attack") && IsOnFloor())
		{
			_animatedSprite.Play("attack");
			SpawnAttack();
		}

		Velocity = velocity;
		var hasCollided = MoveAndSlide();

		if (!hasCollided)
		{
			return;
		}

		var moveCollider = GetLastSlideCollision().GetCollider();

		//Game finished
		if (moveCollider is End)
		{
			GD.Print("Player got to home!");
			//await ToSignal(GetTree().CreateTimer(0.3), "timeout");
			GetTree().ChangeSceneToFile("res://Scenes/finish.tscn");
			return;
		}
			
			/*if (moveCollider is Collectable)
        {
			(moveCollider as Collectable).getPoints();
			var ui = GetNode<Ui>("/root/Main/UI");
    		ui.AddPoints(10);
        }*/

		// Game Over
		if ((moveCollider is Hawk) || (moveCollider is Fox) || (moveCollider is Bush))
		{
			// Only die if the fox is NOT defeated
			if (moveCollider is Fox fox && fox.IsDefeated)
				return;
				
			SetPhysicsProcess(false);
			SetProcess(false);
			_animatedSprite.Play("die");
			EmitSignal(SignalName.ObstacleHit);

			if (moveCollider is Hawk)
			{
				(moveCollider as Hawk).stop();
			}
			if (moveCollider is Fox)
			{
				(moveCollider as Fox).stop();
			}
		}

	}

	private void SpawnAttack()
	{
		// Prevent multiple attacks at once
		if (_activeAttack != null) return;

		// Instance the attack scene
		_activeAttack = (Attack)_attackScene.Instantiate();

		// Set position relative to player (e.g. in front of player)
		_activeAttack.GlobalPosition = GlobalPosition + new Vector2(40, -10); // tweak as needed

		// Add it to the same parent as Player (or to Player as a child)
		GetParent().AddChild(_activeAttack);

		// Start the attack animation
		_activeAttack.StartAnimation();

		// Listen for when it's done
		_activeAttack.AnimationFinished += OnAttackFinished;
	}
	
	private void OnAttackFinished()
{
    if (_activeAttack != null)
    {
        _activeAttack.QueueFree();
        _activeAttack = null;
    }
}
}

/*Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();*/