db = db.getSiblingDB("skiservice");

db.createCollection("ServiceOrders", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: [
        "FirstName",
        "LastName",
        "Email",
        "Phone",
        "CreateDate",
        "PriorityId",
        "ServiceId",
        "StatusId",
      ],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        FirstName: {
          bsonType: "string",
          minLength: 1,
          maxLength: 50,
        },
        LastName: {
          bsonType: "string",
          minLength: 1,
          maxLength: 50,
        },
        Email: {
          bsonType: "string",
          pattern: "^.+@.+$",
        },
        Phone: {
          bsonType: "string",
          pattern: "^(\\+\\d{1,3}[- ]?)?[0-9 ]{13}$",
        },
        CreateDate: {
          bsonType: "date",
        },
        PriorityId: {
          bsonType: "objectId",
        },
        Priority: {
          bsonType: "object",
        },
        PickupDate: {
          bsonType: "date",
        },
        ServiceId: {
          bsonType: "objectId",
        },
        Service: {
          bsonType: "object",
        },
        TotalPrice_CHF: {
          bsonType: "double",
          minimum: 0,
        },
        StatusId: {
          bsonType: "objectId",
        },
        Status: {
          bsonType: "object",
        },
        Comment: {
          bsonType: "string",
          maxLength: 250,
        },
        UserId: {
          bsonType: "objectId",
        },
        User: {
          bsonType: "object",
        },
      },
      additionalProperties: false,
    },
  },
  validationAction: "warn",
});