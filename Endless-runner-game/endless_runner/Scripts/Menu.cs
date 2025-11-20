using Godot;
using System;

public partial class Menu : Node
{

	[Export]

	private TextureButton _startButton;
	private TextureButton _exitButton;
	private TextureButton _creditsButton;

	private AudioStreamPlayer2D _soundEffect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startButton = GetNode<TextureButton>("%Start");
		_exitButton = GetNode<TextureButton>("%Exit");
		_creditsButton = GetNode<TextureButton>("%Credits");
		_soundEffect = GetNode<AudioStreamPlayer2D>("%Effect");


		_startButton.Pressed += OnStartPressed;
		_exitButton.Pressed += OnExitPressed;
		_creditsButton.Pressed += OnCreditsPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.


	private async void OnStartPressed()
	{
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}
	private async void OnExitPressed()
	{
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		GetTree().Quit();
	}
	private async void OnCreditsPressed()
    {
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().ChangeSceneToFile("res://Scenes/credits.tscn");
    }


	public override void _Process(double delta)
	{

	}
}
