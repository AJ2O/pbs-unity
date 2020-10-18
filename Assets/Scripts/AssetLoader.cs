using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetLoader
{
    //create an object of SingleObject
    private static AssetLoader singleton = new AssetLoader();

    //make the constructor private so that this class cannot be
    //instantiated
    private AssetLoader() { }

    //Get the only object available
    public static AssetLoader instance
    {
        get
        {
            return singleton;
        }
        private set
        {
            singleton = value;
        }
    }


    // Sprites
    public Sprite GetPokemonSprite(string pokemonID)
    {
        //Addressables.LoadAssetAsync<Sprite>() 

        return null;
    }

}
