db = db.getSiblingDB("skiservice");

db.createCollection("Statuses", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["StatusName"],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        StatusName: {
          bsonType: "string",
          maxLength: 20,
          enum: ["Offen", "In Arbeit", "Erledigt", "Storniert"],
        },
      },
    },
  },
  validationAction: "warn",
});