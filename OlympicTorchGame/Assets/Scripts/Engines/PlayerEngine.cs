using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerEngine : Player
{
    public static PlayerEngine Instance 
    {
        get 
        {
            return (PlayerEngine)instance;
        }
    }

    private readonly Torch torch;

    public Torch _Torch
    {
        get 
        {
            return torch ?? FindObjectOfType<Torch>();
        }
    }

    private Bubi bubi;

    public Bubi Bubi 
    {
        get
        {
           return bubi = bubi ?? FindObjectOfType<Bubi>();
        }
    }
}
