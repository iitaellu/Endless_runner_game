using Godot;
using System;

public partial class Ground : Node2D
{

	[Export]
	private float _speedIncreaseForDifficulty = -125.0f;

	[Export]
	private float _speed = -125.0f;



	private StaticBody2D _staticBody2D;
	private StaticBody2D _staticBody2D2;
	private int _texturewidth;

	private Player _player;

	private ObstacleSpawner _obstacleSpawner;

	private Ui _ui;

	private Levels _levels;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get elements
		_ui = GetNode<Ui>("/root/Main/UI");
		_levels = GetNode<Levels>("/root/Main/Levels");
		_player = GetNode<Player>("../Player");
		_staticBody2D = GetNode<StaticBody2D>("Ground_1");
		_staticBody2D2 = GetNode<StaticBody2D>("Ground_2");


		//This part works, when picture is not scaled manually
		//From .staticBody2d we want create node, whit sprite value from "GroundSprite1 and get its texture and its widght"
		//_texturewidth = _staticBody2D.GetNode<Sprite2D>("GroundSprite").Texture.GetWidth();

		Sprite2D groundSprite = _staticBody2D.GetNode<Sprite2D>("GroundSprite");
		_texturewidth = (int)(groundSprite.Texture.GetSize().X * groundSprite.Scale.X);

		_obstacleSpawner = GetNode<ObstacleSpawner>("ObstacleSpawner");

		_levels.LevelInfoFinished += () => {
            _obstacleSpawner.StartSpawning();
            _ui.StartScoring();
        };
		//_staticBody2D.GlobalPosition = _staticBody2D.GlobalPosition + new Vector2(x: _staticBody2D.GlobalPosition.X + _texturewidth, y: _staticBody2D.GlobalPosition.Y);
		_staticBody2D2.GlobalPosition = _staticBody2D2.GlobalPosition with { X = _staticBody2D.GlobalPosition.X + _texturewidth};

		_player.ObstacleHit += () => {
			_speed = 0;
			_obstacleSpawner.QueueFree();
		};

		_ui.IncreseDifficulty += () => {
			_speed += _speedIncreaseForDifficulty;
			_obstacleSpawner.IncreaseDifficulty();
		};


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_staticBody2D.GlobalPosition += new Vector2(_speed * (float)delta, 0);
		_staticBody2D2.GlobalPosition += new Vector2(_speed * (float)delta, 0);

		if (_staticBody2D.GlobalPosition.X < -+_texturewidth) {
			_staticBody2D.GlobalPosition = _staticBody2D2.GlobalPosition + new Vector2(_texturewidth, 0);

		}
		if (_staticBody2D2.GlobalPosition.X < -+_texturewidth) {
			_staticBody2D2.GlobalPosition = _staticBody2D.GlobalPosition + new Vector2(_texturewidth, 0);
		}
	}
}
