using Godot;

public partial class Player : CharacterBody3D
{		
	public enum States
	{
		NORMAL,
		SITTING,
		RUNNING,
		DEAD
	}

	//Constants
	private const float JUMP_VELOCITY = 4.5f;
	private const float NORMAL_SPEED = 5.0f;
	private float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private Vector3 NormalDetectionScale;
	private float Speed = NORMAL_SPEED;

	public States State { get; set; }

	public override void _Ready()
	{
		State = States.NORMAL;
		Position = GetNode<Node3D>("../SpawnPoint").Position;
		NormalDetectionScale = GetNode<Node3D>("DetectionCollisionShape").Scale;
	}

	public override void _PhysicsProcess(double delta)
	{
		Movement(ref delta);
		MoveAndSlide();
	}

	private void Movement(ref double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			velocity.Y = JUMP_VELOCITY;

		if (Input.IsActionJustPressed("Sit") && IsOnFloor())
		{
			if (State == States.NORMAL || State == States.RUNNING) 
			{
				State = States.SITTING;
				Speed = NORMAL_SPEED/2;
				GetNode<CollisionShape3D>("DetectionCollisionShape").Scale = NormalDetectionScale/3;
			}
			else 
			{
				State = States.NORMAL;
				Speed = NORMAL_SPEED;
				GetNode<CollisionShape3D>("DetectionCollisionShape").Scale = NormalDetectionScale;
			}
		}

		if (Input.IsActionJustPressed("Running") && IsOnFloor())
		{
			if (State == States.NORMAL || State == States.SITTING)
			{
				State = States.RUNNING;
				Speed = NORMAL_SPEED*2;
				GetNode<CollisionShape3D>("DetectionCollisionShape").Scale = NormalDetectionScale*2;
			}
			else
			{
				State = States.NORMAL;
				Speed = NORMAL_SPEED;
				GetNode<CollisionShape3D>("DetectionCollisionShape").Scale = NormalDetectionScale;
			}
		}

		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		GetNode<Hud>("../HUD").AviableLabels["Player speed"].Text = $"Player speed: {Speed.ToString()}";
		GetNode<Hud>("../HUD").AviableLabels["Player velocity"].Text = $"Player velocity: {Velocity.ToString()}";
		Velocity = velocity;
	}




}
