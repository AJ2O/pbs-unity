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


        // Backend (201 - 299)
        const int MODELUPDATE = 201;


        // Trainer Interactions (501 - 599)
        const int TRAINERSENDOUT = 501;
        const int TRAINERMULTISENDOUT = 502;


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

        // Stats (1401 - 1499)

        // Items (1501 - 1599)
        const int POKEMONITEMQUICKCLAW = 1550;




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
                case POKEMONITEMQUICKCLAW:
                    return new PBS.Battle.View.Events.PokemonItemQuickClaw
                    {
                        pokemonUniqueID = reader.ReadString(),
                        itemID = reader.ReadString()
                    };

                default:
                    throw new System.Exception($"Invalid event type {type}");
            }
        }
    }
}
