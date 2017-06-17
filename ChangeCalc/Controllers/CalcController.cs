using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;

namespace ChangeCalc.Controllers
{
    /// <summary>
    /// Calculation Controller
    /// </summary>
    public class CalcController : ApiController
    {
        // A list of valid sterling currency values
        // TODO use a sorted list or specifically sort these into the right order before using since the order of the items in the list is important
        private readonly List<decimal> _validSterlingValues = new List<decimal> {
            50M,
            20M,
            10M,
            5M,
            2M,
            1M,
            0.50M,
            0.20M,
            0.10M,
            0.05M,
            0.02M,
            0.01M
        };

        // Exception constants
        private const string TenderedValueIsLessThanItemValue = "Tendered value is less than Item value";
        private const string NoChangeNeeded = "No change needed";

        // Response constants
        private const string ResponsePrefix = "Your change is: ";

        /// <summary>
        /// Get the amount of change for a tendered amount and item value
        /// </summary>
        /// <param name="tendered">Tendered amount</param>
        /// <param name="itemValue">Item value</param>
        /// <returns></returns>
        public string Get(decimal tendered, decimal itemValue)
        {
            try
            {
                if (!ModelState.IsValid) throw new HttpResponseException(HttpStatusCode.BadRequest);

                ValidateParameters(tendered, itemValue);

                // If the Tendered and ItemValue are the same there is no change required
                if (tendered == itemValue) return NoChangeNeeded;

                // Begin creating the response
                var changeStringBuilder = new StringBuilder();
                changeStringBuilder.Append(ResponsePrefix);

                // Calculate the value we need to determine the change for
                var currentValue = tendered - itemValue;

                // Create a Dictionary of valid sterling currency values and how many are required
                var changeValues = new Dictionary<decimal, int>();

                foreach (var sterlingValue in _validSterlingValues)
                {
                    changeValues.Add(sterlingValue, (int)(currentValue / sterlingValue));
                    currentValue = currentValue % sterlingValue;
                }

                // Process the number required list
                var changeResponses = (from changeValue in changeValues
                                       where changeValue.Value > 0
                                       select changeValue.Key < 1.00M
                                           ? $"{changeValue.Value} x {changeValue.Key * 100:G0}p"
                                           : $"{changeValue.Value} x £{changeValue.Key}").ToList();

                // Flatten the number required list into a CSV
                changeStringBuilder.Append(string.Join(", ", changeResponses));

                // Return calculated result
                return changeStringBuilder.ToString();
            }
            catch (Exception e)
            {
                // TODO better exception handling
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Validate the parameters passed into the Get routine
        /// </summary>
        /// <param name="tendered">amount tendered</param>
        /// <param name="itemValue">item value</param>
        private static void ValidateParameters(decimal tendered, decimal itemValue)
        {
            // Both parameters should be greater than zero
            if (tendered <= 0) throw new ArgumentOutOfRangeException(nameof(tendered));
            if (itemValue <= 0) throw new ArgumentOutOfRangeException(nameof(itemValue));

            // Tendered value must be greater than the item value
            if (tendered < itemValue) throw new ArgumentOutOfRangeException(TenderedValueIsLessThanItemValue);
        }
    }
}
