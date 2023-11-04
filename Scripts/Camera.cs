using Godot;

public partial class Camera : Camera3D
{
	public override void _Process(double delta) => 
		Position = new Vector3(GetNode<Node3D>("../Player").Position.X, Position.Y, GetNode<Node3D>("../Player").Position.Z + 3);
}
