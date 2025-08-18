using GymMgmt.Domain.Common.ValueObjects;
using GymMgmt.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMgmt.Domain.Exceptions;

namespace GymMgmt.Domain.Entities.ClubSettingsConfig
{
    /// <summary>
    /// Represents global configuration for the gym club.
    /// Controls policies like insurance fees and registration rules.
    /// </summary>
    public class ClubSettings :AuditableEntity<ClubSettingsId>, IAggregateRoot
    {
        public InsuranceFee CurrentInsuranceFee { get; private set; } = null!;
        public bool AreNewMembersAllowed { get; private set; } = true;
        public bool IsInsuranceFeeRequired { get; private set; } = false;
        public int SubscriptionGracePeriodInDays { get; private set; } = 5;
        public int InsuranceValidityInDays { get; private set; } = 365;

        // EF Core
        private ClubSettings() { }

        /// <summary>
        /// Creates club settings with a default or initial insurance fee.
        /// </summary>
        public static ClubSettings Create(InsuranceFee initialInsuranceFee)
        {
            if (initialInsuranceFee == null)
                throw new InsuranceFeeRequiredException();

            if (!initialInsuranceFee.IsActive)
                throw new InactiveFeeCannotBeCurrentException();

            return new ClubSettings
            {
                CurrentInsuranceFee = initialInsuranceFee
            };
        }

        /// <summary>
        /// Updates the current insurance fee (e.g., price changed from 50 to 60 MAD).
        /// </summary>
        public void UpdateInsuranceFee(InsuranceFee newFee)
        {
            if (newFee == null)
                throw new InsuranceFeeRequiredException();

            if (!newFee.IsActive)
                throw new InactiveFeeCannotBeCurrentException(Id.Value);

            CurrentInsuranceFee = newFee;
        }

        /// <summary>
        /// Toggles whether the insurance fee is required for new members.
        /// </summary>
        public void SetInsuranceFeeRequirement(bool isRequired)
        {
            IsInsuranceFeeRequired = isRequired;
        }

        /// <summary>
        /// Allows or blocks new member registrations.
        /// </summary>
        public void SetNewMembersAllowed(bool allowed)
        {
            AreNewMembersAllowed = allowed;
        }

        /// <summary>
        /// Updates the grace period for expired subscriptions (e.g., 5 days).
        /// </summary>
        public void UpdateSubscriptionGracePeriod(int periodInDays)
        {
            if (periodInDays < 0)
                throw new SubscriptionGracePeriodNegativeException(periodInDays);

            SubscriptionGracePeriodInDays = periodInDays;
        }

        /// <summary>
        /// Updates the validity period of the insurance fee (e.g., 365 days).
        /// </summary>
        public void UpdateInsuranceValidity(int days)
        {
            if (days <= 0)
                throw new InsuranceValidityNotPositiveException(days);

            InsuranceValidityInDays = days;
        }
    }
}
