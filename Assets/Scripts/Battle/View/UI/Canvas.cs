using PBS.Databases;
using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI
{
    /// <summary>
    /// The canvas handles orchestrating the battle UI and its components. This includes the command selection UI,
    /// dialog boxes, health bars, and other UI components.
    /// </summary>
    public class Canvas : MonoBehaviour
    {
        #region Attributes
        [Header("Panels")]
        [Tooltip("The battle command UI panel.")]
        public Panels.Command cmdPanel;
        [Tooltip("The move selection UI panel.")]
        public Panels.Fight fightPanel;
        [Tooltip("The move targeting UI panel.")]
        public Panels.FieldTarget fieldTargetPanel;
        [Tooltip("The party member UI panel.")]
        public Panels.Party partyPanel;
        [Tooltip("The bag pocket UI panel.")]
        public Panels.Bag bagPanel;
        [Tooltip("The items UI panel.")]
        public Panels.BagItem bagItemPanel;
        [Tooltip("The HUD UI panel.")]
        public HUD.Panel HUDPanel;

        [Tooltip("The dialog box.")]
        public Dialog dialog;

        /// <summary>
        /// The currently selected panel type.
        /// </summary>
        [HideInInspector] public Enums.Panel panelType;
        /// <summary>
        /// The currently drawn panel on the UI.
        /// </summary>
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
            HidePanels();
            dialog.gameObject.SetActive(true);
            dialog.ClearBox();
        }
        #endregion

        #region Panel Management
        /// <summary>
        /// Switches the UI panel to the selected panel.
        /// </summary>
        /// <param name="newPanel">The panel to switch to.</param>
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
        /// <summary>
        /// Hides all currently displayed UI panels.
        /// </summary>
        public void HidePanels()
        {
            cmdPanel.gameObject.SetActive(false);
            fightPanel.gameObject.SetActive(false);
            fieldTargetPanel.gameObject.SetActive(false);
            partyPanel.gameObject.SetActive(false);
            bagPanel.gameObject.SetActive(false);
            SwitchPanel(Enums.Panel.None);
        }

        #region Command Panel
        /// <summary>
        /// Sets the available commands for the given Pokemon in the UI.
        /// </summary>
        /// <param name="pokemon">The pokemon for whom to display commands.</param>
        /// <param name="commandList">The commands to be displayed.</param>
        public void SetCommands(WifiFriendly.Pokemon pokemon, IEnumerable<BattleCommandType> commandList)
        {
            HashSet<BattleCommandType> commandSet = new HashSet<BattleCommandType>(commandList);
            cmdPanel.SetCommands(pokemon, commandList);
        }
        /// <summary>
        /// Switch to displaying the selected command.
        /// </summary>
        /// <param name="commandType"></param>
        public void SwitchSelectedCommandTo(BattleCommandType commandType)
        {
            cmdPanel.HighlightCommand(commandType);
        }
        #endregion

        #region Fight Panel
        /// <summary>
        /// Sets the available moves for the given Pokemon in the UI.
        /// </summary>
        /// <param name="pokemon">The Pokemon for whom to display moves.</param>
        /// <param name="moveslots">The list of moves to display.</param>
        /// <param name="canMegaEvolve">If true, the UI displays the option to mega-evolve.</param>
        /// <param name="canZMove">If true, the UI displays the option to use Z-moves.</param>
        /// <param name="canDynamax">If true, the UI displays the option to dynamax.</param>
        /// <param name="choosingZMove">If true, the UI displays the user as currently selecting a Z-move.</param>
        /// <param name="choosingMaxMove">If true, the UI displays the user as currently selecting a max move.</param>
        public void SetMoves(
            WifiFriendly.Pokemon pokemon, 
            List<Events.CommandAgent.Moveslot> moveslots, 
            bool canMegaEvolve, bool canZMove = false, bool canDynamax = false,
            bool choosingZMove = false, bool choosingMaxMove = false)
        {
            List<Events.CommandAgent.Moveslot> filteredMoveslots 
                = new List<Events.CommandAgent.Moveslot>(moveslots);
            for (int i = 0; i < filteredMoveslots.Count; i++)
            {
                Events.CommandAgent.Moveslot moveslot = filteredMoveslots[i];
                if (moveslot == null)
                {
                    filteredMoveslots.RemoveAt(i);
                    i--;
                }
            }
            fightPanel.SetMoves(
                moveList: filteredMoveslots,
                canMegaEvolve: canMegaEvolve, canZMove: canZMove, canDynamax: canDynamax);
        }
        /// <summary>
        /// Switch to displaying the selected move.
        /// </summary>
        /// <param name="pokemon">The Pokemon for whom to display moves.</param>
        /// <param name="selected">The index of the selected move.</param>
        /// <param name="choosingSpecial">If true, the UI displays the user as currently selecting a special action.</param>
        /// <param name="choosingZMove">If true, the UI displays the user as currently selecting a Z-move.</param>
        /// <param name="choosingMaxMove">If true, the UI displays the user as currently selecting a max move.</param>
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
        /// <summary>
        /// Switch to displaying the back button on the Fight UI.
        /// </summary>
        public void SwitchSelectedMoveToBack()
        {
            fightPanel.HighlightBackButton();
        }
        #endregion

        #region Field Targeting Panel
        /// <summary>
        /// Sets the UI components for displaying move targets.
        /// </summary>
        /// <param name="model">The battle model to evaluate.</param>
        /// <param name="teamPos">The team perspective to use.</param>
        public void SetFieldTargets(Model model, int teamPos)
        {
            fieldTargetPanel.SetFieldTargets(teamPerspective: teamPos, battleModel: model);
        }
        /// <summary>
        /// Switches to displaying the selected target group.
        /// </summary>
        /// <param name="model">The battle model to evaluate.</param>
        /// <param name="userPos">The position of the user Pokemon.</param>
        /// <param name="chooseIndex">The selected target group.</param>
        /// <param name="choices">The list of possible target groups.</param>
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
        #endregion

        #region Party Panel
        /// <summary>
        /// Sets the UI components for displaying party members.
        /// </summary>
        /// <param name="pokemon">The list of party members to display.</param>
        /// <param name="forceSwitch">If true, display on the UI that this Pokemon is forced to switch.</param>
        public void SetParty(List<WifiFriendly.Pokemon> pokemon, bool forceSwitch = false)
        {
            List<WifiFriendly.Pokemon> filteredPokemon = new List<WifiFriendly.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                if (pokemon[i] != null)
                {
                    filteredPokemon.Add(pokemon[i]);
                }
            }

            partyPanel.SetParty(party: filteredPokemon);
            partyPanel.backBtn.gameObject.SetActive(!forceSwitch);
        }
        /// <summary>
        /// Switch to displaying the selected party member.
        /// </summary>
        /// <param name="selected">The selected party member.</param>
        public void SwitchSelectedPartyMemberTo(WifiFriendly.Pokemon selected)
        {
            partyPanel.HighlightPokemon(selected.uniqueID);
        }
        /// <summary>
        /// Switch to displaying the back button on the Party UI.
        /// </summary>
        public void SwitchSelectedPartyMemberToBack()
        {
            partyPanel.HighlightBackButton();
        }
        #endregion

        #region Party Commands
        /// <summary>
        /// Sets the UI components for displaying party commands.
        /// </summary>
        /// <param name="pokemon">The party member to display commands for.</param>
        /// <param name="commands">The party commands to display.</param>
        public void SetPartyCommands(WifiFriendly.Pokemon pokemon, List<BattleExtraCommand> commands)
        {
            partyPanel.SetCommands(commands);
        }
        /// <summary>
        /// Switch to displaying the selected party command.
        /// </summary>
        /// <param name="selected">The selected party command.</param>
        public void SwitchSelectedPartyCommandTo(BattleExtraCommand selected)
        {
            partyPanel.HighlightCommand(selected);
        }
        #endregion

        #region Bag Panel
        /// <summary>
        /// Sets the UI componenets for displaying bag pockets.
        /// </summary>
        /// <param name="list">The bag pockets to display.</param>
        public void SetBagPockets(List<ItemBattlePocket> list)
        {
            bagPanel.SetPockets(list);
        }
        /// <summary>
        /// Switch to displaying the selected bag pocket.
        /// </summary>
        /// <param name="selected">The selected bag pocket.</param>
        public void SwitchSelectedBagPocketTo(ItemBattlePocket selected)
        {
            bagPanel.HighlightPocket(selected);
        }
        /// <summary>
        /// Switch to displaying the back button on the Bag Pocket UI.
        /// </summary>
        public void SwitchSelectedBagPocketToBack()
        {
            bagPanel.HighlightBackButton();
        }
        #endregion

        #region Bag Item Panel
        /// <summary>
        /// Sets the UI components for displaying items.
        /// </summary>
        /// <param name="trainer">The trainer for whom the items are displayed for.</param>
        /// <param name="pocket">The bag pocket the items are in.</param>
        /// <param name="items">The list of items in the bag pocket.</param>
        /// <param name="offset">The offset in the item list to display items.</param>
        public void SetItems(WifiFriendly.Trainer trainer, ItemBattlePocket pocket, List<Item> items, int offset)
        {
            List<Item> filteredItems = new List<Item>();
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    filteredItems.Add(items[i]);
                }
            }
            bagItemPanel.SetItems(trainer, filteredItems, offset);
            bagItemPanel.backBtn.gameObject.SetActive(true);
        }
        /// <summary>
        /// Switch to displaying the selected item.
        /// </summary>
        /// <param name="selected">The item to select.</param>
        public void SwitchSelectedItemTo(Item selected)
        {
            bagItemPanel.HighlightButton(selected.itemID);
        }
        /// <summary>
        /// Switch to displaying the back button on the Bag UI.
        /// </summary>
        public void SwitchSelectedItemToBack()
        {
            bagItemPanel.HighlightBackButton();
        }
        #endregion

        #endregion

        #region HUD
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
        #endregion

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
                return ElementalTypes.instance.GetTypeData(typeIDs[0]).typeName + "-type";
            }
            else if (typeIDs.Count == 2)
            {
                return ElementalTypes.instance.GetTypeData(typeIDs[0]).typeName 
                    + "- and " 
                    + ElementalTypes.instance.GetTypeData(typeIDs[1]).typeName + "-type";
            }
            else
            {
                for (int i = 0; i < typeIDs.Count; i++)
                {
                    names += (i == typeIDs.Count - 1) ?
                        "and " + ElementalTypes.instance.GetTypeData(typeIDs[i]).typeName + "-type" :
                        ElementalTypes.instance.GetTypeData(typeIDs[i]).typeName + "-, ";
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
                GameText.instance.GetGameTextData("trainer-perspective-opposing")
                : (myTrainer == null)? GameText.instance.GetGameTextData("trainer-perspective-ally")
                : GameText.instance.GetGameTextData("trainer-perspective-player");

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
                (teamID != teamPerspectiveID)? GameText.instance.GetGameTextData("team-perspective-opposing")
                : (myTrainer == null)? GameText.instance.GetGameTextData("team-perspective-ally")
                : GameText.instance.GetGameTextData("team-perspective-player");

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
            GameTextData textData = GameText.instance.GetGameTextData(message.messageCode);
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
                PokemonData pokemonData = Pokemon.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonUserID))
            {
                WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonUserID);
                PokemonData pokemonData = Pokemon.instance.GetPokemonData(pokemon.pokemonID);
                newString = newString.Replace("{{-user-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-user-pokemon-form-}}", pokemonData.formName);
                newString = newString.Replace("{{-user-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonTargetID))
            {
                WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonTargetID);
                PokemonData pokemonData = Pokemon.instance.GetPokemonData(pokemon.pokemonID);
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
                TypeData typeData = ElementalTypes.instance.GetTypeData(message.typeID);
                newString = newString.Replace("{{-type-name-}}", typeData.typeName + "-type");
            }
            if (message.typeIDs.Count > 0)
            {
                newString = newString.Replace("{{-type-list-}}", GameText.ConvertTypesToString(message.typeIDs.ToArray()));
            }

            if (!string.IsNullOrEmpty(message.moveID))
            {
                MoveData moveData = Moves.instance.GetMoveData(message.moveID);
                newString = newString.Replace("{{-move-name-}}", moveData.moveName);
            }
            if (message.moveIDs.Count > 0)
            {
                for (int i = 0; i < message.moveIDs.Count; i++)
                {
                    MoveData moveXData = Moves.instance.GetMoveData(message.moveIDs[i]);
                    string partToReplace = "{{-move-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, moveXData.moveName);
                }
            }

            if (!string.IsNullOrEmpty(message.abilityID))
            {
                AbilityData abilityData = Abilities.instance.GetAbilityData(message.abilityID);
                newString = newString.Replace("{{-ability-name-}}", abilityData.abilityName);
            }
            if (message.abilityIDs.Count > 0)
            {
                for (int i = 0; i < message.abilityIDs.Count; i++)
                {
                    AbilityData abilityXData = Abilities.instance.GetAbilityData(message.abilityIDs[i]);
                    string partToReplace = "{{-ability-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, abilityXData.abilityName);
                }
            }

            if (!string.IsNullOrEmpty(message.itemID))
            {
                ItemData itemData = Items.instance.GetItemData(message.itemID);
                newString = newString.Replace("{{-item-name-}}", itemData.itemName);
            }

            if (!string.IsNullOrEmpty(message.statusID))
            {
                StatusPKData statusData = PokemonStatuses.instance.GetStatusData(message.statusID);
                newString = newString.Replace("{{-status-name-}}", statusData.conditionName);
            }
            if (!string.IsNullOrEmpty(message.statusTeamID))
            {
                StatusTEData statusData = TeamStatuses.instance.GetStatusData(message.statusTeamID);
                newString = newString.Replace("{{-team-status-name-}}", statusData.conditionName);
            }
            if (!string.IsNullOrEmpty(message.statusEnvironmentID))
            {
                StatusBTLData statusData = BattleStatuses.instance.GetStatusData(message.statusEnvironmentID);
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

