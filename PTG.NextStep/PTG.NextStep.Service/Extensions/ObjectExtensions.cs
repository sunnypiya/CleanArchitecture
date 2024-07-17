using System.Reflection;

namespace PTG.NextStep.Service.Extensions
{
    public static class ObjectExtensions
    {
        public static bool ArePropertiesEqual<TSource, TDestination>(this TSource source, TDestination destination, string propertyName)
        {
            if (source == null || destination == null || string.IsNullOrEmpty(propertyName))
                return false;

            PropertyInfo sourceProperty = typeof(TSource).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo destinationProperty = typeof(TDestination).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (sourceProperty == null || destinationProperty == null)
                throw new ArgumentException($"Property '{propertyName}' not found on either type '{typeof(TSource)}' or '{typeof(TDestination)}'");

            object sourceValue = sourceProperty.GetValue(source);
            object destinationValue = destinationProperty.GetValue(destination);

            if (sourceValue == null && destinationValue == null)
                return true;

            return sourceValue != null && sourceValue.Equals(destinationValue);
        }
    }
}
