using Godot;
using System;
using System.Xml.Serialization;

public partial class Ui : CanvasLayer
{

	[Signal]
	public delegate void IncreseDifficultyEventHandler();

	[Export]
	private int _scoreToIncreaseDifficulty = 60;

	[Export]
	private float _pointupdateInterval = 0.3f;

	private float _pointUpdateTimer = 0.0f;

	private Label _scoreLabel;

	private Label _pointLabel;

	private int _score;

	private int _points;

	private Player _player;

	private TextureButton _restartButton;
	private TextureButton _menuButton;

	private VBoxContainer _gameOverContainer;

	private AudioStreamPlayer2D _soundEffect;

	private Panel _panel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gameOverContainer = GetNode<VBoxContainer>("%GameOverContainer");
		_panel = GetNode<Panel>("Panel");
		_restartButton = GetNode<TextureButton>("%Restart");
		_menuButton = GetNode<TextureButton>("%Menu");
		_player = GetNode<Player>("/root/Main/Player");
		_scoreLabel = GetNode<Label>("%TimeLabel");
		_pointLabel = GetNode<Label>("%PointLabel");
		_soundEffect = GetNode<AudioStreamPlayer2D>("MarginContainer/GameOverContainer/AudioStreamPlayer2D");
		_score = 0;
		_points = 0;
		_scoreLabel.Text = $"{_score.ToString("D5")}";

		_player.ObstacleHit += () =>
		{
			_gameOverContainer.Visible = true;
			_panel.Visible = true;
			//_audio.Play();
			SetProcess(false);
		};

		_restartButton.Pressed += OnStartPressed;
		_menuButton.Pressed += OnBackPressed;

		SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_pointUpdateTimer += (float)delta;
		if (_pointUpdateTimer >= _pointupdateInterval)
		{
			_score++;
			_scoreLabel.Text = $"{_score.ToString("D5")}";
			_pointUpdateTimer = 0.0f;

			if (_score % _scoreToIncreaseDifficulty == 0)
			{
				EmitSignal(SignalName.IncreseDifficulty);
			}
		}

	}

	public void StartScoring()
	{
		GD.Print("UI: Scoring started!");
		SetProcess(true);  // resume _Process loop
	}

	public void AddPoints(int amount)
	{
		_points += amount;
		_pointLabel.Text = $"{_points.ToString("D5")}";
	}

	private async void OnStartPressed()
	{
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		GetTree().ReloadCurrentScene();
	}
	
	 private async void OnBackPressed()
    {
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
    }
}
