using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using SharpDX;
using System.Windows.Forms;

class Player : GameObject
{
    private float movespeed;
    private Vector3 position;
    private Vector3 rotation;
    Input input;
     
    public Player()
    {
        this.movespeed = NORMAL_MOVESPEED;
    }
}
