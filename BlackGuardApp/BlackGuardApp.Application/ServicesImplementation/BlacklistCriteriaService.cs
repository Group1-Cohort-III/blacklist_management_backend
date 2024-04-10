using BlackGuardApp.Application.Interfaces.Repositories;
using BlackGuardApp.Application.Interfaces.Services;
using BlackGuardApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackGuardApp.Application.ServicesImplementation
{
    public class BlacklistCriteriaService : IBlacklistCriteriaService
    {

        private readonly IUnitOfWork _unitOfWork;

        public BlacklistCriteriaService(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddCategories()
        {
            var categories = new List<BlacklistCriteria>
        {
            new BlacklistCriteria
            {
                CategoryName = "Expired Products",
                CategoryDescription = "Contains products that have exceeded their expiry period."
            },
            new BlacklistCriteria
            {
                CategoryName = "Counterfeit Products",
                CategoryDescription = "Contains product names associated with counterfeit goods or trademark infringements."
            },
            new BlacklistCriteria
            {
                CategoryName = "Fraudulent Listings",
                CategoryDescription = "Contains keywords commonly found in listings for fraudulent or scam products."
            },
            new BlacklistCriteria
            {
                CategoryName = "Misleading Descriptions",
                CategoryDescription = "Contains phrases or patterns indicating misleading or inaccurate product descriptions, such as false claims or deceptive marketing tactics."
            },
            new BlacklistCriteria
            {
                CategoryName = "Restricted Products",
                CategoryDescription = "Contains categories of products that are prohibited or restricted for sale on the platform, such as illegal drugs, weapons, or adult content."
            },
            new BlacklistCriteria
            {
                CategoryName = "Unauthorised Brand Listings",
                CategoryDescription = "Contains brand names used without authorization from the brand owner, potentially indicating counterfeit or unauthorized products."
            },
            new BlacklistCriteria
            {
                CategoryName = "Copyright Infringement",
                CategoryDescription = "Contains image URLs or identifiers associated with copyrighted material used without proper permission or licensing."
            },
            new BlacklistCriteria
            {
                CategoryName = "Fake Reviews",
                CategoryDescription = "Contains seller or product reviews suspected of being fake, manipulated, or incentivized, aimed at deceiving customers."
            }
        };

            foreach (var category in categories)
            {
                 
                var existingCategory = await _unitOfWork.BlacklistCriteriaRepository.GetAllAsync();
               if (existingCategory == null)
               {
                    // If the category doesn't exist, insert it into the database
                 await _unitOfWork.BlacklistCriteriaRepository.AddAsync(category) ;
                  
               }

            }
            return "Added successfully";
        }
    }
}

