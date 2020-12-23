using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI
{
    public class Canvas : MonoBehaviour
    {
        #region Attributes
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
        #endregion

        #region Unity
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
        #endregion

        // Panel Management
        public void SwitchPanel(Enums.Panel newPanel)
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
        public void SetCommands(WifiFriendly.Pokemon pokemon, IEnumerable<BattleCommandType> commandList)
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
            WifiFriendly.Pokemon pokemon, 
            List<Events.CommandAgent.Moveslot> moveslots, 
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
            WifiFriendly.Pokemon pokemon, 
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
        public void SetFieldTargets(Model model, int teamPos)
        {
            fieldTargetPanel.SetFieldTargets(teamPos: teamPos, battleModel: model);
        }
        public void SwitchSelectedMoveTargetsTo(
            Model model,
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
        public void SetParty(List<WifiFriendly.Pokemon> pokemon, bool forceSwitch = false, Item item = null)
        {
            List<PBS.Battle.View.WifiFriendly.Pokemon> filteredPokemon = new List<PBS.Battle.View.WifiFriendly.Pokemon>();
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
        public void SwitchSelectedPartyMemberTo(WifiFriendly.Pokemon selected)
        {
            partyPanel.HighlightPokemon(selected.uniqueID);
        }
        public void SwitchSelectedPartyMemberToBack()
        {
            partyPanel.HighlightBackButton();
        }

        // Party Commands
        public void SetPartyCommands(WifiFriendly.Pokemon pokemon, List<BattleExtraCommand> commands)
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
        public void SetItems(WifiFriendly.Trainer trainer, ItemBattlePocket pocket, List<Item> list, int offset)
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
        public HUD.PokemonHUD DrawPokemonHUD(WifiFriendly.Pokemon pokemon, TeamMode teamMode, bool isNear)
        {
            return HUDPanel.DrawPokemonHUD(pokemon, teamMode, isNear);
        }
        public bool UndrawPokemonHUD(WifiFriendly.Pokemon pokemon)
        {
            return HUDPanel.UndrawPokemonHUD(pokemon);
        }

        public HUD.PokemonHUD GetPokemonHUD(WifiFriendly.Pokemon pokemon)
        {
            return HUDPanel.GetPokemonHUD(pokemon);
        }
        public void UpdatePokemonHUD(WifiFriendly.Pokemon pokemon)
        {
            HUDPanel.UpdatePokemonHUD(pokemon);
        }

        public void SetPokemonHUDActive(WifiFriendly.Pokemon pokemon, bool active)
        {
            HUDPanel.SetPokemonHUDActive(pokemon, active);
        }
        public void SetPokemonHUDsActive(bool active)
        {
            HUDPanel.SetPokemonHUDsActive(active);
        }

        public IEnumerator AnimatePokemonHUDHPChange(WifiFriendly.Pokemon pokemon, int preHP, int postHP, int maxHP, float timeSpan = 1f)
        {
            yield return StartCoroutine(HUDPanel.AnimatePokemonHUDHPChange(
                pokemon: pokemon,
                preHP: preHP, 
                postHP: postHP,
                maxHP: maxHP,
                timeSpan: timeSpan
                ));
        }

        #region Dialog

        /// <summary>
        /// Removes the dialog box from view.
        /// </summary>
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
        #endregion

        #region In-Game Text Rendering

        #region Names
        /// <summary>
        /// Returns the display name of the given pokemon in the given battle.
        /// </summary>
        /// <param name="pokemon">The pokemon whose name to retrieve.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <returns></returns>
        public static string GetPokemonName(WifiFriendly.Pokemon pokemon, Model myModel)
        {
            return GetPokemonNames(new List<PBS.Battle.View.WifiFriendly.Pokemon> { pokemon }, myModel);
        }
        /// <summary>
        /// Returns the display names of the given pokemon in the given battle.
        /// </summary>
        /// <param name="pokemonList">The pokemon whose names will be retrieved.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="conjunct">Whether to use "Or" (True) or "And" (False).</param>
        /// <returns></returns>
        public static string GetPokemonNames(List<WifiFriendly.Pokemon> pokemonList, Model myModel, bool conjunct = false)
        {
            string conjunctString = (conjunct) ? "or" : "and";

            string names = "";
            if (pokemonList.Count == 1)
            {
                return pokemonList[0].nickname;
            }
            else if (pokemonList.Count == 2)
            {
                return pokemonList[0].nickname + " " + conjunctString + " " + pokemonList[1].nickname;
            }
            else
            {
                for (int i = 0; i < pokemonList.Count; i++)
                {
                    names += (i == pokemonList.Count - 1) ?
                        conjunctString + " " + pokemonList[i].nickname :
                        pokemonList[i].nickname + ", ";
                }
            }
            return names;
        }
        /// <summary>
        /// Returns the display names for the given trainers.
        /// </summary>
        /// <param name="trainers">The trainers whose names will be retrieved.</param>
        /// <returns></returns>
        public static string GetTrainerNames(List<WifiFriendly.Trainer> trainers)
        {
            string text = "";
            for (int i = 0; i < trainers.Count; i++)
            {
                text += (i == 0)? trainers[i].name : " and " + trainers[i].name;
            }
            return text;
        }
        #endregion

        #region View Perspective
        /// <summary>
        /// Returns the view perspective on the given pokemon.
        /// </summary>
        /// <param name="pokemon">The pokemon for whom to retrieve perspective on.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="teamPerspectiveID">The team for whom to retrieve the perspective on.</param>
        /// <param name="myPlayerID">The player for whom to retrieve the perspective on.</param>
        /// <returns></returns>
        public static Enums.ViewPerspective GetPerspective(
            WifiFriendly.Pokemon pokemon,
            Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0)
        {
            WifiFriendly.Trainer trainer = myModel.GetTrainer(pokemon);
            WifiFriendly.Team team = myModel.GetTeamOfTrainer(trainer);
            if (team.teamID != teamPerspectiveID)
            {
                return Enums.ViewPerspective.Enemy;
            }
            else
            {
                if (myPlayerID == 0)
                {
                    return Enums.ViewPerspective.Ally;
                }
                return Enums.ViewPerspective.Player;
            }
        }
        /// <summary>
        /// Returns the view perspective on the given pokemon.
        /// </summary>
        /// <param name="pokemon">The pokemon for whom to retrieve perspective on.</param>
        /// <param name="viewPerspective">The pokemon for whom to retrieve perspective on.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="teamPerspectiveID">The team for whom to retrieve the perspective on.</param>
        /// <param name="myPlayerID">The player for whom to retrieve the perspective on.</param>
        /// <returns></returns>
        public static List<WifiFriendly.Pokemon> FilterPokemonByPerspective(
            List<WifiFriendly.Pokemon> pokemon,
            Enums.ViewPerspective viewPerspective,
            Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0
            )
        {
            List<WifiFriendly.Pokemon> filteredPokemon = new List<WifiFriendly.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                Enums.ViewPerspective perspective = GetPerspective(
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
        /// <summary>
        /// Returns text dependent on the given view perspective.
        /// </summary>
        /// <param name="viewPerspective">The given view perspective.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="lowercase">If true, the text is forced to lowercase.</param>
        /// <returns></returns>
        public static string GetPrefix(
            Enums.ViewPerspective viewPerspective,
            Model myModel,
            bool lowercase = false)
        {
            string prefix = (viewPerspective == Enums.ViewPerspective.Ally) ? "The ally "
                : (viewPerspective == Enums.ViewPerspective.Enemy) ? 
                    (myModel.settings.isWildBattle? "The wild " : "The foe's ")
                : "";
            if (lowercase)
            {
                prefix = prefix.ToLower();
            }

            return prefix;
        }
        /// <summary>
        ///  Returns prefix text dependent on the given pokemon.
        /// </summary>
        /// <param name="pokemon">The pokemon for whom to retrieve prefix text on.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="teamPerspectiveID">The team for whom to retrieve the perspective on.</param>
        /// <param name="myPlayerID">The player for whom to retrieve the perspective on.</param>
        /// <param name="lowercase">If true, the text is forced to lowercase.</param>
        /// <returns></returns>
        public static string GetPrefix(
            WifiFriendly.Pokemon pokemon,
            Model myModel,
            int teamPerspectiveID = -1,
            int myPlayerID = 0,
            bool lowercase = false)
        {
            string text = "";
            WifiFriendly.Trainer trainer = myModel.GetTrainer(pokemon);
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
            if (lowercase)
            {
                text = text.ToLower();
                text = " " + text;
            }
            return text;
        }
        #endregion

        #region Message Rendering
        /// <summary>
        /// Returns the text-name of the given stat.
        /// </summary>
        /// <param name="stat">The stat whose name to retrieve.</param>
        /// <param name="capitalize">If true, all letters are capitalized.</param>
        /// <returns></returns>
        public static string ConvertStatToString(PokemonStats stat, bool capitalize = true)
        {
            string statString = (stat == PokemonStats.Attack) ? "Attack"
                : (stat == PokemonStats.Defense) ? "Defense"
                : (stat == PokemonStats.SpecialAttack) ? "Special Attack"
                : (stat == PokemonStats.SpecialDefense) ? "Special Defense"
                : (stat == PokemonStats.Speed) ? "Speed"
                : (stat == PokemonStats.Accuracy) ? "Accuracy"
                : (stat == PokemonStats.Evasion) ? "Evasion"
                : "HP";
            if (capitalize)
            {
                statString = statString.ToUpper();
            }
            return statString;
        }
        /// <summary>
        /// Returns the text-names of the given stats.
        /// </summary>
        /// <param name="statList">The stats whos names to retrieve.</param>
        /// <param name="capitalize">If true, all letters are capitalized.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the text-names of the given types.
        /// </summary>
        /// <param name="typeIDs">The types to retrieve the names of.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns formatted text for the given trainer.
        /// </summary>
        /// <param name="playerID">The ID of the trainer to evaluate.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="teamPerspectiveID">The team for whom to retrieve the perspective on.</param>
        /// <param name="baseString">A base string to format, given the trainer.</param>
        /// <param name="myPlayerID">The ID of the player for whom the text is evaluated for.</param>
        /// <param name="myTrainer">The ID of the trainer for whom the text is evaluated for.</param>
        /// <param name="myTeamPerspective">The team for whom the text is evaluated for.</param>
        /// <returns></returns>
        public static string RenderMessageTrainer(
            int playerID, 
            Model myModel,
            int teamPerspectiveID = -1, 
            string baseString = "",
            int myPlayerID = 0,
            WifiFriendly.Trainer myTrainer = null,
            WifiFriendly.Team myTeamPerspective = null)
        {
            if (teamPerspectiveID == -1)
            {
                teamPerspectiveID = myTeamPerspective.teamID;
            }
            WifiFriendly.Trainer trainer = myModel.GetMatchingTrainer(playerID);
            GameTextData textData = 
                (trainer.teamPos != teamPerspectiveID)? 
                GameTextDatabase.instance.GetGameTextData("trainer-perspective-opposing")
                : (myTrainer == null)? GameTextDatabase.instance.GetGameTextData("trainer-perspective-ally")
                : GameTextDatabase.instance.GetGameTextData("trainer-perspective-player");

            string replaceString = textData.languageDict[GameSettings.language];
            if (replaceString == "{{-trainer-}}")
            {
                replaceString = trainer.name;
            }
            string replaceStringPoss = replaceString;
            if (!string.IsNullOrEmpty(baseString))
            {
                if (GameSettings.language == GameLanguages.English)
                {
                    if (myPlayerID == playerID)
                    {
                        replaceStringPoss = "Your";
                        if (!baseString.StartsWith("{{-trainer-"))
                        {
                            replaceString = replaceString.ToLower();
                            replaceStringPoss = replaceStringPoss.ToLower();
                        }
                    }
                    else
                    {
                        replaceStringPoss += "'s";
                    }
                }
            }
           
            string newString = baseString;
            newString = newString.Replace("{{-trainer-}}", replaceString);
            newString = newString.Replace("{{-trainer-poss-}}", replaceStringPoss);

            return newString;
        }
        /// <summary>
        /// Returns formatted text for the given team.
        /// </summary>
        /// <param name="teamID">The ID of the team to evaluate.</param>
        /// <param name="teamPerspectiveID">The team for whom to retrieve the perspective on.</param>
        /// <param name="baseString">A base string to format, given the trainer.</param>
        /// <param name="myPlayerID">The ID of the player for whom the text is evaluated for.</param>
        /// <param name="myTrainer">The ID of the trainer for whom the text is evaluated for.</param>
        /// <param name="myTeamPerspective">The team for whom the text is evaluated for.</param>
        /// <returns></returns>
        public static string RenderMessageTeam(
            int teamID,
            int teamPerspectiveID = -1, 
            string baseString = "",
            int myPlayerID = 0,
            WifiFriendly.Trainer myTrainer = null,
            WifiFriendly.Team myTeamPerspective = null)
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
        /// <summary>
        /// Returns formatted text given a text-code.
        /// </summary>
        /// <param name="message">The message code.</param>
        /// <param name="myModel">The battle model to evaluate.</param>
        /// <param name="myPlayerID">The ID of the player for whom the text is evaluated for.</param>
        /// <param name="myTrainer">The ID of the trainer for whom the text is evaluated for.</param>
        /// <param name="myTeamPerspective">The team for whom the text is evaluated for.</param>
        /// <returns></returns>
        public static string RenderMessage(
            Events.MessageParameterized message,
            Model myModel,
            int myPlayerID = 0,
            WifiFriendly.Trainer myTrainer = null,
            WifiFriendly.Team myTeamPerspective = null)
        {
            GameTextData textData = GameTextDatabase.instance.GetGameTextData(message.messageCode);
            if (textData == null)
            {
                return "";
            }
            string baseString = textData.languageDict[GameSettings.language];
            string newString = baseString;

            WifiFriendly.Trainer trainerPerspective = 
                (myTrainer == null)? myModel.GetMatchingTrainer(message.playerPerspectiveID)
                : myTrainer;
            WifiFriendly.Team teamPerspective = 
                (myTeamPerspective == null)? myModel.GetMatchingTeam(message.teamPerspectiveID)
                : myTeamPerspective;

            // player
            newString = newString.Replace("{{-player-name-}}", PlayerSave.instance.name);

            if (!string.IsNullOrEmpty(message.pokemonID))
            {
                WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonID);
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonUserID))
            {
                WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonUserID);
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-user-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-user-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-user-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonTargetID))
            {
                WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonTargetID);
                PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-target-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-target-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-target-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (message.pokemonListIDs.Count > 0)
            {
                List<WifiFriendly.Pokemon> pokemonList = new List<Battle.View.WifiFriendly.Pokemon>();
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
        #endregion

        #endregion
    }
}

