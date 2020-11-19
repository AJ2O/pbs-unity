using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI
{
    public class Canvas : MonoBehaviour
    {
        [Header("Panels")]
        public Panels.Command cmdPanel;
        public Panels.Fight fightPanel;
        public Panels.FieldTarget fieldTargetPanel;
        public Panels.Party partyPanel;
        public Panels.Bag bagPanel;
        public Panels.BagItem bagItemPanel;
        public HUD.Panel HUDPanel;

        [Header("Dialog")]
        public Dialog dialog;

        [HideInInspector] public PBS.Battle.View.Enums.Panel panelType;
        [HideInInspector] public Panels.BasePanel currentPanel;

        void Awake()
        {
            currentPanel = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            panelType = PBS.Battle.View.Enums.Panel.None;
            SwitchPanel(panelType);
            UnsetPanels();
            dialog.gameObject.SetActive(true);
            dialog.ClearBox();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // Panel Management
        public void SwitchPanel(PBS.Battle.View.Enums.Panel newPanel)
        {
            panelType = newPanel;
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }
        
            switch (newPanel)
            {
                case PBS.Battle.View.Enums.Panel.Command:
                    currentPanel = cmdPanel;
                    break;

                case PBS.Battle.View.Enums.Panel.Fight:
                    currentPanel = fightPanel;
                    break;

                case PBS.Battle.View.Enums.Panel.FieldTargeting:
                    currentPanel = fieldTargetPanel;
                    break;

                case PBS.Battle.View.Enums.Panel.Party:
                    currentPanel = partyPanel;
                    partyPanel.commandPanel.gameObject.SetActive(false);
                    break;

                case PBS.Battle.View.Enums.Panel.PartyCommand:
                    currentPanel = partyPanel;
                    partyPanel.commandPanel.gameObject.SetActive(true);
                    break;

                case PBS.Battle.View.Enums.Panel.Bag:
                    currentPanel = bagPanel;
                    break;

                case PBS.Battle.View.Enums.Panel.BagItem:
                    currentPanel = bagItemPanel;
                    break;

                default:
                    currentPanel = null;
                    break;
            }
    
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(true);
            }

            // HUD (only on None or Fight Panels)
            HUDPanel.gameObject.SetActive(
                panelType == PBS.Battle.View.Enums.Panel.None 
                || panelType == PBS.Battle.View.Enums.Panel.Command);
        }
        public void UnsetPanels()
        {
            cmdPanel.gameObject.SetActive(false);
            fightPanel.gameObject.SetActive(false);
            fieldTargetPanel.gameObject.SetActive(false);
            partyPanel.gameObject.SetActive(false);
            bagPanel.gameObject.SetActive(false);
            SwitchPanel(PBS.Battle.View.Enums.Panel.None);
        }

        // Command Panel
        public void SetCommands(PBS.Battle.View.Compact.Pokemon pokemon, IEnumerable<BattleCommandType> commandList)
        {
            HashSet<BattleCommandType> commandSet = new HashSet<BattleCommandType>(commandList);
            cmdPanel.SetCommands(pokemon, commandList);
        }
        public void SwitchSelectedCommandTo(BattleCommandType commandType)
        {
            cmdPanel.HighlightCommand(commandType);
        }

        // Fight Panel
        public void SetMoves(
            PBS.Battle.View.Compact.Pokemon pokemon, 
            List<PBS.Battle.View.Events.CommandAgent.Moveslot> moveslots, 
            bool canMegaEvolve, bool canZMove = false, bool canDynamax = false,
            bool choosingZMove = false, bool choosingMaxMove = false)
        {
            List<PBS.Battle.View.Events.CommandAgent.Moveslot> filteredMoveslots 
                = new List<PBS.Battle.View.Events.CommandAgent.Moveslot>(moveslots);
            for (int i = 0; i < filteredMoveslots.Count; i++)
            {
                PBS.Battle.View.Events.CommandAgent.Moveslot moveslot = filteredMoveslots[i];
                if (moveslot == null)
                {
                    filteredMoveslots.RemoveAt(i);
                    i--;
                }
            }
            fightPanel.SetMoves(
                pokemon: pokemon,
                moveList: filteredMoveslots,
                canMegaEvolve: canMegaEvolve, canZMove: canZMove, canDynamax: canDynamax,
                choosingZMove: choosingZMove, choosingMaxMove: choosingMaxMove);
        }
        public void SwitchSelectedMoveTo(
            PBS.Battle.View.Compact.Pokemon pokemon, 
            int selected, 
            bool choosingSpecial, bool choosingZMove, bool choosingMaxMove)
        {
            fightPanel.HighlightMove(selected);
            if (choosingSpecial || choosingZMove || choosingMaxMove)
            {
                fightPanel.specialBtn.SelectSelf();
            }
            else
            {
                fightPanel.specialBtn.UnselectSelf();
            }
        }
        public void SwitchSelectedMoveToBack()
        {
            fightPanel.HighlightBackButton();
        }

        // Field Targeting Panel
        public void SetFieldTargets(PBS.Battle.View.Model model, int teamPos)
        {
            fieldTargetPanel.SetFieldTargets(teamPos: teamPos, battleModel: model);
        }
        public void SwitchSelectedMoveTargetsTo(
            PBS.Battle.View.Model model,
            BattlePosition userPos,
            int chooseIndex,
            List<List<BattlePosition>> choices)
        {
            List<BattlePosition> choice = choices[chooseIndex];
            if (choice == null)
            {
                fieldTargetPanel.HighlightBackButton();
            }
            else
            {
                fieldTargetPanel.HighlightFieldTargets(userPos, choice);
            }
        }

        // Party Panel
        public void SetParty(List<PBS.Battle.View.Compact.Pokemon> pokemon, bool forceSwitch = false, Item item = null)
        {
            List<PBS.Battle.View.Compact.Pokemon> filteredPokemon = new List<PBS.Battle.View.Compact.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                if (pokemon[i] != null)
                {
                    filteredPokemon.Add(pokemon[i]);
                }
            }

            partyPanel.SetParty(party: filteredPokemon, item: item);
            partyPanel.backBtn.gameObject.SetActive(!forceSwitch);
        }
        public void SwitchSelectedPartyMemberTo(PBS.Battle.View.Compact.Pokemon selected)
        {
            partyPanel.HighlightPokemon(selected.uniqueID);
        }
        public void SwitchSelectedPartyMemberToBack()
        {
            partyPanel.HighlightBackButton();
        }

        // Party Commands
        public void SetPartyCommands(PBS.Battle.View.Compact.Pokemon pokemon, List<BattleExtraCommand> commands)
        {
            partyPanel.SetCommands(commands);
        }
        public void SwitchSelectedPartyCommandTo(BattleExtraCommand selected)
        {
            partyPanel.HighlightCommand(selected);
        }

        // Bag Panel
        public void SetBagPockets(List<ItemBattlePocket> list)
        {
            bagPanel.SetPockets(list);
        }
        public void SwitchSelectedBagPocketTo(ItemBattlePocket selected)
        {
            bagPanel.HighlightPocket(selected);
        }
        public void SwitchSelectedBagPocketToBack()
        {
            bagPanel.HighlightBackButton();
        }

        // Bag Item Panel
        public void SetItems(PBS.Battle.View.Compact.Trainer trainer, ItemBattlePocket pocket, List<Item> list, int offset)
        {
            List<Item> filteredItems = new List<Item>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    filteredItems.Add(list[i]);
                }
            }
            bagItemPanel.SetItems(trainer, filteredItems, offset);
            bagItemPanel.backBtn.gameObject.SetActive(true);
        }
        public void SwitchSelectedItemTo(Item selected)
        {
            bagItemPanel.HighlightButton(selected.itemID);
        }
        public void SwitchSelectedItemToBack()
        {
            bagItemPanel.HighlightBackButton();
        }

        // HUD
        public HUD.PokemonHUD DrawPokemonHUD(PBS.Battle.View.Compact.Pokemon pokemon, TeamMode teamMode, bool isNear)
        {
            return HUDPanel.DrawPokemonHUD(pokemon, teamMode, isNear);
        }
        public bool UndrawPokemonHUD(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            return HUDPanel.UndrawPokemonHUD(pokemon);
        }

        public HUD.PokemonHUD GetPokemonHUD(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            return HUDPanel.GetPokemonHUD(pokemon);
        }
        public void UpdatePokemonHUD(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            HUDPanel.UpdatePokemonHUD(pokemon);
        }

        public void SetPokemonHUDActive(PBS.Battle.View.Compact.Pokemon pokemon, bool active)
        {
            HUDPanel.SetPokemonHUDActive(pokemon, active);
        }
        public void SetPokemonHUDsActive(bool active)
        {
            HUDPanel.SetPokemonHUDsActive(active);
        }

        public IEnumerator AnimatePokemonHUDHPChange(PBS.Battle.View.Compact.Pokemon pokemon, int preHP, int postHP, int maxHP, float timeSpan = 1f)
        {
            yield return StartCoroutine(HUDPanel.AnimatePokemonHUDHPChange(
                pokemon: pokemon,
                preHP: preHP, 
                postHP: postHP,
                maxHP: maxHP,
                timeSpan: timeSpan
                ));
        }

        // Dialog
        public void UndrawDialogBox()
        {
            dialog.dialogBox.gameObject.SetActive(false);
            if (panelType == PBS.Battle.View.Enums.Panel.Command)
            {
                cmdPanel.gameObject.SetActive(true);
            }
            else if (panelType == PBS.Battle.View.Enums.Panel.Fight)
            {
                fightPanel.gameObject.SetActive(true);
            }
            HUDPanel.pokemonHUDNearRoot.gameObject.SetActive(true);
        }

        public IEnumerator DrawText(string text, bool undrawOnFinish = true, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text,
                secPerChar: 1f / dialog.charPerSec,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawTextNoWait(string text, bool undrawOnFinish = true, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text,
                secPerChar: 1f / dialog.charPerSec,
                time: 0,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawTextInstant(string text, bool undrawOnFinish = true, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text,
                secPerChar: 0,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawTextInstantNoWait(string text, bool undrawOnFinish = true, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text,
                secPerChar: 0,
                time: 0,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawText(string text, float time, float lockedTime, bool undrawOnFinish = true, Text textBox = null, int lines = -1)
        {
            yield return StartCoroutine(DrawText(
                text: text,
                secPerChar: 1f / dialog.charPerSec,
                time: time,
                lockedTime: lockedTime,
                silent: true,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines));
        }
        public IEnumerator DrawText(
            string text,
            float secPerChar,
            float time = 2f,
            float lockedTime = 0,
            bool silent = true,
            bool hold = false,
            bool undrawOnFinish = true,
            Text textBox = null,
            int lines = -1
            )
        {
            // Default Dialog Box
            if (textBox == null)
            {
                HUDPanel.pokemonHUDNearRoot.gameObject.SetActive(false);
            }

            yield return StartCoroutine(dialog.DrawText(
                text: text,
                secPerChar: secPerChar,
                time: time,
                lockedTime: lockedTime,
                silent: silent,
                hold: hold,
                undrawOnFinish: undrawOnFinish,
                textBox: textBox,
                lines: lines
                ));

            if (textBox == null && undrawOnFinish)
            {
                UndrawDialogBox();
            }
        }

        // Message Rendering
        public static string GetPokemonName(PBS.Battle.View.Compact.Pokemon pokemon, PBS.Battle.View.Model myModel)
        {
            return GetPokemonNames(new List<PBS.Battle.View.Compact.Pokemon> { pokemon }, myModel);
        }
        public static string GetPokemonNames(List<PBS.Battle.View.Compact.Pokemon> pokemonList, PBS.Battle.View.Model myModel, bool orConjunct = false)
        {
            string conjunct = (orConjunct) ? "or" : "and";

            string names = "";
            if (pokemonList.Count == 1)
            {
                return pokemonList[0].nickname;
            }
            else if (pokemonList.Count == 2)
            {
                return pokemonList[0].nickname + " " + conjunct + " " + pokemonList[1].nickname;
            }
            else
            {
                for (int i = 0; i < pokemonList.Count; i++)
                {
                    names += (i == pokemonList.Count - 1) ?
                        conjunct + " " + pokemonList[i].nickname :
                        pokemonList[i].nickname + ", ";
                }
            }
            return names;
        }
        public static string GetTrainerNames(List<PBS.Battle.View.Compact.Trainer> trainers, PBS.Battle.View.Model myModel)
        {
            string text = "";
            for (int i = 0; i < trainers.Count; i++)
            {
                text += (i == 0)? trainers[i].name : " and " + trainers[i].name;
            }
            return text;
        }

        public static PBS.Battle.View.Enums.ViewPerspective GetPerspective(
            PBS.Battle.View.Compact.Pokemon pokemon,
            PBS.Battle.View.Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0)
        {
            PBS.Battle.View.Compact.Trainer trainer = myModel.GetTrainer(pokemon);
            PBS.Battle.View.Compact.Team team = myModel.GetTeamOfTrainer(trainer);
            if (team.teamID != teamPerspectiveID)
            {
                return PBS.Battle.View.Enums.ViewPerspective.Enemy;
            }
            else
            {
                if (myPlayerID == 0)
                {
                    return PBS.Battle.View.Enums.ViewPerspective.Ally;
                }
                return PBS.Battle.View.Enums.ViewPerspective.Player;
            }
        }
        public static List<PBS.Battle.View.Compact.Pokemon> FilterPokemonByPerspective(
            List<PBS.Battle.View.Compact.Pokemon> pokemon,
            PBS.Battle.View.Enums.ViewPerspective viewPerspective,
            PBS.Battle.View.Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0
            )
        {
            List<PBS.Battle.View.Compact.Pokemon> filteredPokemon = new List<PBS.Battle.View.Compact.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                PBS.Battle.View.Enums.ViewPerspective perspective = GetPerspective(
                    pokemon: pokemon[i],
                    myModel: myModel,
                    teamPerspectiveID: teamPerspectiveID,
                    myPlayerID: myPlayerID);
                if (perspective == viewPerspective)
                {
                    filteredPokemon.Add(pokemon[i]);
                }
            }
            return filteredPokemon;
        }

        public static string GetPrefix(
            PBS.Battle.View.Enums.ViewPerspective viewPerspective,
            PBS.Battle.View.Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0,
            bool lowercase = false)
        {
            string prefix = (viewPerspective == PBS.Battle.View.Enums.ViewPerspective.Ally) ? "The ally "
                : (viewPerspective == PBS.Battle.View.Enums.ViewPerspective.Enemy) ? 
                    (myModel.settings.isWildBattle? "The wild " : "The foe's ")
                : "";
            if (lowercase)
            {
                prefix = prefix.ToLower();
            }

            return prefix;
        }
        public static string GetPrefix(
            PBS.Battle.View.Compact.Pokemon pokemon,
            PBS.Battle.View.Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0,
            bool capitalize = true)
        {
            string text = "";
            PBS.Battle.View.Compact.Trainer trainer = myModel.GetTrainer(pokemon);
            if (pokemon.teamPos != teamPerspectiveID)
            {
                text = "The opposing ";
            }
            else
            {
                if (myPlayerID != 0)
                {
                    if (trainer.playerID != myPlayerID)
                    {
                        text = "The ally ";
                    }
                }
            }
            if (!capitalize)
            {
                text = text.ToLower();
                text = " " + text;
            }
            return text;
        }

        public string GetPokemonName(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            return GetPokemonNames(new List<PBS.Battle.View.Compact.Pokemon> { pokemon });
        }
        public string GetPokemonNames(List<PBS.Battle.View.Compact.Pokemon> pokemonList, bool orConjunct = false)
        {
            string conjunct = (orConjunct) ? "or" : "and";

            string names = "";
            if (pokemonList.Count == 1)
            {
                return pokemonList[0].nickname;
            }
            else if (pokemonList.Count == 2)
            {
                return pokemonList[0].nickname + " " + conjunct + " " + pokemonList[1].nickname;
            }
            else
            {
                for (int i = 0; i < pokemonList.Count; i++)
                {
                    names += (i == pokemonList.Count - 1) ?
                        conjunct + " " + pokemonList[i].nickname :
                        pokemonList[i].nickname + ", ";
                }
            }
            return names;
        }
        public string GetTrainerNames(List<PBS.Battle.View.Compact.Trainer> trainers)
        {
            string text = "";
            for (int i = 0; i < trainers.Count; i++)
            {
                text += (i == 0)? trainers[i].name : " and " + trainers[i].name;
            }
            return text;
        }

        public static string ConvertStatToString(PokemonStats stat, bool capitalize = true)
        {
            return (stat == PokemonStats.Attack) ? "Attack"
                : (stat == PokemonStats.Defense) ? "Defense"
                : (stat == PokemonStats.SpecialAttack) ? "Special Attack"
                : (stat == PokemonStats.SpecialDefense) ? "Special Defense"
                : (stat == PokemonStats.Speed) ? "Speed"
                : (stat == PokemonStats.Accuracy) ? "Accuracy"
                : (stat == PokemonStats.Evasion) ? "Evasion"
                : "HP";
        }
        public static string ConvertStatsToString(PokemonStats[] statList, bool capitalize = true)
        {
            if (statList.Length == 7)
            {
                string s = "Stats";
                s = (capitalize) ? s : s.ToLower();
                return s;
            }

            string text = "";
            if (statList.Length == 1)
            {
                return ConvertStatToString(statList[0], capitalize);
            }
            else if (statList.Length == 2)
            {
                return ConvertStatToString(statList[0], capitalize) 
                    + " and " 
                    + ConvertStatToString(statList[1], capitalize);
            }
            else
            {
                for (int i = 0; i < statList.Length; i++)
                {
                    text += (i == statList.Length - 1) ? "and " + ConvertStatToString(statList[i], capitalize) 
                        : ConvertStatToString(statList[i], capitalize) + ", ";
                }
            }
            return text;
        }

        public static string ConvertTypesToString(List<string> typeIDs)
        {
            string names = "";
            if (typeIDs.Count == 1)
            {
                return TypeDatabase.instance.GetTypeData(typeIDs[0]).typeName + "-type";
            }
            else if (typeIDs.Count == 2)
            {
                return TypeDatabase.instance.GetTypeData(typeIDs[0]).typeName 
                    + "- and " 
                    + TypeDatabase.instance.GetTypeData(typeIDs[1]).typeName + "-type";
            }
            else
            {
                for (int i = 0; i < typeIDs.Count; i++)
                {
                    names += (i == typeIDs.Count - 1) ?
                        "and " + TypeDatabase.instance.GetTypeData(typeIDs[i]).typeName + "-type" :
                        TypeDatabase.instance.GetTypeData(typeIDs[i]).typeName + "-, ";
                }
            }
            return names;
        }

        public static string RenderMessageTrainer(
            int playerID, 
            PBS.Battle.View.Model myModel,
            int teamPerspectiveID = -1, 
            string baseString = "",
            int myPlayerID = 0,
            PBS.Battle.View.Compact.Trainer myTrainer = null,
            PBS.Battle.View.Compact.Team myTeamPerspective = null)
        {
            if (teamPerspectiveID == -1)
            {
                teamPerspectiveID = myTeamPerspective.teamID;
            }
            PBS.Battle.View.Compact.Trainer trainer = myModel.GetMatchingTrainer(playerID);
            GameTextData textData = 
                (trainer.teamPos != teamPerspectiveID)? GameTextDatabase.instance.GetGameTextData("trainer-perspective-opposing")
                : (myTrainer == null)? GameTextDatabase.instance.GetGameTextData("trainer-perspective-ally")
                : GameTextDatabase.instance.GetGameTextData("trainer-perspective-player");

            string replaceString = textData.languageDict[GameSettings.language];
            string replaceStringPoss = replaceString;
            if (!string.IsNullOrEmpty(baseString))
            {
                if (GameSettings.language == GameLanguages.English && myPlayerID == playerID)
                {
                    if (!baseString.StartsWith("{{-trainer-"))
                    {
                        replaceString = replaceString.ToLower();
                        replaceStringPoss = replaceStringPoss.ToLower();
                    }
                }
            }
           
            string newString = baseString;
            newString = newString.Replace("{{-trainer-}}", replaceString);
            newString = newString.Replace("{{-trainer-poss-}}", replaceStringPoss);

            return newString;
        }
        public static string RenderMessageTeam(
            int teamID,
            int teamPerspectiveID = -1, 
            string baseString = "",
            int myPlayerID = 0,
            PBS.Battle.View.Compact.Trainer myTrainer = null,
            PBS.Battle.View.Compact.Team myTeamPerspective = null)
        {
            if (teamPerspectiveID == -1)
            {
                teamPerspectiveID = myTeamPerspective.teamID;
            }
            GameTextData textData = 
                (teamID != teamPerspectiveID)? GameTextDatabase.instance.GetGameTextData("team-perspective-opposing")
                : (myTrainer == null)? GameTextDatabase.instance.GetGameTextData("team-perspective-ally")
                : GameTextDatabase.instance.GetGameTextData("team-perspective-player");

            string teamString = textData.languageDict[GameSettings.language];
            if (!string.IsNullOrEmpty(baseString))
            {
                if (GameSettings.language == GameLanguages.English)
                {
                    if (!baseString.StartsWith("{{-target-team-"))
                    {
                        teamString = teamString.ToLower();
                    }
                }
            }
            string newString = baseString;
            newString = newString.Replace("{{-target-team-}}", teamString);
            newString = newString.Replace("{{-target-team-poss-}}", teamString
                + (teamString.EndsWith("s") ? "'" : "'s")
                );

            return newString;
        }
        public static string RenderMessage(
            PBS.Battle.View.Events.MessageParameterized message,
            PBS.Battle.View.Model myModel,
            int myPlayerID = 0,
            PBS.Battle.View.Compact.Trainer myTrainer = null,
            PBS.Battle.View.Compact.Team myTeamPerspective = null)
        {
            GameTextData textData = GameTextDatabase.instance.GetGameTextData(message.messageCode);
            if (textData == null)
            {
                return "";
            }
            string baseString = textData.languageDict[GameSettings.language];
            string newString = baseString;

            PBS.Battle.View.Compact.Trainer trainerPerspective = 
                (myTrainer == null)? myModel.GetMatchingTrainer(message.playerPerspectiveID)
                : myTrainer;
            PBS.Battle.View.Compact.Team teamPerspective = 
                (myTeamPerspective == null)? myModel.GetMatchingTeam(message.teamPerspectiveID)
                : myTeamPerspective;

            // player
            newString = newString.Replace("{{-player-name-}}", PlayerSave.instance.name);

            if (!string.IsNullOrEmpty(message.pokemonID))
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonID);
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonUserID))
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonUserID);
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-user-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-user-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-user-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonTargetID))
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonTargetID);
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-target-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-target-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-target-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (message.pokemonListIDs.Count > 0)
            {
                List<PBS.Battle.View.Compact.Pokemon> pokemonList = new List<Battle.View.Compact.Pokemon>();
                for (int i = 0; i < message.pokemonListIDs.Count; i++)
                {
                    pokemonList.Add(myModel.GetMatchingPokemon(message.pokemonListIDs[i]));
                }
                string pokemonNameList = GetPokemonNames(pokemonList, myModel);
                newString = newString.Replace("{{-pokemon-list-}}", pokemonNameList);
            }

            if (message.trainerID != 0)
            {
                newString = RenderMessageTrainer(
                    message.trainerID, 
                    myModel,
                    teamPerspective.teamID, 
                    newString,
                    
                    myPlayerID: myPlayerID,
                    myTrainer: myTrainer,
                    myTeamPerspective: myTeamPerspective);
            }
            
            if (message.teamID != 0)
            {
                newString = RenderMessageTeam(
                    teamID: message.teamID,
                    teamPerspectiveID: teamPerspective.teamID, 
                    baseString: newString,

                    myPlayerID: myPlayerID,
                    myTrainer: myTrainer,
                    myTeamPerspective: myTeamPerspective);
            }

            if (!string.IsNullOrEmpty(message.typeID))
            {
                TypeData typeData = TypeDatabase.instance.GetTypeData(message.typeID);
                newString = newString.Replace("{{-type-name-}}", typeData.typeName + "-type");
            }
            if (message.typeIDs.Count > 0)
            {
                newString = newString.Replace("{{-type-list-}}", GameTextDatabase.ConvertTypesToString(message.typeIDs.ToArray()));
            }

            if (!string.IsNullOrEmpty(message.moveID))
            {
                MoveData moveData = MoveDatabase.instance.GetMoveData(message.moveID);
                newString = newString.Replace("{{-move-name-}}", moveData.moveName);
            }
            if (message.moveIDs.Count > 0)
            {
                for (int i = 0; i < message.moveIDs.Count; i++)
                {
                    MoveData moveXData = MoveDatabase.instance.GetMoveData(message.moveIDs[i]);
                    string partToReplace = "{{-move-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, moveXData.moveName);
                }
            }

            if (!string.IsNullOrEmpty(message.abilityID))
            {
                AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(message.abilityID);
                newString = newString.Replace("{{-ability-name-}}", abilityData.abilityName);
            }
            if (message.abilityIDs.Count > 0)
            {
                for (int i = 0; i < message.abilityIDs.Count; i++)
                {
                    AbilityData abilityXData = AbilityDatabase.instance.GetAbilityData(message.abilityIDs[i]);
                    string partToReplace = "{{-ability-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, abilityXData.abilityName);
                }
            }

            if (!string.IsNullOrEmpty(message.itemID))
            {
                ItemData itemData = ItemDatabase.instance.GetItemData(message.itemID);
                newString = newString.Replace("{{-item-name-}}", itemData.itemName);
            }

            if (!string.IsNullOrEmpty(message.statusID))
            {
                StatusPKData statusData = StatusPKDatabase.instance.GetStatusData(message.statusID);
                newString = newString.Replace("{{-status-name-}}", statusData.conditionName);
            }
            if (!string.IsNullOrEmpty(message.statusTeamID))
            {
                StatusTEData statusData = StatusTEDatabase.instance.GetStatusData(message.statusTeamID);
                newString = newString.Replace("{{-team-status-name-}}", statusData.conditionName);
            }
            if (!string.IsNullOrEmpty(message.statusEnvironmentID))
            {
                StatusBTLData statusData = StatusBTLDatabase.instance.GetStatusData(message.statusEnvironmentID);
                newString = newString.Replace("{{-battle-status-name-}}", statusData.conditionName);
            }

            // swapping substrings
            for (int i = 0; i < message.intArgs.Count; i++)
            {
                string partToReplace = "{{-int-" + i + "-}}";
                newString = newString.Replace(partToReplace, message.intArgs[i].ToString());
            }

            if (message.statList.Count > 0)
            {
                newString = newString.Replace("{{-stat-types-}}", ConvertStatsToString(message.statList.ToArray()));
                if (GameSettings.language == GameLanguages.English)
                {
                    newString = newString.Replace("{{-stat-types-was-}}", (message.statList.Count == 1)? "was" : "were");
                }
                else
                {
                    newString = newString.Replace("{{-stat-types-was-}}", "");
                }
                newString = newString.Replace("{{-stat-types-LC-}}", ConvertStatsToString(message.statList.ToArray(), false));
            }

            return newString;
        }
    }
}

