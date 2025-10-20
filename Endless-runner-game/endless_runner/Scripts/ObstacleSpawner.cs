using Godot;
using System;
using System.Xml.Serialization;

public partial class ObstacleSpawner : Node
{
	[Export]
	private float _decreaseSpawnTimeOnDifficultyIncrease = 0.2f;

	[Export]
	private float _changeToSpawnHawk = 0.2f;
	private float _changeToSpawnFox = 0.0f;

	private PackedScene _bushScene = GD.Load<PackedScene>("res://Scenes/bush.tscn");
	private PackedScene _hawkScene = GD.Load<PackedScene>("res://Scenes/hawk.tscn");

	private PackedScene _foxScene = GD.Load<PackedScene>("res://Scenes/fox.tscn");

	private StaticBody2D _ground1;
	private StaticBody2D _ground2;

	private CollisionShape2D _collisionShape;
	private Sprite2D _sprite2D;

	private Timer _obstaclespawnTimer;
	private Node2D _spawnPoint;
	private Node main;

	private float[] _obstacleSpawnTimeRange = {1f, 2f};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		main = GetNode<Node>("/root/Main");
		_spawnPoint = GetNode<Node2D>("SpawnPoint");
		_obstaclespawnTimer = GetNode<Timer>("Timer");
		_ground1 = GetParent().GetNode<StaticBody2D>("Ground_1");
		_ground2 = GetParent().GetNode<StaticBody2D>("Ground_2");

		//_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		//_sprite2D = GetNode<Sprite2D>("Sprite2D");

		_obstaclespawnTimer.Timeout += SpawnObstacle;
		_obstaclespawnTimer.Stop();

	}

	public void StartSpawning()
{
    GD.Print("Obstacle spawning started!");
    _obstaclespawnTimer.Start();
}

private void SpawnObstacle(){

	Random random = new Random();
	float randomValue = (float)random.NextDouble();

	if (randomValue <= _changeToSpawnHawk && randomValue > _changeToSpawnFox){

		SpawnHawk();

	} 
	else if (randomValue <= _changeToSpawnFox){

		SpawnFox();

	}
	else {
		SpawnBush();
	}

	_obstaclespawnTimer.Stop();
	_obstaclespawnTimer.WaitTime = (float)GD.RandRange(_obstacleSpawnTimeRange[0], _obstacleSpawnTimeRange[1]);
	_obstaclespawnTimer.Start();
}

	private void SpawnHawk()
	{
		/*var hawk = _hawkScene.Instantiate<Hawk>();

		//var parentGround = _ground1.GlobalPosition.X > _ground2.GlobalPosition.X ? _ground1 : _ground2;

		main.AddChild(hawk);
		var positionY = GetViewport().GetVisibleRect().Size.Y - (float)GD.RandRange(175.0, 180.0);
		hawk.Position = new Vector2(_spawnPoint.Position.X, positionY);

		//hawk.GlobalPosition = new Vector2(_spawnPoint.Position.X, parentGround.GlobalPosition.Y - 100);
	*/

		var hawk = _hawkScene.Instantiate<Hawk>();
		var parentGround = _ground1.GlobalPosition.X > _ground2.GlobalPosition.X ? _ground1 : _ground2;
		parentGround.AddChild(hawk);
		var positionY = GetViewport().GetVisibleRect().Size.Y - (float)GD.RandRange(180.0, 180.0);
		hawk.GlobalPosition = new Vector2(_spawnPoint.GlobalPosition.X, positionY);
	}

private void SpawnFox(){
	var fox = _foxScene.Instantiate<Fox>();

	var parentGround = _ground1.GlobalPosition.X > _ground2.GlobalPosition.X ? _ground1 : _ground2;

	parentGround.AddChild(fox);
	fox.GlobalPosition = new Vector2(_spawnPoint.GlobalPosition.X, parentGround.GlobalPosition.Y - 100);
}
private void SpawnBush(){
	var bush = _bushScene.Instantiate<Bush>();

	var parentGround = _ground1.GlobalPosition.X > _ground2.GlobalPosition.X ? _ground1 : _ground2;

	parentGround.AddChild(bush);

	//bush=_sprite2D;

	bush.GlobalPosition = new Vector2(_spawnPoint.Position.X, parentGround.GlobalPosition.Y - 100);
}

	public void IncreaseDifficulty() {
		_changeToSpawnFox += 0.05f;
		_changeToSpawnHawk += 0.05f;
		if (_obstacleSpawnTimeRange[0] > 0.5f){
			_obstacleSpawnTimeRange[0] -= _decreaseSpawnTimeOnDifficultyIncrease;
			_obstacleSpawnTimeRange[1] -= _decreaseSpawnTimeOnDifficultyIncrease;
		}
	}

}
