using Dal;
using DO;
using System;
using System.Globalization;
using System.Xml.Linq;

namespace DalXml
{
    public class Class1
    {
        /// <summary>
        /// Create an Order instance from an XElement.
        /// Expects child elements: Id, Type, Description (opt), Address, Latitude, Longitude,
        /// CustomerName, CustomerPhone, Weight (opt), Volume (opt), IsFragile (opt), OpenedAt.
        /// Throws DalXMLException on missing/invalid required values.
        /// </summary>
        public static Order getOrder(XElement elem)
        {
            try
            {
                int id = elem.ToIntNullable(nameof(Order.Id)) ?? throw new DalXMLException("Order.Id is missing or invalid");
                OrderType type = elem.ToEnumNullable<OrderType>(nameof(Order.Type)) ?? throw new DalXMLException("Order.Type is missing or invalid");
                string? description = (string?)elem.Element(nameof(Order.Description));
                string address = (string?)elem.Element(nameof(Order.Address)) ?? throw new DalXMLException("Order.Address is missing");
                double latitude = elem.ToDoubleNullable(nameof(Order.Latitude)) ?? throw new DalXMLException("Order.Latitude is missing or invalid");
                double longitude = elem.ToDoubleNullable(nameof(Order.Longitude)) ?? throw new DalXMLException("Order.Longitude is missing or invalid");
                string customerName = (string?)elem.Element(nameof(Order.CustomerName)) ?? throw new DalXMLException("Order.CustomerName is missing");
                string customerPhone = (string?)elem.Element(nameof(Order.CustomerPhone)) ?? throw new DalXMLException("Order.CustomerPhone is missing");
                double? weight = elem.ToDoubleNullable(nameof(Order.Weight));
                double? volume = elem.ToDoubleNullable(nameof(Order.Volume));
                bool isFragile = bool.TryParse((string?)elem.Element(nameof(Order.IsFragile)), out var b) && b;
                DateTime openedAt = elem.ToDateTimeNullable(nameof(Order.OpenedAt)) ?? throw new DalXMLException("Order.OpenedAt is missing or invalid");

                return new Order(
                    Id: id,
                    Type: type,
                    Description: description,
                    Address: address,
                    Latitude: latitude,
                    Longitude: longitude,
                    CustomerName: customerName,
                    CustomerPhone: customerPhone,
                    Weight: weight,
                    Volume: volume,
                    IsFragile: isFragile,
                    OpenedAt: openedAt
                );
            }
            catch (DalXMLException) { throw; }
            catch (Exception ex)
            {
                throw new DalXMLException("Failed to parse Order element", ex);
            }
        }

        /// <summary>
        /// Create an XElement representing the provided Order.
        /// Omits optional elements when null. Numeric values are formatted invariantly; dates use round-trip ("o") format.
        /// </summary>
        public static XElement getOrderXElement(Order order)
        {
            if (order is null) throw new DalXMLException("order is null");

            var elem = new XElement("Order",
                new XElement(nameof(Order.Id), order.Id),
                new XElement(nameof(Order.Type), order.Type.ToString()),
                order.Description is not null ? new XElement(nameof(Order.Description), order.Description) : null,
                new XElement(nameof(Order.Address), order.Address),
                new XElement(nameof(Order.Latitude), order.Latitude.ToString(CultureInfo.InvariantCulture)),
                new XElement(nameof(Order.Longitude), order.Longitude.ToString(CultureInfo.InvariantCulture)),
                new XElement(nameof(Order.CustomerName), order.CustomerName),
                new XElement(nameof(Order.CustomerPhone), order.CustomerPhone),
                order.Weight.HasValue ? new XElement(nameof(Order.Weight), order.Weight.Value.ToString(CultureInfo.InvariantCulture)) : null,
                order.Volume.HasValue ? new XElement(nameof(Order.Volume), order.Volume.Value.ToString(CultureInfo.InvariantCulture)) : null,
                new XElement(nameof(Order.IsFragile), order.IsFragile),
                new XElement(nameof(Order.OpenedAt), order.OpenedAt.ToString("o"))
            );

            return elem;
        }
    }
}
