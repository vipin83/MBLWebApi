using ManxBirdLifeAppWebApi.Data;
using System;

namespace ManxBirdLifeAppWebApi.Models
{
    public static class CommonUtility
    {
        public static bool SpeciesNotInSenstiveDateRange(MBL_SpeciesSightingDetails species)
        {
            
            var sightDateTime = species.MBL_SightDetails.SightDateTime;

            var speciesSensitiveStartDate = species.SpeciesLookup.SensitiveStartDate;
            var speciesSensitiveEndDate = species.SpeciesLookup.SensitiveEndDate;

            //if no sensitive date range specified then show that species record
            if (!speciesSensitiveStartDate.HasValue || !speciesSensitiveEndDate.HasValue)
            {
                return true;
                //return !(species.boolOverrideSensitiveSpeciesRecord.HasValue && species.boolOverrideSensitiveSpeciesRecord.Value);

            }
               
            var newStartDate = new DateTime(sightDateTime.Year, speciesSensitiveStartDate.Value.Month, speciesSensitiveStartDate.Value.Day);
            var newEndDate = new DateTime(sightDateTime.Year, speciesSensitiveEndDate.Value.Month, speciesSensitiveEndDate.Value.Day);

            //not in sensitive date range and not overriden by admin
            if (!(sightDateTime >= newStartDate && sightDateTime <= newEndDate))                  
            //&& !(species.boolOverrideSensitiveSpeciesRecord.HasValue && species.boolOverrideSensitiveSpeciesRecord.Value )) 
            {
                return true;
            }
                              

            return false;
        }


        public static bool DoesSightEntryNeedApproval(MBL_SpeciesSightingDetails species) 
        {
            //if there is any manually added record for species or location which has not been approved yet.
            return (HasManuallyAddedUnApprovedSpeciesName(species) || HasManuallyAddedUnApprovedLocationName(species));                
        }

        public static bool HasManuallyAddedUnApprovedSpeciesName(MBL_SpeciesSightingDetails species)
        {
            return (HasManuallyAddedSpeciesEntry(species) && !HasApprovedSpeciesEntry(species));
        }

        public static bool HasManuallyAddedUnApprovedLocationName(MBL_SpeciesSightingDetails species)
        {
            return (HasManuallyAddedLocationEntry(species) && !HasApprovedLocationEntry(species));
        }

        public static bool HasManuallyAddedSpeciesEntry(MBL_SpeciesSightingDetails species)
        {
            return species.SpeciesLookup.boolManuallyAddedRecord.HasValue &&
                                            species.SpeciesLookup.boolManuallyAddedRecord.Value;             
        }

        public static bool HasApprovedSpeciesEntry(MBL_SpeciesSightingDetails species)
        {
            
            return  species.SpeciesLookup.boolApprovedByAdmin.HasValue &&
                                            species.SpeciesLookup.boolApprovedByAdmin.Value;            
        }

        public static bool HasManuallyAddedLocationEntry(MBL_SpeciesSightingDetails location)
        {
            return location.MBL_SightDetails.LocationLookup.boolManuallyAddedRecord.HasValue &&
                                            location.MBL_SightDetails.LocationLookup.boolManuallyAddedRecord.Value;
        }

        public static bool HasApprovedLocationEntry(MBL_SpeciesSightingDetails location)
        {

            return location.MBL_SightDetails.LocationLookup.boolApprovedByAdmin.HasValue &&
                                            location.MBL_SightDetails.LocationLookup.boolApprovedByAdmin.Value;
        }
    }
}