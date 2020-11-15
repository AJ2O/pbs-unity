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


        // Trainer Interactions (501 - 599)
        const int TRAINERSENDOUT = 501;
        const int TRAINERMULTISENDOUT = 502;
        const int TRAINERWITHDRAW = 503;
        const int TRAINERITEMUSE = 510;


        // Team Interactions (601 - 699)


        // Environmental Interactions (701 - 799)
        const int ENVIRONMENTALCONDITIONSTART = 701;
        const int ENVIRONMENTALCONDITIONEND = 702;


        // --- Pokemon Interactions ---

        // General (1001 - 1099)

        // Damage / Health (1101 - 1199)
        const int POKEMONHEALTHDAMAGE = 1101;
        const int POKEMONHEALTHHEAL = 1102;
        const int POKEMONHEALTHFAINT = 1103;
        const int POKEMONHEALTHREVIVE = 1104;

        // Abilities (1201 - 1299)
        const int POKEMONABILITYACTIVATE = 1201;
        const int POKEMONABILITYQUICKDRAW = 1250;

        // Moves (1301 - 1399)
        const int POKEMONMOVEUSE = 1301;

        // Stats (1401 - 1499)
        const int POKEMONSTATCHANGE = 1401;
        const int POKEMONSTATUNCHANGEABLE = 1402;

        // Items (1501 - 1599)
        const int POKEMONITEMQUICKCLAW = 1550;

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
                writer.WriteArray(messageParameterized.parameters);
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
                writer.Write(modelUpdate.model);
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



            else if (obj is PBS.Battle.View.Events.EnvironmentalConditionStart environmentalConditionStart)
            {
                writer.WriteInt32(ENVIRONMENTALCONDITIONSTART);
                writer.WriteString(environmentalConditionStart.conditionID);
            }
            else if (obj is PBS.Battle.View.Events.EnvironmentalConditionEnd environmentalConditionEnd)
            {
                writer.WriteInt32(ENVIRONMENTALCONDITIONEND);
                writer.WriteString(environmentalConditionEnd.conditionID);
            }


            else if (obj is PBS.Battle.View.Events.PokemonHealthDamage pokemonHealthDamage)
            {
                writer.WriteInt32(POKEMONHEALTHDAMAGE);
                writer.WriteString(pokemonHealthDamage.pokemonUniqueID);
                writer.WriteInt32(pokemonHealthDamage.preHP);
                writer.WriteInt32(pokemonHealthDamage.postHP);
            }
            else if (obj is PBS.Battle.View.Events.PokemonHealthHeal pokemonHealthHeal)
            {
                writer.WriteInt32(POKEMONHEALTHDAMAGE);
                writer.WriteString(pokemonHealthHeal.pokemonUniqueID);
                writer.WriteInt32(pokemonHealthHeal.preHP);
                writer.WriteInt32(pokemonHealthHeal.postHP);
            }
            else if (obj is PBS.Battle.View.Events.PokemonHealthFaint pokemonHealthFaint)
            {
                writer.WriteInt32(POKEMONHEALTHDAMAGE);
                writer.WriteString(pokemonHealthFaint.pokemonUniqueID);
            }
            else if (obj is PBS.Battle.View.Events.PokemonHealthRevive pokemonHealthRevive)
            {
                writer.WriteInt32(POKEMONHEALTHFAINT);
                writer.WriteString(pokemonHealthRevive.pokemonUniqueID);
            }

            else if (obj is PBS.Battle.View.Events.PokemonMoveUse pokemonMoveUse)
            {
                writer.WriteInt32(POKEMONMOVEUSE);
                writer.WriteString(pokemonMoveUse.pokemonUniqueID);
                writer.WriteString(pokemonMoveUse.moveID);
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

            else if (obj is PBS.Battle.View.Events.PokemonAbilityQuickDraw pokemonAbilityQuickDraw)
            {
                writer.WriteInt32(POKEMONABILITYACTIVATE);
                writer.WriteString(pokemonAbilityQuickDraw.pokemonUniqueID);
                writer.WriteString(pokemonAbilityQuickDraw.abilityID);
            }
            else if (obj is PBS.Battle.View.Events.PokemonAbilityActivate pokemonAbilityActivate)
            {
                writer.WriteInt32(POKEMONABILITYACTIVATE);
                writer.WriteString(pokemonAbilityActivate.pokemonUniqueID);
                writer.WriteString(pokemonAbilityActivate.abilityID);
            }

            else if (obj is PBS.Battle.View.Events.PokemonItemQuickClaw pokemonItemQuickClaw)
            {
                writer.WriteInt32(POKEMONITEMQUICKCLAW);
                writer.WriteString(pokemonItemQuickClaw.pokemonUniqueID);
                writer.WriteString(pokemonItemQuickClaw.itemID);
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
                    return new PBS.Battle.View.Events.MessageParameterized
                    {
                        messageCode = reader.ReadString(),
                        parameters = reader.ReadArray<string>()
                    };
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
                        model = reader.Read<PBS.Battle.View.Model>()
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

                case ENVIRONMENTALCONDITIONSTART:
                    return new PBS.Battle.View.Events.EnvironmentalConditionStart
                    {
                        conditionID = reader.ReadString()
                    };
                case ENVIRONMENTALCONDITIONEND:
                    return new PBS.Battle.View.Events.EnvironmentalConditionEnd
                    {
                        conditionID = reader.ReadString()
                    };


                case POKEMONHEALTHDAMAGE:
                    return new PBS.Battle.View.Events.PokemonHealthDamage
                    {
                        pokemonUniqueID = reader.ReadString(),
                        preHP = reader.ReadInt32(),
                        postHP = reader.ReadInt32()
                    };
                case POKEMONHEALTHHEAL:
                    return new PBS.Battle.View.Events.PokemonHealthHeal
                    {
                        pokemonUniqueID = reader.ReadString(),
                        preHP = reader.ReadInt32(),
                        postHP = reader.ReadInt32()
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
                        pokemonUniqueID = reader.ReadString()
                    };

                case POKEMONABILITYACTIVATE:
                    return new PBS.Battle.View.Events.PokemonAbilityActivate
                    {
                        pokemonUniqueID = reader.ReadString(),
                        abilityID = reader.ReadString()
                    };
                case POKEMONABILITYQUICKDRAW:
                    return new PBS.Battle.View.Events.PokemonAbilityQuickDraw
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

                case POKEMONITEMQUICKCLAW:
                    return new PBS.Battle.View.Events.PokemonItemQuickClaw
                    {
                        pokemonUniqueID = reader.ReadString(),
                        itemID = reader.ReadString()
                    };

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
    }
}
