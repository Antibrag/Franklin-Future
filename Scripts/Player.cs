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
	private Vector3 DetectionScale;

	private float Speed { get; set; } = NORMAL_SPEED;
	public States State { get; set; } = States.NORMAL;

	public override void _Ready()
	{
		Position = GetNode<Node3D>("../SpawnPoint").Position;

		NormalDetectionScale = DetectionScale = GetNode<Node3D>("DetectionCollisionShape").Scale;	
	}

	public override void _PhysicsProcess(double delta)
	{	
		Vector2 MousePosition = GetViewport().GetMousePosition();
		Vector2 LookPosition = MousePosition - GetViewport().GetVisibleRect().Size / 2; //Get Center of Screen

		LookAt(new Vector3(LookPosition.X, Position.Y, LookPosition.Y));

		Movement(delta);
		MoveAndSlide();
	}

	private void Movement(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
			Jump(ref velocity);

		if (Input.IsActionJustPressed("Sit") && IsOnFloor())
			Sitting();
		
		if (Input.IsActionJustPressed("Running") && IsOnFloor())
			Running();

		Vector2 inputDir = Input.GetVector("Left", "Right", "Forward", "Backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)delta * Speed * 2.5f);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, (float)delta * Speed * 2.5f);
		}

		GetNode<Hud>("../HUD").AviableLabels["Player speed"].Text = $"Player speed: {Speed}";
		GetNode<Hud>("../HUD").AviableLabels["Player velocity"].Text = $"Player velocity: {Velocity}";
		Velocity = velocity;
	}

	private void Sitting() 
	{

		if (State != States.SITTING)
		{
			Speed = NORMAL_SPEED / 2;
			State = States.SITTING;
			DetectionScale = NormalDetectionScale / 3;

		}
		else
		{
			Speed = NORMAL_SPEED;
            State = States.NORMAL;
			DetectionScale = NormalDetectionScale;
		}
	}

	private void Running()
	{
		if (State != States.RUNNING)
        {
            Speed = NORMAL_SPEED * 2;
			State = States.RUNNING;
            DetectionScale = NormalDetectionScale * 2;
        }
        else
        {
            State = States.NORMAL;
            Speed = NORMAL_SPEED;
            DetectionScale = NormalDetectionScale;
        }
	}

	private void Jump(ref Vector3 velocity)
	{
		velocity.Y = JUMP_VELOCITY;
		//Player animation = jumping
		//Play
	}
}
