using PBS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class ElementalTypes
    {
        //create an object of SingleObject
        private static ElementalTypes singleton = new ElementalTypes();

        //make the constructor private so that this class cannot be
        //instantiated
        private ElementalTypes() { }

        //Get the only object available
        public static ElementalTypes instance
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

        // Methods
        public Data.ElementalType GetTypeData(string ID)
        {
            return Loader.instance.GetElementalTypeData(ID);
        }
        public List<string> GetAllTypes(bool filterBaseOnly = false)
        {
            List<string> allTypes = new List<string>(Loader.instance.typesDB.Keys);
            for (int i = 0; i < allTypes.Count; i++)
            {
                Data.ElementalType typeData = GetTypeData(allTypes[i]);

                if (string.IsNullOrEmpty(typeData.ID))
                {
                    bool removeType = true;

                    if (filterBaseOnly && !string.IsNullOrEmpty(typeData.baseID))
                    {
                        removeType = false;
                    }

                    if (removeType)
                    {
                        allTypes.RemoveAt(i);
                        i--;
                    }
                }
            }
            return new List<string>(allTypes);
        }
    }
}