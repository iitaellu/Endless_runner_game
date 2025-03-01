using Godot;
using System;

public partial class Player : CharacterBody2D
{

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

    private float _yOffsetCrouch = 30.0f;
    private float _collisionShapeStartYPosition;


    public override void _Ready()
    {
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        _collisionShape2D.Shape = _runShape;
        _collisionShapeStartYPosition = _collisionShape2D.Position.Y;
    }

    public override void _Process(double delta)
    {
		if (_animatedSprite.Animation == "attack" && _animatedSprite.IsPlaying()){
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

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		if (!IsOnFloor()){
			velocity.Y += gravity * _gravityMultiplier * (float)delta;
		}
		if (Input.IsActionJustPressed("jump") && IsOnFloor()){
			velocity.Y = _jumpVelocity;
			_animatedSprite.Play("jump");
		}
		if (Input.IsActionJustPressed("attack") && IsOnFloor()){
			_animatedSprite.Play("attack");
		}

		Velocity = velocity;
		MoveAndSlide();
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