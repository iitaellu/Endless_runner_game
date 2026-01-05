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

	[Signal]
	public delegate void GoalReachedEventHandler();


	//Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private RectangleShape2D _runShape = GD.Load<RectangleShape2D>("res://collisionShapes/player/player_run.tres");
	private RectangleShape2D _crouchShape = GD.Load<RectangleShape2D>("res://collisionShapes/player/player_crouching.tres");

	private AudioStreamPlayer2D _audio;

	private AnimatedSprite2D _animatedSprite;
	private CollisionShape2D _collisionShape2D;

	private PackedScene _attackScene;

	private PackedScene _endScene;
	private Attack _activeAttack;

    private float _yOffsetCrouch = 30.0f;
	private float _collisionShapeStartYPosition;

	private AudioStreamPlayer2D _soundEffect;
	private AudioStream _deathStream;
	private AudioStream _winningStream;
	private AudioStream _jumpStream;

	private AudioStream _crawlingStream;

	private bool _autoRunToFinish = false;
	private float _autoRunSpeed = 400f;

	public override void _Ready()
	{
		GetNode<GlobalScore>("/root/GlobalScore").ResetScore();
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		_attackScene = GD.Load<PackedScene>("res://Scenes/attack.tscn");
		_endScene = GD.Load<PackedScene>("res://Scenes/end.tscn");
		_collisionShape2D.Shape = _runShape;
		_collisionShapeStartYPosition = _collisionShape2D.Position.Y;

		_soundEffect = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

		_deathStream = GD.Load<AudioStream>("res://Music/sounds/game-over-401236.mp3");
		_winningStream = GD.Load<AudioStream>("res://Music/sounds/level-passed-143039.mp3");
		_jumpStream = GD.Load<AudioStream>("res://Music/sounds/thud-362392.mp3");
		_crawlingStream = GD.Load<AudioStream>("res://Music/sounds/walking-on-grass-381887.mp3");
	}

    public override async void _Process(double delta)
	{
		if (_autoRunToFinish)
    {
        GlobalPosition += new Vector2(_autoRunSpeed * (float)delta, 0);
        _animatedSprite.Play("run");

        // Kun pelaaja on pois ruudulta → vaihda scene
        if (GlobalPosition.X > GetViewportRect().Size.X + 100)
        {
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
            GetTree().ChangeSceneToFile("res://Scenes/finish.tscn");
        }

        return;
    }

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
				if (!_soundEffect.Playing || _soundEffect.Stream != _crawlingStream)
            {
                _soundEffect.Stream = _crawlingStream;
                _soundEffect.Play();
            }
            } else {
				if (_collisionShape2D.Shape != _runShape){
					_collisionShape2D.Shape = _runShape;
					_collisionShape2D.Position = new Vector2(_collisionShape2D.Position.X, _collisionShapeStartYPosition);
					_animatedSprite.Offset = new Vector2(0,0);
				}
				_animatedSprite.Play("run");
				if (_soundEffect.Stream == _crawlingStream && _soundEffect.Playing)
            {
                _soundEffect.Stop();
            }
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
			_soundEffect.Stream = _jumpStream;
			_soundEffect.Play();
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
			_soundEffect.Stream = _winningStream;
			_soundEffect.Play();
			GD.Print("Player got to home!");
			GetNode<Main>("/root/Main").StopMusic();
			GetNode<GlobalScore>("/root/GlobalScore").SaveScore();

			//Stop user inputs and start auto run for character
			SetPhysicsProcess(false);
    		SetProcess(false);
			_autoRunToFinish = true;
    		SetProcess(true);
			//Stop rest the world
			EmitSignal(SignalName.GoalReached);
			//await ToSignal(GetTree().CreateTimer(0.3), "timeout");
			//GetTree().ChangeSceneToFile("res://Scenes/finish.tscn");
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
			GetNode<Main>("/root/Main").StopMusic();
			_soundEffect.Stream = _deathStream;
			_soundEffect.Play();
			// Only die if the fox is NOT defeated
			if (moveCollider is Fox fox && fox.IsDefeated)
				return;
				
			SetPhysicsProcess(false);
			SetProcess(false);
			_animatedSprite.Play("die");
			EmitSignal(SignalName.ObstacleHit);

			if (moveCollider is Bush)
			{
				(moveCollider as Bush).SoundEffect();
			}
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