using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking.CustomSerialization.Battle.View
{
    public static class Events
    {
        const int BASE = 0;

        // Battle Phases (1 - 99)
        const int STARTBATTLE = 1;
        const int ENDBATTLE = 2;


        // Messages (101 - 199)
        const int MESSAGE = 101;
        const int MESSAGEPARAMETERIZED = 102;
        const int MESSAGEPOKEMON = 103;
        const int MESSAGETRAINER = 104;
        const int MESSAGETEAM = 105;


        // Backend (201 - 299)
        const int MODELUPDATE = 201;

        // Command Prompts (301 -399)
        const int COMMANDGENERALPROMPT = 301;
        const int COMMANDREPLACEMENTPROMPT = 302;


        // Trainer Interactions (501 - 599)
        const int TRAINERSENDOUT = 501;
        const int TRAINERMULTISENDOUT = 502;
        const int TRAINERWITHDRAW = 503;
        const int TRAINERITEMUSE = 510;


        // Team Interactions (601 - 699)


        // Environmental Interactions (701 - 799)


        // --- Pokemon Interactions ---

        // General (1001 - 1099)
        const int POKEMONCHANGEFORM = 1005;
        const int POKEMONSWITCHPOSITION = 1050;

        // Damage / Health (1101 - 1199)
        const int POKEMONHEALTHDAMAGE = 1101;
        const int POKEMONHEALTHHEAL = 1102;
        const int POKEMONHEALTHFAINT = 1103;
        const int POKEMONHEALTHREVIVE = 1104;

        // Abilities (1201 - 1299)
        const int POKEMONABILITYACTIVATE = 1201;

        // Moves (1301 - 1399)
        const int POKEMONMOVEUSE = 1301;
        const int POKEMONMOVEHIT = 1302;

        // Stats (1401 - 1499)
        const int POKEMONSTATCHANGE = 1401;
        const int POKEMONSTATUNCHANGEABLE = 1402;

        // Items (1501 - 1599)

        // Status

        // Misc Status (2001 - 2099)
        const int POKEMONMISCPROTECT = 2001;
        const int POKEMONMISCMATBLOCK = 2002;


        public static void WriteBattleViewEvent(this NetworkWriter writer, PBS.Battle.View.Events.Base obj)
        {
            if (obj is PBS.Battle.View.Events.Message message)
            {
                writer.WriteInt32(MESSAGE);
                writer.WriteString(message.message);
            }
            else if (obj is PBS.Battle.View.Events.MessageParameterized messageParameterized)
            {
                writer.WriteInt32(MESSAGEPARAMETERIZED);
                writer.WriteString(messageParameterized.messageCode);
                writer.WriteBoolean(messageParameterized.isQueryResponse);
                writer.WriteBoolean(messageParameterized.isQuerySuccessful);
                writer.WriteInt32(messageParameterized.playerPerspectiveID);
                writer.WriteInt32(messageParameterized.teamPerspectiveID);

                writer.WriteString(messageParameterized.pokemonID);
                writer.WriteString(messageParameterized.pokemonUserID);
                writer.WriteString(messageParameterized.pokemonTargetID);
                writer.WriteList(messageParameterized.pokemonListIDs);

                writer.WriteInt32(messageParameterized.trainerID);

                writer.WriteInt32(messageParameterized.teamID);

                writer.WriteString(messageParameterized.typeID);
                writer.WriteList(messageParameterized.typeIDs);

                writer.WriteString(messageParameterized.moveID);
                writer.WriteList(messageParameterized.moveIDs);

                writer.WriteString(messageParameterized.abilityID);
                writer.WriteList(messageParameterized.abilityIDs);

                writer.WriteString(messageParameterized.itemID);
                writer.WriteList(messageParameterized.itemIDs);

                writer.WriteString(messageParameterized.statusID);
                writer.WriteString(messageParameterized.statusTeamID);
                writer.WriteString(messageParameterized.statusEnvironmentID);

                writer.WriteList(messageParameterized.intArgs);

                List<int> statInts = new List<int>();
                for (int i = 0; i < messageParameterized.statList.Count; i++)
                {
                    statInts.Add((int)messageParameterized.statList[i]);
                }
                writer.WriteList(statInts);
            }
            else if (obj is PBS.Battle.View.Events.MessagePokemon messagePokemon)
            {
                writer.WriteInt32(MESSAGEPOKEMON);
                writer.WriteString(messagePokemon.preMessage);
                writer.WriteString(messagePokemon.postMessage);
                writer.WriteList(messagePokemon.pokemonUniqueIDs);
            }
            else if (obj is PBS.Battle.View.Events.MessageTrainer messageTrainer)
            {
                writer.WriteInt32(MESSAGETRAINER);
                writer.WriteString(messageTrainer.preMessage);
                writer.WriteString(messageTrainer.postMessage);
                writer.WriteList(messageTrainer.playerIDs);
            }
            else if (obj is PBS.Battle.View.Events.MessageTeam messageTeam)
            {
                writer.WriteInt32(MESSAGETEAM);
                writer.WriteString(messageTeam.preMessage);
                writer.WriteString(messageTeam.postMessage);
                writer.WriteInt32(messageTeam.teamID);
            }


            else if (obj is PBS.Battle.View.Events.ModelUpdate modelUpdate)
            {
                writer.WriteInt32(MODELUPDATE);
                writer.WriteInt32((int)modelUpdate.updateType);
                writer.WriteBoolean(modelUpdate.synchronize);
                writer.WriteBattleViewModel(modelUpdate.model);
            }


            else if (obj is PBS.Battle.View.Events.CommandGeneralPrompt commandGeneralPrompt)
            {
                writer.WriteInt32(COMMANDGENERALPROMPT);
                writer.WriteInt32(commandGeneralPrompt.playerID);
                writer.WriteBoolean(commandGeneralPrompt.canMegaEvolve);
                writer.WriteBoolean(commandGeneralPrompt.canZMove);
                writer.WriteBoolean(commandGeneralPrompt.canDynamax);
                writer.WriteList(commandGeneralPrompt.items);
                writer.WriteList(commandGeneralPrompt.pokemonToCommand);
            }
            else if (obj is PBS.Battle.View.Events.CommandReplacementPrompt commandReplacementPrompt)
            {
                writer.WriteInt32(COMMANDREPLACEMENTPROMPT);
                writer.WriteInt32(commandReplacementPrompt.playerID);
                writer.WriteArray(commandReplacementPrompt.fillPositions);
            }


            else if (obj is PBS.Battle.View.Events.StartBattle startBattle)
            {
                writer.WriteInt32(STARTBATTLE);
            }
            else if (obj is PBS.Battle.View.Events.EndBattle endBattle)
            {
                writer.WriteInt32(ENDBATTLE);
                writer.WriteInt32(endBattle.winningTeam);
            }


            else if (obj is PBS.Battle.View.Events.TrainerSendOut trainerSendOut)
            {
                writer.WriteInt32(TRAINERSENDOUT);
                writer.WriteInt32(trainerSendOut.playerID);
                writer.WriteList(trainerSendOut.pokemonUniqueIDs);
            }
            else if (obj is PBS.Battle.View.Events.TrainerMultiSendOut trainerMultiSendOut)
            {
                writer.WriteInt32(TRAINERMULTISENDOUT);
                writer.WriteList(trainerMultiSendOut.sendEvents);
            }
            else if (obj is PBS.Battle.View.Events.TrainerWithdraw trainerWithdraw)
            {
                writer.WriteInt32(TRAINERWITHDRAW);
                writer.WriteInt32(trainerWithdraw.playerID);
                writer.WriteList(trainerWithdraw.pokemonUniqueIDs);
            }
            else if (obj is PBS.Battle.View.Events.TrainerItemUse trainerItemUse)
            {
                writer.WriteInt32(TRAINERITEMUSE);
                writer.WriteInt32(trainerItemUse.playerID);
                writer.WriteString(trainerItemUse.itemID);
            }


            else if (obj is PBS.Battle.View.Events.PokemonChangeForm pokemonChangeForm)
            {
                writer.WriteInt32(POKEMONCHANGEFORM);
                writer.WriteString(pokemonChangeForm.pokemonUniqueID);
                writer.WriteString(pokemonChangeForm.preForm);
                writer.WriteString(pokemonChangeForm.postForm);
            }
            else if (obj is PBS.Battle.View.Events.PokemonSwitchPosition pokemonSwitchPosition)
            {
                writer.WriteInt32(POKEMONSWITCHPOSITION);
                writer.WriteString(pokemonSwitchPosition.pokemonUniqueID1);
                writer.WriteString(pokemonSwitchPosition.pokemonUniqueID2);
            }

            else if (obj is PBS.Battle.View.Events.PokemonHealthDamage pokemonHealthDamage)
            {
                writer.WriteInt32(POKEMONHEALTHDAMAGE);
                writer.WriteString(pokemonHealthDamage.pokemonUniqueID);
                writer.WriteInt32(pokemonHealthDamage.preHP);
                writer.WriteInt32(pokemonHealthDamage.postHP);
                writer.WriteInt32(pokemonHealthDamage.maxHP);
            }
            else if (obj is PBS.Battle.View.Events.PokemonHealthHeal pokemonHealthHeal)
            {
                writer.WriteInt32(POKEMONHEALTHHEAL);
                writer.WriteString(pokemonHealthHeal.pokemonUniqueID);
                writer.WriteInt32(pokemonHealthHeal.preHP);
                writer.WriteInt32(pokemonHealthHeal.postHP);
                writer.WriteInt32(pokemonHealthHeal.maxHP);
            }
            else if (obj is PBS.Battle.View.Events.PokemonHealthFaint pokemonHealthFaint)
            {
                writer.WriteInt32(POKEMONHEALTHFAINT);
                writer.WriteString(pokemonHealthFaint.pokemonUniqueID);
            }
            else if (obj is PBS.Battle.View.Events.PokemonHealthRevive pokemonHealthRevive)
            {
                writer.WriteInt32(POKEMONHEALTHREVIVE);
                writer.WriteString(pokemonHealthRevive.pokemonUniqueID);
            }

            else if (obj is PBS.Battle.View.Events.PokemonMoveUse pokemonMoveUse)
            {
                writer.WriteInt32(POKEMONMOVEUSE);
                writer.WriteString(pokemonMoveUse.pokemonUniqueID);
                writer.WriteString(pokemonMoveUse.moveID);
            }
            else if (obj is PBS.Battle.View.Events.PokemonMoveHit pokemonMoveHit)
            {
                writer.WriteInt32(POKEMONMOVEHIT);
                writer.WriteString(pokemonMoveHit.pokemonUniqueID);
                writer.WriteString(pokemonMoveHit.moveID);
                writer.WriteInt32(pokemonMoveHit.currentHit);
                writer.WriteList(pokemonMoveHit.hitTargets);
            }

            else if (obj is PBS.Battle.View.Events.PokemonStatChange pokemonStatChange)
            {
                writer.WriteInt32(POKEMONSTATCHANGE);
                writer.WriteString(pokemonStatChange.pokemonUniqueID);
                writer.WriteInt32(pokemonStatChange.modValue);
                writer.WriteBoolean(pokemonStatChange.maximize);
                writer.WriteBoolean(pokemonStatChange.minimize);

                List<int> statInts = new List<int>();
                for (int i = 0; i < pokemonStatChange.statsToMod.Count; i++)
                {
                    statInts.Add((int)pokemonStatChange.statsToMod[i]);
                }
                writer.WriteList(statInts);
            }
            else if (obj is PBS.Battle.View.Events.PokemonStatUnchangeable pokemonStatUnchangeable)
            {
                writer.WriteInt32(POKEMONSTATUNCHANGEABLE);
                writer.WriteString(pokemonStatUnchangeable.pokemonUniqueID);
                writer.WriteBoolean(pokemonStatUnchangeable.tooHigh);

                List<int> statInts = new List<int>();
                for (int i = 0; i < pokemonStatUnchangeable.statsToMod.Count; i++)
                {
                    statInts.Add((int)pokemonStatUnchangeable.statsToMod[i]);
                }
                writer.WriteList(statInts);
            }

            else if (obj is PBS.Battle.View.Events.PokemonAbilityActivate pokemonAbilityActivate)
            {
                writer.WriteInt32(POKEMONABILITYACTIVATE);
                writer.WriteString(pokemonAbilityActivate.pokemonUniqueID);
                writer.WriteString(pokemonAbilityActivate.abilityID);
            }

            else if (obj is PBS.Battle.View.Events.PokemonMiscProtect pokemonMiscProtect)
            {
                writer.WriteInt32(POKEMONMISCPROTECT);
                writer.WriteString(pokemonMiscProtect.pokemonUniqueID);
            }
            else if (obj is PBS.Battle.View.Events.PokemonMiscMatBlock pokemonMiscMatBlock)
            {
                writer.WriteInt32(POKEMONMISCMATBLOCK);
                writer.WriteInt32(pokemonMiscMatBlock.teamID);
            }

        }
        public static PBS.Battle.View.Events.Base ReadBattleViewEvent(this NetworkReader reader)
        {
            int type = reader.ReadInt32();
            switch(type)
            {
                case STARTBATTLE:
                    return new PBS.Battle.View.Events.StartBattle
                    {

                    };
                case ENDBATTLE:
                    return new PBS.Battle.View.Events.EndBattle
                    {
                        winningTeam = reader.ReadInt32()
                    };


                case MESSAGE:
                    return new PBS.Battle.View.Events.Message
                    {
                        message = reader.ReadString()
                    };
                case MESSAGEPARAMETERIZED:
                    PBS.Battle.View.Events.MessageParameterized messageParameterized = new PBS.Battle.View.Events.MessageParameterized
                    {
                        messageCode = reader.ReadString(),
                        isQueryResponse = reader.ReadBoolean(),
                        isQuerySuccessful = reader.ReadBoolean(),
                        playerPerspectiveID = reader.ReadInt32(),
                        teamPerspectiveID = reader.ReadInt32(),

                        pokemonID = reader.ReadString(),
                        pokemonUserID = reader.ReadString(),
                        pokemonTargetID = reader.ReadString(),
                        pokemonListIDs = reader.ReadList<string>(),

                        trainerID = reader.ReadInt32(),

                        teamID = reader.ReadInt32(),
                        
                        typeID = reader.ReadString(),
                        typeIDs = reader.ReadList<string>(),

                        moveID = reader.ReadString(),
                        moveIDs = reader.ReadList<string>(),

                        abilityID = reader.ReadString(),
                        abilityIDs = reader.ReadList<string>(),

                        itemID = reader.ReadString(),
                        itemIDs = reader.ReadList<string>(),

                        statusID = reader.ReadString(),
                        statusTeamID = reader.ReadString(),
                        statusEnvironmentID = reader.ReadString(),

                        intArgs = reader.ReadList<int>(),
                    };

                    List<PokemonStats> messageParameterizedStatList = new List<PokemonStats>();
                    List<int> messageParameterizedstatInts = reader.ReadList<int>();
                    for (int i = 0; i < messageParameterizedstatInts.Count; i++)
                    {
                        messageParameterizedStatList.Add((PokemonStats)messageParameterizedstatInts[i]);
                    }

                    messageParameterized.statList.AddRange(messageParameterizedStatList);
                    return messageParameterized;
                case MESSAGEPOKEMON:
                    return new PBS.Battle.View.Events.MessagePokemon
                    {
                        preMessage = reader.ReadString(),
                        postMessage = reader.ReadString(),
                        pokemonUniqueIDs = reader.ReadList<string>()
                    };
                case MESSAGETRAINER:
                    return new PBS.Battle.View.Events.MessageTrainer
                    {
                        preMessage = reader.ReadString(),
                        postMessage = reader.ReadString(),
                        playerIDs = reader.ReadList<int>()
                    };
                case MESSAGETEAM:
                    return new PBS.Battle.View.Events.MessageTeam
                    {
                        preMessage = reader.ReadString(),
                        postMessage = reader.ReadString(),
                        teamID = reader.ReadInt32()
                    };


                case MODELUPDATE:
                    return new PBS.Battle.View.Events.ModelUpdate
                    {
                        updateType = (PBS.Battle.View.Events.ModelUpdate.UpdateType)reader.ReadInt32(),
                        synchronize = reader.ReadBoolean(),
                        model = reader.ReadBattleViewModel()
                    };

                case COMMANDGENERALPROMPT:
                    return new PBS.Battle.View.Events.CommandGeneralPrompt
                    {
                        playerID = reader.ReadInt32(),
                        canMegaEvolve = reader.ReadBoolean(),
                        canZMove = reader.ReadBoolean(),
                        canDynamax = reader.ReadBoolean(),
                        items = reader.ReadList<string>(),
                        pokemonToCommand = reader.ReadList<PBS.Battle.View.Events.CommandAgent>()
                    };
                case COMMANDREPLACEMENTPROMPT:
                    return new PBS.Battle.View.Events.CommandReplacementPrompt
                    {
                        playerID = reader.ReadInt32(),
                        fillPositions = reader.ReadArray<int>()
                    };


                case TRAINERSENDOUT:
                    return new PBS.Battle.View.Events.TrainerSendOut
                    {
                        playerID = reader.ReadInt32(),
                        pokemonUniqueIDs = reader.ReadList<string>()
                    };
                case TRAINERMULTISENDOUT:
                    return new PBS.Battle.View.Events.TrainerMultiSendOut
                    {
                        sendEvents = reader.ReadList<PBS.Battle.View.Events.TrainerSendOut>()
                    };
                case TRAINERWITHDRAW:
                    return new PBS.Battle.View.Events.TrainerWithdraw
                    {
                        playerID = reader.ReadInt32(),
                        pokemonUniqueIDs = reader.ReadList<string>()
                    };
                case TRAINERITEMUSE:
                    return new PBS.Battle.View.Events.TrainerItemUse
                    {
                        playerID = reader.ReadInt32(),
                        itemID = reader.ReadString()
                    };


                case POKEMONCHANGEFORM:
                    return new PBS.Battle.View.Events.PokemonChangeForm
                    {
                        pokemonUniqueID = reader.ReadString(),
                        preForm = reader.ReadString(),
                        postForm = reader.ReadString()
                    };
                case POKEMONSWITCHPOSITION:
                    return new PBS.Battle.View.Events.PokemonSwitchPosition
                    {
                        pokemonUniqueID1 = reader.ReadString(),
                        pokemonUniqueID2 = reader.ReadString()
                    };

                case POKEMONHEALTHDAMAGE:
                    return new PBS.Battle.View.Events.PokemonHealthDamage
                    {
                        pokemonUniqueID = reader.ReadString(),
                        preHP = reader.ReadInt32(),
                        postHP = reader.ReadInt32(),
                        maxHP = reader.ReadInt32()
                    };
                case POKEMONHEALTHHEAL:
                    return new PBS.Battle.View.Events.PokemonHealthHeal
                    {
                        pokemonUniqueID = reader.ReadString(),
                        preHP = reader.ReadInt32(),
                        postHP = reader.ReadInt32(),
                        maxHP = reader.ReadInt32()
                    };
                case POKEMONHEALTHFAINT:
                    return new PBS.Battle.View.Events.PokemonHealthFaint
                    {
                        pokemonUniqueID = reader.ReadString()
                    };
                case POKEMONHEALTHREVIVE:
                    return new PBS.Battle.View.Events.PokemonHealthRevive
                    {
                        pokemonUniqueID = reader.ReadString()
                    };

                case POKEMONMOVEUSE:
                    return new PBS.Battle.View.Events.PokemonMoveUse
                    {
                        pokemonUniqueID = reader.ReadString(),
                        moveID = reader.ReadString()
                    };
                case POKEMONMOVEHIT:
                    return new PBS.Battle.View.Events.PokemonMoveHit
                    {
                        pokemonUniqueID = reader.ReadString(),
                        moveID = reader.ReadString(),
                        currentHit = reader.ReadInt32(),
                        hitTargets = reader.ReadList<PBS.Battle.View.Events.PokemonMoveHitTarget>()
                    };

                case POKEMONABILITYACTIVATE:
                    return new PBS.Battle.View.Events.PokemonAbilityActivate
                    {
                        pokemonUniqueID = reader.ReadString(),
                        abilityID = reader.ReadString()
                    };

                case POKEMONSTATCHANGE:
                    PBS.Battle.View.Events.PokemonStatChange statChange = new PBS.Battle.View.Events.PokemonStatChange
                    {
                        pokemonUniqueID = reader.ReadString(),
                        modValue = reader.ReadInt32(),
                        maximize = reader.ReadBoolean(),
                        minimize = reader.ReadBoolean(),
                        statsToMod = new List<PokemonStats>()
                    };
                    List<PokemonStats> statsToMod = new List<PokemonStats>();
                    List<int> statInts = reader.ReadList<int>();
                    for (int i = 0; i < statInts.Count; i++)
                    {
                        statsToMod.Add((PokemonStats)statInts[i]);
                    }
                    statChange.statsToMod.AddRange(statsToMod);
                    return statChange;
                case POKEMONSTATUNCHANGEABLE:
                    PBS.Battle.View.Events.PokemonStatUnchangeable statUnchangeable = new PBS.Battle.View.Events.PokemonStatUnchangeable
                    {
                        pokemonUniqueID = reader.ReadString(),
                        tooHigh = reader.ReadBoolean(),
                        statsToMod = new List<PokemonStats>()
                    };
                    List<PokemonStats> statsToModUnchangeable = new List<PokemonStats>();
                    List<int> statIntsUnchangeable = reader.ReadList<int>();
                    for (int i = 0; i < statIntsUnchangeable.Count; i++)
                    {
                        statsToModUnchangeable.Add((PokemonStats)statIntsUnchangeable[i]);
                    }
                    statUnchangeable.statsToMod.AddRange(statsToModUnchangeable);
                    return statUnchangeable;

                case POKEMONMISCPROTECT:
                    return new PBS.Battle.View.Events.PokemonMiscProtect
                    {
                        pokemonUniqueID = reader.ReadString()
                    };
                case POKEMONMISCMATBLOCK:
                    return new PBS.Battle.View.Events.PokemonMiscMatBlock
                    {
                        teamID = reader.ReadInt32()
                    };

                default:
                    throw new System.Exception($"Invalid event type {type}");
            }
        }

        public static void WriteBattleViewEventPokemonMoveHitTarget(this NetworkWriter writer, PBS.Battle.View.Events.PokemonMoveHitTarget obj)
        {
            writer.WriteString(obj.pokemonUniqueID);
            writer.WriteBoolean(obj.affectedByMove);
            writer.WriteBoolean(obj.missed);
            writer.WriteBoolean(obj.criticalHit);
            writer.WriteInt32(obj.preHP);
            writer.WriteInt32(obj.postHP);
            writer.WriteInt32(obj.maxHP);
            writer.WriteInt32(obj.damageDealt);
            writer.WriteDouble((double)obj.effectiveness);
        }
        public static PBS.Battle.View.Events.PokemonMoveHitTarget ReadBattleViewEventPokemonMoveHitTarget(this NetworkReader reader)
        {
            return new PBS.Battle.View.Events.PokemonMoveHitTarget
            {
                pokemonUniqueID = reader.ReadString(),
                affectedByMove = reader.ReadBoolean(),
                missed = reader.ReadBoolean(),
                criticalHit = reader.ReadBoolean(),
                preHP = reader.ReadInt32(),
                postHP = reader.ReadInt32(),
                maxHP = reader.ReadInt32(),
                damageDealt = reader.ReadInt32(),
                effectiveness = (float)reader.ReadDouble()
            };
        }
    
        public static void WriteBattleViewEventCommandAgentMoveslot(this NetworkWriter writer, PBS.Battle.View.Events.CommandAgent.Moveslot obj)
        {
            writer.WriteString(obj.moveID);
            writer.WriteInt32(obj.PP);
            writer.WriteInt32(obj.maxPP);
            writer.WriteInt32(obj.basePower);
            writer.WriteDouble((double)obj.accuracy);
            writer.WriteBoolean(obj.hide);
            writer.WriteBoolean(obj.useable);
            writer.WriteString(obj.failMessageCode);
            writer.WriteList<List<BattlePosition>>(obj.possibleTargets);
        }
        public static PBS.Battle.View.Events.CommandAgent.Moveslot ReadBattleViewEventCommandAgentMoveslot(this NetworkReader reader)
        {
            return new PBS.Battle.View.Events.CommandAgent.Moveslot
            {
                moveID = reader.ReadString(),
                PP = reader.ReadInt32(),
                maxPP = reader.ReadInt32(),
                basePower = reader.ReadInt32(),
                accuracy = (float)reader.ReadDouble(),
                hide = reader.ReadBoolean(),
                useable = reader.ReadBoolean(),
                failMessageCode = reader.ReadString(),
                possibleTargets = reader.ReadList<List<BattlePosition>>()
            };
        }
        public static void WriteBattleViewEventCommandAgent(this NetworkWriter writer, PBS.Battle.View.Events.CommandAgent obj)
        {
            writer.WriteString(obj.pokemonUniqueID);
            writer.WriteBoolean(obj.canMegaEvolve);
            writer.WriteBoolean(obj.canZMove);
            writer.WriteBoolean(obj.canDynamax);
            writer.WriteBoolean(obj.isDynamaxed);
            writer.WriteList(obj.moveslots);
            writer.WriteList(obj.zMoveSlots);
            writer.WriteList(obj.dynamaxMoveSlots);

            List<int> commandInts = new List<int>();
            for (int i = 0; i < obj.commandTypes.Count; i++)
            {
                commandInts.Add((int)obj.commandTypes[i]);
            }
            writer.WriteList(commandInts);
        }
        public static PBS.Battle.View.Events.CommandAgent ReadBattleViewEventCommandAgent(this NetworkReader reader)
        {
            PBS.Battle.View.Events.CommandAgent obj = new PBS.Battle.View.Events.CommandAgent
            {
                pokemonUniqueID = reader.ReadString(),
                canMegaEvolve = reader.ReadBoolean(),
                canZMove = reader.ReadBoolean(),
                canDynamax = reader.ReadBoolean(),
                isDynamaxed = reader.ReadBoolean(),
                moveslots = reader.ReadList<PBS.Battle.View.Events.CommandAgent.Moveslot>(),
                zMoveSlots = reader.ReadList<PBS.Battle.View.Events.CommandAgent.Moveslot>(),
                dynamaxMoveSlots = reader.ReadList<PBS.Battle.View.Events.CommandAgent.Moveslot>()
            };
            obj.commandTypes = new List<BattleCommandType>();

            List<int> commandInts = reader.ReadList<int>();
            for (int i = 0; i < commandInts.Count; i++)
            {
                obj.commandTypes.Add((BattleCommandType)commandInts[i]);
            }
            return obj;
        }
    }
}
