db = db.getSiblingDB("skiservice");

db.createCollection("Priorities", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      required: ["PriorityName"],
      properties: {
        _id: {
          bsonType: "objectId",
        },
        PriorityName: {
          bsonType: "string",
          enum: ["Tief", "Standard", "Hoch"],
        },
        Price: {
          bsonType: "string",
        },
      },
    },
  },
  validationAction: "warn",
});