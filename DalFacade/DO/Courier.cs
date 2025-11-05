using System;

namespace DO;

/// <summary>
/// Data-layer representation of a courier. Validation, password-strength checks and encryption
/// must be performed in the business/logical layer; the DAL assumes values are already valid/encoded.
/// </summary>
public record Courier(
    int Id,                                  // ת"ז שליח - numeric id with check-digit (validate in logic layer)
    string FullName,                         // שם מלא (פרטי ומשפחה)
    string Phone,                            // טלפון סלולרי - 10 digits, starts with '0' (validate in logic layer)
    string Email,
    DateTime StartWork,
    DeliveryType DeliveryType = DeliveryType.Vehicle, // סוג השילוח: רכב/אופנוע/אופניים/רגלי
     // מייל - validate format in logic layer
    string? Password = null,                 // encrypted password stored in DAL. May be null if the courier hasn't been given a password in the system.
    bool IsActive = true,                    // האם פעיל - managed by admin only
    double? MaxDeliveryDistance = null      // מרחק מירבי אישי (null = no limit). Value checked against company max in logic layer.
                         // זמן תחילת עבודה - set at creation (system time) in logic layer
);

