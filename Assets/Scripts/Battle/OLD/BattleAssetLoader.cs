using PBS.Battle;
using PBS.Databases;
using PBS.Main.Pokemon;
using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class BattleAssetLoader : MonoBehaviour
{
    // Single object
    public static BattleAssetLoader instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.loadedItemSprites = new Dictionary<string, Sprite>();
            instance.loadedPokemonSprites = new Dictionary<string, Sprite>();
            instance.loadedTrainerSprites = new Dictionary<string, Sprite>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Dictionary<string, Sprite> loadedItemSprites;
    public Dictionary<string, Sprite> loadedPokemonSprites;
    public Dictionary<string, Sprite> loadedTrainerSprites;

    public Sprite nullSprite,
        nullPokemonSprite,
        nullPokemonIconSprite;

    public IEnumerator LoadBattleAssets(Battle battle)
    {
        float startTime = Time.time;
        for (int i = 0; i < battle.teams.Count; i++)
        {
            for (int k = 0; k < battle.teams[i].trainers.Count; k++)
            {
                Trainer trainer = battle.teams[i].trainers[k];

                // load pokemon assets
                List<PBS.Main.Pokemon.Pokemon> party = trainer.party;
                for (int j = 0; j < party.Count; j++)
                {
                    yield return StartCoroutine(LegacyLoadPokemon(party[j]));
                }

                // load item assets
                for (int j = 0; j < trainer.items.Count; j++)
                {
                    yield return StartCoroutine(LoadItem(trainer.items[j]));
                }

                // load trainer assets
                yield return StartCoroutine(LegacyLoadTrainer(trainer));
            }
        }
        Debug.Log("Time taken to load battle assets: " + (Time.time - startTime));
        yield return null;
    }
    
    public IEnumerator LoadBattleAssets(Model battle)
    {
        float startTime = Time.time;
        for (int i = 0; i < battle.teams.Count; i++)
        {
            for (int k = 0; k < battle.teams[i].trainers.Count; k++)
            {
                Trainer trainer = battle.teams[i].trainers[k];

                // load pokemon assets
                List<PBS.Main.Pokemon.Pokemon> party = trainer.party;
                for (int j = 0; j < party.Count; j++)
                {
                    yield return StartCoroutine(LegacyLoadPokemon(party[j]));
                }

                // load item assets
                for (int j = 0; j < trainer.items.Count; j++)
                {
                    yield return StartCoroutine(LoadItem(trainer.items[j]));
                }

                // load trainer assets
                yield return StartCoroutine(LegacyLoadTrainer(trainer));
            }
        }
        Debug.Log("Time taken to load battle assets: " + (Time.time - startTime));
        yield return null;
    }

    public IEnumerator LoadBattleAssets(PBS.Battle.View.Model battle)
    {
        float startTime = Time.time;
        for (int i = 0; i < battle.teams.Count; i++)
        {
            for (int k = 0; k < battle.teams[i].trainers.Count; k++)
            {
                PBS.Battle.View.WifiFriendly.Trainer trainer = battle.teams[i].trainers[k];

                // load pokemon assets
                List<PBS.Battle.View.WifiFriendly.Pokemon> party = trainer.party;
                for (int j = 0; j < party.Count; j++)
                {
                    yield return StartCoroutine(LoadPokemon(party[j]));
                }

                // load item assets
                for (int j = 0; j < trainer.items.Count; j++)
                {
                    yield return StartCoroutine(LoadItem(trainer.items[j]));
                }

                // load trainer assets
                yield return StartCoroutine(LoadTrainer(trainer));
            }
        }
        Debug.Log("Time taken to load battle assets: " + (Time.time - startTime));
        yield return null;
    }

    // ---POKEMON---
    public IEnumerator LoadPokemon(
        PBS.Battle.View.WifiFriendly.Pokemon pokemon,
        bool useicon = false,
        bool useFront = false,
        bool useBack = false,
        BTLSCN_Pokemon scnPokemon = null,
        BTLSCN_PokemonBW scnPokemonBW = null,
        PBS.Battle.View.Scene.Entities.Pokemon scnPokemonNew = null,
        Image imagePokemon = null
        )
    {
        PokemonData pokemonData = PBS.Databases.Pokemon.instance.GetPokemonData(pokemon.pokemonID);
        yield return StartCoroutine(LegacyLoadPokemon(
            data: pokemonData,
            useicon: useicon, useFront: useFront, useBack: useBack,
            scnPokemon: scnPokemon, scnPokemonBW: scnPokemonBW, scnPokemonNew: scnPokemonNew,
            imagePokemon: imagePokemon
            ));
    }


    public IEnumerator LegacyLoadPokemon(PBS.Main.Pokemon.Pokemon pokemon,
        bool useicon = false,
        bool useFront = false,
        bool useBack = false,
        BTLSCN_Pokemon scnPokemon = null,
        BTLSCN_PokemonBW scnPokemonBW = null,
        PBS.Battle.View.Scene.Entities.Pokemon scnPokemonNew = null,
        Image imagePokemon = null)
    {
        PokemonData pokemonData = (pokemon.bProps.illusion != null) ?
            PBS.Databases.Pokemon.instance.GetPokemonIllusionData(pokemon.bProps.illusion) : pokemon.data;
        yield return StartCoroutine(LegacyLoadPokemon(
            data: pokemonData,
            useicon: useicon, useFront: useFront, useBack: useBack,
            scnPokemon: scnPokemon, scnPokemonBW: scnPokemonBW, scnPokemonNew: scnPokemonNew,
            imagePokemon: imagePokemon
            ));
    }
    public IEnumerator LegacyLoadPokemon(PokemonData data, 
        bool useicon = false,
        bool useFront = false,
        bool useBack = false,
        BTLSCN_Pokemon scnPokemon = null,
        BTLSCN_PokemonBW scnPokemonBW = null,
        PBS.Battle.View.Scene.Entities.Pokemon scnPokemonNew = null,
        Image imagePokemon = null)
    {
        // sprite loading
        string iconSprite = "pokemonSprites/icon/" + data.displayID;
        string frontSprite = "pokemonSprites/front/" + data.displayID;
        string backSprite = "pokemonSprites/back/" + data.displayID;

        // icon
        if (!loadedPokemonSprites.ContainsKey(iconSprite))
        {
            loadedPokemonSprites[iconSprite] = null;
            var op = Addressables.LoadAssetAsync<Sprite>(iconSprite);
            yield return op;
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                loadedPokemonSprites[iconSprite] = op.Result;
                if (useicon)
                {
                    SetPokemonSprite(
                        spritePath: iconSprite,
                        scnPokemon: scnPokemon,
                        scnPokemonBW: scnPokemonBW,
                        scnPokemonNew: scnPokemonNew,
                        imagePokemon: imagePokemon
                        );
                }
            }
        }
        else if (useicon)
        {
            SetPokemonSprite(
                spritePath: iconSprite,
                scnPokemon: scnPokemon, 
                scnPokemonBW: scnPokemonBW, 
                scnPokemonNew: scnPokemonNew,
                imagePokemon: imagePokemon
                );
        }

        // front
        if (!loadedPokemonSprites.ContainsKey(frontSprite))
        {
            loadedPokemonSprites[frontSprite] = null;
            var op = Addressables.LoadAssetAsync<Sprite>(frontSprite);
            yield return op;
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                loadedPokemonSprites[frontSprite] = op.Result;
                if (useFront)
                {
                    SetPokemonSprite(
                        spritePath: frontSprite,
                        scnPokemon: scnPokemon,
                        scnPokemonBW: scnPokemonBW,
                        scnPokemonNew: scnPokemonNew,
                        imagePokemon: imagePokemon
                        );
                }
            }
        }
        else if (useFront)
        {
            SetPokemonSprite(
                spritePath: frontSprite,
                scnPokemon: scnPokemon,
                scnPokemonBW: scnPokemonBW,
                scnPokemonNew: scnPokemonNew,
                imagePokemon: imagePokemon
                );
        }

        // back
        if (!loadedPokemonSprites.ContainsKey(backSprite))
        {
            loadedPokemonSprites[backSprite] = null;
            var op = Addressables.LoadAssetAsync<Sprite>(backSprite);
            yield return op;
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                loadedPokemonSprites[backSprite] = op.Result;
                if (useBack)
                {
                    SetPokemonSprite(
                        spritePath: backSprite,
                        scnPokemon: scnPokemon,
                        scnPokemonBW: scnPokemonBW,
                        scnPokemonNew: scnPokemonNew,
                        imagePokemon: imagePokemon
                        );
                }
            }
        }
        else if (useBack)
        {
            SetPokemonSprite(
                spritePath: backSprite,
                scnPokemon: scnPokemon,
                scnPokemonBW: scnPokemonBW,
                scnPokemonNew: scnPokemonNew,
                imagePokemon: imagePokemon
                );
        }
    }
    public void SetPokemonSprite(
        string spritePath,
        BTLSCN_Pokemon scnPokemon = null,
        BTLSCN_PokemonBW scnPokemonBW = null,
        PBS.Battle.View.Scene.Entities.Pokemon scnPokemonNew = null,
        Image imagePokemon = null)
    {
        if (scnPokemon != null)
        {
            scnPokemon.spriteRenderer.sprite = loadedPokemonSprites[spritePath];
        }
        if (scnPokemonBW != null)
        {
            scnPokemonBW.spriteRenderer.sprite = loadedPokemonSprites[spritePath];
            scnPokemonBW.shadowRenderer.sprite = loadedPokemonSprites[spritePath];
        }
        if (scnPokemonNew != null)
        {
            scnPokemonNew.spriteRenderer.sprite = loadedPokemonSprites[spritePath];
            scnPokemonNew.shadowRenderer.sprite = loadedPokemonSprites[spritePath];
        }
        if (imagePokemon != null)
        {
            imagePokemon.sprite = loadedPokemonSprites[spritePath];
        }
    }

    // ---TRAINERS---
    
    public IEnumerator LoadTrainer(PBS.Battle.View.WifiFriendly.Trainer trainer)
    {
        yield return null;
    }
    public IEnumerator LegacyLoadTrainer(Trainer trainer)
    {
        yield return null;
    }

    // ---ITEMS---

    public IEnumerator LoadItem(
        Item item, 
        SpriteRenderer spriteRenderer = null,
        Image image = null
        )
    {
        yield return StartCoroutine(
            LoadItem(itemID: item.itemID, spriteRenderer: spriteRenderer, image: image
            ));
    }
    public IEnumerator LoadItem(
        string itemID, 
        SpriteRenderer spriteRenderer = null,
        Image image = null)
    {
        string itemSprite = "itemSprites/" + itemID;

        if (!loadedItemSprites.ContainsKey(itemSprite))
        {
            loadedItemSprites[itemSprite] = null;
            var op = Addressables.LoadAssetAsync<Sprite>(itemSprite);
            yield return op;
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                loadedItemSprites[itemSprite] = op.Result;
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = loadedItemSprites[itemSprite];
                }
                if (image != null)
                {
                    image.sprite = loadedItemSprites[itemSprite];
                }
            }
        }
        else
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = loadedItemSprites[itemSprite];
            }
            if (image != null)
            {
                image.sprite = loadedItemSprites[itemSprite];
            }
        }
    }

    public void UnloadBattleAssets()
    {
        instance.loadedItemSprites.Clear();
        instance.loadedPokemonSprites.Clear();
        instance.loadedTrainerSprites.Clear();
    }

    private void OnDestroy()
    {
        UnloadBattleAssets();
    }
}
