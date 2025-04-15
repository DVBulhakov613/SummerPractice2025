using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Package_related.Methods
{
    public class ContentMethods
    {
        private readonly ContentRepository _contentRepository;

        public ContentMethods(ContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        // add a content instance 
        public async Task AddContentAsync(Person user, Package package, Content content)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreateContent"))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати вміст посилки.");
            }
            
            if (package.PackageStatus != PackageStatus.DELIVERED)
            {
                throw new InvalidOperationException("Не можна додати вміст до посилки яку не доставили.");
            }
            await _contentRepository.AddAsync(content);
        }

        // add a content instance
        public async Task EditContentAsync(Person user, Content content, Package package)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdateContent"))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати вміст посилки.");
            }

            if (package.PackageStatus != PackageStatus.DELIVERED)
            {
                throw new InvalidOperationException("Не можна змінити вміст посилки яку не доставили.");
            }
            await _contentRepository.UpdateAsync(content);
        }

        public async Task DeleteContentAsync(Person user, Content content, Package package)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeleteContent"))
            {
                throw new UnauthorizedAccessException("Нема дозволу видалити вміст посилки.");
            }

            if (package.PackageStatus != PackageStatus.DELIVERED)
            {
                throw new InvalidOperationException("Не можна видалити вміст посилки яку не доставили.");
            }
            await _contentRepository.DeleteAsync(content);
        }
    }
}
