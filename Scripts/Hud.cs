using Godot;
using System;
using System.Collections.Generic;

public partial class Hud : Control
{
	public Dictionary<String, Label> AviableLabels {get; set;}

    public override void _Ready()
    {
        AviableLabels = new Dictionary<string, Label>
		{
			{"Player speed", GetNode<Label>("SpeedLabel")},
			{"Player velocity", GetNode<Label>("VelocityLabel")}
		};
    }
}
