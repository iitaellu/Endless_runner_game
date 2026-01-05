using Godot;
using System;

public partial class Finish : CanvasLayer
{

	private Label _first;
    private Label _second;
	private Label _third;
	
	private GlobalScore _globalScore;

	private TextureButton _finishButton;
	private TextureButton _startAgainButton;

	private AudioStreamPlayer2D _soundEffect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		_finishButton = GetNode<TextureButton>("%Finish");
		_startAgainButton = GetNode<TextureButton>("%StartNext");
		_first = GetNode<Label>("%1place");
        _second = GetNode<Label>("%2place");
		_third = GetNode<Label>("%3place");

		_soundEffect = GetNode<AudioStreamPlayer2D>("%ButtonEffects");

		_globalScore = GetNode<GlobalScore>("/root/GlobalScore");

		// Save latest score if not already
		//_globalScore.SaveScore();

		var scores = _globalScore.TopScores;
		
		_first.Text = $"1st place: {scores[0]}";
        _second.Text = $"2nd place: {scores[1]}";
        _third.Text = $"3rd place: {scores[2]}";

		//Highlight current score
		switch (_globalScore.LastScoreRank)
	{
		case 0:
			_first.Modulate = Colors.Red;
			break;
		case 1:
			_second.Modulate = Colors.Red;
			break;
		case 2:
			_third.Modulate = Colors.Red;
			break;
	}

		_finishButton.Pressed += OnStartPressed;
		_startAgainButton.Pressed += OnExitPressed;
    }

	private async void OnStartPressed()
	{
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
	}
	private async void OnExitPressed()
	{
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
