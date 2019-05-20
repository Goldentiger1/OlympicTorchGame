using UnityEngine;
using Valve.VR.InteractionSystem;

public class PlayerEngine : Player
{
    #region VARIABLES

    public static PlayerEngine Instance
    {
        get 
        {
            return (PlayerEngine)instance;
        }
    }

    private Torch torch;

    private Bubi bubi;

    #endregion VARIABLES

    #region PROPERTIES

    public Torch Torch
    {
        get
        {
            return torch = torch ?? FindObjectOfType<Torch>();
        }
    }

    public Bubi Bubi
    {
        get
        {
            return bubi = bubi ?? FindObjectOfType<Bubi>();
        }
    }

    #endregion PROPERTIES
}
