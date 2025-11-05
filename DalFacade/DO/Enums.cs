using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;

/// <summary>
/// Delivery modes supported by the system.
/// </summary>
public enum DeliveryType
{
    Vehicle,    // רכב
    Motorcycle, // אופנוע
    Bicycle,    // אופניים
    Pedestrian  // רגלי
}

/// <summary>
/// Example order types. Adjust values to match company-specific taxonomy.
/// </summary>
public enum OrderType
{
    Standard,
    Express,
    Food,
    Grocery,
    Heavy
}

/// <summary>
/// How a delivery ended. null in Delivery.EndType means delivery is still in progress.
/// </summary>
public enum DeliveryEndType
{
    Completed,
    Cancelled,
    Failed
}
