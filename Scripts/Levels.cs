using Godot;
using System;
using System.Collections.Generic;

public partial class Levels : Node2D
{

	[Signal] public delegate void LevelInfoFinishedEventHandler();

	private AudioStreamPlayer2D _soundEffect;

	private static Dictionary<string, string[]> _LEVEL_INFOS
	= new Dictionary<string, string[]>() {
		{"level1", new string[] {
		"I am a black cat, free to roam outside whenever I want. Chasing butterflies is my favorite thing!",
		"I never wander far from the garden… but this time, I followed one a little too eagerly.",
		"The sun is setting, and the forest is growing darker. Too many things here could end my journey.",
		"I must hurry back home.",
		"Up ahead, sharp thorny bushes block my path. One wrong step, and I could get badly hurt.",
		"I need to jump over them to stay alive. (Press 'W' to jump)",
		"Home is still far away… but I have to keep going.)"
		 }
		 },{ "level2", new string[] {
		"It is getting late, and my paws are tired. Home still feels so far away.",
		"I hear wings above me… hawks are circling the sky.",
		"If they spot me, I will not stand a chance.",
		"I must stay low and move carefully to survive. (Press 'S' to crawl)"
		}
		},{ "level3", new string[] {
		"I can smell home now. I am so close.",
		"But something is wrong… foxes are lurking ahead.",
		"They are fast and dangerous, but I will not give up now.",
		"I can fight back, or leap over them to escape. (Press 'Space' to attack, or 'W' to jump)",
		"If I survive this, I will finally make it home."
		}
	}
};
	private MarginContainer _infoContainer;
	private MarginContainer _levelContainer;
	private Label _levelLabel;
	private Label _infoLabel;
	private TextureButton _nextButton;
	private int _infoSentenceIDx;
	private int _levelnumber = 0;
	private string _infoID;

	private Panel _panel;

	private Action _onFinishedCallback;

	private bool _isTyping = false;
	private float _typingSpeed = 0.03f;
	
	private bool _skipRequested = false;
	private string _currentSentence = "";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_levelContainer = GetNode<MarginContainer>("%LevelContainer");
		_infoContainer = GetNode<MarginContainer>("%InfoContainer");
		_panel = GetNode<Panel>("Panel");

		_soundEffect = GetNode<AudioStreamPlayer2D>("InfoContainer/AudioStreamPlayer2D");

		_levelnumber++;
		_infoID = "level" + _levelnumber.ToString();

		_levelLabel = GetNode<Label>("%Level");
		_infoLabel = GetNode<Label>("%Info");

		_nextButton = GetNode<TextureButton>("%Next");
		_nextButton.Pressed += OnNextPressed;

		ShowLevelInfo();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void ShowLevelInfo(Action onFinished = null)
	{
		_onFinishedCallback = onFinished;

		_levelContainer.Visible = true;
		_infoContainer.Visible = true;
		_panel.Visible = true;

		_infoSentenceIDx = 0;
		_infoID = "level" + _levelnumber.ToString();

		_levelLabel.Text = "Level: " + _levelnumber.ToString();
		getDialogy(false);
	}

private void OnNextPressed()
{
	if (_isTyping)
	{
		_skipRequested = true;
		_soundEffect.Play();
	}
	else
	{
		getDialogy(true);
	}
}

	public async void getDialogy(bool playSound = true)
{
	// Estä spämmi
	if (_isTyping)
		return;

	if (playSound)
		_soundEffect.Play();

	string[] sentences = _LEVEL_INFOS[_infoID];

	// Jos dialogi loppui
	if (_infoSentenceIDx >= sentences.Length)
	{
		_infoLabel.Text = "";
		_levelContainer.Visible = false;
		_infoContainer.Visible = false;
		_panel.Visible = false;

		if (_infoID == "level1")
			EmitSignal(SignalName.LevelInfoFinished);
		else
			_onFinishedCallback?.Invoke();

		return;
	}

	_isTyping = true;
	_skipRequested = false;
	_infoLabel.Text = "";

	_currentSentence = sentences[_infoSentenceIDx];
	_infoSentenceIDx++;

	foreach (char c in _currentSentence)
	{
		if (_skipRequested)
		{
			_infoLabel.Text = _currentSentence;
			break;
		}
		_infoLabel.Text += c;
		await ToSignal(GetTree().CreateTimer(_typingSpeed), "timeout");
	}

	_isTyping = false;
}

	public void ShowNextLevelInfo(Action onFinished)
	{
		_levelnumber++;
		if (_levelnumber == 4)
		{
			GD.Print("Game finished! Returning to Menu.");
			_onFinishedCallback?.Invoke();
		}
		else
			ShowLevelInfo(onFinished);

	}
}
