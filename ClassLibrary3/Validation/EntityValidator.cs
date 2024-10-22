using System.ComponentModel.DataAnnotations;

namespace Infrastucture.Validation
{
    public class EntityValidator
    {

        public static bool TryValidateEntity(object entity, out List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(entity);
            validationResults = new List<ValidationResult>();
            return Validator.TryValidateObject(entity, validationContext, validationResults, true);
        }

        public static List<string?> GetValidationResults(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(entity, validationContext, validationResults, true);
            return validationResults.Select(x => x.ErrorMessage).ToList();
        }


    }
}
